using ConvertApiDotNet;

namespace ConvertApiSVG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            async static Task Main(string[] args)
            {

                string From = @"C:\Users\Prozkb\Desktop\WJ\1-8.dwg";
                string To = @"C:\Users\Prozkb\Desktop\WJ\";

                ConvertApi convertApi = new ConvertApi("7a2BOAw4bHQVjzU3");

                var convert = await convertApi.ConvertAsync("dwg", "svg", new ConvertApiFileParam("File", From));
                await convert.SaveFilesAsync(To);
            }
        }
    }
}