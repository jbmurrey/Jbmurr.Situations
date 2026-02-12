using Microsoft.Extensions.DependencyInjection;
using Moq;
using Situations.Core;

namespace Situations.Moq
{
    public class MoqRegisteredSituation<TService> : ISituationInvoker
        where TService : class
    {
        public void OnInvocation(Action<Mock<TService>> action)
        {
            Invoke += (args) =>
            {
                if (args != null)
                {
                    action((Mock<TService>)args);
                }
            };
        }

        public Action<object?>? Invoke { get; set; }
        public Func<IServiceProvider, object?> ParameterFactory => (sp) => sp.GetService<Mock<TService>>();
    }
}
