using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.DB
{ 
    public abstract class BaseTable<T> where T : BaseTable<T>
    {
        public Guid? ParentId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public DateTime? LastModifiedOnDate { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? LastModifiedByUserId { get; set; }
    }
}
