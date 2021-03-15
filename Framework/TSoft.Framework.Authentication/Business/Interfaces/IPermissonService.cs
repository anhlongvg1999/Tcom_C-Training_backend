using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace TSoft.Framework.Authentication
{
    public interface IPermissonService
    {
        Task<List<Permisson>> GetAll();
    }
}
