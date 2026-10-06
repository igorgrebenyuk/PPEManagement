using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PPEManagement.Context;

/// <summary>
/// Фабрика для создания контекста в DesignTime
/// </summary>
public class PPEManagementDesignTimeContextFactory : IDesignTimeDbContextFactory<PPEManagementContext>
{
    
        /// <summary>
        /// Creates a new instance of a derived context
        /// </summary>
        /// <remarks>
        /// 1) dotnet tool install --global dotnet-ef
        /// 2) dotnet tool update --global dotnet-ef
        /// 3) dotnet ef migrations add [name] --project DataAccessLayer/PPEManagement.Context/PPEManagement.Context.csproj
        /// 4) dotnet ef database update --project DataAccessLayer/PPEManagement.Context/PPEManagement.Context.csproj
        /// 5) dotnet ef database update [targetMigrationName] --project DataAccessLayer/PPEManagement.Context/PPEManagement.Context.csproj
        /// </remarks>
        public PPEManagementContext CreateDbContext(string[] args)
        {
        var connectionString = "Host=localhost;Port=5103;Database=PPEManagement;Username=postgres;Password=";
        var options = new DbContextOptionsBuilder<PPEManagementContext>()
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine)
            .Options;
        
        return new PPEManagementContext(options);
    }
}