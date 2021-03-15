using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class TrainingProgramViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public Guid? OrganizationId { get; set; }

        public Organization Organization { get; set; }

        public Guid? TrainingFormId { get; set; }

        public TrainingForm TrainingForm { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public TrainingProgramMetaDataObject MetaDataObject { get; set; }

        public ICollection<TrainingProgram_User> TrainingProgram_Users { get; set; }

        public int NumberOfParticipants { get; set; }

        public DateTime? LastModifiedOnDate { get; set; }
    }
}
