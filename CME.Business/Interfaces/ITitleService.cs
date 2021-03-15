using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface ITitleService
    {
        Task<Pagination<TitleViewModel>> GetAllAsync(TitleQueryModel queryModel);

        Task<Title> GetById(Guid id);

        Task<Title> SaveAsync(Title model);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
