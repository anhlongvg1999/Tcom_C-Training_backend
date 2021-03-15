using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("cat_TrainingForms")]
    public class TrainingForm : BaseTable<Title>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<TrainingSubject> TrainingSubjects { get; set; }
    }
}
