using System.Net;
using HtmlAgilityPack;


namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string keyword = "销售";
            string url = "https://www.zhipin.com/web/geek/job?query=%E7%A0%94%E5%8F%91&city=101020100";

            // 创建Web客户端
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            // 下载页面内容
            string html = client.DownloadString(url);

            // 使用HtmlAgilityPack解析HTML
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 获取岗位列表
            HtmlNodeCollection jobNodes = doc.DocumentNode.SelectNodes("//div[@class='listBox']/ul/li");

            // 遍历岗位列表
            foreach (HtmlNode jobNode in jobNodes)
            {
                // 获取岗位名称
                string jobTitle = jobNode.SelectSingleNode(".//span[@class='jobName']/a").InnerText.Trim();

                // 获取公司信息
                string company = jobNode.SelectSingleNode(".//div[@class='searchResultCompanyname']/a").InnerText.Trim();

                // 获取HR联系方式
                string hrContact = jobNode.SelectSingleNode(".//div[@class='searchResultCompanyname']/span").InnerText.Trim();

                // 获取HR姓氏
                string hrLastName = hrContact.Split(' ')[0];

                // 获取岗位职责
                string jobDescription = jobNode.SelectSingleNode(".//p[@class='searchResultJobdescription']").InnerText.Trim();

                // 输出结果
                Console.WriteLine("岗位名称: " + jobTitle);
                Console.WriteLine("公司信息: " + company);
                Console.WriteLine("HR联系方式: " + hrContact);
                Console.WriteLine("HR姓氏: " + hrLastName);
                Console.WriteLine("岗位职责: " + jobDescription);
                Console.WriteLine();
            }
        }
    }
}