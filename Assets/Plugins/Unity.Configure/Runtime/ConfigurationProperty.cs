using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Configure
{
    public class ConfigurationProperty : UnityEngine.GUIExtensions.IGUIProperty
    {
        private object value;


        public string Name { get; internal set; }

        public object DefaultValue { get; internal set; }

        public bool HasDefaultValue { get; internal set; }

        public bool IsDefaultValue { get; internal set; }

        public List<MemberInfo> Members { get; internal set; } = new List<MemberInfo>();

        public Type ValueType { get; internal set; }

        public object Value
        {
            get => value;
        }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        //public object[] OptionValues { get; set; }

        //public string[] OptionDisplays { get; set; }

        //public bool HasOptionValues { get; set; }

        //public string EnumSeparator { get; set; }
        //public bool EnumMask { get; set; }

        public string Group { get; set; }
        public int Order { get; set; }

        public bool IsHide { get; set; }


        public bool Changed { get; private set; }

        public string Tooltip { get; set; }


        //public bool Enabled { get; set; }

        public event Action<ConfigurationProperty, object> ValueChanged;
        public static event ConfigurationPropertyChanged PropertyChanged;

        public delegate void ConfigurationPropertyChanged(ConfigurationProperty property, object value);

        private static Dictionary<string, ConfigurationProperty> all;

        public bool SetValue(object value)
        {
            return SetValue(value, raiseEvent: true);
        }

        public bool SetValue(object value, bool raiseEvent = true, bool force = false)
        {
            if (force || !object.Equals(value, this.value))
            {
                object oldValue = this.value;
                this.value = value;
                foreach (var member in Members)
                {
                    if (member.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pInfo = (PropertyInfo)member;
                        pInfo.SetValue(null, value, null);
                    }
                    else if (member.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fInfo = (FieldInfo)member;
                        fInfo.SetValue(null, value);
                    }
                }
                if (raiseEvent)
                {
                    ValueChanged?.Invoke(this, oldValue);
                    PropertyChanged?.Invoke(this, oldValue);
                }
                return true;
            }
            return false;
        }


        public static void RegisterProperty(ConfigurationProperty property)
        {
            all[property.Name] = property;
        }

        public static ConfigurationProperty GetProperty(string name)
        {
            ConfigurationProperty prop;
            if (all.TryGetValue(name, out prop))
                return prop;
            return null;
        }

        public static IEnumerable<ConfigurationProperty> GetAllProperties()
        {
            if (all == null)
            {
                all = new Dictionary<string, ConfigurationProperty>();


                ConfigurableAttribute configAttr;
                object defaultValue;

                foreach (MemberInfo member in AppDomain.CurrentDomain.GetAssemblies()
                    .Referenced(typeof(ConfigurableAttribute).Assembly)
                    .SelectMany(o => o.GetTypes())
                    .SelectMany(o => o.GetMembers()))
                {
                    if (!member.IsDefined(typeof(ConfigurableAttribute), false))
                        continue;
                    configAttr = member.GetCustomAttributes(typeof(ConfigurableAttribute), false).FirstOrDefault() as ConfigurableAttribute;

                    Type valueType = null;

                    if (member.MemberType == MemberTypes.Property)
                    {
                        var pInfo = (PropertyInfo)member;
                        valueType = pInfo.PropertyType;
                    }
                    else if (member.MemberType == MemberTypes.Field)
                    {
                        var fInfo = (FieldInfo)member;
                        valueType = fInfo.FieldType;
                    }
                    else
                    {
                        continue;
                    }

                    string name = null;
                    if (!string.IsNullOrEmpty(configAttr.Name))
                        name = configAttr.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        //name = member.DeclaringType.FullName + "." + member.Name;
                        name = member.Name;
                    }

                    ConfigurationProperty configProperty;

                    if (!all.TryGetValue(name, out configProperty))
                    {
                        configProperty = new ConfigurationProperty();
                        configProperty.Name = name;
                        configProperty.ValueType = valueType;

                        if (!string.IsNullOrEmpty(configAttr.DisplayName))
                        {
                            configProperty.DisplayName = configAttr.DisplayName;
                        }
                        else
                        {
                            string displayName = name;
                            int index = displayName.LastIndexOf('.');
                            if (index >= 0)
                                displayName = displayName.Substring(index + 1);
                            configProperty.DisplayName = displayName;
                        }

                        if (member.GetDefaultValueWithDefaultValueAttribute(out defaultValue))
                        {
                            configProperty.DefaultValue = defaultValue;
                            configProperty.HasDefaultValue = true;
                        }
                        else
                        {
                            bool setDefaultValue = false;
                            if (member.MemberType == MemberTypes.Property)
                            {
                                var pInfo = ((PropertyInfo)member);
                                if (pInfo.GetGetMethod().IsStatic)
                                {
                                    configProperty.DefaultValue = pInfo.GetValue(null, null);
                                    setDefaultValue = true;
                                }
                            }
                            else if (member.MemberType == MemberTypes.Field)
                            {
                                var fInfo = (FieldInfo)member;
                                if (fInfo.IsStatic)
                                {
                                    configProperty.DefaultValue = fInfo.GetValue(null);
                                    setDefaultValue = true;
                                }
                            }

                            if (!setDefaultValue)
                            {
                                if (valueType == typeof(string))
                                {
                                    configProperty.DefaultValue = null;
                                }
                                else if (valueType.IsValueType)
                                {
                                    configProperty.DefaultValue = Activator.CreateInstance(valueType);
                                }
                                else
                                {
                                    configProperty.DefaultValue = null;
                                }
                            }

                        }

                        configProperty.SetValue(configProperty.DefaultValue, raiseEvent: false, force: false);

                        //  configProperty.Drawer = PropertyDrawer.GetDrawer(configProperty.ValueType);

                        all[name] = configProperty;
                    }

                    if (!configProperty.HasDefaultValue)
                    {
                        var defaultValueAttr = member.GetCustomAttribute<System.ComponentModel.DefaultValueAttribute>();
                        if (defaultValueAttr != null)
                        {
                            configProperty.DefaultValue = defaultValueAttr.Value;
                            configProperty.HasDefaultValue = true;
                        }
                    }

                    var descAttr = member.GetCustomAttribute<DescriptionAttribute>();
                    if (descAttr != null)
                        configProperty.Description = descAttr.Description;

                    var tooltipAttr = member.GetCustomAttribute<TooltipAttribute>();
                    if (tooltipAttr != null)
                        configProperty.Tooltip =tooltipAttr.tooltip;
                    

                    //if (!configProperty.HasOptionValues)
                    //{
                    //    var optAttr = member.GetCustomAttribute<EnumValuesAttribute>();
                    //    if (optAttr != null)
                    //    {
                    //        configProperty.HasOptionValues = true;
                    //        configProperty.OptionValues = optAttr.Values;
                    //        configProperty.OptionDisplays = optAttr.DisplayTexts;
                    //        if (configProperty.OptionValues == null)
                    //            configProperty.OptionValues = new object[0];
                    //        if (configProperty.OptionDisplays == null || configProperty.OptionDisplays.Length == 0)
                    //        {
                    //            configProperty.OptionDisplays = configProperty.OptionValues.Select(o => o == null ? string.Empty : o.ToString()).ToArray();
                    //        }
                    //        configProperty.EnumMask = optAttr.IsMask;
                    //        configProperty.EnumSeparator = optAttr.Separator;
                    //    }
                    //}

                    if (!string.IsNullOrEmpty(configAttr.DisplayName))
                        configProperty.DisplayName = configAttr.DisplayName;

                    if (!string.IsNullOrEmpty(configAttr.Category))
                        configProperty.Group = configAttr.Category;
                    configProperty.Order = configAttr.Order;

                    //if (configProperty.DrawerAttribute == null)
                    //{
                    //    var propAttr = member.GetCustomAttribute<ConfigurationPropertyAttribute>();
                    //    configProperty.DrawerAttribute = propAttr;                        
                    //}

                    if (!configProperty.IsHide)
                    {
                        if (member.IsDefined(typeof(HideAttribute), true))
                            configProperty.IsHide = true;
                    }

                    configProperty.Members.Add(member);
                }
            }

            return all.Values;
        }


    }



}
