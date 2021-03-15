using CME.Business.Models;
using CME.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.DB;

namespace CME.Business.Interfaces
{
    public interface IExportService
    {
        Task<MemoryStream> ReportByDepartment(int fromYear, int toYear);
    }
}
