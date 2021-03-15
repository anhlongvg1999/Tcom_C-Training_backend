using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public class PermissonService : IPermissonService
    {
        private readonly DataContextBase _dataContext;
        public PermissonService(DataContextBase dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Permisson>> GetAll()
        {
            var result = _dataContext.Permissons.ToList();
            return result;
        }

    }
}
