using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Attributes.Security
{

    //这个特性只能用在类上。允许你在同一个类上多次使用这个特性。允许这个特性被继承，也就是说，如果一个类继承了带这个特性的父类，子类也会自动拥有这个特性。
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute: Attribute
    {

        public AuthorizeAttribute() { }

        public string Roles { set; get; } = string.Empty;


        public string Policy { set; get; } = string.Empty;

    }
}
