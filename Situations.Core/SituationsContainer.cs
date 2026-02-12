using Microsoft.Extensions.DependencyInjection;

namespace Situations.Core
{
    public class SituationsContainer<SituationEnum>
        where SituationEnum : Enum
    {
        private readonly Dictionary<SituationEnum, ISituationInvoker> _situationActions;
        private readonly IServiceProvider _serviceProvider;

        public SituationsContainer(Dictionary<SituationEnum, ISituationInvoker> situationActions, IServiceProvider serviceProvider)
        {
            _situationActions = situationActions;
            _serviceProvider = serviceProvider;
        }

        public ConfiguredService<TService, SituationEnum> GetConfiguredService<TService>()
            where TService : class
        {
            return new ConfiguredService<TService, SituationEnum>(_situationActions, _serviceProvider.CreateScope());
        }
    }
}
