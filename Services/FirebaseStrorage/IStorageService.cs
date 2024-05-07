namespace Project_v1.Services.FirebaseStrorage {
    public interface IStorageService {
        Task<string> UploadFile(Stream stream, string fileName);
    }
}
