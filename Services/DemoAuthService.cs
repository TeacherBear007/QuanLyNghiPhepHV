using System.Security.Cryptography;
using System.Text;
using QuanLyNghiPhepHV.Models;

namespace QuanLyNghiPhepHV.Services;

// ĐĂNG NHẬP MÔ PHỎNG dành riêng cho bài tập (không dùng khi triển khai thật).
// Không có hệ thống Identity/cookie xác thực và mật khẩu là tài khoản mẫu.
public sealed class DemoSession
{
    public string UserName { get; private set; } = "";
    public UserRole Role { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public bool CanManage => IsAuthenticated && Role == UserRole.TrungDoi;
    public bool CanApprove => IsAuthenticated && Role == UserRole.PhongQLHV;
    public bool CanExport => IsAuthenticated && (Role == UserRole.TrungDoi || Role == UserRole.PhongQLHV);
    public string RoleDisplay => RoleLabels.Display(Role);

    internal void SignIn(string name, UserRole role)
    {
        UserName = name;
        Role = role;
        IsAuthenticated = true;
    }

    public void SignOut()
    {
        UserName = "";
        IsAuthenticated = false;
    }
}

public sealed class DemoAuthService
{
    private readonly DemoSession _session;
    private int _failedAttempts;
    private DateTime? _lockedUntil;

    // SHA-256 của mật khẩu demo dùng chung, nhằm không đặt mật khẩu gốc trong code.
    private static readonly byte[] DemoPasswordHash = Convert.FromHexString(
        "6269BDC44514C668EFB1EE9442D68AC81C662367FA36C1883E3027EE197F4036");

    private static readonly Dictionary<string, UserRole> Accounts = new(StringComparer.OrdinalIgnoreCase)
    {
        ["trungdoi"] = UserRole.TrungDoi,
        ["phongqlhv"] = UserRole.PhongQLHV,
        ["bomon"] = UserRole.BoMon
    };

    public DemoAuthService(DemoSession session) => _session = session;

    public bool TryLogin(string username, string password, UserRole requestedRole, out string error)
    {
        error = "";
        if (_lockedUntil.HasValue && DateTime.Now < _lockedUntil.Value)
        {
            error = "Bạn đã nhập sai quá nhiều lần. Vui lòng thử lại sau 1 phút.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            error = "Vui lòng nhập tài khoản và mật khẩu.";
            return false;
        }

        var digest = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        bool passwordCorrect = CryptographicOperations.FixedTimeEquals(digest, DemoPasswordHash);
        bool roleCorrect = Accounts.TryGetValue(username.Trim(), out var realRole)
            && realRole == requestedRole;

        if (!passwordCorrect || !roleCorrect)
        {
            _failedAttempts++;
            if (_failedAttempts >= 5)
            {
                _lockedUntil = DateTime.Now.AddMinutes(1);
                _failedAttempts = 0;
                error = "Đã khóa đăng nhập tạm thời trong 1 phút do nhập sai 5 lần.";
            }
            else
            {
                error = "Tài khoản, mật khẩu hoặc vai trò không đúng.";
            }
            return false;
        }

        _failedAttempts = 0;
        _lockedUntil = null;
        _session.SignIn(username.Trim().ToLowerInvariant(), requestedRole);
        return true;
    }
}
