﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Core.Interfaces
{
    public interface IGenRepo<T1,T2> where T1:class 
    {
        Task<bool> Create(T1 entity);
        Task<T1> GetById(T2 Id);
        Task<IEnumerable<T1>> GetAll();
        Task<bool> Delete(T2 Id);
    }
}
