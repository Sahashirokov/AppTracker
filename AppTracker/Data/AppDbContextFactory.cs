using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LauncherApp.Data;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "AppTrackerDB.db"
        );
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={path}")
            .Options;

        return new AppDbContext(options);
    }
}