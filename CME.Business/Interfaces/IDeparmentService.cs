using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<Pagination<DepartmentViewModel>> GetAllAsync(DepartmentQueryModel queryModel);

        Task<Department> GetById(Guid id);

        Task<Department> SaveAsync(Department model);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
