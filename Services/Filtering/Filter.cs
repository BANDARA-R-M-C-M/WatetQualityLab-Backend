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

        /*public IQueryable<T> Search<T>(IQueryable<T> query, String searchTerm, String queryParameter) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(searchTerm)) {
                    searchResults = searchResults.Where($"{queryParameter}.Contains(@0)", searchTerm);
                }

                return searchResults;
            } catch (Exception e) {
                Console.WriteLine("Error in Searching Query: " + e.Message);
                throw;
            }
        }

        public IQueryable<T> Sort<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var sortedQuery = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.SortBy)) {
                    sortedQuery = queryObject.IsAscending ? sortedQuery.OrderBy($"{queryObject.SortBy} ascending")
                                                      : sortedQuery.OrderBy($"{queryObject.SortBy} descending");
                }

                return sortedQuery;
            } catch (Exception e) {
                Console.WriteLine("Error in Sorting Query: " + e.Message);
                throw;
            }
        }

        public async Task<PaginatedResult<T>> Paginate<T>(IQueryable<T> list, int pageNumber, int pageSize) {
            try {
                var itemCount = await list.CountAsync();
                var totalPages = (int)Math.Ceiling(itemCount / (double)pageSize);

                var skipNumber = pageSize * (pageNumber - 1);
                var paginatedList = await list.Skip(skipNumber).Take(pageSize).ToListAsync();
                *//*return await list.Skip(skipNumber).Take(pageSize).ToListAsync();*//*

                return new PaginatedResult<T> {
                    Items = paginatedList,
                    TotalPages = totalPages
                };
            } catch (Exception e) {
                Console.WriteLine("Error Paginating: " + e.Message);
                throw;
            }
        }*/

        /*public async Task<List<T>> SearchInstrumentalQualityRecords<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.InstrumentId)) {
                    searchResults = searchResults.Where($"InstrumentId.Contains(@0)", queryObject.InstrumentId);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchMediaQualityRecords<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.MediaId)) {
                    searchResults = searchResults.Where($"MediaId.Contains(@0)", queryObject.MediaId);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchNewSamples<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.YourRefNo)) {
                    searchResults = searchResults.Where($"YourRefNo.Contains(@0)", queryObject.YourRefNo);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchGeneralCategories<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.GeneralCategoryName)) {
                    searchResults = searchResults.Where($"GeneralCategoryName.Contains(@0)", queryObject.GeneralCategoryName);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchSurgicalCategories<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.SurgicalCategoryName)) {
                    searchResults = searchResults.Where($"SurgicalCategoryName.Contains(@0)", queryObject.SurgicalCategoryName);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchGeneralItems<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.GeneralItemName)) {
                    searchResults = searchResults.Where($"ItemName.Contains(@0)", queryObject.GeneralItemName);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }

        public async Task<List<T>> SearchSurgicalItems<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.SurgicalItemName)) {
                    searchResults = searchResults.Where($"ItemName.Contains(@0)", queryObject.SurgicalItemName);
                }

                var skipNumber = queryObject.PageSize * (queryObject.PageNumber - 1);

                return await searchResults.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }*/
    }
}   
