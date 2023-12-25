using FastEndpoints;

namespace EndpointsFastWebApi.Endpoints
{
   
    public class DemoFirst : Endpoint<MyRequest, MyResponse>
    {       
        public override void Configure()
        {
            Post("v1/create");           
            AllowAnonymous();
            Options(x => x.WithTags("Environments_Devices_Api"));
        }


        public override void OnBeforeHandle(MyRequest req)
        {
            base.OnBeforeHandle(req);
        }

        public override void OnValidationFailed()
        {
            base.OnValidationFailed();
        }

        public override async Task HandleAsync(MyRequest req, CancellationToken ct)
        {
            await SendAsync(new()
            {
                FullName = req.FirstName + " " + req.LastName,
                IsOver18 = req.Age > 18
            }, 300, ct);
        }

        public override Task OnAfterHandleAsync(MyRequest req, MyResponse res, CancellationToken ct)
        {
            return base.OnAfterHandleAsync(req, res, ct);
        }

    }


    public class MyRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class MyResponse
    {
        public string FullName { get; set; }
        public bool IsOver18 { get; set; }
    }



}
