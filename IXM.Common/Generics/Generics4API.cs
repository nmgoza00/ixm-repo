
using IXM.Models.Notify;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IXM.Common.Generics
{
    public class Generics4API
    {

        public class PageList<T>
        {
            private PageList(List<T> itemList, int page, int pagesize, int totalcount)
            {

                ItemList = itemList;
                Page = page;
                PageSize = pagesize;
                TotalCount = totalcount;
            }

            public List<T> ItemList { get; }
            public int Page { get; }
            public int PageSize { get; }
            public int TotalCount { get; }
            public bool HasNextPage => Page * PageSize < TotalCount;
            public bool HasPreviousPage => PageSize > 1;

            public static async Task<PageList<T>> CreateAsync(IQueryable<T> query, int page, int pagesize)
            {

                var totalRecs = await query.CountAsync();
                var items = await query.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

                return new(items, page, pagesize, totalRecs);

            }
        }

        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public int StatusCode { get; set; }
            public string GeneralMessage { get; set; }
            public string TechnicalMessage { get; set; }
            public T Data { get; set; }
            //public IEnumerable<string>? Errors { get; set; }

            public static ApiResponse<T> Ok(T data, API_RESPONSE ApiResponseInfo)
                => new() { Success = true, StatusCode = ApiResponseInfo.StatusCode, GeneralMessage = ApiResponseInfo.GeneralMessage, TechnicalMessage = ApiResponseInfo.TechnicalMessage, Data = data };

        }


    }
}
