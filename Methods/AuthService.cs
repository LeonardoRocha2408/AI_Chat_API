using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ChatBot.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatBot.Methods
{
    public class AuthService
    {
        private readonly PasswordHasher<UserEntity> _hasher = new PasswordHasher<UserEntity>();
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HashPassword(UserEntity user, string Password)
        {
            return _hasher.HashPassword(user, Password);
        }

        public PasswordVerificationResult VerityPassword(UserEntity user, string hashPassword, string Password)
        {
            return _hasher.VerifyHashedPassword(user, hashPassword, Password);
        }

        public string GenerateToken(UserEntity user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            string? keyValue = _configuration["Jwt:Key"];
            if (keyValue == null)
            {
                throw new Exception("The Jwt:Key key is not configured");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);
            return jwt;

        }
    }
}
