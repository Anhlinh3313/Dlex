using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Infrastructure.Utils
{
    public static class PaginatorUtil
    {
		public static IEnumerable<dynamic> GetDataPaginator(IEnumerable<dynamic> data, int? pageSize = null, int? pageNumber = null)
		{
            if (pageSize.HasValue && pageNumber.HasValue)
            {
                int iPageNumber = (int)pageNumber;
                int iPageSize = (int)pageSize;

                var totalCount = data.Count();
                var totalPages = Math.Ceiling((double)totalCount / iPageSize);

                var res = data.Skip((iPageNumber - 1) * iPageSize).Take(iPageSize);
                return res;
            }
            return data;
        }

        public static IEnumerable<dynamic> GetDataPaginatorWithSort(IEnumerable<dynamic> data, int? pageSize = null, int? pageNumber = null, bool? isSortDescending = null)
        {
            if (pageSize.HasValue && pageNumber.HasValue)
            {
                int iPageNumber = (int)pageNumber;
                int iPageSize = (int)pageSize;

                var totalCount = data.Count();
                if (isSortDescending == true)
                {
                    data = data.OrderByDescending(x => x.Id);
                }
                var totalPages = Math.Ceiling((double)totalCount / iPageSize);

                var res = data.Skip((iPageNumber - 1) * iPageSize).Take(iPageSize);
                return res;
            }
            return data;
        }
    }
}
