using Ardalis.GuardClauses;
using Ardalis.Specification;

namespace ArdalisSpecification
{
    public class Store
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }

        public int CustomerId { get; private set; }

        public Store(string name, string address)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));

            this.Name = name;
            this.Address = address;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var sdsd = new Store("", "1111");

            var sd = nameof(Store);
            var sd2 = nameof(Get);
            var sd3 = nameof(Type);

            Example.GetEnvironment("DEV");
        }

        public static string Get()
        {
            return "";
        }
    }

    #region

    ////Specification 类应该继承自Specification<T>，T查询中检索的类型在哪里：
    //public class ItemByIdSpec : Specification<Item>, ISingleResultSpecification
    //{
    //    public ItemByIdSpec(int Id)
    //    {
    //        Query.Where(x => x.Id == Id);
    //    }
    //}


    //public class ProjectByProjectStatusSpec : Specification<Project>
    //{
    //    public ProjectByProjectStatusSpec(ProjectStatus projectStatus)
    //    {
    //        Guard.Against.Null(projectStatus, nameof(projectStatus));

    //        Query.Where(p => p.Status == projectStatus);
    //    }
    //}

    //public class ProjectByProjectStatusWithToDoItemsSpec : Specification<Project>
    //{
    //    public ProjectByProjectStatusWithToDoItemsSpec(ProjectStatus projectStatus)
    //    {
    //        Guard.Against.Null(projectStatus, nameof(projectStatus));

    //        Query.Where(p => p.Status == projectStatus)
    //          .Include(p => p.Items);
    //    }
    //}

    ////ISingleResultSpecification接口用于指示规范将返回实体的单个实例。
    //public class ProjectByIdSpec : Specification<Project>, ISingleResultSpecification
    //{
    //    public ProjectByIdSpec(long id)
    //    {
    //        Guard.Against.Default(id, nameof(id));

    //        Query.Where(p => p.Id == id);
    //    }
    //}


    ////规范作为验证器
    //public class ValidateCompletedProjectSpec : Specification<Project>
    //{
    //    public ValidateCompletedProjectSpec()
    //    {
    //        Query
    //          .Include(p => p.Items)
    //          .Where(p => p.Status == ProjectStatus.Complete && p.Items.All(i => i.IsDone));
    //    }
    //}



    //如何将规范与 DbContext 一起使用




    //使用内置抽象存储库 Abstract Repository

    public class Hero
    {
        public Hero(string a, string b, bool c, bool d) { }

        public Hero() { }
    }

    public class HeroService
    {
        private readonly IRepository<Hero> _heroRepository;

        public HeroService(IRepository<Hero> heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public async Task<Hero> Create(string name, string superPower, bool isAlive, bool isAvenger)
        {
            var hero = new Hero(name, superPower, isAlive, isAvenger);

            await _heroRepository.AddAsync(hero);

            await _heroRepository.SaveChangesAsync();

            return hero;
        }

        public async Task Delete(Hero hero)
        {
            await _heroRepository.DeleteAsync(hero);

            await _heroRepository.SaveChangesAsync();
        }

        public async Task DeleteRange(List<Hero> heroes)
        {
            await _heroRepository.DeleteRangeAsync(heroes);

            await _heroRepository.SaveChangesAsync();
        }

        public async Task<Hero> GetById(int id)
        {
            return await _heroRepository.GetByIdAsync(id);
        }

        //public async Task<Hero> GetByName(string name)
        //{
        //    var spec = new HeroByNameSpec(name);

        //    return await _heroRepository.FirstOrDefaultAsync(spec);
        //}

        //public async Task<List<Hero>> GetHeroesFilteredByNameAndSuperPower(string name, string superPower)
        //{
        //    var spec = new HeroByNameAndSuperPowerFilterSpec(name, superPower);

        //    return await _heroRepository.ListAsync(spec);
        //}

        //public async Task<Hero> SetIsAlive(int id, bool isAlive)
        //{
        //    var hero = await _heroRepository.GetByIdAsync(id);

        //    hero.IsAlive = isAlive;

        //    await _heroRepository.UpdateAsync(hero);

        //    await _heroRepository.SaveChangesAsync();

        //    return hero;
        //}

        public async Task SeedData(Hero[] heroes)
        {
            // only seed if no Heroes exist
            if (!await _heroRepository.AnyAsync())
            {
                return;
            }

            // alternatively
            if (await _heroRepository.CountAsync() > 0)
            {
                return;
            }

            foreach (var hero in heroes)
            {
                await _heroRepository.AddAsync(hero);
            }

            await _heroRepository.SaveChangesAsync();
        }
    }

    //public async Task Run()
    //{
    //    var seedData = new[]
    //                   {
    //                    new Hero(
    //                        name: "Batman",
    //                        superPower: "Intelligence",
    //                        isAlive: true,
    //                        isAvenger: false),
    //                    new Hero(
    //                        name: "Iron Man",
    //                        superPower: "Intelligence",
    //                        isAlive: true,
    //                        isAvenger: true)
    //                    };

    //    await heroService.SeedData(seedData);

    //    var captainAmerica = await heroService.Create("Captain America", "Shield", true, true);

    //    var ironMan = await heroService.GetByName("Iron Man");

    //    var alsoIronMan = await heroService.GetById(ironMan.Id);

    //    await heroService.SetIsAlive(ironMan.Id, false);

    //    var shouldOnlyContainBatman = await heroService.GetHeroesFilteredByNameAndSuperPower("Bat", "Intel");

    //    await heroService.Delete(captainAmerica);

    //    var allRemainingHeroes = await heroService.GetHeroesFilteredByNameAndSuperPower("", "");

    //    await heroService.DeleteRange(allRemainingHeroes);

    //}


    public interface IRepository<T> : IRepositoryBase<T>
        where T : class { }

    #endregion
}
