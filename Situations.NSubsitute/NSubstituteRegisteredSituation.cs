using Microsoft.Extensions.DependencyInjection;
using Situations.Core;

namespace Situations.NSubsitute
{
    public class NSubstituteRegisteredSituation<TService> : ISituationInvoker
        where TService : class
    {
        public void OnInvocation(Action<TService> action)
        {
            Invoke += (args) =>
            {
                if (args != null)
                {
                    action((TService)args);
                }
            };
        }

        public Action<object?>? Invoke { get; set; }

        public Func<IServiceProvider, object?> ParameterFactory => (sp) => sp.GetService<TService>();
    }
}
