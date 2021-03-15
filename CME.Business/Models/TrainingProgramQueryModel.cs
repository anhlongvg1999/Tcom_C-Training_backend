using System;
using System.Collections.Generic;
using System.Text;
using TSoft.Framework.DB;

namespace CME.Business.Models
{
    public class TrainingProgramQueryModel : PaginationRequest
    {

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public Guid? TrainingFormId { get; set; }
    }
}
