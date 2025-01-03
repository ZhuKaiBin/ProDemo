using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SessionServerDemo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SessionManager>();
        }

        public void Configure(IApplicationBuilder app, SessionManager sessionManager)
        {
            app.UseRouting();

            // 登录接口
            app.Map("/login", loginApp =>
            {
                loginApp.Run(async context =>
                {
                    string username = context.Request.Query["username"];
                    string password = context.Request.Query["password"];

                    // 简单验证，模拟登录成功
                    if (username == "testuser" && password == "password123")
                    {
                        var sessionId = sessionManager.GenerateSessionId();
                        sessionManager.StoreSession(sessionId, username);
                        context.Response.Cookies.Append("SessionId", sessionId);
                        await context.Response.WriteAsync("Login successful. Session set.");
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid credentials.");
                    }
                });
            });

            // 需要验证 session 的接口
            app.Map("/protected", protectedApp =>
            {
                protectedApp.Run(async context =>
                {
                    var sessionId = context.Request.Cookies["SessionId"];

                    if (sessionManager.ValidateSession(sessionId))
                    {
                        var username = sessionManager.GetUsernameBySession(sessionId);
                        await context.Response.WriteAsync($"Welcome {username}, your session is valid.");
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid session.");
                    }
                });
            });
        }
    }

    public class SessionManager
    {
        private Dictionary<string, string> sessions = new Dictionary<string, string>();

        public string GenerateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public void StoreSession(string sessionId, string username)
        {
            sessions[sessionId] = username;
        }

        public bool ValidateSession(string sessionId)
        {
            return sessions.ContainsKey(sessionId);
        }

        public string GetUsernameBySession(string sessionId)
        {
            return sessions.ContainsKey(sessionId) ? sessions[sessionId] : null;
        }
    }

}
