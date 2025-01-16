using Microsoft.AspNetCore.Mvc;
using ElasticSearchSln.Domain;
using Nest;

namespace ElasticSearchSln.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class VisitLogController : ControllerBase
    {
        private readonly IVisitLogRepository _visitLogRepository;

        public VisitLogController(IVisitLogRepository visitLogRepository)
        {
            _visitLogRepository = visitLogRepository;
        }

        [HttpGet]
        public async Task<IActionResult> QueryAsync(int page = 1, int limit = 10)
        {
            var result = await _visitLogRepository.QueryAsync(page, limit);

            return Ok(new
            {
                total = result.Item1,
                items = result.Item2
            });
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] VisitLog visitLog)
        {
            await _visitLogRepository.InsertAsync(visitLog);

            return Ok("新增成功");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _visitLogRepository.DeleteAsync(id);

            return Ok("删除成功");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] VisitLog visitLog)
        {
            await _visitLogRepository.UpdateAsync(visitLog);

            return Ok("修改成功");
        }


        [HttpGet]
        public async Task<IActionResult> ElasticSearchAPI(int id, string name, int age)
        {
            var person = new Person { Id = id, Name = name, Age = age, DateTimeOffset = DateTimeOffset.UtcNow };
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("prozkb");
            ElasticClient _client = new ElasticClient(settings);
            var indexResponse = await _client.IndexDocumentAsync(person);

            return Ok("成功");
        }


        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }

            public DateTimeOffset DateTimeOffset { get; set; }

        }

    }
}
