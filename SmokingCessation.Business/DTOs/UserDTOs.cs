using SmokingCessation.Common.Enums;

namespace SmokingCessation.Business.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? SmokingQuitDate { get; set; }
        public int? CigarettesPerDay { get; set; }
        public decimal? CostPerPack { get; set; }
        public int? CigarettesPerPack { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public int Age => DateOfBirth.HasValue ? 
            DateTime.Now.Year - DateOfBirth.Value.Year - 
            (DateTime.Now.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0) : 0;
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public UserRole Role { get; set; } = UserRole.Member;
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? SmokingQuitDate { get; set; }
        public int? CigarettesPerDay { get; set; }
        public decimal? CostPerPack { get; set; }
        public int? CigarettesPerPack { get; set; }
    }
}