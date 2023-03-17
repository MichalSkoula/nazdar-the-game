using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Environment;

namespace Nazdar
{
    public class FileIO
    {
        public string File { get; set; }
        public string Folder { get; private set; }
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

            if (!Directory.Exists(this.Folder))
            {
                Directory.CreateDirectory(this.Folder);
            }
        }

        public object Load()
        {
            if (System.IO.File.Exists(this.FullPath))
            {
                string json = System.IO.File.ReadAllText(FullPath);

                try
                {
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
            System.IO.File.WriteAllText(this.FullPath, json);
        }

        public void Delete()
        {
            System.IO.File.Delete(this.FullPath);
        }
    }
}
