namespace Silent.Practices.Patterns
{
    public interface IHandler<in TSource>
    {
        void Handle(TSource instance);
    }

    public interface IHandler<in TSource, out TResult>
    {
        TResult Handle(TSource instance);
    }
}
