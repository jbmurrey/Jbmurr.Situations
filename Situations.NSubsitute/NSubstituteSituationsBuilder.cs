using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Situations.Core;
using Situations.NSubsitute;

namespace Situations.Moq
{
    public class NSubstituteSituationsBuilder<SituationEnum> : SituationsBuilder<SituationEnum>
        where SituationEnum : Enum
    {
        public NSubstituteRegisteredSituation<TService> RegisterSituation<TService>(SituationEnum situation) where TService : class
        {
            var service = Substitute.For<TService>();
            var registration = new NSubstituteRegisteredSituation<TService>();

            ServiceCollection.TryAddScoped(sp => service);
            SituationInvokers.TryAdd(situation, registration);

            return registration;
        }
    }
}