using Scalar2Sln_Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Interfaces
{
    public interface IIdentityService
    {

        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId,string role);

        Task<bool> AuthorizePolicyAsync(string usrtId,string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);


        Task<Result> DeleteUserAsync(string userId);



    }
}
