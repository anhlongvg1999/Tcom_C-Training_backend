using CME.Business.Models;
using CME.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface IOrganizationService
    {
        Task<Pagination<OrganizationViewModel>> GetAllAsync(OrganizationQueryModel queryModel);

        Task<Organization> GetById(Guid id);

        Task<Organization> SaveAsync(Organization model);

        Task<bool> DeleteManyAsync(Guid[] deleteIds);
    }
}
