using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Data.Entities
{
    public class Service
    {
        [Key]
        public int Srn { get; set; }
        [StringLength(300)]
        public string ServiceDesc { get; set; }
        public string Company { get; set; }       
        public DateTime Enddate { get; set; }
        public string Status { get; set; }
        public int Daysnotification { get; set; }
        public string Frequency { get; set; }
        public string Email { get; set; }
        public string SetupBy { get; set; }       
        public string ContactStaff { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Credentials { get; set; }
    }
}
