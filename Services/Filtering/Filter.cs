using Microsoft.EntityFrameworkCore;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace Project_v1.Services.Filtering {
    public class Filter : IFilter {
        public async Task<List<T>> SearchItemsAsync<T>(IQueryable<T> query, QueryObject queryObject) {
            try {
                var searchResults = query.AsQueryable();

                if (!String.IsNullOrWhiteSpace(queryObject.Name)) {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, "InstrumentId");
                    var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var searchExpression = Expression.Call(property, method, Expression.Constant(queryObject.Name));
                    var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);

                    searchResults = searchResults.Where(lambda);
                }

                return await searchResults.ToListAsync();
            } catch (Exception e) {
                Console.WriteLine("Error Searching: " + e.Message);
                throw;
            }
        }
    }
}   
