namespace Project_v1.Models.DTOs.Helper {
    public class PaginatedResult<T> {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
    }
}
