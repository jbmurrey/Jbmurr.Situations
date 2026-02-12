using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Situations.Core.Benchmark
{
    [MemoryDiagnoser]
    public class ConfiguredServiceBenchmarks
    {
        private static Dictionary<SituationEnum, ISituationInvoker> _situationInvokers = new()
        {
            { SituationEnum.Case1, new SituationInvoker() }
        };

        private static ServiceCollection _services = new ServiceCollection();
        private ServiceProvider _serviceProvider;

        /// <summary>
        /// Sets up all the instances that get reused per test run.
        /// </summary>
        [GlobalSetup]
        public void Setup()
        {
            _services.AddTransient<WorkService>();
            _services.AddScoped<DependencyA>();
            _services.AddScoped<DependencyB>();
            _services.AddScoped<DependencyC>();
            _serviceProvider = _services.BuildServiceProvider();
        }

        /// <summary>
        /// This benchmark tests the performance of the ConfiguredService class when using a DI container
        /// to resolve the service instance. It measures the time taken to create a ConfiguredService instance,
        /// invoke a situation, and call the Work method on the service instance.
        /// </summary>
        [Benchmark]
        public void ConfiguredServiceBenchmark()
        {
            // simulate the process of getting a configured service from the situations container and invoking a situation + instance method.
            using ConfiguredService<WorkService, SituationEnum> configuredService = new(_situationInvokers, _serviceProvider.CreateScope());
            configuredService.InvokeSituation(SituationEnum.Case1);
            configuredService.Instance.Work();
        }

        /// <summary>
        /// This benchmark tests the performance of using the Microsoft.Extensions.DependencyInjection DI container to resolve the WorkService instance and call its Work method.
        /// Used to compare against the ConfiguredService benchmark to see the overhead of using the ConfiguredService class and invoking situations compared to directly 
        /// resolving the service from the DI container and calling its method.
        /// </summary>
        [Benchmark]
        public void MicrosoftDIContainerBenchmark()
        {
            using var scope = _serviceProvider.CreateScope();
            var workService = scope.ServiceProvider.GetRequiredService<WorkService>();
            workService.Work();
        }
    }
}
