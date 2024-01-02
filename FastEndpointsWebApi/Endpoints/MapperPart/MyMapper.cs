using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;

namespace EndpointsFastWebApi.Endpoints.MapperPart
{
    public class MyMapper : Mapper<MyRequest, MyResponse, EntityClass>
    {

        public override MyResponse FromEntity(EntityClass e)
        {

            return new MyResponse()
            {
                FullName = e.Name
            };
        }

        public override Task<MyResponse> FromEntityAsync(EntityClass e, CancellationToken ct = default)
        {
            return base.FromEntityAsync(e, ct);
        }

        public override EntityClass ToEntity(MyRequest r)
        {
            return base.ToEntity(r);
        }

        public override Task<EntityClass> ToEntityAsync(MyRequest r, CancellationToken ct = default)
        {
            return base.ToEntityAsync(r, ct);
        }

        public override EntityClass UpdateEntity(MyRequest r, EntityClass e)
        {
            return base.UpdateEntity(r, e);
        }
    }
}
