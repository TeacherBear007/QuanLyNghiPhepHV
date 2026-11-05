# PHÂN TÍCH BÀI TOÁN VÀ ĐỐI CHIẾU TIÊU CHÍ

## 1. Làm rõ bài toán (2 điểm)

**Đối tượng quản lý:** học viên, đơn xin nghỉ phép, tài khoản/role sử dụng, thao tác xét duyệt và lịch sử xử lý.

**Vai trò và quy trình:**

1. Giáo viên trung đội đăng nhập, chọn học viên từ danh mục rồi lập đơn theo biểu mẫu giấy; hệ thống lưu thông tin và tạo mã đơn, trạng thái **Chờ duyệt**.
2. Cán bộ Phòng QLHV đăng nhập và xem các đơn chờ duyệt; duyệt hoặc từ chối, nếu từ chối phải ghi lý do; lưu người và thời điểm xử lý.
3. Giáo viên bộ môn đăng nhập và chỉ tra cứu/đọc các đơn.
4. Vai trò có quyền có thể xuất danh sách `.xlsx` để phục vụ báo cáo.

**Dữ liệu đầu vào (theo mẫu giấy):** tên học viên, lớp, chủ nhiệm trung đội, thời gian xin nghỉ (giờ/phút/ngày/tháng/năm từ và đến), lý do xin nghỉ, địa chỉ nơi nghỉ. Mã học viên xác định học viên khi nhập.

**Dữ liệu đầu ra:** danh sách đơn theo điều kiện lọc, trạng thái xử lý, bản xem trước đơn xin nghỉ phép và tệp Excel.

**Công thức tính:**

- `LoaiDon = "Trong ngày"` khi `TuThoiDiem.Date == DenThoiDiem.Date`, ngược lại `"Dài ngày"`.
- `SoGio = Max(0, (DenThoiDiem - TuThoiDiem).TotalHours)`.
- `SoNgay = Round(SoGio / 24, 2)` (giá trị quy đổi theo thời lượng, không phải số ngày học).
- `TongDon = Count(danh sách đơn)`, `ChoDuyet = Count(trạng thái Chờ duyệt)`, tương tự Đã duyệt / Từ chối.
- `TyLeTrangThai = 100 * SoDonTheoTrangThai / TongDon` (nếu tổng bằng 0 thì tỷ lệ bằng 0).

## 2. Dữ liệu và phương thức (2 điểm)

- `Student`: `MaHocVien`, `HoTen`, `Lop`, `TrungDoi`.
- `LeaveRequest`: `Id`, `MaDon`, `MaHocVien`, `HoTen`, `Lop`, `TrungDoi`, `TuThoiDiem`, `DenThoiDiem`, `LyDo`, `DiaChiNoiNghi`, `TrangThai`, `NgayTao`, `NguoiTao`, `NguoiDuyet`, `NgayDuyet`, `GhiChuDuyet` cùng các trường tính toán.
- `LeaveInput`: thông tin mới/sửa do người nhập.
- `AuditEntry`: thời gian, người dùng, hành động, đối tượng.
- `LeaveRepository`: `GetStudents`, `GetRequests`, `Add`, `Update`, `Delete`, `Review`, `ExportExcel`, `GetAudit`.
- `SeedData` sinh danh mục 12 học viên và 20 đơn thử nghiệm. Các bản ghi phát sinh được lưu vào tệp JSON.

## 3. Giao diện (2 điểm)

- Một giao diện với duy nhất một thanh menu bên trái, đầu trang và các tab chức năng.
- Mẫu đăng nhập gồm tên đăng nhập, mật khẩu, chức vụ.
- Trang quản lý đơn: khung nhập ở phía trên (chỉ giáo viên trung đội), bảng danh sách phía dưới; tìm kiếm, lọc, reset, thêm, sửa, xóa.
- Cửa sổ nổi (dialog/overlay) xem trước giấy xin nghỉ, in đơn, sửa đơn, xác nhận xóa và thao tác xét duyệt.
- Hỗ trợ hiển thị trên nhiều kích thước màn hình và bảng cuộn ngang khi cần.

## 4. Báo cáo đầu ra (1 điểm)

- `ExcelExporter.Build` tạo file Excel `.xlsx` bằng chuẩn OpenXML (không dùng NuGet bên ngoài).
- File xuất gồm mã đơn, mã học viên, tên, lớp, trung đội, loại đơn, thời gian nghỉ, số giờ, lý do, địa chỉ, trạng thái, ngày nộp, người duyệt.
- Trang Báo cáo thống kê theo lớp và trạng thái.

## 5. An ninh an toàn (1 điểm)

- Giới hạn 5 lần đăng nhập sai, khóa thử đăng nhập tạm thời trong phiên.
- Trên giao diện chỉ hiển thị nút theo vai trò; repository **kiểm tra quyền một lần nữa khi thực hiện thao tác**.
- Không sửa/xóa đơn đã xét duyệt, không xét duyệt lại đơn đã xử lý; từ chối cần ghi lý do.
- Kiểm tra học viên có trong danh mục, tính hợp lệ của khoảng thời gian, độ dài dữ liệu; chống chèn HTML khi hiển thị bằng khả năng mã hóa đầu ra mặc định của Razor.
- Thao tác được ghi vào nhật ký; việc lưu file JSON dùng tệp tạm và đổi tên để tránh tệp bị ghi dở.

**Lưu ý quan trọng:** các lớp bảo vệ trên nhằm minh họa cho tiêu chí bài tập, **không phải hệ thống xác thực và an toàn thông tin đủ điều kiện triển khai thật**. Tài khoản demo và phiên Blazor không thay thế xác thực server/cookie, phân quyền thực tế, HTTPS được cấu hình đúng, bảo vệ thông tin cá nhân hoặc database được quản trị.

## 6. Tư duy sáng tạo (2 điểm)

- Đăng nhập theo ba vai trò, tự động điều chỉnh quyền và giao diện.
- Xem trước biểu mẫu giấy theo ảnh được cung cấp, in từ trang web.
- Loại đơn, thời lượng nghỉ và thống kê tính tự động.
- Lưu JSON qua lần chạy sau, nhật ký thao tác, thông báo và cửa sổ xác nhận.
- Xuất `.xlsx` độc lập với thư viện NuGet ngoài.
