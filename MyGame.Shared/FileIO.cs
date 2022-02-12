using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace MyGame
{
    public class FileIO
    {
        public string File { get; set; }

        public FileIO(string file = null)
        {
            this.File = file;
        }

        public object Load()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(this.File))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(this.File, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        string json = reader.ReadToEnd();

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
                }
            }

            return null;
        }

        public void Save(object data)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            FileMode fm = isoStore.FileExists(this.File) ? FileMode.Truncate : FileMode.Create;

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(this.File, fm, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    string json = JsonConvert.SerializeObject(data);
                    writer.WriteLine(json);
                }
            }
        }
    }
}
