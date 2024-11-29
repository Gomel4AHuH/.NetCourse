using ToDoAppAPI.Dtos.Token;

namespace ToDoAppAPI.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateTokenAsync(string email, bool populateExp);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
        void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context);
        void DeleteTokensInsideCookie(HttpContext context);
    }
}
