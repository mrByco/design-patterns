
using DI.Tests.Core;

namespace DI.Core;

public class DIContainer : ICustomServiceProvider
{
    Dictionary<Type, ServiceDescriptor> Services = new Dictionary<Type, ServiceDescriptor>();

    public void Register<TInterface, TImpl>() where TImpl: TInterface, new()
    {
        this.Services.Add(typeof(TInterface), new ServiceDescriptor
        {
            Interface = typeof(TImpl),
            Implementation = typeof(TImpl)
        });
    }

    public T Resolve<T>()
    {
        return (T)this.Resolve(typeof(T));
    }

    public object Resolve(Type type)
    {
        Services.TryGetValue(type, out var t);
        if (t == null) throw new Exception($"No service found for {type}");

        var instance = Activator.CreateInstance(t.Implementation);
        instance.InjectDependencies(this);
        return instance;
    }
}
