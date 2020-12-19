using System.Collections;
using System.Collections.Generic;
using System.Configure;
using System.Linq;
using UnityEngine;

namespace UnityEngine.Configure
{

    public class ResourcesConfigurationSource : IConfigurationSource
    {
        public string Directory { get; private set; }

        public ResourcesConfigurationSource(string directory)
        {
            this.Directory = directory;
        }


        public IEnumerable<string> GetConfigNames()
        {
            return Resources.LoadAll(Directory, typeof(TextAsset)).Select(o => o.name + ".xml").ToArray();
        }

        public string LoadData(string configName)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(Directory + "/" + configName.Substring(0, configName.Length - 4));
            return textAsset.text;
        }
    }

}