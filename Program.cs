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

// [THÊM 1] Đăng ký dịch vụ Health Checks để tạo endpoint kiểm tra trạng thái app
builder.Services.AddHealthChecks();

// [MỚI] App chạy sau nginx (và Cloudflare) nên phải đọc các header X-Forwarded-*
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// [MỚI] Lưu khóa mã hóa (cookie, antiforgery) ra thư mục có volume.
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
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

// [THÊM 2] Map đường dẫn /health để trả về HTTP 200 OK khi kiểm tra
app.MapHealthChecks("/health");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
