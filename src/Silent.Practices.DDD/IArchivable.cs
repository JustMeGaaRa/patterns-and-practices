namespace Silent.Practices.DDD
{
    public interface IArchivable
    {
        bool IsArchived { get; }

        void Archive();
    }
}