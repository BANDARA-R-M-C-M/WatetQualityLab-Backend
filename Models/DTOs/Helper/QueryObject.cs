namespace Project_v1.Models.DTOs.Helper
{
    public class QueryObject {
        public string? UserId { get; set; } = null;
        public string? CategoryId { get; set; } = null;
        /*public string? InstrumentId { get; set; } = null;
        public string? MediaId { get; set; } = null;
        public string? YourRefNo { get; set; } = null;
        public string? MyRefNo { get; set; } = null;
        public string? GeneralCategoryName { get; set; } = null;
        public string? SurgicalCategoryName { get; set; } = null;
        public string? GeneralItemName { get; set; } = null;
        public string? SurgicalItemName { get; set; } = null;*/
        public string? SearchTerm { get; set;} = null;
        public string? SearchParameter { get; set; } = null;
        public string SearchParameterType { get; set; } = "string";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = null;
        public bool IsAscending { get; set; } = true;
    }
}
