using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Situations.Core;

namespace Situations.Moq
{
    public class MoqSituationsBuilder<SituationEnum> : SituationsBuilder<SituationEnum> where SituationEnum : Enum
    {
        public MoqRegisteredSituation<TService> RegisterSituation<TService>(SituationEnum situation) where TService : class
        {
            var mock = new Mock<TService>();
            var registration = new MoqRegisteredSituation<TService>();

            ServiceCollection.TryAddScoped(sp => mock.Object);
            ServiceCollection.TryAddScoped(sp => mock);
            SituationInvokers.TryAdd(situation, registration);

            return registration;
        }

        public void AddMock<TService>() where TService : class
        {
            ServiceCollection.AddScoped(_ => new Mock<TService>().Object);
        }
    }
}