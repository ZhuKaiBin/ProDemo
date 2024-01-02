using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;

namespace EndpointsFastWebApi.EntityMapper
{

    public class PersonMapper : Mapper<Request, Response, Person>
    {
        /// <summary>
        /// 将Request转换成Person
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public override Person ToEntity(Request r) => new()
        {
            Id = r.Id,
            DateOfBirth = DateOnly.Parse(r.BirthDay),
            FullName = $"{r.FirstName}&& {r.LastName}"
        };



        //Request==》Person====》Response


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override Response FromEntity(Person e)
        {
            return new Response()
            {
                Id = e.Id,
                FullName = e.FullName+"$$",
                UserName = $"USR{e.Id:0000000000}",
                Age = (DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - e.DateOfBirth.DayNumber) / 365,
            };
        }

        
    }
}
