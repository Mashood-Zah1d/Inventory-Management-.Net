using Azure;
using Inventory_Management_.NET.Data;
using Inventory_Management_.NET.Dtos;
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

        public AccountService(ApplicationDbContext dbContext , IConfiguration config)
        {
            this.dbContext = dbContext;
            this.config = config;
        }

        public async Task<ResponseMessage<User>> AddUserAsync(SignupDto dto)
        {
            if (dto.secretKey != "abc.123")
            {
                return new ResponseMessage<User>
                {
                    Success = false,
                    Message = "The Provided Key Doesnot Matched. Provide The Correct Secret Key To Signup"
                };
            }
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                UserName = dto.UserName,
                Password = hashPassword,
                Email = dto.Email,
                Name = dto.Name,
            };

            var Results = await dbContext.AddAsync(user);
             await dbContext.SaveChangesAsync();

            if (Results == null)
            {
                return new ResponseMessage<User>
                {
                    Success = false,
                    Message= "Error Inserting The User. Try Again!"
                };
            }

            return new ResponseMessage<User>
            {
                Success = true,
                Message = "User Added SuccessFully !!"
            };
        }

        public async Task<ResponseMessage<string>> VerifyUserAsync(LoginDto dto)
        {
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

        private string GenerateJwtToken(User user , string secretKey)
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
    }
}
