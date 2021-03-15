using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class TrainingFormViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<TrainingSubject> TrainingSubjects { get; set; }

        public DateTime? LastModifiedOnDate { get; set; }
    }
}
