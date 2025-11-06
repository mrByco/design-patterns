using DI.Core;

namespace DI.Tests.Core;

public class DIContainerTests
{

    [Test]
    public void Should_ResolveImplementationWithNoParameters()
    {
        var container = new DIContainer();

        container.Register<IService, ServiceImpl>();
        var type = container.Resolve<IService>();

        Assert.That(type, Is.Not.Null);
        Assert.That(type, Is.TypeOf<ServiceImpl>());
    }

    [Test]
    public void Should_ResolveDependencies()
    {
        var container = new DIContainer();

        container.Register<IService, ServiceImpl>();
        container.Register<IDependentService, DependentImpl>();
        var type = container.Resolve<IDependentService>();

        Assert.That(type.GetDependency(), Is.Not.Null);
        Assert.That(type.GetDependency(), Is.TypeOf<ServiceImpl>());
    }


    private interface IService
    {

    }

    private class ServiceImpl : IService
    {
        public string Key { get; set; }
    }

    private interface IDependentService
    {
        IService GetDependency();
    }

    private class DependentImpl: IDependentService
    {
        [DI]
        public IService _dependency { private get; init; }

        public IService GetDependency() { return _dependency; }
    }


}
