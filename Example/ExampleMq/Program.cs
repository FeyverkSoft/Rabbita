using System;

using Example.Handlers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Rabbita.Core.DefaultHandlers;
using Rabbita.Core.MessageSerializer;
using Rabbita.Mq.Abstraction.Bus;
using Rabbita.Mq.FluentExtensions;

namespace Example
{
    using Rabbita.Mq.FluentExtensions.Event;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConfigServiceProvider();
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

            serviceCollection.AddEventBus(options =>
                {
                    options.AddDefaultInstance(iOptions => { iOptions.ConnectionString = ""; });
                    options.AddInstance("dfjk", iOptions => { iOptions.ConnectionString = ""; });
                },
                messageBinder =>
                {
                    messageBinder.BindMessage<Dsdfs>()
                        .ToInstance("dfjk", iOpt =>
                        {
                            iOpt.ToQueue("test_message")
                                .ToQueue("test_message1");
                        })
                        .HasKey(_ => _.Id)
                        .AddSerializer(new JsonMessageSerializer());

                    messageBinder.BindMessage<Dsdfs1>()
                        .ToInstance("dfjk", iOpt =>
                        {
                            iOpt.ToExchanger("test_message", ExchangeType.Direct)
                                .ToQueue("test_message");
                        })
                        .HasKey(_ => _.Id)
                        .AddSerializer(new JsonMessageSerializer());

                    messageBinder.BindMessage<Dsdfs2>()
                        .ToDefaultInstance(iOpt => { iOpt.ToExchanger("test_message", ExchangeType.Direct); })
                        .HasKey(_ => _.Id);
                });

            serviceCollection.AddCommandBus(options =>
                {
                    options.AddDefaultInstance(iOptions => { iOptions.ConnectionString = ""; });
                    options.AddInstance("dfjk", iOptions => { iOptions.ConnectionString = ""; });
                },
                messageBinder =>
                {
                    messageBinder.BindMessage<CDsdfs>()
                        .ToInstance("dfjk", iOpt =>
                        {
                            iOpt.ToQueue("test_message")
                                .ToQueue("test_message1");
                        })
                        .HasKey(_ => _.Id)
                        .AddSerializer(new JsonMessageSerializer());

                    messageBinder.BindMessage<CDsdfs1>()
                        .ToInstance("dfjk", iOpt =>
                        {
                            iOpt.ToExchanger("test_message", ExchangeType.Direct)
                                .ToQueue("test_message");
                        })
                        .HasKey(_ => _.Id)
                        .AddSerializer(new JsonMessageSerializer());

                    messageBinder.BindMessage<CDsdfs2>()
                        .ToDefaultInstance(iOpt => { iOpt.ToExchanger("test_message", ExchangeType.Direct); })
                        .HasKey(_ => _.Id);
                });

            serviceCollection.AddEventProcessor(options =>
                {
                    options.EnableMultiTypeQueues = true;
                    options.AddDefaultInstance(iOptions => { iOptions.ConnectionString = ""; });
                    options.AddInstance("dfjk", iOptions => { iOptions.ConnectionString = ""; });
                },
                registry =>
                {
                    registry.Register<DsdfsHandler>()
                        .FromInstance("dfjk", iOpt =>
                        {
                            iOpt.FromQueue("test_message");
                            iOpt.FromQueue("test_message1");
                        })
                        .AddSerializer(new JsonMessageSerializer())
                        .AddExceptionHandler<Exception, ExceptionHandler>()
                        .SetConsumerPrefetchCount(4);
                });

            return serviceCollection.BuildServiceProvider();
        }
    }
}