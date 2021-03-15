using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<Pagination<TrainingProgramViewModel>> GetAllAsync(TrainingProgramQueryModel queryModel);

        Task<TrainingProgram> GetById(Guid id);

        Task<TrainingProgram> SaveAsync(TrainingProgram model, ICollection<TrainingProgram_UserRequestModel> trainingProgram_Users);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);

        Task<TrainingProgram_User> Checkin(Guid trainingProgramId, Guid userId, bool active);

        Task<MemoryStream> ExportCertifications(Guid id);

        Task<bool> ChangeStatus(Guid id, string status);

    }
}
