using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class TrainingProgram_UserRequestModel
    {
        public Guid UserId { get; set; }

        public string TrainingSubjectName { get; set; }

        public double Amount { get; set; }

        public bool Active { get; set; }
    }
}
