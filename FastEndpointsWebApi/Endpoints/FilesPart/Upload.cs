using FastEndpoints;
using static System.Collections.Specialized.BitVector32;

namespace EndpointsFastWebApi.Endpoints.FilesPart
{
    /// <summary>
    /// 上传大文件处理
    /// </summary>
    public class Upload : EndpointWithoutRequest
    {
        public override void Configure()
        {

            Post("/api/file-upload");
            AllowAnonymous();
            AllowFileUploads(dontAutoBindFormData: true); //turns off buffering

        }


        public override async Task HandleAsync(CancellationToken ct)
        {
            await foreach (var section in FormFileSectionsAsync(ct))
            {
                if (section is not null)
                {
                    using (var fs = File.Create(section.FileName))
                    {
                        await section.Section.Body.CopyToAsync(fs, 1024 * 64, ct);
                    }
                }
            }

            await SendOkAsync("upload complete!");
        }
    }
}
