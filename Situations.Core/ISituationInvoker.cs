namespace Situations.Core
{
    public interface ISituationInvoker
    {
        Action<object?>? Invoke { get; set; }
        Func<IServiceProvider, object?> ParameterFactory { get; }
    }
}
