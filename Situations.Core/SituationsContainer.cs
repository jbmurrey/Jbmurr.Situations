using Microsoft.Extensions.DependencyInjection;

namespace Situations.Core
{
    public class SituationsContainer<SituationEnum>
        where SituationEnum : Enum
    {
        private readonly Dictionary<SituationEnum, ISituationInvoker> _situationInvokers;
        private readonly IServiceProvider _serviceProvider;

        public SituationsContainer(Dictionary<SituationEnum, ISituationInvoker> situationInvokers, IServiceProvider serviceProvider)
        {
            _situationInvokers = situationInvokers;
            _serviceProvider = serviceProvider;
        }

        public ConfiguredService<TService, SituationEnum> GetConfiguredService<TService>()
            where TService : class
        {
            return new ConfiguredService<TService, SituationEnum>(_situationInvokers, _serviceProvider.CreateScope());
        }
    }
}
