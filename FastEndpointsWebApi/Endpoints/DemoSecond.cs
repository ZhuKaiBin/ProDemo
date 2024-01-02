using FastEndpoints;
using System.Text;

namespace EndpointsFastWebApi.Endpoints
{
    public class DemoSecond : EndpointWithoutRequest<MyResponse>
    {
        public override void Configure()
        {
            Post("v1/create");
            AllowAnonymous();
            Options(x => x.WithTags("DemoSecond_Api"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            //1.直接返回Response
            //Response.FullName = "9090";
            //Response.IsOver18=true;

            //await
            // SendCreatedAtAsync("cctv","Henan",new MyResponse() { FullName = "HOBO", IsOver18 = true });

            //string name = "BOB";
            //SendBytesAsync(Encoding.UTF8.GetBytes(name), "fileName");

            //FileInfo fileInfo=new FileInfo(name)
            //SendFileAsync();
            //SendStreamAsync()

            //Response = new()
            //{
            //    FullName = "john doe",
            //    IsOver18 = false
            //};
            //await Task.CompletedTask;



            await this.SendCode(0000133);
        }

        public override void OnBeforeValidate(EmptyRequest req)
        {
            base.OnBeforeValidate(req);
        }
    }



    public static class EndPointExtensions
    {
        public static async Task SendCode(this IEndpoint ep, int statusCode, CancellationToken ct = default)
        {

            //IEndpoint这个接口中，封装了HttpContext这个上下文，这样依赖，就可以点出来上下文，可以对上下文信息进行修改



            ep.HttpContext.MarkResponseStart(); //don't forget to always do this
            ep.HttpContext.Response.StatusCode = statusCode;




            await ep.HttpContext.Response.StartAsync(ct);
        }    
    }

    
         
}
