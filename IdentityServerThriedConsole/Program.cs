using IdentityModel.Client;
using System.Net.Http;

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

        #region 访问资源1

        {
            var tokenClient = new TokenClient(diso.TokenEndpoint, "client", "secret");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").Result;
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
            var rsp = httpClient.GetAsync("https://localhost:7128/WeatherForecast/").Result;
            if (rsp.IsSuccessStatusCode)
            {
                Console.WriteLine(rsp.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion 访问资源1

        #region 访问资源2

        {
            var tokenClient = new TokenClient(diso.TokenEndpoint, "client2", "secret2");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api2").Result;
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
            var rsp = httpClient.GetAsync("https://localhost:7050/WeatherForecast/").Result;
            if (rsp.IsSuccessStatusCode)
            {
                Console.WriteLine(rsp.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion 访问资源2

        Console.ReadLine();
    }
}