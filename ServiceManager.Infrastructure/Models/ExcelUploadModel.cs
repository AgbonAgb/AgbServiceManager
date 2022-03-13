using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Infrastructure.Models
{
    public class ExcelUploadModel
    {
        public IFormFile CVfile { get; set; }
        public string CvPath { get; set; }
    }
}
