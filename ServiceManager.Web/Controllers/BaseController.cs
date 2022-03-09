using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceManager.Web.Controllers.Common.Enum;

namespace ServiceManager.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public void Alert(string message, dynamic content, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "','" + content + "', '" + message + "','" + notificationType + "')" + " </script>";
            TempData["notification"] = msg;
        }
        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }
        /// <summary>
        /// Sets the information for the system notification.
        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
        public void Message(string message, NotificationType notifyType)
        {
            TempData["Notification2"] = message;

            switch (notifyType)
            {
                case NotificationType.success:
                    TempData["NotificationCSS"] = "alert-box success";
                    break;
                case NotificationType.error:
                    TempData["NotificationCSS"] = "alert-box errors";
                    break;
                case NotificationType.warning:
                    TempData["NotificationCSS"] = "alert-box warning";
                    break;

                case NotificationType.info:
                    TempData["NotificationCSS"] = "alert-box notice";
                    break;
            }
        }
    }
}
