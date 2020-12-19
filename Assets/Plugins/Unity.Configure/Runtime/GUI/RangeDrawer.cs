using System;
using UnityEngine.GUIExtensions;

namespace UnityEngine.Configure.GUI
{
    [CustomGUIPropertyDrawer(typeof(RangeAttribute))]
    class RangeDrawer : GUIPropertyDrawer
    {
        public override void OnGUILayout(IGUIProperty property, Attribute attribute)
        {
           RangeAttribute attr = attribute as RangeAttribute;
            using (new GUILayout.HorizontalScope())
            {
                GUILayoutx.PrefixLabel(property);
                float value = (float)property.Value;
                value = GUILayout.HorizontalSlider(value, attr.Min, attr.Max);
                property.SetValue(value);
            }
        }
    }
}