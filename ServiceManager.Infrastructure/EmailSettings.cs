using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Infrastructure
{
    public class EmailSettings
    {
        public bool SSl { get; set; } = false;
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string EmailPassword { get; set; }
        public string Mfrom { get; set; }
        public string EmailMatchPattern { get; set; }
    }
}
