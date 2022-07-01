using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace SiberianAnabasis
{
    public class FileIO
    {
        public string File { get; set; }
        private IsolatedStorageFile isoStore;
        private IsolatedStorageFileStream isoStream;

        public FileIO(string file = null)
        {
            this.File = file;
            this.isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        }

        public object Load()
        {
            if (isoStore.FileExists(this.File))
            {
                this.isoStream = new IsolatedStorageFileStream(this.File, FileMode.Open, this.isoStore);
                using (this.isoStream)
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        string json = reader.ReadToEnd();
                        this.isoStream.Close();

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
            FileMode fm = this.isoStore.FileExists(this.File) ? FileMode.Truncate : FileMode.Create;
            this.isoStream = new IsolatedStorageFileStream(this.File, fm, this.isoStore);
            using (this.isoStream)
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    writer.WriteLine(json);
                    
                }
                this.isoStream.Close();
            }
        }

        public void Delete()
        {
            this.isoStore.DeleteFile(this.File);
        }

        public string GetPath()
        {
            IsolatedStorageFileStream tempIsoFileStream = this.isoStream;

            if (tempIsoFileStream == null) {
                tempIsoFileStream = new IsolatedStorageFileStream(this.File, FileMode.OpenOrCreate, this.isoStore);
            }

            string result = typeof(IsolatedStorageFileStream)
                .GetField("_fullPath", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(tempIsoFileStream)
                .ToString();

            tempIsoFileStream.Close();

            return result;
        }
    }
}
