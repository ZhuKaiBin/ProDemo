using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;

namespace AliPay
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string APPID = "2021000117685910";
            string APP_PRIVATE_KEY = "";
            string ALIPAY_PUBLIC_KEY = "";
            string CHARSET = "";

            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.open.public.template.message.industry.modify 
            AlipayOpenPublicTemplateMessageIndustryModifyRequest request = new AlipayOpenPublicTemplateMessageIndustryModifyRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数
            //此次只是参数展示，未进行字符串转义，实际情况下请转义
            request.BizContent = "{" +
            "    \"primary_industry_name\":\"IT科技/IT软件与服务\"," +
            "    \"primary_industry_code\":\"10001/20102\"," +
            "    \"secondary_industry_code\":\"10001/20102\"," +
            "    \"secondary_industry_name\":\"IT科技/IT软件与服务\"" +
            "  }";
            AlipayOpenPublicTemplateMessageIndustryModifyResponse response = client.Execute(request);
            //调用成功，则处理业务逻辑
            //if (response.isSuccess())
            //{
            //    //.....
            //}



        }
    }
}
