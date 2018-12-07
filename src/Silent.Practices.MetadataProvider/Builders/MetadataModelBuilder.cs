using System.Linq;

namespace Silent.Practices.MetadataProvider.Builders
{
    public abstract class MetadataModelBuilder : IBuilder<Metadata>
    {
        private readonly TypeCache _typeCache;
        private readonly IMetadataBuilder _metadataBuilder;

        public MetadataModelBuilder()
        {
            _typeCache = new TypeCache();
            _metadataBuilder = new MetadataBuilder(_typeCache);
        }

        public Metadata Build()
        {
            OnModelCreating(_metadataBuilder);
            return new Metadata(_typeCache.ToList());
        }

        protected abstract void OnModelCreating(IMetadataBuilder metadataBuilder);
    }
}