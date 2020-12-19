using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace UnityEngine.Configure.Converters
{

    [CustomConverter(typeof(Vector2))]
    class Vector2Converter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Vector2 result = new Vector2();
            if (value == null)
                return result;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                if (str.StartsWith("("))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                string[] parts = str.Split(',');

                for (int i = 0; i < parts.Length; i++)
                {
                    float n;
                    if (float.TryParse(parts[i], out n))
                    {
                        switch (i)
                        {
                            case 0:
                                result.x = n;
                                break;
                            case 1:
                                result.y = n;
                                break;
                        }
                    }
                }
                return result;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "(0,0)";
                Vector2 v = (Vector2)value;
                return string.Format("({0:0.###},{1:0.###})", v.x, v.y);
            }
            throw new NotImplementedException();
        }
    }
    [CustomConverter(typeof(Vector3))]
    class Vector3Converter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Vector3 result = new Vector3();
            if (value == null)
                return result;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                if (str.StartsWith("("))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                string[] parts = str.Split(',');

                for (int i = 0; i < parts.Length; i++)
                {
                    float n;
                    if (float.TryParse(parts[i], out n))
                    {
                        switch (i)
                        {
                            case 0:
                                result.x = n;
                                break;
                            case 1:
                                result.y = n;
                                break;
                            case 2:
                                result.z = n;
                                break;
                        }
                    }
                }
                return result;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "(0,0,0)";
                Vector3 v = (Vector3)value;
                return string.Format("({0:0.###},{1:0.###},{2:0.###})", v.x, v.y, v.z);
            }
            throw new NotImplementedException();
        }
    }

    [CustomConverter(typeof(Vector3Int))]
    class Vector3IntConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Vector3Int result = new Vector3Int();
            if (value == null)
                return result;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                if (str.StartsWith("("))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                string[] parts = str.Split(',');

                for (int i = 0; i < parts.Length; i++)
                {
                    int n;
                    if (int.TryParse(parts[i], out n))
                    {
                        switch (i)
                        {
                            case 0:
                                result.x = n;
                                break;
                            case 1:
                                result.y = n;
                                break;
                            case 2:
                                result.z = n;
                                break;
                        }
                    }
                }
                return result;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "(0,0,0)";
                Vector3Int v = (Vector3Int)value;
                return string.Format("({0},{1},{2})", v.x, v.y, v.z);
            }
            throw new NotImplementedException();
        }
    }

    [CustomConverter(typeof(Vector4))]
    class Vector4Converter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Vector4 result = new Vector4();
            if (value == null)
                return result;

            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                if (str.StartsWith("("))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                string[] parts = str.Split(',');

                for (int i = 0; i < parts.Length; i++)
                {
                    float n;
                    if (float.TryParse(parts[i], out n))
                    {
                        switch (i)
                        {
                            case 0:
                                result.x = n;
                                break;
                            case 1:
                                result.y = n;
                                break;
                            case 2:
                                result.z = n;
                                break;
                            case 3:
                                result.w = n;
                                break;
                        }
                    }
                }
                return result;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "(0,0,0,0)";
                Vector4 v = (Vector4)value;
                return string.Format("({0:0.###},{1:0.###},{2:0.###},{3:0.###})", v.x, v.y, v.z, v.w);
            }
            throw new NotImplementedException();
        }
    }


    [CustomConverter(typeof(Vector2Int))]
    class Vector2IntConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Vector2Int result = new Vector2Int();
            if (value == null)
                return result;
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                if (str.StartsWith("("))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                string[] parts = str.Split(',');

                for (int i = 0; i < parts.Length; i++)
                {
                    int n;
                    if (int.TryParse(parts[i], out n))
                    {
                        switch (i)
                        {
                            case 0:
                                result.x = n;
                                break;
                            case 1:
                                result.y = n;
                                break;
                        }
                    }
                }
                return result;
            }
            throw new NotImplementedException();
        }


        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "(0,0)";
                Vector2Int v = (Vector2Int)value;
                return string.Format("({0},{1})", v.x, v.y);
            }
            throw new NotImplementedException();
        }
    }


    [CustomConverter(typeof(Color))]
    class ColorConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return new Color(0f, 0f, 0f, 0f);
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                Color v;
                if (!TryParseColor(str, out v))
                    throw new ConvertException();
                return v;
            }
            throw new NotImplementedException();
        }
        static bool TryParseColor(string colorString, out Color color)
        {
            color = new Color();
            colorString = colorString.Trim();

            if (colorString.StartsWith("#"))
            {
                int n;

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (i * 2 + 1 > colorString.Length - 2)
                        {
                            if (i == 3)
                            {
                                n = 0xff;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            n = int.Parse(colorString.Substring(i * 2 + 1, 2), System.Globalization.NumberStyles.HexNumber);
                        }
                        switch (i)
                        {
                            case 0:
                                color.r = Mathf.Clamp01(n / 255f);
                                break;
                            case 1:
                                color.g = Mathf.Clamp01(n / 255f);
                                break;
                            case 2:
                                color.b = Mathf.Clamp01(n / 255f);
                                break;
                            case 3:
                                color.a = Mathf.Clamp01(n / 255f);
                                return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
            }
            else if (colorString.StartsWith("hsv(", StringComparison.InvariantCultureIgnoreCase) && colorString.EndsWith(")"))
            {
                int i = 0;
                int n;
                float h = 0, s = 0, v = 0;
                foreach (var part in colorString.Substring(4, colorString.Length - 5).Split(','))
                {
                    if (!int.TryParse(part, out n))
                    {
                        return false;
                    }
                    switch (i++)
                    {
                        case 0:
                            h = n / 359f;
                            break;
                        case 1:
                            s = n / 255f;
                            break;
                        case 2:
                            v = n / 255f;
                            break;
                    }
                }
                color = Color.HSVToRGB(h, s, v);
                return true;
            }
            else
            {
                int rIdx = 0, gIdx = 1, bIdx = 2, aIdx = 3;

                if (colorString.StartsWith("rgba(", StringComparison.InvariantCultureIgnoreCase) && colorString.EndsWith(")"))
                {
                    colorString = colorString.Substring(5, colorString.Length - 6);
                }
                int i = 0;
                float n;
                foreach (var part in colorString.Split(','))
                {
                    if (!float.TryParse(part, out n))
                    {
                        return false;
                    }
                    if (i == rIdx)
                        color.r = n;
                    else if (i == gIdx)
                        color.g = n;
                    else if (i == bIdx)
                        color.b = n;
                    else if (i == aIdx)
                        color.a = n;
                    i++;
                }

                return true;
            }
            return false;
        }

        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            Color color = (Color)value;
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "rgba(0,0,0,0)";
                return string.Format("rgba({0:0.###},{1:0.###},{2:0.###},{3:0.###})", color.r, color.g, color.b, color.a);
            }
            throw new NotImplementedException();
        }
    }


    [CustomConverter(typeof(Color32))]
    class Color32Converter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return new Color32(0, 0, 0, 0);
            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                string str = (string)value;
                Color32 v;
                if (!TryParseColor32(str, out v))
                    throw new ConvertException();
                return v;
            }
            throw new NotImplementedException();
        }
        static bool TryParseColor32(string colorString, out Color32 color)
        {
            color = new Color32();
            colorString = colorString.Trim();

            if (colorString.StartsWith("#"))
            {
                int n;

                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (i * 2 + 1 > colorString.Length - 2)
                        {
                            if (i == 3)
                            {
                                n = 0xff;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            n = int.Parse(colorString.Substring(i * 2 + 1, 2), System.Globalization.NumberStyles.HexNumber);
                        }
                        switch (i)
                        {
                            case 0:
                                color.r = (byte)n;
                                break;
                            case 1:
                                color.g = (byte)n;
                                break;
                            case 2:
                                color.b = (byte)n;
                                break;
                            case 3:
                                color.a = (byte)n;

                                return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
            }
            else if (colorString.StartsWith("hsv(", StringComparison.InvariantCultureIgnoreCase) && colorString.EndsWith(")"))
            {
                int i = 0;
                int n;
                float h = 0, s = 0, v = 0;
                foreach (var part in colorString.Substring(4, colorString.Length - 5).Split(','))
                {
                    if (!int.TryParse(part, out n))
                    {
                        return false;
                    }
                    switch (i++)
                    {
                        case 0:
                            h = n / 359f;
                            break;
                        case 1:
                            s = n / 255f;
                            break;
                        case 2:
                            v = n / 255f;
                            break;
                    }
                }
                color = Color.HSVToRGB(h, s, v);
                return true;
            }
            else
            {
                int rIdx = 0, gIdx = 1, bIdx = 2, aIdx = 3;
                if (colorString.StartsWith("rgba(", StringComparison.InvariantCultureIgnoreCase) && colorString.EndsWith(")"))
                {
                    colorString = colorString.Substring(5, colorString.Length - 6);
                }

                int i = 0;
                byte n;

                foreach (var part in colorString.Split(','))
                {
                    if (!byte.TryParse(part, out n))
                    {
                        return false;
                    }
                    if (i == rIdx)
                        color.r = n;
                    else if (i == gIdx)
                        color.g = n;
                    else if (i == bIdx)
                        color.b = n;
                    else if (i == aIdx)
                        color.a = n;
                    i++;
                }
                return true;
            }
            return false;
        }

        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            Color32 color = (Color32)value;
            if (targetType == typeof(string))
            {
                if (value == null)
                    return "rgba(0,0,0,0)";
                return string.Format("rgba({0},{1},{2},{3})", color.r, color.g, color.b, color.a);
            }
            throw new NotImplementedException();
        }
    }


    [CustomConverter(typeof(Rect))]
    class RectConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            Rect result = new Rect();
            if (value == null)
                return result;
            if (value.GetType() == typeof(string))
            {
                string str = (string)value;
                if (!string.IsNullOrEmpty(str))
                {
                    float n;
                    string[] parts = str.Split(',');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (float.TryParse(parts[i], out n))
                        {
                            switch (i)
                            {
                                case 0:
                                    result.x = n;
                                    break;
                                case 1:
                                    result.y = n;
                                    break;
                                case 2:
                                    result.width = n;
                                    break;
                                case 3:
                                    result.height = n;
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }

    [CustomConverter(typeof(RectInt))]
    class RectIntConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            RectInt result = new RectInt();
            if (value == null)
                return result;

            if (value.GetType() == typeof(string))
            {
                string str = (string)value;
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.StartsWith("("))
                        str = str.Substring(1, str.Length - 2);

                    int n;
                    string[] parts = str.Split(',');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (int.TryParse(parts[i], out n))
                        {
                            switch (i)
                            {
                                case 0:
                                    result.x = n;
                                    break;
                                case 1:
                                    result.y = n;
                                    break;
                                case 2:
                                    result.width = n;
                                    break;
                                case 3:
                                    result.height = n;
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }


    [CustomConverter(typeof(AnimationCurve))]
    class AnimationCurveConverter : ValueConverter
    {
        public override object ConvertFrom(object value, Type targetType, object parameter)
        {
            if (value == null)
                return new AnimationCurve();

            AnimationCurve curve;

            if (value.GetType() == typeof(string))
            {
                string str = (string)value;
                str = str.Trim();

                if (str.Length > 0 && str[0] == '[')
                    str = str.Substring(1);
                if (str.Length > 0 && str[str.Length - 1] == ']')
                    str = str.Substring(0, str.Length - 1);

                float time, val, inTangent, outTangent, inWeight, outWeight;
                Keyframe keyframe;
                List<Keyframe> keys = new List<Keyframe>();
                foreach (var part in str.Split(']'))
                {
                    string tmp = part.Trim();
                    if (tmp.Length == 0)
                        continue;
                    if (tmp[0] == ',')
                        tmp = tmp.Substring(2);
                    else
                        tmp = tmp.Substring(1);

                    string[] arr = tmp.Split(',');

                    time = float.Parse(arr[0]);
                    val = float.Parse(arr[1]);

                    if (arr.Length > 2)
                        inTangent = float.Parse(arr[2]);
                    else
                        inTangent = 0f;
                    if (arr.Length > 3)
                        outTangent = float.Parse(arr[3]);
                    else
                        outTangent = 0f;
                    if (arr.Length > 4)
                        inWeight = float.Parse(arr[4]);
                    else
                        inWeight = 0f;
                    if (arr.Length > 5)
                        outWeight = float.Parse(arr[5]);
                    else
                        outWeight = 0f;
                    keyframe = new Keyframe(time, val, inTangent, outTangent, inWeight, outWeight);
                    keys.Add(keyframe);
                }

                curve = new AnimationCurve(keys.ToArray());
            }
            else
            {
                curve = AnimationCurve.Linear(0, 0, 1, 1);
            }

            return curve;
        }

        public override object ConvertTo(object value, Type targetType, object parameter)
        {
            if (value == null)
            {
                if (targetType == typeof(string))
                    return "[]";
            }

            AnimationCurve curve = (AnimationCurve)value;

            if (targetType == typeof(string))
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                var keys = curve.keys;

                for (int i = 0, len = keys.Length; i < len; i++)
                {
                    var key = keys[i];

                    if (i > 0)
                        sb.Append(",");
                    sb.Append('[')
                        .Append(ToString3(key.time))
                        .Append(',')
                        .Append(ToString3(key.value))
                        .Append(',')
                        .Append(ToString3(key.inTangent))
                        .Append(',')
                        .Append(ToString3(key.outTangent))
                        .Append(',')
                        .Append(ToString3(key.inWeight))
                        .Append(',')
                        .Append(ToString3(key.outWeight))
                        .Append(']');
                }
                sb.Append("]");
                return sb.ToString();
            }
            throw new NotImplementedException();
        }
        static string ToString3(float f)
        {
            return f.ToString("0.###");
        }
    }


}