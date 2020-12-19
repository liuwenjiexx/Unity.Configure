using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Configure.Converters
{
    [CustomConverter(typeof(byte))]
    class UInt8Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return (byte)0;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                byte n;
                if (!byte.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }
    }

    [CustomConverter(typeof(sbyte))]
    class Int8Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return (sbyte)0;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                sbyte n;
                if (!sbyte.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(short))]
    class Int16Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return (short)0;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                short n;
                if (!short.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(int))]
    class Int32Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return 0;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                int n;
                if (!int.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(long))]
    class Int64Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return 0L;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                long n;
                if (!long.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(float))]
    class Float32Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return 0f;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                float n;
                if (!float.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(double))]
    class Float64Serializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return 0d;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                double n;
                if (!double.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                return value.ToString();
            }
            throw new NotImplementedException();
        }

    }

    [CustomConverter(typeof(string))]
    class StringSerializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return null;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                return str;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                return (string)value;
            }
            throw new NotImplementedException();
        }
    }
    [CustomConverter(typeof(bool))]
    class BoolSerializer : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return null;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                bool n;
                if (!bool.TryParse(str, out n))
                    throw new ConvertException();
                return n;
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "False";
                return value.ToString();
            }
            throw new NotImplementedException();
        }
    }

    [CustomConverter(typeof(Enum), UseForChildren = true)]
    class EnumConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return 0;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                return Enum.Parse(targetType, str);
            }
            throw new NotImplementedException();
        }
        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "0";
                if (typeof(long).IsInstanceOfType(value))
                    return Convert.ToInt64(value).ToString();
                return Convert.ToInt32(value).ToString();
            }
            throw new NotImplementedException();
        }
    }


}