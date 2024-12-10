using IdentityServer4.Models;

namespace IdentityServerCenter
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("api1", "My API"),
             new ApiResource("api2", "My API2")
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
             {
                 new Client
                 {
                     ClientId = "client",
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
             };
        }
    }
}