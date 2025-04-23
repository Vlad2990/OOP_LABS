using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public static class StorageFactory
    {
        public static IStorageStrategy CreateStrategy(StorageType type)
        {
            return type switch
            {
                StorageType.LocalFile => new LocalStorage(),
                StorageType.Db => new DbStorage(),
                StorageType.Firebase => new CloudStorage(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
    public enum StorageType { LocalFile, Db, Firebase }
}
