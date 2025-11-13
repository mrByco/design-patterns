using App.Commands;
using App.Misc;
using DI;
using Moq;

namespace App.Tests.Misc;

public class CommandFactoryTests
{
    private CommandFactory _factory = null!;
    private Mock<ICustomServiceProvider> _serviceProvider = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new CommandFactory();
        _serviceProvider = new Mock<ICustomServiceProvider>();
    }

    [Test]
    public void CreateCommand_ShouldUseConstructorArguments()
    {
        // Arrange
        var commandType = typeof(TestCommandWithArgs);
        string[] parameters = { "testcommand", "42", "hello" };

        // Act
        var command = (TestCommandWithArgs)_factory.CreateCommand(commandType, _serviceProvider.Object, parameters);

        // Assert
        Assert.That(command.IntValue, Is.EqualTo(42));
        Assert.That(command.StringValue, Is.EqualTo("hello"));
    }

    [Test]
    public void CreateCommand_ShouldResolveDependenciesForMissingArgs()
    {
        // Arrange
        var commandType = typeof(TestCommandWithArgs);
        _serviceProvider
            .Setup(x => x.Resolve(typeof(int)))
            .Returns(99);
        _serviceProvider
            .Setup(x => x.Resolve(typeof(string)))
            .Returns("resolved");

        // Act
        var command = (TestCommandWithArgs)_factory.CreateCommand(commandType, _serviceProvider.Object, "testcommand");

        // Assert
        Assert.That(command.IntValue, Is.EqualTo(99));
        Assert.That(command.StringValue, Is.EqualTo("resolved"));
    }

    [Test]
    public void CreateCommand_ShouldInjectFieldsAndProperties()
    {
        // Arrange
        var commandType = typeof(TestCommandWithInjection);
        var loggerMock = new Mock<ILogger>();
        var repoMock = new Mock<IRepository>();

        _serviceProvider.Setup(x => x.Resolve(typeof(ILogger))).Returns(loggerMock.Object);
        _serviceProvider.Setup(x => x.Resolve(typeof(IRepository))).Returns(repoMock.Object);

        // Act
        var command = (TestCommandWithInjection)_factory.CreateCommand(commandType, _serviceProvider.Object, "test");

        // Assert
        Assert.That(command.Logger, Is.EqualTo(loggerMock.Object));
        Assert.That(command.GetRepository(), Is.EqualTo(repoMock.Object));
    }

    [Test]
    public void CreateCommand_ShouldThrowIfNoConstructorFound()
    {
        // Arrange
        var commandType = typeof(TestCommandWithoutConstructor);

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() =>
            _factory.CreateCommand(commandType, _serviceProvider.Object));
    }
}



public interface ILogger { }
public interface IRepository { }

public class TestCommandWithArgs : ICommand
{
    public int IntValue { get; }
    public string StringValue { get; }

    public TestCommandWithArgs(int number, string text)
    {
        IntValue = number;
        StringValue = text;
    }

    public Task Execute() => Task.CompletedTask;
}

public class TestCommandWithInjection : ICommand
{
    [DI]
    public ILogger Logger { get; set; } = null!;
    [DI]

    private IRepository _repository = null!;

    public IRepository GetRepository() => _repository;

    public Task Execute() => Task.CompletedTask;
}

public class TestCommandWithoutConstructor : ICommand
{
    private TestCommandWithoutConstructor() { } 
    public Task Execute() => Task.CompletedTask;
}