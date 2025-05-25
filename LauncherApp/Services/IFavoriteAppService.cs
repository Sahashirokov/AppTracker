using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LauncherApp.MVVM.Model;
using LauncherApp.Repository.FavoriteAppRepository;

namespace LauncherApp.Services;

public interface IFavoriteAppService
{
    Task<List<AppM?>> LoadAppsAsync();
    Task AddAsync(AppM app);
    Task<AppM> GetAppByIdAsync(int id);
    Task DeleteAsync(int id);
}

public class FavoriteAppService: IFavoriteAppService
{
    private readonly IFavoriteAppRepository _favoriteAppRepository;

    public FavoriteAppService(IFavoriteAppRepository favoriteAppRepository)
    {
        _favoriteAppRepository = favoriteAppRepository;
    }
    public async Task AddAsync(AppM app)
    {
        if(await CheckIfExistsAsync(app.Name, app.Path))
            throw new InvalidOperationException("Приложение уже в избранном");
        await _favoriteAppRepository.AddAsync(app);
    }
    public async Task<List<AppM?>> LoadAppsAsync()
    {
        
        return await _favoriteAppRepository.GetAllAsync();
    }

    public async Task<AppM> GetAppByIdAsync(int id)
    {
        return await _favoriteAppRepository.GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        AppM appToDelete = await _favoriteAppRepository.GetByIdAsync(id);
        if (appToDelete != null)
        {
            await _favoriteAppRepository.DeleteAsync(appToDelete);
        }
       
    }
    public async Task<bool> CheckIfExistsAsync(string name, string path)
        => await _favoriteAppRepository.ExistsAsync(name, path);
}