using Microsoft.Extensions.DependencyInjection;
using Situations.Core.Exceptions;

namespace Situations.Core.Tests
{
    public class FakeSituationInvoker : ISituationInvoker
    {
        public FakeSituationInvoker()
        {
            ParameterFactory = _ => null!;
            Invoke = args =>
            {
                Invoked = true;
                LastArgs = args;
            };
        }

        public bool Invoked { get; private set; }
        public object? LastArgs { get; private set; }
        public Func<IServiceProvider, object> ParameterFactory { get; set; }
        public Action<object>? Invoke { get; set; }
    }

    public class FakeService { }

    public class FakeServiceScope(IServiceProvider provider) : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; } = provider;
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    public class FakeServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(FakeService))
                return new FakeService();
            return null;
        }
    }

    [TestClass]
    public class ConfiguredServiceTests
    {
        [TestMethod]
        public void Instance_ShouldReturnServiceFromScope()
        {
            //Arrange
            var provider = new FakeServiceProvider();
            var scope = new FakeServiceScope(provider);
            var actions = new Dictionary<TestSituation, ISituationInvoker>();
            var configured = new ConfiguredService<FakeService, TestSituation>(actions, scope);

            //Act & Assert
            Assert.IsInstanceOfType(configured.Instance, typeof(FakeService));
        }

        [TestMethod]
        public void InvokeSituation_ShouldCallInvoker()
        {
            //Arrange
            var provider = new FakeServiceProvider();
            var scope = new FakeServiceScope(provider);
            var invoker = new FakeSituationInvoker();
            var actions = new Dictionary<TestSituation, ISituationInvoker>
            {
                [TestSituation.First] = invoker
            };
            var configured = new ConfiguredService<FakeService, TestSituation>(actions, scope);

            //Act
            configured.InvokeSituation(TestSituation.First);

            //Assert
            Assert.IsTrue(invoker.Invoked);
        }

        [TestMethod]
        public void InvokeSituation_UnregisteredSituation_Throws()
        {
            //Arrange
            var provider = new FakeServiceProvider();
            var scope = new FakeServiceScope(provider);
            var actions = new Dictionary<TestSituation, ISituationInvoker>();
            var configured = new ConfiguredService<FakeService, TestSituation>(actions, scope);

            //Act & Assert
            Assert.ThrowsException<UnregisteredSituationException>(() =>
                configured.InvokeSituation(TestSituation.First));
        }


        [TestMethod]
        public void Dispose_DisposesScope()
        {
            //Arrange
            var provider = new FakeServiceProvider();
            var scope = new FakeServiceScope(provider);
            var actions = new Dictionary<TestSituation, ISituationInvoker>();
            var configured = new ConfiguredService<FakeService, TestSituation>(actions, scope);

            //Act
            configured.Dispose();

            //Assert
            Assert.IsTrue(scope.Disposed);
            Assert.IsNull(configured.Instance);
        }

        [TestMethod]
        public void InvokeSituation_AfterDispose_Throws()
        {
            //Arrange
            var provider = new FakeServiceProvider();
            var scope = new FakeServiceScope(provider);
            var invoker = new FakeSituationInvoker();
            var actions = new Dictionary<TestSituation, ISituationInvoker>
            {
                [TestSituation.First] = invoker
            };
            var configured = new ConfiguredService<FakeService, TestSituation>(actions, scope);

            //Act
            configured.Dispose();

            //Assert
            Assert.ThrowsException<ObjectDisposedException>(() =>
                configured.InvokeSituation(TestSituation.First));
        }
    }
}
