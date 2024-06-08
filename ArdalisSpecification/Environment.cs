using Ardalis.Specification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdalisSpecification
{
    public class Environment
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EnvironmentByNameSpec : Specification<Environment>
    {
        public EnvironmentByNameSpec(string name)
        {
            Query.Where(h => h.Name == name);
        }
    }

    public static class Example
    {
        private static readonly Environment[] Environments = new Environment[]
        {
        new()
        {
            Name = "DEV",
            Description = "this application's development environment"
        },
        new()
        {
            Name = "QA",
            Description = "this application's QA environment"
        },
        new()
        {
            Name = "PROD",
            Description = "this application's production environment"
        }
        };

        public static Environment GetEnvironment(string name)
        {
            var specification = new EnvironmentByNameSpec(name);

            var environment = specification.Evaluate(Environments)
                .Single();

            return environment;
        }
    }

   
}
