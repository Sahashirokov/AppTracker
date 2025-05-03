using System.Collections.Generic;
using System.Threading.Tasks;
using LauncherApp.MVVM.Model;
using LauncherApp.Repository.FavoriteAppRepository;

namespace LauncherApp.Services;

public interface IFavoriteAppService
{
    Task<List<AppM?>> LoadAppsAsync();
    Task<AppM> GetUserByIdAsync(int id);
}

public class FavoriteAppService: IFavoriteAppService
{
    private readonly IFavoriteAppRepository _favoriteAppRepository;

    public FavoriteAppService(IFavoriteAppRepository favoriteAppRepository)
    {
        _favoriteAppRepository = favoriteAppRepository;
    }
    
    public async Task<List<AppM?>> LoadAppsAsync()
    {
        
        return await _favoriteAppRepository.GetAllAsync();
    }

    public Task<AppM> GetUserByIdAsync(int id)
    {
        throw new System.NotImplementedException();
    }
}