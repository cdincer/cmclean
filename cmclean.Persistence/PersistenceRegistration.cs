using cmclean.Application.Interfaces.Repositories.Contacts;
using cmclean.Persistence.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cmclean.Persistence
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContactReadRepository, ContactReadRepository>();
            services.AddTransient<IContactWriteRepository, ContactWriteRepository>();
            return services;
        }
    }
}
