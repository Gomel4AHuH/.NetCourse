using ToDoAppAPI.Dtos.Employee;
using ToDoAppAPI.Dtos.Token;

namespace ToDoAppAPI.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(string email, bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
        void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context);
        void DeleteTokensInsideCookie(HttpContext context);
    }
}
