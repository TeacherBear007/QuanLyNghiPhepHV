using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using QuanLyNghiPhepHV.Components;
using QuanLyNghiPhepHV.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<LeaveRepository>();
builder.Services.AddScoped<DemoSession>();
builder.Services.AddScoped<DemoAuthService>();

// [MỚI] App chạy sau nginx (và Cloudflare) nên phải đọc các header X-Forwarded-*
// để biết người dùng thật đang vào bằng https và IP thật là gì.
// Xóa KnownNetworks/KnownProxies là an toàn vì compose chỉ mở port ở 127.0.0.1
// (chỉ nginx trên cùng máy mới gọi được vào app).
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// [MỚI] Lưu khóa mã hóa (cookie, antiforgery) ra thư mục có volume.
// Không có dòng này thì mỗi lần container khởi động lại sẽ mất khóa -> lỗi khi gửi form.
// Chỉ bật khi chạy thật (Production); chạy F5 trên máy mình thì bỏ qua.
if (!builder.Environment.IsDevelopment())
{
    var keyPath = builder.Configuration["DataProtection:KeyPath"] ?? "/app/keys";
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(keyPath))
        .SetApplicationName("QuanLyNghiPhepHV");
}

var app = builder.Build();

// [MỚI] Phải đặt ngay sau Build(), trước các middleware khác.
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // [SỬA] Chỉ chuyển http -> https khi chạy trên máy mình.
    // Ở server, nginx/Cloudflare đã lo HTTPS rồi, app chỉ nhận http nội bộ.
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
