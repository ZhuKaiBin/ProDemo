using IdentityModel.Client;
using IdentityServer4.Test;

internal class Program
{
    private static void Main()
    {
        //这个是授权服务器的地址
        var diso = DiscoveryClient.GetAsync("https://localhost:7005").Result;
        if (diso.IsError)
        {
            Console.WriteLine(diso.Error);
        }

        #region 账号密码模式请求Token

        var tokenClient = new TokenClient(diso.TokenEndpoint, "pwdClient", "pwdSecret");
        //var tokenClient = new TokenClient(diso.TokenEndpoint, "pwdClient");//如果Config中设置RequireClientSecret=false，那么就不需要设置ClientSecrets
        var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("prozkb", "123456").Result;
        if (tokenResponse.IsError)
        {
            Console.WriteLine(diso.Error);
        }
        else
        {
            Console.WriteLine(diso.Json);
        }

        var httpClient = new HttpClient();
        httpClient.SetBearerToken(tokenResponse.AccessToken);
        //这个是要访问的资源服务器
        var rsp = httpClient.GetAsync("https://localhost:7189/WeatherForecast/").Result;
        if (rsp.IsSuccessStatusCode)
        {
            Console.WriteLine(rsp.Content.ReadAsStringAsync().Result);
        }

        #endregion 账号密码模式请求Token

        Console.ReadLine();
    }
}