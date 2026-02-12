using Microsoft.Extensions.DependencyInjection;

namespace Situations.Core.Tests
{
    public interface IFakeService { }
    public class FakeServiceImpl : IFakeService { }
    public class TestSituationsBuilder : SituationsBuilder<TestSituation>
    {
        public Dictionary<TestSituation, ISituationInvoker> GetInvokers() => SituationInvokers;
        public IServiceCollection GetServiceCollection() => ServiceCollection;
    }

    [TestClass]
    public class SituationsBuilderTests
    {
        [TestMethod]
        public void RegisterService_AddsTransientService()
        {
            //Arrange
            var builder = new TestSituationsBuilder();
            builder.RegisterService<FakeServiceImpl>();

            //Act
            var provider = builder.GetServiceCollection().BuildServiceProvider();
            var service1 = provider.GetService<FakeServiceImpl>();
            var service2 = provider.GetService<FakeServiceImpl>();

            //Assert
            Assert.IsNotNull(service1);
            Assert.IsNotNull(service2);
            Assert.AreNotSame(service1, service2);
        }

        [TestMethod]
        public void RegisterService_WithImplementation_AddsTransientService()
        {
            //Arrange
            var builder = new TestSituationsBuilder();
            builder.RegisterService<IFakeService, FakeServiceImpl>();

            //Act
            var provider = builder.GetServiceCollection().BuildServiceProvider();
            var service = provider.GetService<IFakeService>();

            //Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(FakeServiceImpl));
        }

        [TestMethod]
        public void RegisterService_WithFactory_AddsTransientService()
        {
            //Arrange
            var builder = new TestSituationsBuilder();
            builder.RegisterService(_ => new FakeServiceImpl());

            //Act
            var provider = builder.GetServiceCollection().BuildServiceProvider();
            var service = provider.GetService<FakeServiceImpl>();

            //Assert
            Assert.IsNotNull(service);
        }
    }
}
