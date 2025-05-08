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
    Task<AppM> GetUserByIdAsync(int id);
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

    public Task<AppM> GetUserByIdAsync(int id)
    {
        throw new System.NotImplementedException();
    }
    public async Task<bool> CheckIfExistsAsync(string name, string path)
        => await _favoriteAppRepository.ExistsAsync(name, path);
}