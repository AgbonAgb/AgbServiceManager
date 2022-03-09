using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Web.Controllers.Common
{
    public class AlertMessage
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }
}
