using Project_v1.Models.DTOs.Helper;

namespace Project_v1.Services.Filtering {
    public interface IFilter {
        IQueryable<T> Search<T>(IQueryable<T> query, String searchTerm, String queryParameter);
        IQueryable<T> Sort<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> Paginate<T>(IQueryable<T> list, int pageNumber, int pageSize);
        /*Task<List<T>> SearchInstrumentalQualityRecords<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchMediaQualityRecords<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchNewSamples<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchGeneralCategories<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchSurgicalCategories<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchGeneralItems<T>(IQueryable<T> query, QueryObject queryObject);
        Task<List<T>> SearchSurgicalItems<T>(IQueryable<T> query, QueryObject queryObject);*/
    }
}
