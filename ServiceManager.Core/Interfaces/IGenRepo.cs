using ServiceManager.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Core.Interfaces
{
    public interface IGenRepo<T1,T2> where T1:class 
    {
        Task<bool> Create(T1 entity);
        Task<bool> Update(T1 entity);
        Task<T1> GetById(T2 Id);
        Task<IEnumerable<T1>> GetAll();
        Task<IEnumerable<T1>> GetAllDisabled();
        Task<IEnumerable<T1>> SearchItem(string desc);
        Task<bool> Delete(T2 Id);
        void MonitorServiceAlert();
        Task UploadExcell(string filepath, string ext);
    }
}
