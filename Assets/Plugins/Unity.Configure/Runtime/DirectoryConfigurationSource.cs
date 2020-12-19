using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace UnityEngine.Configure
{
    public class DirectoryConfigurationSource : IConfigurationSource
    {
        private FileSystemWatcher fileSystemWatcher;


        public DirectoryConfigurationSource(string directory)
        {
            this.Directory = directory;
        }

        public string Directory { get; private set; }

        public IEnumerable<string> GetConfigNames()
        {
            string dir = Directory;
            List<string> files = new List<string>();
            string defaultFile = Path.Combine(dir, Configuration.DefaultFileName);
            if (File.Exists(defaultFile))
            {
                files.Add(defaultFile);
            }
            files.AddRange(System.IO.Directory.GetFiles(dir, "*." + Configuration.DefaultFileName, SearchOption.TopDirectoryOnly));
            Ensure();
            return files.ToArray();
        }

        public string LoadData(string configName)
        {
            return File.ReadAllText(configName, Encoding.UTF8);
        }

        void Ensure()
        {
            //ios/android 不支持 FileSystemWatcher
            /*
            if (fileSystemWatcher == null)
            {
                
                //FileSystemWatcher NotImplementedException: The method or operation is not implemented.
                //Api Compatibility Level 切换到 .NET 4.x
                fileSystemWatcher = new FileSystemWatcher();
                fileSystemWatcher.BeginInit();

                fileSystemWatcher.Filter = "*config.xml";
                fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                fileSystemWatcher.EnableRaisingEvents = true;
                fileSystemWatcher.IncludeSubdirectories = false;
                fileSystemWatcher.EndInit();
                fileSystemWatcher.Path = Directory;
            }*/


        }
        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            string filename = Path.GetFileName(e.Name);
            filename = filename.ToLower();
            if (filename == Configuration.DefaultFileName || filename.EndsWith("." + Configuration.DefaultFileName))
            {
                string[] kws = Configuration.ParseKeywords(filename);
                if (Configuration.IsMatchKeyword(filename, kws))
                {
                    Configuration.Reload();
                }
            }
        }

    }

}