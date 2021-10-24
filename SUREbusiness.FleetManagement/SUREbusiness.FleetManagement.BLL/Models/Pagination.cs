using System;
using System.Collections.Generic;

namespace SUREbusiness.FleetManagement.BLL.Models
{
    public class Pagination<T>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPageNumber { get; private set; }
        public int TotalItems { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public Pagination(int pageNumber, int pageSize, int totalItems, IEnumerable<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPageNumber = pageSize > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0;
            Data = data;
        }
    }
}
