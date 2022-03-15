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

namespace ServiceManager.Core.Services
{
    public class ServiceRepo : IGenRepo<Service, int>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IEmailSender _emailSender;

        //private IConfiguration _Configuration;
        // private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceRepo(AppDbContext appDbContext, IEmailSender emailSender)
        {
            _appDbContext=appDbContext;
            _emailSender=emailSender;   
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
             var exp = await _appDbContext.Services.Where(x=>x.Status.ToLower()=="active").ToListAsync();
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

        public async void MonitorServiceAlert()
        {
            //monitor service exirations and send emails
            Console.WriteLine("Hang Fire is here");

            CMail cm = null;
            cm = new CMail();

            cm.Subject = "Service Monitoring";
            cm.AttachedFile = "";
            // cm.ToEmail.Add(mail);


            //cm.Body = body;
            cm.DisplayName = "Service Monitor";// Ruser.UserName;// assets.Name;
            cm.ComposedDate = DateTime.Now.ToLongDateString();

            // brake email and loop
            string em = "agbonwinn@yahoo.com";
            cm.ToEmail.Add(em);
            string Username ="Godwin";//= item[i].ToString().Trim();
            //body = body.Replace("Dear Adewole", "Dear " + Username);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Dear" + " " + Username + ",");
            sb.AppendLine();
            sb.AppendLine("Kindly note that the above service has count down days of");
            sb.AppendLine();
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
           // _logger.LogInformation("User created a new account with username " + Ruser.UserName);

        }

        public async Task<IEnumerable<Service>> SearchItem(string desc)
        {
            var itm= (IEnumerable<Service>)null;
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
                exp.ServiceDesc= entity.ServiceDesc;    
                exp.Company= entity.Company;    
                exp.Frequency= entity.Frequency;    
                exp.Email= entity.Email;    
                exp.Daysnotification= entity.Daysnotification;  
                exp.Credentials = entity.Credentials;   
                exp.SetupBy=entity.SetupBy;
                exp.ContactStaff=entity.ContactStaff;   
                exp.Enddate=entity.Enddate; 
                exp.Status=entity.Status;   


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

                if(ext.ToLower() == ".xls")
                {
                    excelType = ExcelProcessor.ExcelType.Xls;
                }

               
                var excel = new ExcelProcessor();
                var Svc = excel.ImportExcelData<Service>(filepath, excelType);
                //Add to Db
                foreach(Service cs in Svc)
                {
                   await Create(cs);

                   Console.WriteLine("Inserted Desc = " + cs.ServiceDesc);
                }

            }

        }
    }
}
