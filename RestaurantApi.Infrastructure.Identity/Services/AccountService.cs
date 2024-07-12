using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RestaurantApi.Core.Application.Dtos.Account;
using RestaurantApi.Core.Application.Enums;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Domain.Settings;
using RestaurantApi.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace RestaurantApi.Infrastructure.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JWTSettings _jwtSettings;
    
    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
    {
        AuthResponse authResponse = new AuthResponse();

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            authResponse.HasError = true;
            authResponse.Error = $"There aren't any accounts registered with this email: {request.Email}";
            return authResponse;
        }
        
        
        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            authResponse.HasError = true;
            authResponse.Error = $"Wrong credentials for email: {request.Email}";
            return authResponse;
        }
        
        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

        authResponse.Id = user.Id;
        authResponse.Email = user.Email;
        authResponse.UserName = user.UserName;

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

        authResponse.Roles = rolesList.ToList();
        authResponse.IsVerified = true;
        authResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        var refreshToken = GenerateRefreshToken();
        authResponse.RefreshToken = refreshToken.Token;

        return authResponse;
    }

    public async Task<RegisterResponse> Register(RegisterRequest request, string role)
    {
        RegisterResponse registerResponse = new RegisterResponse
        {
            HasError = false
        };

        if (request.Password != request.ConfirmPassword)
        {
            registerResponse.HasError = true;
            registerResponse.Error = "Password and ConfirmPassword fields should be identical.";
            return registerResponse;
        }
        
        var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            registerResponse.HasError = true;
            registerResponse.Error = $"The following username: '{request.UserName}' already exists.";
            return registerResponse;
        }

        var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail != null)
        {
            registerResponse.HasError = true;
            registerResponse.Error = $"The following email: '{request.Email}' is already registered.";
            return registerResponse;
        }
        
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            if (role == "Admin")
            {
                await _userManager.AddToRoleAsync(user, Roles.Administrador.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(user, Roles.Mesero.ToString());
            }

        }
        else
        {
            registerResponse.HasError = true;
            registerResponse.Error = "A wild error appeared! Please try again later.";
            return registerResponse;
        }

        return registerResponse;
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    
    
    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        foreach(string role in roles)
        {
            roleClaims.Add(new Claim("roles", role));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredetials);

        return jwtSecurityToken;
    }
    private RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
    }
    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var ramdomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(ramdomBytes);

        return BitConverter.ToString(ramdomBytes).Replace("-", "");
    }

    //private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
    //{
    //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
    //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //    var route = "User/ResetPassword";
    //    var Uri = new Uri(string.Concat($"{origin}/", route));
    //    var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

    //    return verificationUri;
    //}
}