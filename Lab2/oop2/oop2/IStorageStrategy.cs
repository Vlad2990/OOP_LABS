using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public interface IStorageStrategy
    {
        void Save(string documentName, string content);
        string Load(string documentName);

        void Delete(string documentName);
    }
}