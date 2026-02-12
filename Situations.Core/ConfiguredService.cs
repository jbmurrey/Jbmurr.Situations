using Microsoft.Extensions.DependencyInjection;
using Situations.Core.Exceptions;

namespace Situations.Core
{
    public class ConfiguredService<TService, SituationEnum> : IDisposable
        where SituationEnum : Enum
        where TService : class
    {
        private readonly IServiceScope _serviceScope;
        private readonly Dictionary<SituationEnum, ISituationInvoker> _situationActions;
        private bool _disposed = false;

        public ConfiguredService(Dictionary<SituationEnum, ISituationInvoker> situationActions, IServiceScope serviceScope)
        {
            _situationActions = situationActions;
            _serviceScope = serviceScope;
            Instance = _serviceScope.ServiceProvider.GetService<TService>()!;
        }

        public TService Instance { get; private set; }

        public void InvokeSituation(SituationEnum situationsEnum)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            _situationActions.TryGetValue(situationsEnum, out var situationInvoker);

            UnregisteredSituationException.ThrowIf(situationInvoker == null || situationInvoker.Invoke == null, $"Situation: {situationsEnum} has not been registered with an invocation action.");

            object? args = situationInvoker!.ParameterFactory(_serviceScope.ServiceProvider);
            Action<object?> action = situationInvoker.Invoke!;

            action.Invoke(args);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _serviceScope.Dispose();
                    Instance = null!;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
