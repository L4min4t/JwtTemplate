using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Entities;
using App.Models.Token;
using App.Options;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App.Services;

public class JwtService : IJwtService
{
    protected readonly JwtBearerOptions _jwtBearerOptions;
    protected readonly JwtOptions _jwtOptions;
    protected readonly UserManager<AuthUser> _userManager;
    private readonly int AccessTokenLifeTimeInMinutes = 60 * 2;
    private readonly int RefreshTokenLifeTimeInMinutes = 60 * 24 * 7;
    
    public JwtService
    (
        UserManager<AuthUser> userManager,
        IOptions<JwtOptions> jwtOptions,
        IOptions<JwtBearerOptions> jwtBearerOptions
    )
    {
        _userManager = userManager;
        _jwtBearerOptions = jwtBearerOptions.Value;
        _jwtOptions = jwtOptions.Value;
    }
    
    public async Task<TokenModel> GenerateTokenPairAsync(AuthUser user)
    {
        var claims = await GenerateUserClaims(user);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(AccessTokenLifeTimeInMinutes),
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
            ),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };
        
        var securityToken = tokenHandler.CreateToken(descriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);
        var refreshToken = Guid.NewGuid()
            .ToString();
        
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(RefreshTokenLifeTimeInMinutes);
        user.RefreshToken = refreshToken;
        await _userManager.UpdateAsync(user);
        
        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    public async Task<RefreshTokenResponseModel> RefreshTokenAsync(RefreshTokenModel tokenModel)
    {
        var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
        
        if (principal?.FindFirstValue(ClaimTypes.Email) is null)
            return new RefreshTokenResponseModel
            {
                IsSuccessed = false,
                Errors = new[]
                {
                    "The provided token is not valid!"
                }
            };
        
        var user = await _userManager.FindByEmailAsync(principal.FindFirstValue(ClaimTypes.Email)!);
        
        if (user is null)
            return new RefreshTokenResponseModel
            {
                IsSuccessed = false,
                Errors = new[]
                {
                    $"The user with email {principal.FindFirstValue(ClaimTypes.Email)!} was not found!"
                }
            };
        
        if (user.RefreshToken != tokenModel.RefreshToken)
            return new RefreshTokenResponseModel
            {
                IsSuccessed = false,
                Errors = new[]
                {
                    "The provided refresh token is not valid."
                }
            };
        
        if (user.RefreshTokenExpiryTime <= DateTime.Now)
            return new RefreshTokenResponseModel
            {
                IsSuccessed = false,
                Errors = new[]
                {
                    "The provided refresh token is expired."
                }
            };
        
        var newTokens = await GenerateTokenPairAsync(user);
        return new RefreshTokenResponseModel
        {
            IsSuccessed = true,
            AccessToken = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken
        };
    }
    
    private async Task<List<Claim>> GenerateUserClaims(AuthUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        return claims;
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken
            (token, _jwtBearerOptions.TokenValidationParameters, out var securityToken);
        
        return CheckSecurityToken(securityToken)
            ? principal
            : null;
    }
    
    private static bool CheckSecurityToken(SecurityToken securityToken)
    {
        return securityToken is JwtSecurityToken jwtSecurityToken &&
               jwtSecurityToken.Header.Alg.Equals
                   (SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
