namespace Silent.Practices.MetadataProvider.Builders
{
    public abstract class MetadataModelBuilder
    {
        private readonly TypeCache _typeCache = new TypeCache();
        private readonly IMetadataBuilder _metadataBuilder;

        public MetadataModelBuilder()
        {
            _metadataBuilder = new MetadataBuilder(_typeCache);
        }

        protected abstract void OnModelCreating(IMetadataBuilder metadataBuilder);
    }
}