﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Repository;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<AppM?>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}