using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static System.Environment;

namespace Nazdar
{
    public class FileIO
    {
        public string File { get; set; }
        public string Folder { get; private set; }
        public bool Exists => System.IO.File.Exists(this.FullPath);
        private string FullPath
        {
            get { return Path.Combine(this.Folder, File); }
        }

        public FileIO(string file = null)
        {
            this.File = file;

#if __ANDROID__
            this.Folder = GetFolderPath(SpecialFolder.ApplicationData);
#elif NETFX_CORE
            this.Folder = Windows.Storage.ApplicationData.Current.RoamingFolder.Path;
#else
            this.Folder = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), "NazdarTheGame");
#endif

            this.EnsureFolder();
        }

        // Tests use their own folder so they never touch real saves.
        internal FileIO(string file, string folder)
        {
            this.File = file;
            this.Folder = folder;
            this.EnsureFolder();
        }

        private void EnsureFolder()
        {
            if (!Directory.Exists(this.Folder))
            {
                Directory.CreateDirectory(this.Folder);
            }
        }

        public object Load()
        {
            if (System.IO.File.Exists(this.FullPath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(FullPath);
                    dynamic data = JObject.Parse(json);
                    return data;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public void Save(object data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string temporaryPath = this.FullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                // Replace only after serialization and the complete write succeed, preserving the previous save on failure.
                System.IO.File.WriteAllText(temporaryPath, json);
                if (this.Exists)
                {
                    System.IO.File.Replace(temporaryPath, this.FullPath, null);
                }
                else
                {
                    System.IO.File.Move(temporaryPath, this.FullPath);
                }
            }
            finally
            {
                try
                {
                    if (System.IO.File.Exists(temporaryPath))
                    {
                        System.IO.File.Delete(temporaryPath);
                    }
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        public void Delete()
        {
            System.IO.File.Delete(this.FullPath);
        }
    }
}
