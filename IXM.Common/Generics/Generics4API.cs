using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using IXM.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
