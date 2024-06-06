using iText.Kernel.Geom;
using Microsoft.EntityFrameworkCore;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Project_v1.Services.Filtering {
    public class Filter : IFilter {
        public async Task<PaginatedResult<T>> Filtering<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.SearchTerm)) {
                    switch (queryObject.SearchParameterType) {
                        case "string":
                            searchResults = searchResults.Where($"{queryObject.SearchParameter}.Contains(@0)", queryObject.SearchTerm);
                            break;
                        case "DateOnly":
                            if (DateOnly.TryParse(queryObject.SearchTerm, out var searchDate)) {
                                searchResults = searchResults.Where($"{queryObject.SearchParameter} == @0", searchDate);
                            } else {
                                throw new ArgumentException("Invalid date format for SearchTerm");
                            }
                            break;
                        case "DateTime":
                            if (DateTime.TryParse(queryObject.SearchTerm, out var searchDateTime)) {
                                searchResults = searchResults.Where($"{queryObject.SearchParameter} == @0", searchDateTime);
                            } else {
                                throw new ArgumentException("Invalid date format for SearchTerm");
                            }
                            break;
                        case "int":
                            if (int.TryParse(queryObject.SearchTerm, out var searchInt)) {
                                searchResults = searchResults.Where($"{queryObject.SearchParameter} == @0", searchInt);
                            } else {
                                throw new ArgumentException("Invalid integer format for SearchTerm");
                            }
                            break;
                        default:
                            throw new ArgumentException("Unsupported search parameter type");
                    }
                }

                if (!String.IsNullOrWhiteSpace(queryObject.SortBy)) {
                    searchResults = queryObject.IsAscending
                        ? searchResults.OrderBy($"{queryObject.SortBy} ascending")
                        : searchResults.OrderBy($"{queryObject.SortBy} descending");
                }

                var itemCount = await searchResults.CountAsync();
                var totalPages = (int)Math.Ceiling(itemCount / (double)queryObject.PageSize);

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);
                var paginatedList = await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

                return new PaginatedResult<T> {
                    Items = paginatedList,
                    TotalPages = totalPages
                };
            } catch (Exception e) {
                Console.WriteLine("Error in Filtering: " + e.Message);
                throw;
            }
        }
    }
}   
