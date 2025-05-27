using Microsoft.AspNetCore.Identity;
using Scalar2Sln_Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {

        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }

    }
}
