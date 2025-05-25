using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Repository;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<AppM?>> GetAllAsync();
    Task AddAsync(T entity);
    Task<bool> ExistsAsync(string name, string path);
    Task UpdateAsync(T entity);
    Task DeleteAsync(AppM app);
}