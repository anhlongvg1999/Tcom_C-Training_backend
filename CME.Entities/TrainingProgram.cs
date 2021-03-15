using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;
using TSoft.Framework.DB;

namespace CME.Entities
{
    [Table("TrainingPrograms")]
    public class TrainingProgram : BaseTable<Title>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int Year { get; set; }

        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }

        public Guid? TrainingFormId { get; set; }

        [ForeignKey("TrainingFormId")]
        public TrainingForm TrainingForm { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public ICollection<TrainingProgram_User> TrainingProgram_Users { get; set; }

        [NotMapped]
        public TrainingProgramMetaDataObject MetaDataObject { get; set; }
        public string MetaData
        {
            get
            {
                return MetaDataObject == null ? null : JsonSerializer.Serialize(MetaDataObject);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    MetaDataObject = null;
                else
                    MetaDataObject = JsonSerializer.Deserialize<TrainingProgramMetaDataObject>(value);
            }
        }
    }

    public class TrainingProgramMetaDataObject
    {
        public List<TrainingSubjectObject> TrainingSubjects { get; set; }
    }

    public class TrainingSubjectObject
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public double Amount { get; set; }

        public int Order { get; set; }
    }
}
