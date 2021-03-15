using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.Framework.DB
{
    public static class IQueryableExtensions
    {
        public static async Task<Pagination<T>> GetPagedAsync<T>(this IQueryable<T> query, int currentPage, int pageSize, string sortExpression) where T : class
        {
            if(!String.IsNullOrEmpty(sortExpression))
            {
                string sort = FormatSortInput(sortExpression);
                var ascending = GetAscending(sort);
                sortExpression = sortExpression.Substring(1);
                query = query.OrderByField<T>(sortExpression, ascending);
            }
            var list = query.Skip((currentPage - 1) * pageSize).Take(pageSize);

            var result = new Pagination<T>();
            result.CurrentPage = currentPage;
            result.PageSize = pageSize;
            result.NumberOfRecords = list.Count();
            result.TotalRecords = query.Count();
            result.Content =  list;
            return result;
        }
        public static string FormatSortInput(string sort)
        {
            sort = sort.IndexOf(" ") == 0 ? sort.Replace(" ", "+") : sort;
            var firstPropertyChar = sort[1].ToString().ToUpper();
            sort = sort.Remove(1, 1);
            sort = sort.Insert(1, firstPropertyChar);
            return sort;
        }
        public static string GetAscending(string sort)
        {
            var ascending = sort[0].ToString();
            return ascending;
        }
    }
}
