using Azure;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
using Inventory_Management_.NET.Models;
using Inventory_Management_.NET.Models.Entities;
using Inventory_Management_.NET.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Inventory_Management_.NET.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration config;
        private readonly EmailService emailService;

        public AccountService(ApplicationDbContext dbContext, IConfiguration config, EmailService emailService)
        {
            this.dbContext = dbContext;
            this.config = config;
            this.emailService = emailService;
        }

        private string GenerateJwtToken(User user, string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResponseMessage<User>> AddUserAsync(SignupViewModel model)
        {
            if (model.secretKey != "abc.123")
            {
                return new ResponseMessage<User>
                {
                    Success = false,
                    Message = "Invalid secret key"
                };
            }

            bool emailExists = await dbContext.Users
                .AnyAsync(u => u.Email == model.Email);

            if (emailExists)
            {
                return new ResponseMessage<User>
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return new ResponseMessage<User>
            {
                Success = true,
                Message = "User created successfully"
            };
        }

        public async Task<ResponseMessage<string>> VerifyUserAsync(LoginViewModel model)
        {
            var dto = new LoginDto
            {
                Password = model.Password,
                UserName = model.UserName,
            };

            var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null)
            {
                return new ResponseMessage<string>
                {
                    Success = false,
                    Message = "User Not Found! Enter Correct User Name!"
                };
            }

            var isMatch = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (isMatch == false)
            {
                return new ResponseMessage<string>
                {
                    Success = false,
                    Message = "Wrong Password! Please use Correct Password"
                };
            }

            var token = GenerateJwtToken(user, config["Jwt:Key"]);

            return new ResponseMessage<string>
            {
                Success = true,
                Message = "User Logged In Successfully!",
                Data = token
            };
        }


        public async Task<ResponseMessage<bool>> SendPasswordResetEmailAsync(ForgotPasswordDto dto)
        {



            var emailDto = new EmailDto
            {
                To = dto.Email,
                Subject = "Password Reset Request",
                Body = $@"
                <h3>Password Reset Request</h3>
                <p>Hello,</p>
                <p>Use the code below to reset your password:</p>
                <h2 style='color:blue;'>{dto.Code}</h2>
                <p>If you didn't request a password reset, please ignore this email.</p>
                <br/>
                <p>Thanks,<br/>Inventory Hub</p>"
            };

            await emailService.SendEmailAsync(emailDto);

            return new ResponseMessage<Boolean>
            {
                Success = true,
                Message = "Password reset email sent successfully!",
                Data = true
            };
        }

        public async Task<ResponseMessage<User>> GetUserByEmailAsync(string email)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new ResponseMessage<User>
                {
                    Success=false,
                    Message="User Not Found ! Email Is Wrong"
                };
            }

            return new ResponseMessage<User>
            {
                Success = true,
                Message = "User Found Successfully",
                Data = user
            };
        }

        public async Task SaveResetCodeAsync(string email, string code)
        {
            var existing = await dbContext.ForgotPassword
                                   .FirstOrDefaultAsync(x => x.Email == email);

            if (existing != null)
            {
                existing.Code = code;
                existing.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
            }
            else
            {
                dbContext.ForgotPassword.Add(new ForgotPassword
                {
                    Email = email,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                });
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyResetCodeAsync(string email, string userCode)
        {
            var record = await dbContext.ForgotPassword
                                 .FirstOrDefaultAsync(x => x.Email == email);

            if (record == null)
                return false;

            if (record.ExpiresAt < DateTime.UtcNow)
                return false;

            if (record.Code != userCode)
                return false;

            dbContext.ForgotPassword.Remove(record);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ResponseMessage<User>> UpdatePasswordAsync(setNewPasswordViewModel model)
        {
            var email = model.Email;
            var newPassword = model.NewPassword;
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return new ResponseMessage<User>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await dbContext.SaveChangesAsync();

            return new ResponseMessage<User>
            {
                Success = true,
                Message = "Password updated successfully."
            };
        }


    }
}
