using System.Collections.Generic;
using System.Threading.Tasks;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Repository.FavoriteAppRepository;

public interface IFavoriteAppRepository:IRepository<AppM>
{
    Task<IEnumerable<AppM>> GetActiveAppsAsync();
}