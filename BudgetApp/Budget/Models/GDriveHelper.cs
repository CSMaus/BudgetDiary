using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Budget.Models
{
    public static class GDriveHelper
    {
        // NuGet Google.Apis.Drive.v3

        static DriveService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {
            string[] scopes = new string[] { DriveService.Scope.Drive };
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                },
                scopes,
                userName,
                CancellationToken.None,
                new FileDataStore("AppsToken") // need to get them
            ).Result;

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BudgetDiary",
            });

            return service;
        }

        static void UploadFile(DriveService service, string filePath, string contentType)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath)
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, contentType);
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
        }

        static void DownloadFile(DriveService service, string fileId, string destination)
        {
            var request = service.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();
            request.Download(stream);
            System.IO.File.WriteAllBytes(destination, stream.ToArray());
        }

        static void DownloadFile2(DriveService service, string fileId, string saveTo)
        {
            var request = service.Files.Get(fileId);
            using (var fileStream = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
            {
                request.Download(fileStream);
            }
        }

    }
}
