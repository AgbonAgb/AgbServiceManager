using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Infrastructure
{
    public interface IEmailSender
    {
        Task<bool> sendPlainEmail(CMail cm);
        Task<bool> sendTemplatedEmail(CMail cm);
    }
}
