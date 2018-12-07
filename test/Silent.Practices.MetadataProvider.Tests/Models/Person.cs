using System.Collections.Generic;

namespace Silent.Practices.MetadataProvider.Tests
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Position> Positions { get; set; }
    }
}
