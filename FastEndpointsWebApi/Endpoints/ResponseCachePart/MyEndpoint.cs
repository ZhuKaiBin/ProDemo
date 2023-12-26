using FastEndpoints;

namespace EndpointsFastWebApi.Endpoints.ResponseCachePart
{
    public class MyEndpoint: EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("/api/cached-ticks");
            AllowAnonymous();
            ResponseCache(10); //cache for 60 seconds
        }

        public override Task HandleAsync(CancellationToken ct)
        {
            return SendAsync(new
            {
                Message = "this response is cached",
                Ticks = DateTime.UtcNow.Ticks
            });
        }
    }
}
