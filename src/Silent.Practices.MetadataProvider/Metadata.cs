using System.Collections.Generic;
using System.Linq;

namespace Silent.Practices.MetadataProvider
{
    public class Metadata
    {
        public Metadata(IEnumerable<TypeMetadata> types) => Types = types.ToList();

        public ICollection<TypeMetadata> Types { get; }
    }
}