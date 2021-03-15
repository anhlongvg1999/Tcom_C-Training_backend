using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Business.Models
{
    public class ImportUserRequestModel
    {
        public string SheetName { get; set; }
        public IFormFile File { get; set; }
    }
}
