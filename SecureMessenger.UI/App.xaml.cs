using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SecureMessenger.Infrastructure.Persistence;
using SecureMessenger.Application.Messages.Commands;
using SecureMessenger.Infrastructure.Services;
using SecureMessenger.Workers;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace SecureMessenger.UI
{
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Console.WriteLine("App starting...");

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                var host = ServiceProvider.GetRequiredService<IHost>();
                host.StartAsync(CancellationToken.None).Wait();

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Fatal Error: {ex.Message}\n{ex.StackTrace}", "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Fatal Error: {ex}");
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(SendMessageCommandHandler).Assembly);
            services.AddDbContext<MessengerDbContext>(options =>
                options.UseMySql("Server=localhost;Database=SecureMessenger;User=messenger_user;Password=frizider;",
                    new MySqlServerVersion(new Version(8, 0, 30))));
            services.AddSingleton<AesEncryptionService>();
            services.AddSingleton<MessageWorker>();
            services.AddSingleton<MainWindow>();
            services.AddHostedService<MessageWorker>();
            services.AddValidatorsFromAssemblyContaining<SendMessageCommandValidator>();

            // Dodaj ovo za IHost
            services.AddHostedService<MessageWorker>();
            services.AddSingleton<IHost>(provider => Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MessageWorker>();
                })
                .Build());
        }
    }
}
