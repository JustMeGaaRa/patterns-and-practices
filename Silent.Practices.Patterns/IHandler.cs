namespace Silent.Practices.Patterns
{
    public interface IHandler<in TSource>
    {
        void Handle(TSource instance);
    }
}
