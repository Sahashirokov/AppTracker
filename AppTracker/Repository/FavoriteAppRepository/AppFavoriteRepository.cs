using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LauncherApp.Data;
using LauncherApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace LauncherApp.Repository.FavoriteAppRepository;

public class AppFavoriteRepository:IFavoriteAppRepository
{
    private readonly AppDbContext _dbContext;

    public AppFavoriteRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AppM> GetByIdAsync(int id)
    {
        return await _dbContext.AppMs.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<List<AppM?>> GetAllAsync()
    {
       return await _dbContext.AppMs.ToListAsync();
    }

    public async Task AddAsync(AppM? entity)
    {
       await _dbContext.AppMs.AddAsync(entity);
       await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(string name, string path)
        => await _dbContext.AppMs
            .AnyAsync(a => a.Name == name && a.Path == path);
    public async Task UpdateAsync(AppM entity)
    {
        _dbContext.AppMs.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(AppM? entity)
    {
        if (entity != null)
        {
            _dbContext.AppMs.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public Task<IEnumerable<AppM>> GetActiveAppsAsync()
    {
        throw new System.NotImplementedException();
    }
}