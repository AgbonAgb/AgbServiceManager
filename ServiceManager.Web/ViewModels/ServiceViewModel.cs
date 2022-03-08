using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Web.ViewModels
{
    public class ServiceViewModel
    {
        public int Srn { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Service Desc")]
        [StringLength(300)]
        //[Column(TypeName = "nvarchar(max)")]
        public string ServiceDesc { get; set; }
        public string Company { get; set; }
        [Display(Name = "Anniversary Date")]
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [BindProperty, DataType(DataType.Date)]
        public DateTime Enddate { get; set; }
        public string Status { get; set; }
        [Display(Name = "Count Down Days")]
        public int Daysnotification { get; set; }
        
        public string Frequency { get; set; }
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
           ErrorMessage = "The email address is not entered in a correct format")]
        public string Email { get; set; }
        public string SetupBy { get; set; }
        [Display(Name = "Contact Staff")]
        public string ContactStaff { get; set; }
        [DataType(DataType.MultilineText)]
        public string Credentials { get; set; }
    }
}
