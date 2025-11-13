namespace DI;

public interface ICustomServiceProvider
{
    T Resolve<T>();
    object Resolve(Type type);
}
