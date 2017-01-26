using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Plugin.Media;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;

namespace HackathonPro
{
    public partial class UploadPage : ContentPage
    {
        string lastPictureURL;
        CloudBlobContainer container;
        CloudBlobClient blobClient;
        CloudStorageAccount storageAccount;

        public UploadPage()
        {
            InitializeComponent();
            animation.IsVisible = false;
        }

        public async Task upload(Dictionary<string,string> values){

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://hackathon.hernandis.me/problems", content);
            }
        }

        public void UploadProblem(object sender, EventArgs e)
        {
            var values = new Dictionary<string, string> {
                {"problem[title]", nameText.Text },
                {"problem[teacher_name]",  teacher.Text },
                {"problem[subject_name]",  subject.Text},
                {"problem[image_url]",  lastPictureURL},
                {"problem[difficulty]",  difficulty.Text}
            };

            upload(values);
            uploadLayout.IsVisible = false;
            animation.IsVisible = true;
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            connect();
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            string id = Guid.NewGuid().ToString();
            lastPictureURL = await UploadPicture(id, file.GetStream());
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

        public async Task<String> UploadPicture(String id, System.IO.Stream stream)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);

            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri.ToString();
        }

        public String getContainerToken()
        {
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
            return container.GetSharedAccessSignature(sasConstraints);
        }
    }
}
