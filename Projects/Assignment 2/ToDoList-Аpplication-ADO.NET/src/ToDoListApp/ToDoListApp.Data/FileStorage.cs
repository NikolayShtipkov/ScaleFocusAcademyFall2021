using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace ToDoListApp.Data
{
    public class FileDatabase
    {
        public void Write<T>(string fileName, T data)
        {
            string serializedData = JsonSerializer.Serialize(data);

            File.WriteAllText(fileName, serializedData);

        }

        public T Read<T>(string fileName) where T : class
        {
            if (File.Exists(fileName))
            {
                string serializedData = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<T>(serializedData);
            }
            else
            {
                return null;
            }
        }
    }
}
