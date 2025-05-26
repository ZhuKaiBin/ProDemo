using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Scalar2Sln_Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {

        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        public AuthorizationBehaviour(
            IUser user,
            IIdentityService identityService)
        {
            _user = user;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //看下这个request是否有AuthorizeAttribute特性
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            //如果包含这个特性，就说明这个类要进行身份验证
            if (authorizeAttributes.Any())
            {

                if (_user.Id ==null)
                {
                    throw new UnauthorizedAccessException();
                }

                //1.验证角色
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
                if (authorizeAttributesWithRoles.Any())
                {

                    var authorized = false;

                    //如果有Roles属性，就验证用户是否有这个角色

                    //如果你有 Roles 类常量，也可以这样写（假设有 Roles.SuperAdministrator）：
                    //[Authorize(Roles = Roles.Administrator + "," + Roles.SuperAdministrator)]
                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        //如果用户有多个角色，就要验证这个用户是否有其中的角色
                        foreach (var role in roles)
                        {
                            var isInRole = await _identityService.IsInRoleAsync(_user.Id, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }

                        // Must be a member of at least one role in roles
                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }
                    }

                }

                //授权策略 是 ASP.NET Core 授权系统中的一个概念，它定义了一组规则，用来判定“这个用户有没有权限做某件事”。
                //它可以包含更复杂的逻辑，不仅仅是“角色验证”，还可以是“某个声明（claim）验证”、“用户年龄是否大于18岁”、“用户必须有某个特定的属性”等。
                //换句话说，授权策略是对用户身份的某种自定义规则的封装。

                //角色（Roles） 就像你公司里的职务头衔（管理员、普通员工、访客）。
                //策略（Policy） 是更细致的“权限规则”，比如“你必须通过安全培训才能操作这台机器”或者“你必须完成背景调查才能查看财务报表”。
                //你虽然是管理员，但如果没满足策略要求，也不一定能执行某个命令。


                // 2.验证策略
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {

                        var authorized = await _identityService.AuthorizePolicyAsync(_user.Id, policy);
                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }

                    }

                }
            }

        }



    }
}
