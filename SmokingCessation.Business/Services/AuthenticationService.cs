using AutoMapper;
using SmokingCessation.Business.DTOs;
using SmokingCessation.Business.Interfaces;
using SmokingCessation.Data.Entities;
using SmokingCessation.Data.Repositories;
using SmokingCessation.Common.Constants;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SmokingCessation.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _secretKey;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _secretKey = "YourSecretKeyHere_MakeItLongAndSecure_AtLeast256Bits"; // Should be from configuration
        }

        public async Task<(bool Success, string? Token, UserDto? User)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
                if (user == null || !user.IsActive)
                {
                    return (false, null, null);
                }

                // Verify password
                if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return (false, null, null);
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                // Generate token
                var token = GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDto>(user);

                return (true, token, userDto);
            }
            catch (Exception)
            {
                return (false, null, null);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Validate input
                if (registerDto.Password != registerDto.ConfirmPassword)
                {
                    return (false, "Passwords do not match");
                }

                if (registerDto.Password.Length < ApplicationConstants.MinPasswordLength)
                {
                    return (false, $"Password must be at least {ApplicationConstants.MinPasswordLength} characters long");
                }

                // Check if email already exists
                if (await _unitOfWork.Users.EmailExistsAsync(registerDto.Email))
                {
                    return (false, "Email address already exists");
                }

                // Check if username already exists
                if (await _unitOfWork.Users.UsernameExistsAsync(registerDto.Username))
                {
                    return (false, "Username already exists");
                }

                // Create new user
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = HashPassword(registerDto.Password),
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    DateOfBirth = registerDto.DateOfBirth,
                    Phone = registerDto.Phone,
                    Role = registerDto.Role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task LogoutAsync(int userId)
        {
            // In a real application, you might want to invalidate the token
            // For now, we'll just update the last activity time
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                // Could implement token blacklisting here
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                {
                    return (false, "User not found");
                }

                if (!VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return (false, "Current password is incorrect");
                }

                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
                {
                    return (false, "New passwords do not match");
                }

                if (changePasswordDto.NewPassword.Length < ApplicationConstants.MinPasswordLength)
                {
                    return (false, $"Password must be at least {ApplicationConstants.MinPasswordLength} characters long");
                }

                user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto?> GetCurrentUserAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(token);
                
                var userIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "userId");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return null;
                }

                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                return user != null ? _mapper.Map<UserDto>(user) : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null)
                {
                    // Don't reveal if email exists or not for security
                    return (true, null);
                }

                // Generate random password
                var newPassword = GenerateRandomPassword();
                user.PasswordHash = HashPassword(newPassword);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                // In a real application, you would send the new password via email
                // For now, we'll just return success
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _unitOfWork.Users.EmailExistsAsync(email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _unitOfWork.Users.UsernameExistsAsync(username);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "YourSaltHere"));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}