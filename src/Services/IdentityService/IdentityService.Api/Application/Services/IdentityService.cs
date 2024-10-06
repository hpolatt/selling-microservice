using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;


namespace IdentityService.Api.Application.Services;

public class IdentityService : IIdentityService
{

    private readonly IConfiguration configuration;

    public IdentityService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public Task<LoginResponseModel> Login(LoginRequestModel model)
    {
        // Db process

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, model.UserName),
            new Claim(ClaimTypes.Name, "Huseyin Polat"),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(20);

        var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: credential, notBefore: DateTime.Now);

        string encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        LoginResponseModel response = new LoginResponseModel
        {
            Token = encodedJwtToken,
            UserName = model.UserName,
        };

        return Task.FromResult(response);


    }
}
