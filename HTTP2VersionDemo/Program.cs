﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP2VersionDemo
{
    internal class Program
    {
        private static readonly HttpClient staticClient = new HttpClient();

        private static async Task Main(string[] args)
        {
            // 设置服务集合（依赖注入容器）
            var serviceProvider = new ServiceCollection()
                .AddHttpClient() // 注册 IHttpClientFactory
                .BuildServiceProvider();

            // 通过依赖注入获取 IHttpClientFactory 实例
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();




            // netstat -an | findstr 101.67.61.141
            // netstat -an | findstr 39.156.66.10

            // 发起请求
            try
            {
                {
                    //for (int i = 0; i < 100000000; i++)
                    //{
                    //    using (var newClient = new HttpClient())
                    //    {
                    //        var response = await newClient.GetAsync("https://c-design.oss-cn-hangzhou.aliyuncs.com/env_staging/graphics/materials/dwg/5048DB1602.dwg");
                    //        response.EnsureSuccessStatusCode();
                    //        Console.WriteLine(response.Version);
                    //        Console.WriteLine(DateTime.Now);
                    //    }
                    //}
                }
                {
                    //staticClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    //for (int i = 0; i < 100000000; i++)
                    //{
                    //    var response = await staticClient.GetAsync("https://c-design.oss-cn-hangzhou.aliyuncs.com/env_staging/graphics/materials/dwg/5048DB1602.dwg");
                    //    response.EnsureSuccessStatusCode();
                    //    Task.Delay(100);
                    //    Console.WriteLine(response.Version);
                    //    Console.WriteLine(DateTime.Now);
                    //}
                }

                {
                    var client = httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    for (int i = 0; i < 100000000; i++)
                    {
                        //var response = await httpClientFactory.CreateClient().GetAsync("https://c-design.oss-cn-hangzhou.aliyuncs.com/env_staging/graphics/materials/dwg/5048DB1602.dwg");
                        //response.EnsureSuccessStatusCode();
                        //Console.WriteLine(response.Version);

                        Task.Delay(100);

                        var response = await client.GetAsync("https://c-design.oss-cn-hangzhou.aliyuncs.com/env_staging/graphics/materials/dwg/5048DB1602.dwg");
                        response.EnsureSuccessStatusCode();
                        var sd = response.Headers.Connection;
                        Console.WriteLine(response.Version);
                        Console.WriteLine(DateTime.Now);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"请求失败: {e.Message}");
            }
        }
    }
}