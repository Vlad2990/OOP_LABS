using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    internal class CloudStorage : IStorageStrategy
    {
        StorageClient storage;
        string bucketName = "oop2-cb26b.firebasestorage.app";
        public CloudStorage()
        {
            var credential = GoogleCredential.FromFile("admin.json");
            storage = StorageClient.Create(credential);
        }
        public string Load(string documentName)
        {
            using var stream = new MemoryStream();
            storage.DownloadObject(bucketName, documentName, stream);
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public void Save(string documentName, string content)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            storage.UploadObject(bucketName, documentName, "text/plain", stream);
        }
        public void Delete(string documentName)
        {

            var storageObject = storage.GetObject(bucketName, documentName);
            if (storageObject == null)
            {
                throw new FileNotFoundException($"Document '{documentName}' not found in cloud storage");
            }

            storage.DeleteObject(bucketName, documentName);


        }
    }
}
