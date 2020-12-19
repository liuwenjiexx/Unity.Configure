using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Configure;
using System.IO;
using UnityEngine.Configure;
using UnityEngine.Configure.GUI;
using System.ComponentModel;
using System;

public class TestConfiguration
{

    [DefaultValue("abc")]
    [Configurable]
    public static string str1;

    [Configurable]
    [Tooltip("Int32")]
    public static int int1 = 1;

    [DefaultValue(true)]
    [Configurable]
    public static bool bool1;


    [EnumValues(new object[] { "abc", "ABC", "123" }, new string[] { "小写", "大写", "数字" })]
    [Configurable]
    public static string strOptions = "ABC";

    [Configurable]
    [Tooltip("Enum")]
    public static Enum1 enum1 = Enum1.None;

    public enum Enum1
    {
        None,
        A,
        B,
        C
    }

    [Configurable]
    [Tooltip("Enum Flags")]
    public static Flags1 flags1 = Flags1.None;

    [Flags]
    public enum Flags1
    {
        None,
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2
    }

    [Configurable]
    [UnityEngine.Configure.Range(0, 1f)]
    public static float range1 = 0.5f;

    [Configurable]
    [HideInInspector]
    public static float hide1 = 0.5f;

    [Configurable]
    public static Vector3 vector3 = Vector3.one;




    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInitializeOnLoadMethod()
    {
        if (!Configuration.IsInitialized)
        {
            //配置文件读取目录
            Configuration.Sources.Add(new DirectoryConfigurationSource(Path.Combine(Application.persistentDataPath, Configuration.ConfigDirectory)));
            //设置用户关键字，默认为空，比如用户ID
            Configuration.SetUserKeyword(string.Empty);

            //初始化调试界面
            if (Configuration.IsDebugBuild)
            {
                ConfigurationGUI.IsEnabled = true;
                ConfigurationGUI.Initialize();
                //测试一开始就显示
                ConfigurationGUI.IsShow = true;
            }
            //初始化，在调试界面初始化之后调用，加载用户定制配置
            Configuration.Initialize();
        }
    }

}
