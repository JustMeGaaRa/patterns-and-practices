using System.Collections.Generic;

namespace Silent.Practices.MetadataProvider
{
    public class Metadata
    {
        public Metadata(ICollection<TypeMetadata> types) => Types = types;

        public ICollection<TypeMetadata> Types { get; }
    }
}