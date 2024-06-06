using Project_v1.Models.DTOs.Helper;

namespace Project_v1.Services.Filtering {
    public interface IFilter {
        Task<PaginatedResult<T>> Filtering<T>(IQueryable<T> query, QueryObject queryObject);
    }
}
