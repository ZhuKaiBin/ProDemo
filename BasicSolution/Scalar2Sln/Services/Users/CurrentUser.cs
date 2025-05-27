using Microsoft.AspNetCore.Http;
using Scalar2Sln_Application.Common.Interfaces;
using System.Security.Claims;
namespace Scalar2Sln.Services.Users
{
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
