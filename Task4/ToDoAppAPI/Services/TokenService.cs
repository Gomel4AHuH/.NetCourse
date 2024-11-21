using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoAppAPI.Dtos.Token;
using ToDoAppAPI.Interfaces;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Services
{
    public class TokenService(IConfiguration configuration, UserManager<Employee> userManager) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<Employee> _userManager = userManager;

        public async Task<TokenDto> CreateToken(string email, bool populateExp)
        {
            Employee employee = await _userManager.FindByEmailAsync(email.ToLower());

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, employee.Id)
            ];

            var jwtSettings = _configuration.GetSection("JwtSettings");

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);
            
            SecurityTokenDescriptor tokenOptions = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiryInMinutes"])),
                SigningCredentials = creds,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var refreshToken = GenerateRefreshToken();

            employee.RefreshToken = refreshToken;

            if (populateExp) 
            {
                employee.RefreshTokenExpirationDate = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(employee);

            SecurityToken token = new JwtSecurityTokenHandler().CreateToken(tokenOptions);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            ClaimsPrincipal claimsPrincipal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

            Employee employee = await _userManager.FindByNameAsync(claimsPrincipal.Identity.Name);

            if (employee is null || !(string.Compare(employee.RefreshToken, tokenDto.RefreshToken) == 0) || employee.RefreshTokenExpirationDate <= DateTime.Now)
            {
                return null;
            }

            return await CreateToken(employee.Email, false);
        }

        public void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", tokenDto.AccessToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:expiryInMinutes"])),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            context.Response.Cookies.Append("refreshToken", tokenDto.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }

        public void DeleteTokensInsideCookie(HttpContext context)
        {
            context.Response.Cookies.Delete("accessToken");

            context.Response.Cookies.Delete("refreshToken");
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null || jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }
    }
}
