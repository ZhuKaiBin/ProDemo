using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerCenter
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("api1", "My API"),
            new ApiResource("api2", "My API2"),
            new ApiResource("api3", "My API3")
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId = "client",
                     //“凭据(Credentials)”是一个广义的词，表示可以证明身份或权利的东西。在技术领域中，凭据（credentials） 通常指的是用来认证身份的信息或工具。
                     AllowedGrantTypes = GrantTypes.ClientCredentials,
                     ClientSecrets = { new Secret("secret".Sha256()) },
                     AllowedScopes = { "api1" }
                 },
                  new Client
                 {
                     ClientId = "client2",
                     AllowedGrantTypes = GrantTypes.ClientCredentials,
                     ClientSecrets = { new Secret("secret2".Sha256()) },
                     AllowedScopes = { "api2" }
                 },

                  new Client()
                  {
                     ClientId="pwdClient",
                     ClientSecrets = { new Secret("pwdSecret".Sha256()) },
                     RequireClientSecret=false,
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "api3" }
        }
    };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
             new TestUser{ SubjectId="1",Username="prozkb",Password="123456" }
            };
        }
    }
}