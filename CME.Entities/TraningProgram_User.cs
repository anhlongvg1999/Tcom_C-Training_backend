using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("TrainingProgram_User")]
    public class TrainingProgram_User : BaseTable<Title>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid TrainingProgramId { get; set; }

        [ForeignKey("TrainingProgramId")]
        public TrainingProgram TrainingProgram { get; set; }

        public string TrainingSubjectName { get; set; }

        public bool Active { get; set; }

        public double Amount { get; set; }

        public int Year { get; set; }
    }
}
