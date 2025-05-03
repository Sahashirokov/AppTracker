using LauncherApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;

namespace LauncherApp.Data;

public class AppDbContext:DbContext
{
    public AppDbContext() {} 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<AppM?> AppMs { get; set; }
}