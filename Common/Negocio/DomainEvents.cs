using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Negocio;

public static class DomainEvents
{
    private static List<Type>? _handlers;
    private static IServiceProvider? _serviceProvider;

    public static void Init(IServiceProvider? serviceProvider, Assembly? assembly = null)
    {
        _serviceProvider = serviceProvider;

        _handlers = (assembly ?? Assembly.GetExecutingAssembly())
            .GetTypes() 
            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
            .ToList();
    }

    public static void Dispatch(IDomainEvent domainEvent)
    {
        if (_handlers == null) return;

        foreach (var handlerType in _handlers)
        {
            var canHandleEvent = handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                          && x.GetGenericTypeDefinition() == typeof(IHandler<>)
                          && x.GenericTypeArguments[0] == domainEvent.GetType());

            if (!canHandleEvent) continue;

            dynamic handler = _serviceProvider?.GetRequiredService(handlerType)!;

            if (handler == null)
            {
                return;
            }

            handler.Handle((dynamic)domainEvent);
        }
    }
}