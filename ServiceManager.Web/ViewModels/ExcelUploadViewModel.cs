using System.ComponentModel.DataAnnotations;

namespace ServiceManager.Web.ViewModels
{
    public class ExcelUploadViewModel
    {
       
        public IFormFile CVfile { get; set; }
        [Display(Name = "Upload Excel")]
        public string CvPath { get; set; }
    }
}
