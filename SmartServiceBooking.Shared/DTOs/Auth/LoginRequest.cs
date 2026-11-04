using System.ComponentModel.DataAnnotations;

namespace SmartServiceBooking.Shared.DTOs.Auth;

/// <summary>
/// Request body sent from Blazor Client → POST /api/auth/login
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = true;
}
