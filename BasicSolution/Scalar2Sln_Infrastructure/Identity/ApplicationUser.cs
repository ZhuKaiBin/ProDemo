﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {

        public string? NickName { set; get; }

    }
}
