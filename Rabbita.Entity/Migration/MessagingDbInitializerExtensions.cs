using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Rabbita.Entity.Migration
{
    public static class MessagingDbInitializerExtensions
    {
        public static IHost MessagingDbInitialize(this IHost host)
        {
            host.Services.GetService<IDbMigrationService>().Initialize();
            return host;
        }

        public static IWebHost MessagingDbInitialize(this IWebHost host)
        {
            host.Services.GetService<IDbMigrationService>().Initialize();
            return host;
        }
    }
}