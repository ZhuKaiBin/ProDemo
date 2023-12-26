using EndpointsFastWebApi.EntityMapper;
using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace EndpointsFastWebApi.Endpoints.MapperPart
{
    public class SavePerson : Endpoint<Request, Response, PersonMapper>
    {
        public override void Configure()
        {

            Put("/api/person");
            AllowAnonymous();
        }

        public override Task HandleAsync(Request req, CancellationToken ct)
        {
            //
            Person entity = Map.ToEntity(req);

            Response = Map.FromEntity(entity);

            return SendAsync(Response);
        }
    }
}
