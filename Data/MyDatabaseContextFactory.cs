using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetCoreSqlDb.Data;

public class MyDatabaseContextFactory : IDesignTimeDbContextFactory<MyDatabaseContext>
{
    public MyDatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        var connection = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
            ?? configuration["AZURE_SQL_CONNECTIONSTRING"]
            ?? "Server=localhost;Database=Lab2;Integrated Security=true;Encrypt=true";
        var options = new DbContextOptionsBuilder<MyDatabaseContext>()
            .UseSqlServer(connection).Options;
        return new MyDatabaseContext(options);
    }
}
