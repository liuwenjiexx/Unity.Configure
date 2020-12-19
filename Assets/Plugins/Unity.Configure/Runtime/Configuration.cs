using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine.Configure.Converters;

namespace UnityEngine.Configure
{

    /// <summary>
    /// *config.xml
    /// config.xml > debug.config.xml > user-a.config.xml >user-a.debug.config.xml
    /// </summary>
    [Serializable]
    public abstract class Configuration
    {
        private static List<string> _keywords = new List<string>();

        public const string UserKeywordPrefix = "user-";
        public const string CustomKeywordPrefix = "custom-";

        /// <summary>
        /// <see cref="Debug.isDebugBuild"/>
        /// </summary>
        //public const string DebugKeyword = "debug";

        public static bool IsInitialized { get; private set; }

        public static bool IsLoading { get; private set; }

        public static readonly string DefaultFileName = ConfigName + ExtensionName;
        public const string ExtensionName = ".xml";
        public const string ConfigName = "config";
        public static string ConfigDirectory { get; private set; } = "config";

        public static List<IConfigurationSource> Sources = new List<IConfigurationSource>();

        public static event Action Loading;
        public static event Action Loaded;
        public static Action RestartCallback;

        private static bool? isDebugBuild;

        public static bool IsDebugBuild
        {
            get
            {
                if (isDebugBuild != null)
                    return isDebugBuild.Value;

                return Debug.isDebugBuild;
            }
            set
            {
                isDebugBuild = value;
            }
        }



        public static void Initialize()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            //设置Debug关键字
            //if (IsDebugBuild)
            //    AddKeyword(DebugKeyword);
            //else
            //    RemoveKeyword(DebugKeyword);

            ConfigurationProperty.GetAllProperties();
            Reload();
        }



        public static string[] GetAllPaths(string dir)
        {
            List<string> files = new List<string>();
            //string defaultFile = Path.Combine(dir, DefaultFileName);
            //if (File.Exists(defaultFile))
            //{
            //    files.Add(defaultFile);
            //}
            //files.AddRange(Directory.GetFiles(dir, "*" + DefaultFileName, SearchOption.TopDirectoryOnly));
            if (Directory.Exists(dir))
            {
                foreach (var file in Directory.GetFiles(dir, "*" + ExtensionName, SearchOption.TopDirectoryOnly))
                {
                    string[] parts = Path.GetFileNameWithoutExtension(file).ToLower().Split('.');
                    if (parts.Contains(ConfigName))
                    {
                        files.Add(file);
                    }
                }
            }
            return files.ToArray();
        }


        public static string[] ParseKeywords(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);
            string[] parts;

            parts = name.ToLower().Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == "config")
                {
                    parts[i] = null;
                }
            }
            parts = parts.Where(o => !string.IsNullOrEmpty(o)).ToArray();

            //if (string.Equals(name, DefaultFileName, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    parts = new string[] { };
            //}
            //else if (name.EndsWith("." + DefaultFileName, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    parts = name.Substring(0, name.Length - DefaultFileName.Length - 1).ToLower().Split('.');
            //}
            //else
            //{
            //}

            return parts;
        }

        static string[] OrderKeyword(string[] configNames)
        {
            Dictionary<string, int> orderValues = new Dictionary<string, int>()
            {
                {UserKeywordPrefix,1 },
                {CustomKeywordPrefix,2 }
            };

            foreach (var orderKey in orderValues.OrderByDescending(o => o.Value))
            {
                configNames = configNames.OrderBy(configName =>
                 {
                     return ParseKeywords(configName).Select(o => KeywordToPrefix(o)).Contains(orderKey.Key);
                 }).ToArray();
            }

            return configNames;
        }

        static string KeywordToPrefix(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return string.Empty;
            if (keyword.StartsWith(UserKeywordPrefix))
                return UserKeywordPrefix;
            if (keyword.StartsWith(CustomKeywordPrefix))
                return CustomKeywordPrefix;
            return keyword;
        }


        public static bool IsSavable()
        {
            return (Application.platform == RuntimePlatform.WindowsPlayer || Application.isEditor);
        }



        #region Keyword

        public IList<string> GetKeywords()
        {
            return _keywords;
        }

        public static bool HasKeyword(string keyword)
        {
            return _keywords.Contains(keyword);
        }

        public static void AddKeyword(string keyword)
        {
            if (!HasKeyword(keyword))
                _keywords.Add(keyword);
        }
        public static void RemoveKeyword(string keyword)
        {
            _keywords.Remove(keyword);
        }

        public static void ClearKeyword()
        {
            _keywords.Clear();
        }

        public static bool IsMatchKeyword(string name, IList<string> keywords)
        {
            string[] kws = ParseKeywords(name);
            if (kws.Length == 0 || string.IsNullOrEmpty(kws[0]))
            {
                return true;
            }
            if (kws.Where(o => keywords.Contains(o, StringComparer.InvariantCultureIgnoreCase)).Count() == kws.Length)
            {
                return true;
            }
            return false;
        }

        public static bool IsMatchFullKeyword(string name, IList<string> keywords)
        {
            string[] kws = ParseKeywords(name);

            if (kws.Where(o => keywords.Contains(o, StringComparer.InvariantCultureIgnoreCase)).Count() == keywords.Count)
            {
                return true;
            }
            return false;
        }

        public static void SetKeywordWithPrefix(string prefix, string keyword)
        {
            if (!keyword.StartsWith(prefix))
                throw new Exception(string.Format("version [{0}] not starts with [{1}]", keyword, prefix));

            ClearKeywordWithPrefix(prefix);

            if (string.IsNullOrEmpty(prefix))
            {
                return;
            }

            AddKeyword(keyword);
        }

        public static bool ClearKeywordWithPrefix(string prefix)
        {
            bool changed = false;

            var list = _keywords;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].StartsWith(prefix))
                {
                    list.RemoveAt(i);
                    changed = true;
                }
            }
            return changed;
        }

        public static bool GetKeywordWithPrefix(string prefix, out string keyword)
        {
            var list = _keywords;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StartsWith(prefix))
                {
                    keyword = list[i];
                    return true;
                }
            }
            keyword = null;
            return false;
        }

        public static void SetUserKeyword(string user)
        {
            SetKeywordWithPrefix(UserKeywordPrefix, UserKeywordPrefix + user);
        }

        public static bool RemoveUserKeyword()
        {
            return ClearKeywordWithPrefix(UserKeywordPrefix);
        }

        public static bool GetUserKeyword(out string user)
        {
            string kw;
            if (GetKeywordWithPrefix(UserKeywordPrefix, out kw))
            {
                user = kw.Substring(UserKeywordPrefix.Length);
                return true;
            }
            user = null;
            return false;
        }

        #endregion



        public static void Reload()
        {
            try
            {

                IsLoading = true;
                Loading?.Invoke();

                string kwUser;
                if (!GetUserKeyword(out kwUser))
                    SetUserKeyword(string.Empty);


                Dictionary<ConfigurationProperty, object> values = new Dictionary<ConfigurationProperty, object>();
                foreach (var src in Sources)
                {
                    Load(src, _keywords, values);
                }

                foreach (var prop in ConfigurationProperty.GetAllProperties())
                {
                    if (!values.ContainsKey(prop))
                    {
                        prop.SetValue(prop.DefaultValue, raiseEvent: false, force: true);
                        prop.IsDefaultValue = true;
                    }
                }

                foreach (var item in values)
                {
                    var prop = item.Key;
                    object value = item.Value;
                    if (prop.SetValue(value, raiseEvent: true, force: true))
                    {
                        prop.IsDefaultValue = false;
                    }
                }

                IsLoading = false;
                Loaded?.Invoke();
            }
            catch
            {

            }
            finally
            {
                IsLoading = false;
            }
        }




        public static string[] Load(IConfigurationSource source, IList<string> keywords, Dictionary<ConfigurationProperty, object> values)
        {
            List<string> list = new List<string>();

            if (keywords == null)
                keywords = new string[0];

            foreach (var item in source.GetConfigNames().Where(o => !string.IsNullOrEmpty(o)))
            {
                if (IsMatchKeyword(item, keywords))
                {
                    list.Add(item);
                }
            }
            //for (int i = keywords.Count - 1; i >= 0; i--)
            //{
            //    string keyword = keywords[i];
            //    list = list.OrderBy(o => ParseKeywords(o).Contains(keyword, StringComparer.InvariantCultureIgnoreCase)).ToList();
            //}
            //list = list.OrderBy(o => !string.IsNullOrEmpty(o)).ToList();
            string[] ordered = list.ToArray();
            ordered = OrderKeyword(ordered);

            foreach (var configName in ordered)
            {

                Load(configName, source.LoadData(configName), values);
            }
            return ordered;
        }




        static void Load(string configName, string data, Dictionary<ConfigurationProperty, object> values)
        {
            if (string.IsNullOrEmpty(data))
                return;
            //Debug.Log(configName);
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                string name;
                string strValue;
                object value;
                XmlAttribute nodeAttr;
                foreach (XmlNode node in doc.SelectNodes("/config/*"))
                {
                    nodeAttr = node.Attributes["name"];
                    if (nodeAttr == null)
                        continue;
                    name = nodeAttr.Value;

                    var prop = ConfigurationProperty.GetProperty(name);
                    if (prop == null)
                        continue;
                    strValue = node.InnerText;

                    if (string.IsNullOrEmpty(strValue))
                        value = prop.DefaultValue;
                    else
                    {
                        var converter = ValueConverter.GetConverter(prop.ValueType);
                        if (converter != null)
                            value = converter.ConvertFrom(strValue, prop.ValueType, null);
                        else
                        {
                            if (prop.ValueType.IsEnum)
                                value = Enum.Parse(prop.ValueType, strValue);
                            else
                                value = Convert.ChangeType(strValue, prop.ValueType);
                        }
                    }
                    values[prop] = value;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }


        public static void Save(string filePath, bool defaultValue = false, bool ignoreDefaultValue = true)
        {

            XmlDocument doc = new XmlDocument();
            XmlNode node;
            XmlAttribute attr;
            XmlNode configNode = doc.CreateElement("config");
            object value;

            foreach (var prop in ConfigurationProperty.GetAllProperties())
            {

                if (defaultValue)
                    value = prop.DefaultValue;
                else
                    value = prop.Value;

                if (ignoreDefaultValue && object.Equals(prop.DefaultValue, value))
                    continue;

                node = doc.CreateElement("string");
                attr = doc.CreateAttribute("name");
                attr.Value = prop.Name;
                node.Attributes.Append(attr);

                var converter = ValueConverter.GetConverter(prop.ValueType);
                if (converter != null)
                    node.InnerText = (string)converter.ConvertTo(value, typeof(string), null);
                else
                    node.InnerText = value == null ? "" : value.ToString();
                configNode.AppendChild(node);

            }

            doc.AppendChild(configNode);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            doc.Save(filePath);
        }

    }




}
