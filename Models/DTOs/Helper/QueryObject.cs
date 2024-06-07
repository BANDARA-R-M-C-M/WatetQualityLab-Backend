namespace Project_v1.Models.DTOs.Helper
{
    public class QueryObject {
        public string? UserId { get; set; } = null;
        public string? CategoryId { get; set; } = null;
        public string? SearchTerm { get; set;} = null;
        public string? SearchParameter { get; set; } = null;
        public string SearchParameterType { get; set; } = "string";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = null;
        public bool IsAscending { get; set; } = true;
    }
}
