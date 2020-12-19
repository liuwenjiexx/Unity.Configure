using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEngine.Configure.Converters
{
    public class CustomConverterAttribute : Attribute
    {
        public CustomConverterAttribute(Type valueType)
        {
            ValueType = valueType;
        }

        public Type ValueType { get; set; }
        public int Priority { get; set; }
        public bool UseForChildren { get; set; }
    }

    public class ConvertException : Exception
    {

    }

}