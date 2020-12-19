using System;
using System.Collections.Generic;
using System.Configure;
using System.Linq;

namespace UnityEngine.Configure.Converters
{
    public abstract class ValueConverter
    {
        /// <summary>
        /// Any Type > Custom Type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public abstract object ConvertFrom(object value, Type targetType, object parameter);

        /// <summary>
        /// Custom Type > Any Type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public abstract object ConvertTo(object value, Type targetType, object parameter);

        private static Dictionary<Type, Cached> converters;

        private class Cached
        {
            /// <summary>
            /// Value Type
            /// </summary>
            public Type targetType;
            public int priority;
            public bool useForChildren;
            public Type converterType;
            public ValueConverter converter;
        }

        public static ValueConverter GetConverter(Type targetType)
        {
            Cache();

            Cached cached;
            Type type = targetType;
            while (type != null)
            {
                if (converters.TryGetValue(type, out cached))
                {
                    if (cached.targetType == targetType || cached.useForChildren)
                    {
                        if (cached.converter == null)
                            cached.converter = (ValueConverter)Activator.CreateInstance(cached.converterType);
                        return cached.converter;
                    }
                }
                type = type.BaseType;
            }
            throw new Exception("Converter null :" + targetType);
            return null;
        }

        static void Cache()
        {
            if (converters != null)
                return;

            converters = new Dictionary<Type, Cached>();

            Cached cached;
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .Referenced(typeof(ValueConverter).Assembly)
                .SelectMany(o => o.GetTypes()))
            {
                if (type.IsAbstract)
                    continue;
                if (!typeof(ValueConverter).IsAssignableFrom(type))
                    continue;

                var customAttr = (CustomConverterAttribute)type.GetCustomAttributes(typeof(CustomConverterAttribute), true).FirstOrDefault();
                if (customAttr != null)
                {
                    converters.TryGetValue(customAttr.ValueType, out cached);
                    if (cached == null || cached.priority < customAttr.Priority)
                    {
                        if (cached == null)
                        {
                            cached = new Cached();
                            converters[customAttr.ValueType] = cached;
                        }

                        cached.targetType = customAttr.ValueType;
                        cached.priority = customAttr.Priority;
                        cached.useForChildren = customAttr.UseForChildren;
                        cached.converterType = type;
                        
                    }
                }
            }

        }




    }
}
