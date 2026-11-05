# HỆ THỐNG QUẢN LÝ HỌC VIÊN NGHỈ PHÉP

**Bài tập lớn — Blazor Web App / .NET 8 / Visual Studio 2022**

> Đây là dự án minh họa cho học tập, không phải trang web thật của Học viện và không được sử dụng để xử lý hồ sơ thật hoặc dữ liệu cá nhân nhạy cảm.

## 1. Mở project bằng Visual Studio 2022

1. Giải nén toàn bộ tệp ZIP vào một thư mục, ví dụ `D:\BaiTapLon\QuanLyNghiPhepHV`.
2. Dùng **Visual Studio 2022 phiên bản 17.8 trở lên**; cài workload **ASP.NET and web development** và **.NET 8 SDK**.
3. Chọn **File → Open → Project/Solution** và mở `QuanLyNghiPhepHV.sln` (hoặc `.csproj`).
4. Trên thanh công cụ chọn profile **https** hoặc **http**, nhấn **F5** hoặc **Ctrl+F5**.
5. Trình duyệt sẽ mở trang đăng nhập trên `https://localhost:7187` hoặc `http://localhost:5187` (cổng có thể thay đổi nếu thiết bị đang sử dụng cổng đó).
6. Lần đầu chạy, chương trình tự sinh dữ liệu mẫu. Không cần SQL Server, không phải cài NuGet bên ngoài.

Có thể chạy bằng terminal với `dotnet run --project QuanLyNghiPhepHV.csproj --launch-profile http` nếu đã cài .NET 8 SDK.

## 2. Tài khoản dùng thử

| Vai trò | Tài khoản | Mật khẩu |
|---|---|---|
| Giáo viên trung đội | `trungdoi` | `Demo@2026` |
| Cán bộ Phòng QLHV | `phongqlhv` | `Demo@2026` |
| Giáo viên bộ môn | `bomon` | `Demo@2026` |

**Phải chọn đúng chức vụ khi đăng nhập.** Có giới hạn 5 lần nhập sai, tạm khóa phiên đăng nhập 1 phút.

### Quyền theo vai trò

- **Giáo viên trung đội:** nhập đơn, xem trước mẫu đơn, thêm/sửa/xóa đơn đang chờ duyệt, tìm kiếm/lọc, in và xuất Excel.
- **Cán bộ Phòng QLHV:** xem danh sách, mở chi tiết đơn, duyệt/từ chối (từ chối phải nêu lý do), xem báo cáo và xuất Excel; không thêm/sửa/xóa.
- **Giáo viên bộ môn:** xem danh sách và hồ sơ, tìm kiếm, không được thay đổi dữ liệu và không được xét duyệt.

Lưu ý: đây là **đăng nhập mô phỏng trong phiên Blazor**, không phải xác thực đạt chuẩn triển khai thực tế. Nếu sử dụng trên mạng thật, cần thay bằng ASP.NET Core Identity/cookie authentication, phân quyền server đầy đủ, HTTPS, cơ sở dữ liệu và quản trị tài khoản.

## 3. Tính năng và cách dùng

1. Đăng nhập tài khoản `trungdoi` và chọn vai trò “Giáo viên trung đội”.
2. Mở **Quản lý đơn nghỉ**: phần trên là biểu mẫu nhập đơn dựa theo tờ giấy nghỉ phép; phần dưới là danh sách.
3. Chọn học viên, nhập trung đội, thời gian bắt đầu/kết thúc, lý do và địa chỉ nơi nghỉ. Loại đơn **Trong ngày** hoặc **Dài ngày** được tự tính bằng cách đối chiếu ngày bắt đầu và ngày kết thúc.
4. **Xem mẫu đơn** hiển thị cửa sổ nổi theo biểu mẫu giấy; **Gửi đơn nghỉ phép** thêm đơn ở trạng thái **Chờ duyệt**.
5. Bấm **◉** để xem nội dung/in mẫu; **✎** để sửa và nút xóa để xóa đơn **chưa được duyệt**. Đơn đã duyệt hoặc bị từ chối không cho sửa/xóa.
6. Tìm kiếm theo mã đơn, mã học viên, họ tên, lý do; lọc lớp, loại đơn, trạng thái. **Reset** khôi phục danh sách ban đầu.
7. Đăng xuất, đăng nhập bằng `phongqlhv` + đúng vai trò. Mở đơn chờ duyệt rồi nhấn **Duyệt đơn** hoặc **Từ chối** (kèm lý do).
8. Nhấn **Xuất Excel** để nhận tệp `.xlsx` chứa danh sách hiện tại. Trong tab **Quản lý đơn nghỉ**, file Excel phản ánh bộ lọc đang chọn.
9. Tab **Tổng quan**, **Thống kê báo cáo** và **Nhật ký xử lý** hỗ trợ theo dõi công việc.

## 4. Dữ liệu lưu ở đâu?

- File JSON `AppData/du-lieu-nghi-phep.json` được tạo tự động **trong thư mục project** khi chạy lần đầu.
- Dữ liệu được lưu sau khi thêm, sửa, xóa và duyệt; mở lại chương trình vẫn giữ dữ liệu đã thay đổi.
- Muốn khôi phục dữ liệu mẫu ban đầu, hãy **dừng ứng dụng**, sao lưu rồi xóa file JSON đó, sau đó chạy lại.
- Mọi tên và mã học viên trong dự án đều là **dữ liệu giả lập**.

## 5. Cấu trúc project

```text
QuanLyNghiPhepHV.sln
QuanLyNghiPhepHV.csproj
Program.cs
Components/
  App.razor
  Routes.razor
  _Imports.razor
  Layout/MainLayout.razor
  Pages/Home.razor
  Pages/Error.razor
  LeaveDataTable.razor
  RecentLeaveTable.razor
Models/AppModels.cs
Services/
  DemoAuthService.cs
  LeaveRepository.cs
  ExcelExporter.cs
wwwroot/
  css/site.css
  js/app.js
Properties/launchSettings.json
appsettings.json
README.md
PHAN_TICH_BAI_TOAN.md
```

## 6. Hạn chế được công khai

- Tài khoản đăng nhập là **tài khoản dùng thử**, chưa có ASP.NET Identity, quản lý phiên/cookie hay phân quyền theo danh tính đã xác thực trên HTTP server. Việc phân quyền và kiểm tra quyền tại repository nhằm minh họa cách tổ chức nghiệp vụ.
- Dùng file JSON, phù hợp đồ án demo và một máy chạy thử; **không phù hợp hệ thống nhiều máy/phòng ban sử dụng đồng thời** với yêu cầu độ tin cậy cao.
- Không có ký số/chữ ký thật, không có gửi email/duyệt đa cấp; mẫu giấy là bản xem trước HTML và in trình duyệt.
- Biểu mẫu xây dựng theo ảnh tham khảo người học cung cấp; cần đối chiếu văn bản chính thức trước khi áp dụng thực tế.
- Chưa thể chạy lệnh build bằng SDK trong môi trường tạo file vì không có `dotnet` được cài đặt. Hãy build bằng Visual Studio 2022 theo mục 1 và phản hồi lỗi nếu phát sinh.

## Dua len web (Cloudflare Tunnel hoac Docker)

- Cong chay that: **7187** (http, khong phai https). Cloudflare Tunnel: Service URL = `http://localhost:7187`.
- Chay tren may Windows: chay `1-publish.bat` (build vao `C:\nghiphep\publish`), roi `2-chay-web.bat`.
  Du lieu nam o `C:\nghiphep\publish\AppData`, nho sao luu thu muc nay.
- Chay bang Docker: `docker compose up -d --build` (cong host 127.0.0.1:7187, du lieu o `./data/AppData`).
- Khong chay dong thoi profile https cua Visual Studio (cung cong 7187) voi `2-chay-web.bat`.
- Doi ten mien: sua `AllowedHosts` trong `2-chay-web.bat` va `docker-compose.yml`.
