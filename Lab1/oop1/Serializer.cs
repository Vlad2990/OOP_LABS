using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    public class Serializer
    {
        public void SaveToFile(string fileName, List<Shape> shapes)
        {
            string json = JsonConvert.SerializeObject(shapes, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All  
            });

            using (StreamWriter sw = new StreamWriter(fileName, false)) 
            {
                sw.WriteLine(json);
            }
        }

        public List<Shape> LoadFromFile(string fileName)
        {
            string json = File.ReadAllText(fileName);

            List<Shape> shapes = JsonConvert.DeserializeObject<List<Shape>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All 
            });
            return shapes;
        }
    }
}
