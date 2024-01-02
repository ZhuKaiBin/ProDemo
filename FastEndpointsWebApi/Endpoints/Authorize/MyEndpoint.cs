using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace EndpointsFastWebApi.Endpoints.Authorize
{
    [HttpPost("/my-endpoint")]
    //[Authorize(Roles = "Admin,Manager")]  使用这个的话，要注入授权的Scheme
    //[Authorize(Policy = "ManagersOnly",Roles = "Manager")]

    public class MyEndpoint : Endpoint<MyRequest, MyResponse>
    {
        public override async Task HandleAsync(MyRequest req, CancellationToken ct)
        {
            await SendAsync(new MyResponse { });
        }
    }
}
