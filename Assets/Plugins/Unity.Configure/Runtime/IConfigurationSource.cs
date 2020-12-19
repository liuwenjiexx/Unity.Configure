using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Configure
{
    public interface IConfigurationSource
    {
        IEnumerable<string> GetConfigNames();
        string LoadData(string configName);
    }
}