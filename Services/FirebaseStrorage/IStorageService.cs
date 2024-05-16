namespace Project_v1.Services.FirebaseStrorage {
    public interface IStorageService {
        Task<string> UploadFile(Stream File, string fileName);
        Task<bool> DeleteFile(string fileName);
        Task<string> UploadQRCode(Stream QRCode, string fileName);
        Task<bool> DeleteQRCode(string fileName);
        Task<byte[]> DownloadFile(string url, string fileName);
    }
}
