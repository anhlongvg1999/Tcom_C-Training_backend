using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class TrainingProgram_User_CheckinRequestModel
    {
        public Guid UserId { get; set; }

        public Guid TrainingProgramId { get; set; }

        public bool Active { get; set; }
    }
}
