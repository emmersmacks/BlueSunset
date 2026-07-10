using System;
using System.Collections.Generic;
using System.Linq;
using CascadeDI.Builder;

namespace CutTwice.Core.Lifecycle
{
    public static class DIFrameworkExtensions
    {
        public static IContainerBuilder RegisterTransientWithLifetime<TBase, TImplementation>(
            this IContainerBuilder builder, params Type[] serviceTypes)
        {
            var types = serviceTypes != null ? serviceTypes.ToList() : new List<Type>();
            
            if (typeof(ILifecycleObject).IsAssignableFrom(typeof(TImplementation)))
            {
                types.Add(typeof(ILifecycleObject));
            }
            
            types.Add(typeof(TBase));
            
            return builder.RegisterTransientFor<TImplementation>(types.ToArray());
        }
        
        public static IContainerBuilder RegisterSingletonWithLifetime<T>(
            this IContainerBuilder builder, List<Type> types = null)
        {
            types ??= new List<Type>();
            
            if (typeof(ILifecycleObject).IsAssignableFrom(typeof(T)))
            {
                types.Add(typeof(ILifecycleObject));
            }
            
            types.Add(typeof(T));
            
            return builder.RegisterSingletonFor<T>(types.ToArray());
        }
        
        public static IContainerBuilder RegisterSingletonWithLifetime<TBase, TImplementation>(
            this IContainerBuilder builder, List<Type> types = null)
        {
            types ??= new List<Type>();
            
            if (typeof(ILifecycleObject).IsAssignableFrom(typeof(TImplementation)))
            {
                types.Add(typeof(ILifecycleObject));
            }
            
            types.Add(typeof(TBase));
            
            return builder.RegisterSingletonFor<TImplementation>(types.ToArray());
        }
        
        public static IContainerBuilder RegisterSingletonWithLifetime<T>(
            this IContainerBuilder builder, T instance, List<Type> types = null)
        {
            types ??= new List<Type>();
            
            if (typeof(ILifecycleObject).IsAssignableFrom(typeof(T)))
            {
                types.Add(typeof(ILifecycleObject));
            }
            
            types.Add(typeof(T));
            
            return builder.RegisterSingletonFor(instance, types.ToArray());
        }
    }
}