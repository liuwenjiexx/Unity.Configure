using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace UnityEngine.Configure
{
    internal static class Extension
    {
        public static IEnumerable<Assembly> Referenced(this IEnumerable<Assembly> assemblies, Assembly referenced)
        {
            string fullName = referenced.FullName;

            foreach (var ass in assemblies)
            {
                if (referenced == ass)
                {
                    yield return ass;
                }
                else
                {
                    foreach (var refAss in ass.GetReferencedAssemblies())
                    {
                        if (fullName == refAss.FullName)
                        {
                            yield return ass;
                            break;
                        }
                    }
                }
            }
        }

        public static bool GetDefaultValueWithDefaultValueAttribute(this ICustomAttributeProvider property, out object defaultValue)
        {
            var attrs = property.GetCustomAttributes(typeof(DefaultValueAttribute), false);
            if (attrs == null || attrs.Length == 0)
            {
                defaultValue = null;
                return false;
            }
            var attr = attrs[0] as DefaultValueAttribute;
            defaultValue = attr.Value;
            return true;
            //var propertyType = property.PropertyType;
            //return propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
        }
    }

}