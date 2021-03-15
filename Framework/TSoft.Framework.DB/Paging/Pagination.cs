using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.DB
{
    public class Pagination<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecords { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}
