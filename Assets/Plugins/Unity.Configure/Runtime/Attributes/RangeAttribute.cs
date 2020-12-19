using System;

namespace UnityEngine.Configure
{
    public class RangeAttribute : ConfigurationPropertyAttribute
    {
        public RangeAttribute(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }
        public float Min { get; set; }
        public float Max { get; set; }
    }

    public class ConfigurationPropertyAttribute : Attribute
    {
    }
}