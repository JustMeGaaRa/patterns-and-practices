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
            return _metadataBuilder.Build();
        }

        protected abstract void OnModelCreating(IMetadataBuilder metadataBuilder);
    }
}