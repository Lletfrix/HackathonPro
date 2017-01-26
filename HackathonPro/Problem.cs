using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace HackathonPro
{
    class Problem
    {

        CloudBlobContainer container;
        CloudBlobClient blobClient;
        CloudStorageAccount storageAccount;
        public Problem()
        {
        }

        public string title { get; set; }
        public string image_url { get; set; }
        public string teacher_name { get; set; }
        public string subject_name { get; set; }
        public string difficulty { get; set; }
        public int id { get; set; }

        public string difficulty_show { get
            {
                return "Dificultad: "+difficulty;
            }
        }
        public string subject_show
        {
            get
            {
                return "Asignatura: " + subject_name;
            }
        }
        public string teacher_show
        {
            get
            {
                return "Profesor: " + teacher_name;
            }
        }

        public string getImageURL
        {
            get{
                String url_definitiva = image_url + getContainerToken();
                return url_definitiva;
            }
        }

        public String getContainerToken()
        {
            if (container == null) {
                connect();
            }
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
            return container.GetSharedAccessSignature(sasConstraints);
        }

        public void connect()
        {
            storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=pokedex;AccountKey=7p57hehIeLmgfqlVocGCIrnuvugW26biMosYkLkz3QJwcK8EiCwzQr/LZZERA0y/XsrwEffsQVEOssvD4if2eg==");

            // Create the blob client.
            blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            container = blobClient.GetContainerReference("problemas");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExistsAsync();

        }


    }
}
