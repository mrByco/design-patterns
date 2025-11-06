
using App.Commands;
using App.Misc;
using App.Repositories;
using DI;
using Moq;
using NUnit.Framework.Internal.Commands;
using System.Reflection;
using System.Security;

namespace App.Tests
{
    public class CommandContainerTests
    {
        [Test]
        public void Instances_ShouldBeTheSame()
        {
            // Arrange & Act
            var a = CommandContainer.GetInstance();
            var b = CommandContainer.GetInstance();

            Assert.That(a, Is.EqualTo(b));
        }


        [Test]
        public void Should_Find_TestEmptyCommand()
        {
            // Arrange
            var instance = CommandContainer.GetInstance();
            instance.AddCommandsFromAssembly(Assembly.GetExecutingAssembly());

            // Act
            var commands = instance.GetAllCommands();

            // Assert
            Assert.That(commands, Is.Not.Empty);
            
            var testCommand = commands["testempty"];
            Assert.That(testCommand, Is.EqualTo(typeof(TestEmptyCommand)));
        }

        [Test]
        public void GetCommand_ShouldCallCommandFactoryAndReturnResult()
        {
            // Arrange

            var instance = CommandContainer.GetInstance();
            instance.AddCommandsFromAssembly(Assembly.GetExecutingAssembly());
            var services = new Mock<ICustomServiceProvider>();
            var testCommandInstnace = new TestEmptyCommand();
            var factory = new Mock<ICommandFactory>();
            factory
                .Setup(x => x.CreateCommand(It.IsAny<Type>(), It.IsAny<ICustomServiceProvider>(), It.IsAny<string[]>()))
                .Returns( testCommandInstnace);

            // Act

            var cmd = instance.GetCommand("testempty", services.Object, factory.Object);

            // Assert
            Assert.That (cmd, Is.Not.Null);
            Assert.That(cmd, Is.EqualTo(testCommandInstnace));
            factory.Verify(x => x.CreateCommand(
                It.Is<Type>(t => t == typeof(TestEmptyCommand)),
                It.IsAny<ICustomServiceProvider>(),
                It.IsAny<string[]>()),
                Times.Once);
        }

        [TearDown]
        public void TearDown()
        {
            CommandContainer.ResetForTests();
        }
    }

    public class TestEmptyCommand : ICommand
    {
        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
