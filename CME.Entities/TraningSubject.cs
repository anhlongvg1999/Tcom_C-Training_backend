using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("cat_TrainingSubjects")]
    public class TrainingSubject : BaseTable<TrainingSubject>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public double Amount { get; set; }

        public int Order { get; set; }

        public Guid TrainingFormId { get; set; }

        [ForeignKey("TrainingFormId")]
        public TrainingForm TrainingForm { get; set; }
    }
}
