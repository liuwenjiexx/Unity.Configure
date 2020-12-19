using System;

namespace UnityEngine.Configure
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConfigurableAttribute : Attribute
    {
        public ConfigurableAttribute()
        {
        }

        public ConfigurableAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Category { get; set; }

        public int Order { get; set; }
    }



}