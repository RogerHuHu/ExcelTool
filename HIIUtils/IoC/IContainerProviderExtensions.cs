using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIIUtils.IoC
{
    public static class IContainerProviderExtensions
    {
        public static T Resolve<T>(this IContainerProvider provider)
        {
            return (T)provider.Resolve(typeof(T));
        }

        public static T Resolve<T>(this IContainerProvider provider, params NewStruct[] parameters)
        {
            return (T)provider.Resolve(typeof(T), parameters);
        }

        public static T Resolve<T>(this IContainerProvider provider, string name, params NewStruct[] parameters)
        {
            return (T)provider.Resolve(typeof(T), name, parameters);
        }

        public static T Resolve<T>(this IContainerProvider provider, string name)
        {
            return (T)provider.Resolve(typeof(T), name);
        }

        public static bool IsRegistered<T>(this IContainerProvider provider)
        {
            return provider.IsRegistered(typeof(T));
        }

        internal static bool IsRegistered(this IContainerProvider provider, Type type)
        {
            if(provider is IContainerProvider registry)
                return registry.IsRegistered(type);
            return false;
        }

        public static bool IsRegistered<T>(this IContainerProvider provider, string name)
        {
            return provider.IsRegistered(typeof(T), name);
        }

        internal static bool IsRegistered(this IContainerProvider provider, Type type, string name)
        {
            if (provider is IContainerProvider registry)
                return registry.IsRegistered(type, name);
            return false;
        }
    }
}
