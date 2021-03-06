using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("cat_Organizations")]
    public class Organization : BaseTable<Organization>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }
    }
}
