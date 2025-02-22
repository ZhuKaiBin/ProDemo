namespace BuilderModel_Console_2nd
{
    //https://www.bilibili.com/video/BV1KNKGeDEge/?spm_id_from=333.1365.list.card_archive.click&vd_source=b75e521f89833d861c15a89a5ef8cca9
    internal class Program
    {
        //提高代码可读性
        static void Main(string[] args)
        {
            //1.一次性提供所有参数
            var person = new Person.Builder()
                .WithName("Tom")
                .WithYearOfBirth(1990)
                .Build();

            //2.逐个提供参数
            var name = new Person.Builder().WithName("Tom");
            var birth = name.WithYearOfBirth(100);
            birth.Build();
        }
    }

    public class Person
    {
        private string Name { get; }
        private int YearOfBirth { get; }

        private Person(string name, int yearOfBirth)
        {
            this.Name = name;
            this.YearOfBirth = yearOfBirth;
        }

        public class Builder
        {
            private string _name;
            private int _yearOfBirth;
            private int _age;

            public Builder WithName(string name)
            {
                _name = name;
                return this;
            }

            public Builder WithYearOfBirth(int yearOfBirth)
            {
                if (yearOfBirth > DateTime.Now.Year || yearOfBirth < 0)
                {
                    throw new ArgumentException("Invalid year of birth");
                }

                _yearOfBirth = yearOfBirth;
                return this;
            }

            public Person Build()
            {
                return new Person(_name, _yearOfBirth);
            }

        }

    }
}
