using Contacts.Domain.Interfaces.Repositories;
using Contacts.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Persistence
{
    public class DependencyInjectionConfig
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }
    }
}
