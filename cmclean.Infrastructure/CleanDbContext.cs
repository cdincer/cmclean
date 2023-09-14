using cmclean.Domain.Contacts;
using Microsoft.EntityFrameworkCore;

namespace cmclean.Infrastructure
{
    public class CleanDbContext : DbContext
    {

        public CleanDbContext(DbContextOptions<CleanDbContext> options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseNpgsql("Server=localhost;Port=5432;Database=Contactmanagerdb;User Id=admin;Password=admin1234;");

    }
}