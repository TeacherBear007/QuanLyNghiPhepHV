# ===== Giai đoạn 1: BUILD (xưởng) - có đủ công cụ để biên dịch, khá nặng =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Chỉ chép file .csproj trước rồi restore (tải thư viện).
# Nhờ vậy lần build sau, nếu chỉ sửa code mà không đổi .csproj thì Docker dùng lại bước này (nhanh hơn).
COPY QuanLyNghiPhepHV.csproj ./
RUN dotnet restore QuanLyNghiPhepHV.csproj

# Chép toàn bộ source còn lại (bin, obj đã bị loại bởi .dockerignore)
COPY . .

# Biên dịch C# thành QuanLyNghiPhepHV.dll và gom mọi thứ cần để chạy vào /app/publish.
# Ghi rõ tên .csproj vì trong thư mục còn có cả file .sln.
RUN dotnet publish QuanLyNghiPhepHV.csproj -c Release -o /app/publish

# ===== Giai đoạn 2: RUN (cửa hàng) - chỉ có runtime để chạy, nhẹ hơn nhiều =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Lấy kết quả đã build từ giai đoạn 1 (giai đoạn 1 không nằm trong ảnh cuối)
COPY --from=build /app/publish .

# Biến môi trường:
#  - ASPNETCORE_URLS: app lắng nghe cổng 8080 trên mọi địa chỉ trong container
#  - TZ: múi giờ Việt Nam (để DateTime.Now ra đúng giờ)
ENV ASPNETCORE_URLS=http://+:8080 \
    TZ=Asia/Ho_Chi_Minh

EXPOSE 8080

# Lệnh chạy khi container khởi động
ENTRYPOINT ["dotnet", "QuanLyNghiPhepHV.dll"]
