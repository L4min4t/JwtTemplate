using App.Entities;
using App.Models.Token;

namespace App.Services.Interfaces;

public interface IJwtService
{
    Task<TokenModel> GenerateTokenPairAsync(AuthUser user);
    Task<RefreshTokenResponseModel> RefreshTokenAsync(RefreshTokenModel tokenModel);
}
