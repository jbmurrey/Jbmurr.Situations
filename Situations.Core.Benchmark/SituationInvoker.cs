namespace Situations.Core.Benchmark
{
    public class SituationInvoker : ISituationInvoker
    {
        public Action<object?>? Invoke { get; set; } = (_) => { };

        public Func<IServiceProvider, object?> ParameterFactory => (_) => null;
    }
}
