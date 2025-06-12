using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommonShared;
using Microsoft.AspNetCore.SignalR;


namespace AuthServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapPost("/token", ([FromBody] TokenRequest request) =>
        {
            if (!AuthOptions.Audiences.Contains(request.Audience))
                return Results.BadRequest("Invalid audience");


           //Access Token 就是一张快递单，包裹里是用户的权限信息。
           //sub 就像是 收件人栏，写着：这个包裹是“发给谁的”。

            var claims = new[]
            {
                //sub 是系统内部用的稳定唯一ID;sub 是唯一标识一个用户的 ID（通常是数据库里的 UserId、GUID、手机号、OpenID等）。
                //sub = Subject = “这个 token 是颁发给谁的”;表示这个 token 是发给 user123 这个“人”的，系统通过 sub 知道这个 token 属于谁。
                  new Claim(JwtRegisteredClaimNames.Sub, "user123"),


                //jti = JWT ID
                //它的作用是：为这个 Token 提供一个唯一的标识符;每次都是一个全新的值
                //jti 就是这一张发票/快递单的编号，全系统唯一。
                //不同的 token，即使发给同一个人（同一个 sub），也应该有不同的 jti
                //
                //【防重放攻击（Replay Attack）】:
                //如果攻击者截获了某个 token，试图重复使用它——服务器可以通过查重 jti 来拒绝它。
                //搭配 Redis/数据库存储 jti 用来判断 Token 是否已经被吊销、黑名单等。
                //【Token 追踪、审计】:
                //记录用户使用了哪个 Token 访问了什么资源。
               //【Token 吊销（Blacklist）】:
                //某个 Token 被标记为失效，可根据 jti 将它列入黑名单。
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Iss, AuthOptions.Issuer),
                  new Claim(JwtRegisteredClaimNames.Aud, request.Audience),


                  new Claim(ClaimTypes.Role, "SuperAdmin")  // 👈 添加角色 claim
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: request.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        });

        app.Run();
    }
}

public class TokenRequest
{ 
    public required string Audience { set; get; }
}
