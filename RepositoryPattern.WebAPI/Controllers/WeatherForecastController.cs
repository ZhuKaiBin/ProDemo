using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.WebAPI.Entities.RepositoryWrapper;
using RepositoryPattern.WebAPI.Entities.Models;
namespace RepositoryPattern.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public WeatherForecastController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var domesticAccounts = _repository.Account.FindByCondition(x => x.AccountType.Equals("Domestic"));
            var owners = _repository.Owner.FindAll();

            Owner owner = new Owner()
            {
                OwnerId = Guid.NewGuid()
            };


            _repository.Owner.Create(owner);

            return new string[] { "value1", "value2" };
        }
    }
}
