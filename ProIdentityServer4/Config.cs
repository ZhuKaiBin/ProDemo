using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProIdentityServer4
{
    public static class Config
    {
        //APi范围  ApiScope  分组
        //
        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
             new ApiScope{
              Name="sample_api",//Name  APi范围的一个名称
              DisplayName="Sample API",//api名称的一个描述
             },
        };



        //客户端的创建
        public static IEnumerable<Client> clients => new[]
        {
             new Client{
              ClientId="sample_client",//客户端ID
              ClientSecrets={
                  new Secret("sample_client_secrent".Sha256()),//客户端秘钥  256加密算法
                 },
              AllowedGrantTypes=GrantTypes.ClientCredentials,//指定授权类型
              AllowedScopes={ "sample_api" }//此客户端允许访问的API范围
             },
        };

    }
}
