using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICollectionViewStuff
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }

    public class SampleModel
    {
        public IList<Person> GenerateSampleData()
        {
            return new[]
            {
                new Person{ ID=1, Name="Marlon", Location="Malta"},
                new Person{ ID=3, Name="Dr.WPF", Location="N/A"},
                new Person{ ID=12, Name="Ian", Location="UK"},
                new Person{ ID=5, Name="Karl", Location="USA"},
                new Person{ ID=4, Name="Sasha", Location="UK"},
                new Person{ ID=6, Name="Brennon", Location="USA"},
                new Person{ ID=7, Name="Corrado", Location="Italy"},
                new Person{ ID=14, Name="Jerimiah", Location="USA"},
                new Person{ ID=8, Name="Andrew", Location="USA"},
                new Person{ ID=9, Name="Bill", Location="USA"},
                new Person{ ID=10, Name="Rudi", Location="South Africa"},
                new Person{ ID=2, Name="Josh", Location="USA"},
                new Person{ ID=11, Name="Paul", Location="Australia"},
                new Person{ ID=18, Name="Mike", Location="USA"},
                new Person{ ID=13, Name="Bob", Location="N/A"},
                new Person{ ID=15, Name="John", Location="USA"},
                new Person{ ID=16, Name="Pavan", Location="USA"},
                new Person{ ID=17, Name="Laurent", Location="Swizerland"},
            };
        }
    }
}
