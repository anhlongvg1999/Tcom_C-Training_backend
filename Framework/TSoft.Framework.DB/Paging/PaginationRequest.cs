using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSoft.Framework.DB
{
    public class PaginationRequest
    {
        public virtual string Sort { get; set; }
        public string Fields { get; set; }
        [Range(1, int.MaxValue)]
        public int? CurrentPage { get; set; }
        [Range(1, int.MaxValue)]
        public int? PageSize { get; set; }
        public string Filter { get; set; }
        public string FullTextSearch { get; set; }
        public Guid? Id { get; set; }
        public List<Guid> ListId { get; set; }
        public bool SearchAllApp { get; set; }
        public List<string> ListTextSearch { get; set; }
    }
}
