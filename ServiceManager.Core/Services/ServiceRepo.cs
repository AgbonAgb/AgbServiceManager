using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Interfaces;
using ServiceManager.Data;
using ServiceManager.Data.Entities;
using ServiceManager.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.Extensions.Configuration;
using System.Data;
using ServiceManager.Infrastructure.Services;
using ServiceManager.Infrastructure;
using Microsoft.Extensions.Options;

namespace ServiceManager.Core.Services
{
    public class ServiceRepo : IGenRepo<Service, int>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailSender _emailSender;
        private readonly DefaultEmail _emailDeafult;
        //

        //private IConfiguration _Configuration;
        // private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceRepo(AppDbContext appDbContext, IEmailSender emailSender, IOptions<DefaultEmail> emailDeafult)//DefaultEmail emailDeafult
        {
            _appDbContext = appDbContext;
            _emailSender = emailSender;
            _emailDeafult = emailDeafult.Value;
            //_Configuration=Configuration;   
        }
        public async Task<bool> Create(Service entity)
        {
            bool rtn = false;
            try
            {
                var exp = await _appDbContext.Services.Where(x => x.ServiceDesc == entity.ServiceDesc || x.Srn == entity.Srn).FirstOrDefaultAsync();
                if (exp == null)
                {
                    await _appDbContext.AddAsync(entity);
                    await _appDbContext.SaveChangesAsync();
                    rtn = true;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return rtn;
        }

        public async Task<bool> Delete(int Id)
        {
            bool rtn = false;
            var exp = await _appDbContext.Services.Where(x => x.Srn == Id).FirstOrDefaultAsync();
            if (exp != null)
            {
                _appDbContext.Remove(exp);
                await _appDbContext.SaveChangesAsync();
                rtn = true;
            }

            return rtn;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            var exp = await _appDbContext.Services.Where(x => x.Status.ToLower() == "active").ToListAsync();
            return exp;
        }

        public async Task<IEnumerable<Service>> GetAllDisabled()
        {
            var exp = await _appDbContext.Services.Where(x => x.Status.ToLower() == "non active").ToListAsync();
            return exp;
        }

        public async Task<Service> GetById(int Id)
        {
            //var exp= await _appDbContext.Services.Where(x => x.Srn == Id).FirstOrDefaultAsync();


            //return exp;
            return await _appDbContext.Services.FindAsync(Id);//.Where(x => x.Srn == Id).FirstOrDefaultAsync();


        }

        public async Task MonitorServiceAlert()
        {
            var exp = await _appDbContext.Services.Where(x => x.Status.ToLower() == "active").ToListAsync();
            //foreach(Service cproj in exp)
            //{

            int cnt = 0;

            cnt = exp.Count;
            string date1 = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dt = Convert.ToDateTime(date1);
            DateTime nextanni = dt.AddYears(1);


            string AdminMail1 =  _emailDeafult.AdminEmail;// Settings.Default.AdminMail1;
                                                          //"agbonwinn@yahoo.com";//                                           //string AdminMail2 = Settings.Default.Adminmail2;



            string Title = "";
            string Description = "";
            string EventDate = "";
            int CountDown = 0;
            string NotificationEmail = "";
            string Frequency = "";
            string Status = "";
            string WeekDays = "";
            string Stop = "";
            string task = "";
            string Company = "";
            int taskid = 0;

            string names = "";
            string email = "";

            //Current year, month, day
            DateTime prev = dt.AddDays(-1);
            string prvdate = prev.ToString("yyyy-MM-dd");

            int month = dt.Month;//Currnet month
            int day = dt.Day;//current day
            string dname = dt.DayOfWeek.ToString();
            int yr = DateTime.Now.Year;


            DateTime dt2;
            foreach (Service cproj in exp)
            {


                Description = "";
                EventDate = "";
                CountDown = 0;
                NotificationEmail = "";
                Frequency = "";
                Status = "";
                Title = "";
                WeekDays = "";
                Stop = "";
                task = "";
                taskid = cproj.Srn;//.ServiceDesc;
                names = "";
                email = "";
                Company = "";

                Console.WriteLine();
                Console.WriteLine("Found " + cproj.ServiceDesc);
               

                if (string.IsNullOrEmpty(cproj.ContactStaff))
                {
                    Title = "Admin";
                }
                else
                {
                    Title = cproj.ContactStaff;
                }
                //.StaffId;
                Description = cproj.ServiceDesc;// (string)item["Description"];
                EventDate = cproj.Enddate.ToString();// (string)item["EventDate"];
                CountDown = cproj.Daysnotification;// Convert.ToInt32(item["CountDown"].ToString());
                NotificationEmail = cproj.Email; // (string)item["NotificationEmail"];
                Frequency = cproj.Frequency;// (string)item["Frequency"];
                Company = cproj.Company;
                //Status = cproj.Status;// (string)item["Status"];
                WeekDays = "";// (string)item["WeekDays"];
                Stop = "expire";
                //task = cproj.Task;
                //taskid = cproj.Id;


                if (DateTime.TryParse(EventDate, out dt2))
                {
                    int mnt = dt2.Month;
                    int dd = dt2.Day;
                    int expyr = dt2.Year;
                    Console.WriteLine("Retrieved: " + Description + ", Event Date: " + EventDate);
                    Console.WriteLine("Created By: " + cproj.SetupBy);

                    int cday = dd - day;
                    //yearly date
                    string realdate = yr.ToString("0000") + "-" + mnt.ToString("00") + "-" + dd.ToString("00");

                    //Not yearly monitor
                    string Staticdate = expyr.ToString("0000") + "-" + mnt.ToString("00") + "-" + dd.ToString("00");


                    DateTime yrdate;
                    DateTime yrdate2;
                    DateTime currdate = DateTime.Now;//.ToString("yyyy-MM-dd");
                    if (DateTime.TryParse(realdate, out yrdate) && DateTime.TryParse(Staticdate, out yrdate2))
                    {
                        Console.WriteLine("Date Zone: " + Description + ", Event Date: " + EventDate);
                        TimeSpan ts = yrdate.Subtract(currdate.AddDays(-1));
                        TimeSpan ts2 = yrdate2.Subtract(currdate.AddDays(-1));
                        TimeSpan ts3 = yrdate2.Subtract(currdate);
                        //TimeSpan ts3 = currdate.Subtract(yrdate2);
                        //ts.Days = ts.Days + 1;
                        //ts2.Days = ts2.Days + 1;

                        //int days1 = ts.Days + 1;
                        //int days2 = ts2.Days + 1;
                        if (Frequency.ToLower() == "yearly")
                        {
                            //int month = dt.Month;//Currnet month
                            //int day = dt.Day;//current day

                            if (ts3.Days >= 0 && ts3.Days <= CountDown)
                            {
                                //sendmail(Description, Title, NotificationEmail, realdate, ts.Days, task, taskid, nextanni);
                              await  sendServiceMonitormail(Description, Title, NotificationEmail, realdate, ts3.Days, task, taskid, nextanni, Company, cproj);

                            }
                        }

                        if (Frequency.ToLower() == "monthly")
                        {
                            if (cday >= 0)
                            {
                                if (cday <= CountDown)
                                {
                                 await  sendmailMonthly(Description, Title, NotificationEmail, realdate, ts3.Days, task, taskid, nextanni, Company, cproj);
                                    Console.WriteLine("File_Name " + Description);
                                }
                            }

                        }


                    }
                }
                //  Thread.Sleep(Settings.Default.SleepInterval);
            }


            //}

            //monitor service exirations and send emails
            ////Console.WriteLine("Hang Fire is here");

            ////CMail cm = null;
            ////cm = new CMail();

            ////cm.Subject = "Service Monitoring";
            ////cm.AttachedFile = "";
            ////// cm.ToEmail.Add(mail);


            //////cm.Body = body;
            ////cm.DisplayName = "Service Monitor";// Ruser.UserName;// assets.Name;
            ////cm.ComposedDate = DateTime.Now.ToLongDateString();

            ////// brake email and loop
            ////string em = "agbonwinn@yahoo.com";
            ////cm.ToEmail.Add(em);
            ////string Username = "Godwin";//= item[i].ToString().Trim();
            //////body = body.Replace("Dear Adewole", "Dear " + Username);
            ////StringBuilder sb = new StringBuilder();
            ////sb.AppendLine("Dear" + " " + Username + ",");
            ////sb.AppendLine();
            ////sb.AppendLine("Kindly note that the above service has count down days of");
            ////sb.AppendLine();
            //////sb.AppendLine("Hopefully, we shall work together");


            ////cm.Body = sb.ToString();// = body.Replace("Dear Adewole", "Dear " + Username);
            ////bool semail = await _emailSender.sendPlainEmail(cm);
            ////if (semail == true)
            ////{
            ////    Console.WriteLine("Email Sent");
            ////}
            ////else
            ////{
            ////    Console.WriteLine("Email not Sent");
            ////}
            // _logger.LogInformation("User created a new account with username " + Ruser.UserName);

        }

        //private Task sendmailMonthly(string description, string title, string notificationEmail, string realdate, int days, string task, int taskid, DateTime nextanni, string company, Service cproj)
        //{
        //    throw new NotImplementedException();
        //}

        private async Task sendServiceMonitormail(string Description, string Staff, string mail, string RealDate, int cday, string task, long taskid, DateTime nextanni, string Company, Service cproj)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string msg1 = "";
                string pdeadlin = "";
                if (cday == 0)
                {
                    //string remark = "Could not meet deadline of " + RealDate;
                    msg1 = "This service expires today: " + Description;

                    //update Db till next year
                    cproj.Enddate = nextanni;
                    await Update(cproj);

                }
                else
                {
                    msg1 = cday.ToString() + " DAY(S) to expiration for this Service: " + Description;
                }

                string attached = "";
                string subject = "";
                if (msg1.Length > 25)
                {
                    subject = msg1.Substring(0, 25);
                }
                else
                {
                    subject = msg1;
                }
                //;// "Service Monitor"; //msg1;//

                //string subst = st.Substring(0, 2);
                //string heading = "COMMING EVENT: " + Description;
                string heading = Company + " " + "Service Expiration Notice: " + Description;

                sb.AppendLine("Dear " + Staff.ToUpper() + ",");
                sb.AppendLine("");
                sb.AppendLine(heading);
                sb.AppendLine("");
                //sb.AppendLine("Services: ");

                sb.AppendLine("");
                sb.AppendLine(task);
                sb.AppendLine("");
                sb.AppendLine("This is to remind you that the above service will expire on " + RealDate);
                sb.AppendLine("");
                if (cday == 0)
                {
                    sb.AppendLine(pdeadlin);
                }
                else
                {
                    sb.AppendLine("COUNT DOWN: " + cday.ToString() + " DAY(S) TO ");
                }

                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("Regards");
                sb.AppendLine("");
                //sb.AppendLine("http://intranet.cyberspace.net.ng");
                sb.AppendLine("");

                CMail cm = null;
                cm = new CMail();

                cm.Subject = subject;// "Service Monitoring";
                cm.AttachedFile = "";
                // cm.ToEmail.Add(mail);


                //cm.Body = body;
                cm.DisplayName = subject;// "Service Monitor";// Ruser.UserName;// assets.Name;
                cm.ComposedDate = DateTime.Now.ToLongDateString();

                // brake email and loop
                string em = mail;// "agbonwinn@yahoo.com";
                cm.ToEmail.Add(em);
               // string Username = "Godwin";//= item[i].ToString().Trim();
                                           //body = body.Replace("Dear Adewole", "Dear " + Username);
                
                //sb.AppendLine("Hopefully, we shall work together");


                cm.Body = sb.ToString();// = body.Replace("Dear Adewole", "Dear " + Username);
                bool semail = await _emailSender.sendPlainEmail(cm);
                if (semail == true)
                {
                    Console.WriteLine("Email Sent");
                }
                else
                {
                    Console.WriteLine("Email not Sent");
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + "||" + ex.StackTrace);
            }
        }

        private async Task sendmailMonthly(string Description, string Staff, string mail, string RealDate, int cday, string task, long taskid, DateTime nextanni, string Company, Service cproj)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string msg1 = "";
                string pdeadlin = "";
                if (cday == 0)
                {
                    //string remark = "Could not meet deadline of " + RealDate;
                    msg1 = "This service expires today: " + Description;

                    //update Db till next year
                    //cproj.Enddate = nextanni;
                    //await Update(cproj);

                }
                else
                {
                    msg1 = cday.ToString() + " DAY(S) to expiration for this Service: " + Description;
                }

                string attached = "";
                string subject = "";
                if (msg1.Length > 25)
                {
                    subject = msg1.Substring(0, 25);
                }
                else
                {
                    subject = msg1;
                }
                //;// "Service Monitor"; //msg1;//

                //string subst = st.Substring(0, 2);
                //string heading = "COMMING EVENT: " + Description;
                string heading = Company + " " + "Service Expiration Notice: " + Description;

                sb.AppendLine("Dear " + Staff.ToUpper() + ",");
                sb.AppendLine("");
                sb.AppendLine(heading);
                sb.AppendLine("");
                //sb.AppendLine("Services: ");

                sb.AppendLine("");
                sb.AppendLine(task);
                sb.AppendLine("");
                sb.AppendLine("This is to remind you that the above service will expire on " + RealDate);
                sb.AppendLine("");
                if (cday == 0)
                {
                    sb.AppendLine(pdeadlin);
                }
                else
                {
                    sb.AppendLine("COUNT DOWN: " + cday.ToString() + " DAY(S) TO ");
                }

                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("Regards");
                sb.AppendLine("");
                //sb.AppendLine("http://intranet.cyberspace.net.ng");
                sb.AppendLine("");

                CMail cm = null;
                cm = new CMail();

                cm.Subject = subject;// "Service Monitoring";
                cm.AttachedFile = "";
                // cm.ToEmail.Add(mail);


                //cm.Body = body;
                cm.DisplayName = subject;// "Service Monitor";// Ruser.UserName;// assets.Name;
                cm.ComposedDate = DateTime.Now.ToLongDateString();

                // brake email and loop
                string em = mail;// "agbonwinn@yahoo.com";
                cm.ToEmail.Add(em);
                // string Username = "Godwin";//= item[i].ToString().Trim();
                //body = body.Replace("Dear Adewole", "Dear " + Username);

                //sb.AppendLine("Hopefully, we shall work together");


                cm.Body = sb.ToString();// = body.Replace("Dear Adewole", "Dear " + Username);
                bool semail = await _emailSender.sendPlainEmail(cm);
                if (semail == true)
                {
                    Console.WriteLine("Email Sent");
                }
                else
                {
                    Console.WriteLine("Email not Sent");
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + "||" + ex.StackTrace);
            }
        }

        public async Task<IEnumerable<Service>> SearchItem(string desc)
        {
            var itm = (IEnumerable<Service>)null;
            try
            {
                itm = await _appDbContext.Services.Where(x => x.ServiceDesc.Contains(desc)).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            return itm;
        }

        public async Task<bool> Update(Service entity)
        {
            bool rtn = false;
            //var exp = await _appDbContext.Services.Where(x => x.Srn == entity.Srn).FirstOrDefaultAsync();
            var exp = await _appDbContext.Services.FindAsync(entity.Srn);//.Where(x => x.Srn == entity.Srn).FirstOrDefaultAsync();
            if (exp != null)
            {
                exp.ServiceDesc = entity.ServiceDesc;
                exp.Company = entity.Company;
                exp.Frequency = entity.Frequency;
                exp.Email = entity.Email;
                exp.Daysnotification = entity.Daysnotification;
                exp.Credentials = entity.Credentials;
                exp.SetupBy = entity.SetupBy;
                exp.ContactStaff = entity.ContactStaff;
                exp.Enddate = entity.Enddate;
                exp.Status = entity.Status;


                _appDbContext.Update(exp);
                await _appDbContext.SaveChangesAsync();
                rtn = true;
            }

            return rtn;
        }

        //public void UploadExcell(ExcelUploadModel exl)
        public async Task UploadExcell(string filepath, string ext)
        {
            string fp = filepath;
            //string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ExcelData");


            if (System.IO.File.Exists(filepath))
            {
                var excelType = ExcelProcessor.ExcelType.Xlsx;

                if (ext.ToLower() == ".xls")
                {
                    excelType = ExcelProcessor.ExcelType.Xls;
                }


                var excel = new ExcelProcessor();
                var Svc = excel.ImportExcelData<Service>(filepath, excelType);
                //Add to Db
                foreach (Service cs in Svc)
                {
                    await Create(cs);

                    Console.WriteLine("Inserted Desc = " + cs.ServiceDesc);
                }

            }

        }
    }
}
