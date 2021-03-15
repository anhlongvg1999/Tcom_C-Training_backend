using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface ITrainingFormService
    {
        Task<Pagination<TrainingFormViewModel>> GetAllAsync(TrainingFormQueryModel queryModel);

        Task<TrainingFormViewModel> GetById(Guid id);

        Task<TrainingForm> SaveAsync(TrainingForm model, ICollection<TrainingSubject> trainingSubjects);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
