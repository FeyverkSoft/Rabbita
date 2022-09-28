using Example.Handlers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Example
{
    using Microsoft.Extensions.Hosting;

    using Rabbita.Core.Command;
    using Rabbita.Core.Event;
    using Rabbita.InProc.FluentExtensions.Command;
    using Rabbita.InProc.FluentExtensions.Event;

    class Program
    {
        static async Task Main(string[] args)
        {
            var sp = ConfigServiceProvider().CreateScope();
            var cb = sp.ServiceProvider.GetService<ICommandBus>();
            var eb = sp.ServiceProvider.GetService<IEventBus>();
            await eb.SendAsync(new Dsdfs
            {
                Id = "1"
            });
            await cb.SendAsync(new CDsdfs
            {
                Id = "2"
            });
            var hs = sp.ServiceProvider.GetServices<IHostedService>();
            foreach (var hostedService in hs)
            {
                await hostedService.StartAsync(CancellationToken.None);
            }

            Console.ReadLine();
        }

        private static ServiceProvider ConfigServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logging =>
            {
                logging.AddConsole(options =>
                {
                    options.Format = Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat.Default;
                    options.DisableColors = false;
                    options.IncludeScopes = true;
                    options.TimestampFormat = "HH:mm:ss.ffff ";
                });
            });

            serviceCollection.AddEventBus();
            serviceCollection.AddCommandBus();
            serviceCollection
                .AddEventProcessor(
                    registry =>
                    {
                        registry.Register<DsdfsHandler>()
                            .AddExceptionHandler<NotImplementedException, DsdfsHandler>()
                            .AddExceptionHandler<Exception, DsdfsHandler>();
                    })
                .AddCommandProcessor(
                    registry =>
                    {
                        registry.Register<CDsdfsHandler>()
                            .AddExceptionHandler<Exception, CDsdfsHandler>();
                    });   
            return serviceCollection.BuildServiceProvider();
        }
    }
}