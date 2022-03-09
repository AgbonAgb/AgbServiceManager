using Microsoft.EntityFrameworkCore;
using ServiceManager.Core.Interfaces;
using ServiceManager.Data;
using ServiceManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Core.Services
{
    public class ServiceRepo : IGenRepo<Service, int>
    {
        private readonly AppDbContext _appDbContext;
        public ServiceRepo(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }
        public async Task<bool> Create(Service entity)
        {
            bool rtn = false;
            var exp = await _appDbContext.Services.Where(x => x.ServiceDesc == entity.ServiceDesc).FirstOrDefaultAsync();
            if (exp == null)
            {
               await _appDbContext.AddAsync(entity);
               await _appDbContext.SaveChangesAsync();
                rtn = true;
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
             var exp = await _appDbContext.Services.ToListAsync();
            return exp;
        }

        public async Task<Service> GetById(int Id)
        {
            //var exp= await _appDbContext.Services.Where(x => x.Srn == Id).FirstOrDefaultAsync();


            //return exp;
            return await _appDbContext.Services.FindAsync(Id);//.Where(x => x.Srn == Id).FirstOrDefaultAsync();


        }

        public async Task<bool> Update(Service entity)
        {
            bool rtn = false;
            var exp = await _appDbContext.Services.Where(x => x.Srn == entity.Srn).FirstOrDefaultAsync();
            if (exp == null)
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


                 _appDbContext.Update(exp);
                await _appDbContext.SaveChangesAsync();
                rtn = true;
            }

            return rtn;
        }
    }
}
