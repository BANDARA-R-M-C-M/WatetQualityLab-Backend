
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Firebase;
using Google.Apis.Auth.OAuth2;
using Google.Cloud;
using Google.Cloud.Storage.V1;
using System.Net;

namespace Project_v1.Services.FirebaseStrorage {
    public class StorageService : IStorageService {
        private readonly IConfiguration _configuration;

        public StorageService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<bool> DeleteFile(string fileName) {
            try {
                var task = new FirebaseStorage(_configuration["Firebase:ProjectId"], new FirebaseStorageOptions {
                    ThrowOnCancel = true
                }).Child("Water Quality Reports").Child(fileName).DeleteAsync();

                await task;

                return true;
            } catch (Exception e) {
                Console.WriteLine("Error deleting file: " + e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteQRCode(string fileName) {
            try {
                var task = new FirebaseStorage(_configuration["Firebase:ProjectId"], new FirebaseStorageOptions {
                    ThrowOnCancel = true
                }).Child("QR Codes").Child(fileName).DeleteAsync();

                await task;

                return true;
            } catch (Exception e) {
                Console.WriteLine("Error deleting QR: " + e.Message);
                throw;
            }
        }

        public async Task<byte[]> DownloadFile(string url, string fileName) {
            try {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";

                using HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using Stream httpResponseStream = httpResponse.GetResponseStream();

                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;

                using MemoryStream memoryStream = new MemoryStream();
                while ((bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0) {
                    memoryStream.Write(buffer, 0, bytesRead);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                byte[] fileBytes = memoryStream.ToArray();

                return fileBytes;
            } catch (Exception e) {
                Console.WriteLine("Error uploading file: " + e.Message);
                throw;
            }
        }

        public async Task<string> UploadFile(Stream File, string fileName) {
            try {
                String nameNew = $"{fileName}";

                var task = new FirebaseStorage(_configuration["Firebase:ProjectId"], new FirebaseStorageOptions {
                    ThrowOnCancel = true
                }).Child("Water Quality Reports").Child(nameNew).PutAsync(File);

                var downloadURL = await task;

                return downloadURL;
            } catch (Exception e) {
                Console.WriteLine("Error uploading file: " + e.Message);
                throw;
            }
        }

        public async Task<string> UploadQRCode(Stream QRCode, string fileName) {
            try {
                String nameNew = $"{fileName}";

                var task = new FirebaseStorage(_configuration["Firebase:ProjectId"], new FirebaseStorageOptions {
                    ThrowOnCancel = true
                }).Child("QR Codes").Child(nameNew).PutAsync(QRCode);

                var downloadURL = await task;

                return downloadURL;
            } catch (Exception e) {
                Console.WriteLine("Error uploading file: " + e.Message);
                throw;
            }
        }
    }
}
