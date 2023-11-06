using Contacts.Application.Services;
using Contacts.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Application
{
    public class DependencyInjectionConfig
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IContactTypeService, ContactTypeService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ILogService, LogService>();
        }
    }
}
