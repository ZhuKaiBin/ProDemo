﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProJWT
{
    class Program
    {

        static string Jwt_Issuer = "Jwt:Issuer";
        static string Jwt_Key = "Jwt:Key11111111111111111111111111111111111111111111111111111111111111111111111111111111";
        static string Jwt_ExpiresInMinutes = "60";
        static string Jwt_Audience = "Audience";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var token = GenerateJwtToken(10086);

            var bools = ValidateJwtToken(token);

        }


        private static string GenerateJwtToken(int userId)
        {
            // 从配置文件中获取JWT密钥和有效期
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt_Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(Jwt_ExpiresInMinutes));

            // 生成JWT令牌
            var token = new JwtSecurityToken(
                issuer: Jwt_Issuer,
                audience: Jwt_Audience,
                claims: new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) },
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateJwtToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(Jwt_Key);

                // 配置令牌验证参数
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Jwt_Issuer,
                    ValidAudience = Jwt_Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // 验证令牌
                SecurityToken validatedToken;

                JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

                var principal = _tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                var vatoken= ((System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken).RawData;

                return token.Equals(((System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken).RawData);
            }
            catch
            {
                return false;
            }
        }
    }
}