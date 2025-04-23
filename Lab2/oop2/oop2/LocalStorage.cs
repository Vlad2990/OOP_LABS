using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    internal class LocalStorage : IStorageStrategy
    {
        public void Save(string documentName, string content)
        {
            File.WriteAllText(documentName, content);
        }

        public string Load(string documentName)
        {
            if (!File.Exists(documentName)) throw new FileNotFoundException();
            return File.ReadAllText(documentName);
        }
    }
}