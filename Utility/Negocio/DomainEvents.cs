using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Utility.Negocio
{
    public static class DomainEvents
    {
        private static List<Type>? _handlers;
        private static IServiceProvider? _serviceProvider;
        private static IHttpContextAccessor? _httpContextAccessor;

        private static IHttpContextAccessor? HttpContextAccessor
        {
            get
            {
                return _httpContextAccessor ??= _serviceProvider?.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            }
        }

        public static void Init(IServiceProvider? serviceProvider, Assembly assembly)
        {
            _serviceProvider = serviceProvider;

            _handlers = assembly
                .GetTypes()
                .Where(x => x.IsInterface &&  x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
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

                dynamic handler = HttpContextAccessor?.HttpContext.RequestServices.GetService(handlerType);

                if (handler == null)
                {
                    handler = Activator.CreateInstance(handlerType) ?? ;
                }

                handler.Handle((dynamic)domainEvent);
            }
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            var domainEvent = args;
            if (_handlers ==null)
            {
                return;
            }

            foreach (var handlerType in _handlers)
            {
                var canHandleEvent = handlerType.GetInterfaces()
                    .Any(x => x.IsGenericType
                              && x.GetGenericTypeDefinition() == typeof(IHandler<>)
                              && x.GenericTypeArguments[0] == domainEvent.GetType());

                if (!canHandleEvent) continue;

                dynamic handler = HttpContextAccessor.HttpContext.RequestServices.GetService(handlerType);

                if (handler == null)
                {
                    handler = Activator.CreateInstance(handlerType);
                }

                handler.Handle((dynamic) domainEvent);
            }
        }
    }
}
