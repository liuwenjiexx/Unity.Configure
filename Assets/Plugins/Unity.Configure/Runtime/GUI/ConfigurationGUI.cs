using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Configure;
using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.GUIExtensions;
using UnityEngine.Profiling;

namespace UnityEngine.Configure.GUI
{
    using GUI = UnityEngine.GUI;

    public class ConfigurationGUI : MonoBehaviour
    {

        private int hotControl;
        private int keyboardControl;


        private Vector2 scrollPos;
        private bool lastIsShow;

        private static string[] allCustomKeywords;
        /// <summary>
        /// 是否启用 UI
        /// </summary>
        public static bool IsEnabled = false;
        /// <summary>
        /// 是否显示 UI
        /// </summary>
        public static bool IsShow;
        static string KeywordPrefix = Configuration.CustomKeywordPrefix;
        private static ConfigurationGUI instance;

        /// <summary>
        /// 触发按钮，点击后显示UI界面
        /// </summary>
        public static Rect ShowButtonScreenRect = new Rect(0.95f, 0f, 0.05f, 0.05f);

        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;
            if (!Application.isPlaying)
                return;

            initialized = true;
            if (Configuration.IsDebugBuild)
            {
                if (!string.IsNullOrEmpty(CustomKeyword))
                {
                    Configuration.AddKeyword(CustomKeyword);
                }

                instance = new GameObject(typeof(ConfigurationGUI).Name).AddComponent<ConfigurationGUI>();
                //instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
                DontDestroyOnLoad(instance.gameObject);

                InitializeEditor();

                foreach (var prop in ConfigurationProperty.GetAllProperties())
                {
                    if (prop.Members.Count > 0)
                    {
                        foreach (var member in prop.Members)
                        {
                            if (!prop.IsHide)
                            {
                                if (member.IsDefined(typeof(HideInInspector), true))
                                    prop.IsHide = true;
                            }
                        }
                    }
                }

            }
        }

        private static string cachedCustomKeyword;
        /// <summary>
        /// 用户定制配置
        /// </summary>
        static string CustomKeyword
        {
            get
            {
                if (cachedCustomKeyword == null)
                {
                    cachedCustomKeyword = PlayerPrefs.GetString("Config.Keyword.Custom", string.Empty);
                    if (cachedCustomKeyword == null)
                        cachedCustomKeyword = string.Empty;
                }
                return cachedCustomKeyword;
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                if (value != CustomKeyword)
                {
                    foreach (var kw in allCustomKeywords)
                    {
                        Configuration.RemoveKeyword(kw);
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        Configuration.AddKeyword(value);
                    }
                    cachedCustomKeyword = value;
                    PlayerPrefs.SetString("Config.Keyword.Custom", value);
                    PlayerPrefs.Save();
                }
            }
        }

        static string DirectoryPath
        {
            get
            {
                string dir = null;
                dir = Path.GetFullPath(Path.Combine(Application.persistentDataPath, Configuration.ConfigDirectory));
                return dir;
            }
        }

        static object CreateDefaultValue(Type valueType)
        {

            object defaultValue;
            if (valueType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(valueType);
            }
            else
            {
                defaultValue = null;
            }
            return defaultValue;
        }

        public static GUISkin skin;
        float? prevPauseTimeScale;
        void OnShow()
        {
            Reload();

            if (Options.Instance.pauseOnShow)
            {
                prevPauseTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }

        }

        void OnHide()
        {
            if (prevPauseTimeScale.HasValue)
            {
                Time.timeScale = prevPauseTimeScale.Value;
                prevPauseTimeScale = null;
            }
        }

        void Reload()
        {

            string[] configPaths = Configuration.GetAllPaths(DirectoryPath);

            var keywords = new HashSet<string>();
            foreach (var path in configPaths)
            {
                var kws = Configuration.ParseKeywords(path);
                string kwDebug = kws.Where(o => o.StartsWith(KeywordPrefix)).FirstOrDefault();

                if (!string.IsNullOrEmpty(kwDebug))
                {
                    keywords.Add(kwDebug);
                }
            }

            keywords.Add(KeywordPrefix);
            allCustomKeywords = keywords.OrderBy(o => o).ToArray();

        }

        static string FindUserConfigPath(string dir, IList<string> keywords)
        {
            string customKeyword = CustomKeyword;

            foreach (var path in Configuration.GetAllPaths(dir))
            {
                if (Configuration.IsMatchFullKeyword(path, keywords))
                {
                    return path;
                }
            }
            return null;
        }
        static string FindCustomConfigPath()
        {
            string path = null;
            string user;
            if (!Configuration.GetKeywordWithPrefix(Configuration.UserKeywordPrefix, out user))
                user = Configuration.UserKeywordPrefix;

            path = FindUserConfigPath(DirectoryPath, string.IsNullOrEmpty(CustomKeyword) ? new string[] { user } : new string[] { user, CustomKeyword });

            return path;
        }


        public void ResetConfig(string path)
        {

            if (!string.IsNullOrEmpty(path))
            {
                Configuration.Save(path, defaultValue: true);
            }

        }




        public void SaveCustomConfig()
        {
            string path = null;

            path = FindCustomConfigPath();

            if (!string.IsNullOrEmpty(path))
            {
                Configuration.Save(path);
            }
        }

        int FindUserKeywordIndex()
        {
            int index = -1;
            for (int i = 0; i < allCustomKeywords.Length; i++)
            {
                if (allCustomKeywords[i] == CustomKeyword)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static string GetPathWithCustomKeyword(string customKeyword)
        {
            string path;
            string kwUser;

            if (!Configuration.GetKeywordWithPrefix(Configuration.UserKeywordPrefix, out kwUser))
                kwUser = Configuration.UserKeywordPrefix;

            if (string.IsNullOrEmpty(customKeyword))
                path = Path.Combine(DirectoryPath, KeywordPrefix + "." + Configuration.DefaultFileName);
            else
                path = Path.Combine(DirectoryPath, customKeyword + "." + kwUser + "." + Configuration.DefaultFileName);
            return path;
        }

        public bool ExistsCustomFile(string keyword)
        {
            string path = GetPathWithCustomKeyword(keyword);
            return File.Exists(path);
        }

        public bool IsChanged(string keyword)
        {
            if (keyword == CustomKeyword)
            {
                return changed || !ExistsCustomFile(keyword);
            }
            return false;
        }

        bool changed;
        string newItem;
        bool isEditNewItem;
        int newItemInputId;
        bool showUIOptions;

        [Serializable]
        class Options
        {

            public float scale = 1f;
            public float width = 1f;
            public string skin;
            public bool pauseOnShow = true;

            private static Options instance;

            public static Options Instance
            {
                get
                {
                    if (instance == null)
                    {
                        try
                        {
                            string json = PlayerPrefs.GetString("ConfigurationGUI.Options", string.Empty);
                            if (!string.IsNullOrEmpty(json))
                            {
                                instance = JsonUtility.FromJson<Options>(json);
                            }
                        }
                        catch
                        {
                        }
                        if (instance == null)
                        {
                            instance = new Options();
                        }
                    }
                    return instance;
                }
                set => instance = value;
            }

            public void Save()
            {
                string json = JsonUtility.ToJson(this);
                PlayerPrefs.SetString("ConfigurationGUI.Options", json);
                PlayerPrefs.Save();
            }

        }
        string selectedPath;

        //gc 24.6k
        private void OnGUI()
        {


            if (!IsEnabled || !Configuration.IsDebugBuild)
                return;

            if (GUI.Button(new Rect(Screen.width * ShowButtonScreenRect.x, Screen.height * ShowButtonScreenRect.y, Screen.width * ShowButtonScreenRect.width, Screen.height * ShowButtonScreenRect.height), GUIContent.none, GUIStyle.none))
            {
                IsShow = !IsShow;
            }

            if (IsShow != lastIsShow)
            {
                lastIsShow = IsShow;
                if (IsShow)
                {
                    OnShow();
                }
                else
                {
                    OnHide();
                }
            }

            if (!IsShow)
                return;

            if (skin)
            {
                GUI.skin = skin;
            }
            else
            {
                skin = GUILayoutx.LightSkin;
            }


            var oldMat = GUI.matrix;
            GUI.matrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * Options.Instance.scale);

            GUIStyle boxStyle = "box";

            float maxWidth = 300;
            maxWidth = Screen.width * Options.Instance.width - boxStyle.margin.horizontal;


            using (new GUILayout.VerticalScope(GUILayout.Height(Screen.height - boxStyle.margin.vertical)))
            {

                DrawOptions();


                hotControl = GUIUtility.hotControl;
                keyboardControl = GUIUtility.keyboardControl;

                int selectedIndex = FindUserKeywordIndex();
                selectedPath = FindCustomConfigPath();
                if (string.IsNullOrEmpty(selectedPath))
                    selectedPath = GetPathWithCustomKeyword(CustomKeyword);
                GUIContent[] items = allCustomKeywords.Select(o => new GUIContent((o == KeywordPrefix ? "Default" : o.Substring(KeywordPrefix.Length)) + (IsChanged(o) ? "*" : ""))).ToArray();



                using (new GUILayout.VerticalScope("box", GUILayout.Width(maxWidth)))
                {

                    using (new GUILayout.HorizontalScope())
                    {


                        for (int i = 0; i < items.Length; i++)
                        {
                            bool isDefault = false;
                            if (allCustomKeywords[i] == Configuration.CustomKeywordPrefix)
                            {
                                isDefault = true;
                            }

                            if (selectedIndex == i)
                            {
                                GUI.color = Color.yellow;
                                
                                if (isDefault)
                                {
                                    if (!File.Exists(selectedPath))
                                    {
                                        Configuration.Save(selectedPath);
                                    }
                                }
                            }

                            

                            if (GUILayout.Button(items[i]))
                            {
                                if (i == selectedIndex)
                                {
                                    Configuration.Save(selectedPath);
                                }
                                else
                                {
                                    selectedIndex = i;
                                    CustomKeyword = allCustomKeywords[selectedIndex];
                                    selectedPath = GetPathWithCustomKeyword(CustomKeyword);
                                    if (!File.Exists(selectedPath))
                                    {
                                        Configuration.Save(selectedPath);
                                    }
                                    Configuration.Reload();
                                }
                                changed = false;
                            }

                            GUI.color = Color.white;
                        }


                        DrawNewItemInput();

                    }


                    DrawProperties();

                    using (new GUILayout.HorizontalScope())
                    {
                        if (string.IsNullOrEmpty(selectedPath) || !File.Exists(selectedPath))
                        {
                            GUI.enabled = false;
                        }
                        if (GUILayout.Button("Delete"))
                        {
                            File.Delete(selectedPath);
                            CustomKeyword = null;
                            Reload();
                            Configuration.Reload();
                            changed = false;
                        }

                        if (GUILayout.Button("Reset"))
                        {
                            Configuration.Reload();
                            changed = false;
                        }
                        //else
                        //{
                        //    GUILayout.FlexibleSpace();
                        //}
                        GUI.enabled = true;
                    }


                    GUI.matrix = oldMat;

                }
            }
        }

        void DrawNewItemInput()
        {
            //new item

            if (isEditNewItem)
            {
                var evt = Event.current;
                if (evt.type == EventType.KeyDown)
                {
                    if (evt.keyCode == KeyCode.Escape)
                        isEditNewItem = false;
                }

                newItem = GUILayoutx.DelayedTextField(newItem, GUILayout.Width(80));
                if (!string.IsNullOrEmpty(newItem))
                {
                    isEditNewItem = false;
                    newItem = KeywordPrefix + newItem;
                    string path = GetPathWithCustomKeyword(newItem);
                    Configuration.Save(path);
                    CustomKeyword = newItem;
                    Reload();
                    Configuration.Reload();

                }
                if (GUIUtility.keyboardControl != newItemInputId)
                {
                    isEditNewItem = false;
                }

            }
            else
            {
                int oldNewItemInputId = GUIUtility.keyboardControl;
                GUILayoutx.DelayedTextField(string.Empty, GUILayout.Width(20));
                if (GUIUtility.keyboardControl != oldNewItemInputId)
                {
                    newItem = string.Empty;
                    newItemInputId = GUIUtility.keyboardControl;
                    isEditNewItem = true;
                }
            }
        }


        public static IGrouping<string, ConfigurationProperty>[] groupProperties;

        //gc 14.0K =>12.1K
        void DrawProperties()
        {
            TypeCode typeCode;
            object value;
            Profiler.BeginSample("GUI Draw Property");

            if (groupProperties == null)
            {
                groupProperties = ConfigurationProperty.GetAllProperties()
                        .Where(o => !o.IsHide)
                        .OrderBy(o => o.DisplayName)
                        .OrderBy(o => o.Order)
                        .OrderBy(o => o.Group ?? string.Empty)
                        .GroupBy(o => o.Group).ToArray();

            }

            using (var sv = new GUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = sv.scrollPosition;

                using (var checker = new GUIx.Scopes.ChangedScope())
                {

                    foreach (var g in groupProperties)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            GUI.backgroundColor = Color.gray;
                            if (!string.IsNullOrEmpty(g.Key))
                            {
                                using (new GUILayout.HorizontalScope())
                                {
                                    GUILayout.Label(g.Key ?? "");
                                    GUILayout.FlexibleSpace();
                                }
                            }
                            GUI.backgroundColor = Color.white;
                            foreach (var prop in g)
                            {
                                typeCode = Type.GetTypeCode(prop.ValueType);

                                GUIPropertyDrawer drawer = null;
                                Attribute drawerAttr;
                                drawer = GetDrawer(prop, out drawerAttr);
                                //if (prop.DrawerAttribute != null)
                                //{
                                //    drawer = PropertyDrawer.GetDrawer(prop.DrawerAttribute.GetType());
                                //}

                                value = prop.Value;

                                if (drawer != null)
                                {
                                    drawer.OnGUILayout(prop, drawerAttr);
                                    if (!object.Equals(value, prop.Value))
                                    {
                                        changed |= true;
                                    }
                                }
                                else
                                {
                                    using (new GUILayout.HorizontalScope())
                                    {
                                        GUILayoutx.PrefixLabel(new GUIContent(prop.DisplayName, prop.Tooltip));
                                        //value = GUILayoutx.ValueField(prop.ValueType, prop.Value);
                                    }

                                    if (!object.Equals(value, prop.Value))
                                    {
                                        if (prop.SetValue(value))
                                        {
                                            changed |= true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //changed |= checker.changed;
                }

                //GUILayout.FlexibleSpace();
            }
            Profiler.EndSample();
        }


        void DrawToolbar()
        {

            using (new GUILayout.HorizontalScope())
            {


                if (GUILayout.Button("Start"))
                {
                    IsShow = false;
                    OnHide();

                    if (Configuration.RestartCallback != null)
                    {
                        Configuration.RestartCallback();
                    }
                    else
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }

                if (GUILayout.Button("Hide"))
                {
                    IsShow = false;
                }

            }
        }
        static GUIContent CollapseContent = new GUIContent("+");
        static GUIContent ExpandContent = new GUIContent("-");

        void DrawOptions()
        {
            GUILayout.Space(5);
            using (new GUILayout.VerticalScope("box"))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(5);
                    if (GUILayout.Button(showUIOptions ? ExpandContent : CollapseContent, "label", GUILayout.Width(30)))
                    {
                        showUIOptions = !showUIOptions;
                    }

                    DrawToolbar();
                }


                if (showUIOptions)
                {
                    using (var checker = new GUIx.Scopes.ChangedScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayoutx.PrefixLabel("Scale");
                            Options.Instance.scale = GUILayout.HorizontalSlider(Options.Instance.scale, 0.5f, 2f);
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayoutx.PrefixLabel("Width");
                            Options.Instance.width = GUILayout.HorizontalSlider(Options.Instance.width, 0.2f, 1f);
                        }

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayoutx.PrefixLabel("PauseOnShow");
                            Options.Instance.pauseOnShow = GUILayout.Toggle(Options.Instance.pauseOnShow, GUIContent.none);
                        }

                        if (checker.changed)
                        {
                            Options.Instance.Save();
                        }
                    }
                    if (GUILayout.Button("Reset"))
                    {
                        Options.Instance = new Options();
                        Options.Instance.Save();
                    }
                }
            }
        }



        public GUIPropertyDrawer GetDrawer(ConfigurationProperty property, out Attribute attribute)
        {
            InitializeEditor();
            DrawerCache cache;
            if (drawerCached.TryGetValue(property, out cache))
            {
                attribute = cache.DrawerAttribute;
                return cache.drawer;
            }
            attribute = null;
            return null;
        }
        private static bool editorInitialized;

        class DrawerCache
        {
            public Attribute DrawerAttribute { get; set; }
            public GUIPropertyDrawer drawer;

        }

        static Dictionary<ConfigurationProperty, DrawerCache> drawerCached;

        /// <summary>
        /// 初始化 Editor 相关数据，节省性能
        /// </summary>
        public static void InitializeEditor()
        {
            if (!editorInitialized)
            {
                editorInitialized = true;
                drawerCached = new Dictionary<ConfigurationProperty, DrawerCache>();

                foreach (var prop in ConfigurationProperty.GetAllProperties())
                {
                    DrawerCache drawerCache;
                    if (drawerCached.TryGetValue(prop, out drawerCache))
                    {

                    }
                    if (prop.Members.Count > 0)
                    {
                        foreach (var member in prop.Members)
                        {
                            if (drawerCache == null)
                            {
                                var attrs = member.GetCustomAttributes(false);
                                foreach (Attribute attr in attrs)
                                {
                                    var drawer = GUIPropertyDrawer.GetDrawer(attr.GetType());
                                    if (drawer != null)
                                    {
                                        drawerCache = new DrawerCache();
                                        drawerCache.DrawerAttribute = attr;
                                        drawerCache.drawer = drawer;
                                    }
                                }
                            }



                        }
                    }

                    if (drawerCache == null)
                    {
                        var drawer = GUIPropertyDrawer.GetDrawer(prop.ValueType);
                        if (drawer != null)
                        {
                            drawerCache = new DrawerCache();
                            drawerCache.drawer = drawer;
                        }
                    }
                    if (drawerCache == null && prop.ValueType.IsEnum)
                    {
                        var drawer = GUIPropertyDrawer.GetDrawer(typeof(Enum));
                        if (drawer != null)
                        {
                            drawerCache = new DrawerCache();
                            drawerCache.drawer = drawer;
                        }
                    }

                    if (drawerCache != null)
                        drawerCached[prop] = drawerCache;

                }
            }
        }
    }



}