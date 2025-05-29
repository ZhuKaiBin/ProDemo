using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Infrastructure.Identity
{
    /// <summary>
    /// 定义权限，这里是说对角色的权限控制，比如一个主席角色，可以有查看、创建、更新和删除的权限
    /// </summary>
    public static class Permissions
    {
        public const string View = "Permission.View";
        public const string Create = "Permission.Create";
        public const string Update = "Permission.Update";
        public const string Delete = "Permission.Delete";

        public static List<string> All => new() { View, Create, Update, Delete };
    }

    public static class CustomClaimTypes
    {
        public static string Permission => nameof(Permission);
    }
}
