using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;

namespace EndpointsFastWebApi.Endpoints.FilesPart
{
    public class MyFileEndpoint : Endpoint<MyFileRequest>
    {
        public override void Configure()
        {
            Post("/api/uploads/image");
            AllowAnonymous();
            AllowFileUploads();
        }

        public override async Task HandleAsync(MyFileRequest req, CancellationToken ct)
        {
            if (Files.Count > 0)
            {
                var file = Files[0];

                await SendStreamAsync(
                    stream: file.OpenReadStream(),
                    fileName: "自定义的名称",
                    fileLengthBytes: file.Length,
                    contentType: "image/png"
                    );
                return;
            }

            await SendNoContentAsync();
        }
    }
}
