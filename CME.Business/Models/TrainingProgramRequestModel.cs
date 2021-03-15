using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class TrainingProgramRequestModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public Guid? OrganizationId { get; set; }

        public Guid? TrainingFormId { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public TrainingProgramMetaDataObject MetaDataObject { get; set; }

        public ICollection<TrainingProgram_UserRequestModel> TrainingProgram_Users { get; set; }
    }
}
