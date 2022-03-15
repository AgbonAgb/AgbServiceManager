using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceManager.Infrastructure.Services
{
    public class EmailSenderServices : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private static string RegexMail;// =con Settings.Default.EmailMatchPattern;
        public EmailSenderServices(IOptions<EmailSettings> emailSettings)
        {
            //get SMTP details
            _emailSettings = emailSettings.Value;
        }
        public async Task<bool> sendPlainEmail(CMail cmail)
        {
            // errortype = "";
            bool rtn = false;
            try
            {
                //Builed The MSG
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                foreach (string strTo in cmail.ToEmail)
                {
                    //msg.To.Add(strTo.ToLower());
                    if (strTo.Trim() != "")
                    {
                        if (MailValid(strTo))
                        {
                            msg.To.Add(strTo.ToLower());
                        }
                    }

                }



                msg.From = new MailAddress(_emailSettings.Sender, _emailSettings.Mfrom, System.Text.Encoding.UTF8);
                msg.Subject = cmail.Subject;

                //smtp.gmail.com:465
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = cmail.Body;
                if (cmail.AttachedFile.Trim() != "")
                {
                    Attachment attachFile = new Attachment(cmail.AttachedFile);
                    //attachFile.f
                    msg.Attachments.Add(attachFile);
                }
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Priority = MailPriority.High;

                System.Net.Mail.SmtpClient mailclient = new System.Net.Mail.SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                mailclient.Host = _emailSettings.MailServer;// SmtpServer;
                mailclient.Port = _emailSettings.MailPort;//.Sender SmtpPort;
                mailclient.UseDefaultCredentials = false;
                mailclient.Credentials = auth;
                mailclient.EnableSsl = _emailSettings.SSl;// false;
                mailclient.Send(msg);

                msg.Attachments.Dispose();
                msg.Dispose();

                //string status = "";
                //client.Send(msg);

                //Console.WriteLine(cmail.ToEmail[0].ToString() + " has been sent mail");
                rtn = true;

                //Console.WriteLine("Sent---------");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Console.WriteLine(ex.Message, "Send Mail Error");
                //if (ex.Message.ToLower().Contains("specified e-mail address is currently not supported") || ex.Message.ToLower().Contains("a recipient must be specified"))
                //{
                //    errortype = "Bad mailAddress" + "; Mail=" + cmail.ToEmail[0].ToString();
                //}

            }
            return rtn;
        }

        public async Task<bool> sendTemplatedEmail(CMail cm)
        {
            throw new System.NotImplementedException();
        }
        private bool MailValid(string strTo)
        {
            bool rtn = false;

            // string patternStrict = ConfigurationManager.AppSettings["regexmail"];

            MatchCollection mc = Regex.Matches(strTo, _emailSettings.EmailMatchPattern);
            string mail = "";
            for (int i = 0; i < mc.Count; i++)
            {
                mail = mc[0].ToString();
            }

            if (mail != "")
            {
                rtn = true;
            }


            return rtn;
        }
    }
}
