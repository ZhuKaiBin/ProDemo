using Microsoft.AspNetCore.Mvc;
using ElasticSearchSln.Domain;

namespace ElasticSearchSln.Controllers
{
    [Route("api/[controller]")]
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
    }
}
