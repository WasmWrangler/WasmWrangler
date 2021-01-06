using System;
using System.Collections.Generic;

namespace WasmWrangler
{
    public static class JSObjectWrapperFactory
    {
        private static Dictionary<Type, Func<object, object>> _factories = new Dictionary<Type, Func<object, object>>();

        public static void RegisterFactory(Type type, Func<object, object> factory)
        {
            _factories[type] = factory;
        }

        public static T Create<T>(object obj)
        {
            return (T)_factories[typeof(T)].Invoke(obj);
        }
    }
}
