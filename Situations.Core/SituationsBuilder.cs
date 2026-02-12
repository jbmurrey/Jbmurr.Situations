using Microsoft.Extensions.DependencyInjection;

namespace Situations.Core
{
    public abstract class SituationsBuilder<SituationEnum> 
        where SituationEnum : Enum
    {
        protected IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        protected Dictionary<SituationEnum, ISituationInvoker> SituationInvokers = [];

        public void RegisterService<TService>()
            where TService : class
        {
            ServiceCollection.AddTransient<TService>();
        }

        public void RegisterService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollection.AddTransient<TService, TImplementation>();
        }

        public void RegisterService<TService>(Func<IServiceProvider, TService> serviceFactory)
            where TService : class
        {
            ServiceCollection.AddTransient(serviceFactory);
        }

        public SituationsContainer<SituationEnum> Build()
        {
            return new SituationsContainer<SituationEnum>(SituationInvokers, ServiceCollection.BuildServiceProvider());
        }
    }
}
