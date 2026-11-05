namespace QuanLyNghiPhepHV.Models;

public enum UserRole
{
    TrungDoi,
    PhongQLHV,
    BoMon
}

public static class RoleLabels
{
    public static string Display(UserRole role) => role switch
    {
        UserRole.TrungDoi => "Giáo viên trung đội",
        UserRole.PhongQLHV => "Cán bộ Phòng QLHV",
        UserRole.BoMon => "Giáo viên bộ môn",
        _ => "Không xác định"
    };
}

public sealed class Student
{
    public string MaHocVien { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string Lop { get; set; } = "";
    public string TrungDoi { get; set; } = "";
}

public sealed class LeaveRequest
{
    public int Id { get; set; }
    public string MaDon { get; set; } = "";
    public string MaHocVien { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string Lop { get; set; } = "";
    public string TrungDoi { get; set; } = "";
    public DateTime TuThoiDiem { get; set; }
    public DateTime DenThoiDiem { get; set; }
    public string LyDo { get; set; } = "";
    public string DiaChiNoiNghi { get; set; } = "";
    public string TrangThai { get; set; } = "Chờ duyệt";
    public DateTime NgayTao { get; set; }
    public string NguoiTao { get; set; } = "";
    public string NguoiDuyet { get; set; } = "";
    public DateTime? NgayDuyet { get; set; }
    public string GhiChuDuyet { get; set; } = "";

    public string LoaiDon => TuThoiDiem.Date == DenThoiDiem.Date ? "Trong ngày" : "Dài ngày";
    public double SoGio => Math.Max(0, (DenThoiDiem - TuThoiDiem).TotalHours);
    public double SoNgay => Math.Round(SoGio / 24.0, 2);
    public LeaveRequest Clone() => (LeaveRequest)MemberwiseClone();
}

public sealed class LeaveInput
{
    public string MaHocVien { get; set; } = "";
    public string TrungDoi { get; set; } = "";
    public DateTime TuThoiDiem { get; set; } = DateTime.Today.AddHours(7);
    public DateTime DenThoiDiem { get; set; } = DateTime.Today.AddHours(17);
    public string LyDo { get; set; } = "";
    public string DiaChiNoiNghi { get; set; } = "";

    public static LeaveInput From(LeaveRequest item) => new()
    {
        MaHocVien = item.MaHocVien,
        TrungDoi = item.TrungDoi,
        TuThoiDiem = item.TuThoiDiem,
        DenThoiDiem = item.DenThoiDiem,
        LyDo = item.LyDo,
        DiaChiNoiNghi = item.DiaChiNoiNghi
    };
}

public sealed class AuditEntry
{
    public DateTime At { get; set; } = DateTime.Now;
    public string User { get; set; } = "";
    public string Action { get; set; } = "";
    public string Detail { get; set; } = "";
}

public sealed class RepositoryData
{
    public List<Student> Students { get; set; } = new();
    public List<LeaveRequest> Requests { get; set; } = new();
    public List<AuditEntry> Audit { get; set; } = new();
    public int NextId { get; set; } = 1;
}
