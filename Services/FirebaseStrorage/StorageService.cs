
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
                Console.WriteLine("Error deleting file: " + e.Message);
                throw;
            }
        }

        public async Task<string> UploadFile(Stream File, string fileName) {
            try {
                /*var credential = GoogleCredential.FromJson(@"
                    {
                      ""type"": ""service_account"",
                      ""project_id"": ""system-development-e2712"",
                      ""private_key_id"": ""82bf70990232810e9c1f572888f0b8735f545b3b"",
                      ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC+Z0Q+gFL3hr/y\n3WBobyZYchYi2koFU2nFWZ0JfcNL6KpQf77mF/AEF/PLSKJwa387oeHbQSOO/0H5\n0z3yWKd01Q2ZamPPeU8F5v/Zs7psblfdHB6KPVFtyJ4g+75Ny5WHDPCCy63FCp9m\nN6QojhgrIva5EaGIsccuKnBXlXXQsyUsCyFZKXYaqQIMMxz+aP+0LEa6zy3rmQNo\n8e6uhPlNCUIf5vluAK+tcy/jU7QyEn3e64QVZOtqUTlhtoanForxsXWJ1mH867Yc\n1KvtON/BJBed35aF8EmwiesxiLzmpmCKGzsWMgyZZuEWO/y55PUPwg+TGXF4/P48\ny85OLTVhAgMBAAECggEATYvaaAg0gjp/x8awwqfuOpxwdo5oEkY4MFfAlQKC/VIL\nt5Bnq+e46bnloXf6LIYjgiJ6zaT4ef9tG/YUZzUDMpqpGMATWYcjD5jWwBAqj4nb\nQCL5Lz0tAmTwPie5iI6vXhr/g423vqYGaIel1JyWrE7npFVRcE5TfHJtYcZt/uqa\nhqBhAhPUiTTtIF1rFKePTRS0fplwvntkg/GVCGln1y+hIUKU4EpbrU75AsPpuTkf\nImWAkeHKJlXa5/CnhgWkZ8UBldJH01NxcVssVlweDM51K6NEGaPaljIlizc2HaFy\n3IVUaws/DhM1dLXpVPxMox3HkykrrsF2xcUwPEs/LwKBgQD2C+7jFeWUGBBH+DB7\n21ZdvxlGKBfXrAClBNxvnNwc4evt2vHWHeZCrPzbVv5O09xolIiqziCneYIFKtY4\nWD0jyuIOXLtfkK/FvbZux932mvX1QE+8x2+iCYi2AqujxiZOJTmphEebPaOawzi6\nVcBwo00Sl3wETctqTifwVp4uDwKBgQDGGxcey30Y534aiZxYlHKMbccbfgic9ZWb\nRFxOtMGeMNDHoveZ/ndzKtHa0iNazB8g2P6R1LFvd/FU2s+aeA96Yyhz5Av9Coqv\nECdbrIlwGKDB36VsYz+wB5lTGBjI9OwyY+G/JKXhTp7Y97npRtmq2xrqqhzMHcAY\nl/YRN5HVjwKBgEVzetvou2QBIt9dxDnB4PAXWAgAmzJKn+f0plBvA4a3ksQSED9z\nMyLQ95LmTGRt0wJGIwmro3YCi1vwCoXfCmsHAqYxMMa57ZOwiEVe712DwjeXDqrn\n1K/1ZvyRZmeVMAzQ3yTlbNz6Gis+Pc4DFI322nOMBPqmBuwb4ZQqrv2rAoGBAJZo\nrw8kdGBaMjY30Mq4OphAKPHIqQVyuBngtyzStEZEzd8k7lSvDUYVdqtq7IjdfazU\n08Mo92aobER+tSyUhzvm2SnUNP3z6QOtghRqVRcUu8k24kP+vrYJrvGV1AuPWV2Q\n1C7pxj/gD16wBykEL/M206LjcWXQGK8TgTHlPQYlAoGBANvae1PSwMdoQbL89H0l\nFlRjIzmSzT6MPtnwC8Kf7ZbajQ/0OpY1XPAU8h/jHjDWblCRGrFOO0wdai3RVa2J\n97PovimKbMV+5nGjcfaeGttSc+DPMfF4CwI30b8BItDgnVWduU4lAznqZwF279HD\naezJXVy+JYJWDYLL7qotwRZj\n-----END PRIVATE KEY-----\n"",
                      ""client_email"": ""firebase-adminsdk-sxq1p@system-development-e2712.iam.gserviceaccount.com"",
                      ""client_id"": ""114892897203758698242"",
                      ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                      ""token_uri"": ""https://oauth2.googleapis.com/token"",
                      ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                      ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-sxq1p%40system-development-e2712.iam.gserviceaccount.com"",
                      ""universe_domain"": ""googleapis.com""
                    }")
                    .CreateScoped(new[] { "https://www.googleapis.com/auth/cloud-platform" });

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                var storage = StorageClient.Create(credential);*/

                String nameNew = $"{fileName}";

                var task = new FirebaseStorage(_configuration["Firebase:ProjectId"], new FirebaseStorageOptions {
                    /*AuthTokenAsyncFactory = () => Task.FromResult(accessToken),*/
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
