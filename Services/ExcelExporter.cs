using System.IO.Compression;
using System.Security;
using System.Text;
using QuanLyNghiPhepHV.Models;

namespace QuanLyNghiPhepHV.Services;

// Tạo tệp Excel .xlsx chuẩn OpenXML bằng thư viện hệ thống, không cần tải NuGet.
public static class ExcelExporter
{
    private static readonly string[] Headers =
    {
        "STT", "Mã đơn", "Mã học viên", "Họ và tên", "Lớp", "Trung đội",
        "Loại đơn", "Bắt đầu nghỉ", "Kết thúc nghỉ", "Số giờ nghỉ",
        "Lý do nghỉ", "Địa chỉ nơi nghỉ", "Trạng thái", "Ngày nộp", "Người duyệt"
    };

    public static byte[] Build(IReadOnlyList<LeaveRequest> records)
    {
        using var output = new MemoryStream();
        using (var zip = new ZipArchive(output, ZipArchiveMode.Create, true))
        {
            Write(zip, "[Content_Types].xml", """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
                <Default Extension="xml" ContentType="application/xml"/>
                <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
                <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
                </Types>
                """);
            Write(zip, "_rels/.rels", """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
                </Relationships>
                """);
            Write(zip, "xl/workbook.xml", """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
                <sheets><sheet name="Danh sach nghi phep" sheetId="1" r:id="rId1"/></sheets>
                </workbook>
                """);
            Write(zip, "xl/_rels/workbook.xml.rels", """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
                </Relationships>
                """);
            Write(zip, "xl/worksheets/sheet1.xml", Worksheet(records));
        }
        return output.ToArray();
    }

    private static string Worksheet(IReadOnlyList<LeaveRequest> records)
    {
        var xml = new StringBuilder();
        xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        xml.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");
        WriteRow(xml, 1, Headers);
        for (int i = 0; i < records.Count; i++)
        {
            var x = records[i];
            WriteRow(xml, i + 2, new string[]
            {
                (i + 1).ToString(), x.MaDon, x.MaHocVien, x.HoTen, x.Lop,
                x.TrungDoi, x.LoaiDon, x.TuThoiDiem.ToString("dd/MM/yyyy HH:mm"),
                x.DenThoiDiem.ToString("dd/MM/yyyy HH:mm"), x.SoGio.ToString("0.##"),
                x.LyDo, x.DiaChiNoiNghi, x.TrangThai,
                x.NgayTao.ToString("dd/MM/yyyy HH:mm"), x.NguoiDuyet
            });
        }
        xml.Append("</sheetData></worksheet>");
        return xml.ToString();
    }

    private static void WriteRow(StringBuilder xml, int number, IEnumerable<string> cells)
    {
        xml.Append("<row r=\"").Append(number).Append("\">");
        int col = 0;
        foreach (var value in cells)
        {
            xml.Append("<c r=\"").Append(ColumnName(col++)).Append(number)
                .Append("\" t=\"inlineStr\"><is><t xml:space=\"preserve\">")
                .Append(SecurityElement.Escape(value ?? ""))
                .Append("</t></is></c>");
        }
        xml.Append("</row>");
    }

    private static string ColumnName(int zeroBased)
    {
        var result = "";
        for (int x = zeroBased + 1; x > 0; x = (x - 1) / 26)
            result = (char)('A' + (x - 1) % 26) + result;
        return result;
    }

    private static void Write(ZipArchive zip, string name, string contents)
    {
        var entry = zip.CreateEntry(name, CompressionLevel.Fastest);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
        writer.Write(contents);
    }
}
