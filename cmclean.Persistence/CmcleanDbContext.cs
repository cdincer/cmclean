using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using cmclean.Domain.Model;

namespace cmclean.Persistence;

public class CmcleanDbContext : DbContext
{
    public CmcleanDbContext()
    {

    }

    public CmcleanDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Contact> contacts { get; set; } = null!;
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CmcleanDbContext>
{
    public CmcleanDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("Default");
        var builder = new DbContextOptionsBuilder<CmcleanDbContext>();
        if (string.IsNullOrEmpty(connectionString))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Default");
        }
        builder.UseSqlServer(connectionString);
        return new CmcleanDbContext(builder.Options);
    }
}
