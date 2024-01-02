using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;

namespace EndpointsFastWebApi.Endpoints.MapperPart
{
    public class SavePerson2 : EndpointWithMapping<Request, Response, Person>
    {
        public override void Configure()
        {
            Put("/api/person2");
            AllowAnonymous();
        }


        public override Task HandleAsync(Request r, CancellationToken c)
        {
            Person entity = MapToEntity(r);
            Response = MapFromEntity(entity);
            return SendAsync(Response);
        }

        public override Person MapToEntity(Request r) => new()
        {
            Id = r.Id,
            DateOfBirth = DateOnly.Parse(r.BirthDay),
            FullName = $"{r.FirstName} {r.LastName}"
        };

        public override Response MapFromEntity(Person e) => new()
        {
            Id = e.Id,
            FullName = e.FullName,
            UserName = $"USR{e.Id:0000000000}",
            Age = (DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - e.DateOfBirth.DayNumber) / 365,
        };

    }
}
