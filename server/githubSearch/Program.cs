using githubSearch.Models;
using githubSearch.Sessions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string CORSOpenPolicy = "OpenCORSPolicy";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache(); // Use a distributed cache implementation suitable for your production environment
builder.Services.AddSession(options =>
{
    // Configure session options as needed
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder =>
      {
          builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
      });
});

var app = builder.Build();

// authenticate use with jwt token based on custom session
app.MapGet("/github/token", [AllowAnonymous] async (context) =>
{
    var token = context.Request.Headers["Authorization"]
                            .FirstOrDefault()?.Split(" ").Last();

    // Check if the user is already authenticated
    if (context.User.Identity.IsAuthenticated)
    {
        // If authenticated, retrieve the existing token from the session
        var existingToken = Sessions.GetString(token);
        if (existingToken != null)
        {
            await Results.Ok(existingToken.Token).ExecuteAsync(context);
            return;
        }
    }

    // If not authenticated or no existing token, generate a new token
    var issuer = builder.Configuration["Jwt:Issuer"];
    var audience = builder.Configuration["Jwt:Audience"];
    var key = Encoding.ASCII.GetBytes
    (builder.Configuration["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
        Expires = DateTime.UtcNow.AddMinutes(5),
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials
        (new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var createtoken = tokenHandler.CreateToken(tokenDescriptor);
    token = tokenHandler.WriteToken(createtoken);
    // Store the new token in the session
    Sessions.SetString(token);

    await Results.Ok(token).ExecuteAsync(context);

});



app.UseAuthentication();
app.UseAuthorization();
app.UseCors(CORSOpenPolicy);
app.UseSession();

app.MapControllers();

app.Run();
