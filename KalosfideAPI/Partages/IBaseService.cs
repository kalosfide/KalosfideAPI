﻿using KalosfideAPI.Erreurs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public interface IBaseService<T> where T: class
    {
        Task<ErreurDeModel> Validation(T donnée);
        Task<RetourDeService> SaveChangesAsync();
        Task<RetourDeService<T>> SaveChangesAsync(T donnée);
    }
}
