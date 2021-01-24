using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Serilog;

using Todo.AppCtx;
using Todo.DbContexts;
using Todo.Repositories;
using Todo.Services;
using Todo.ViewModels;
using Todo.Views;

namespace Todo
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            IHostBuilder builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddSerilog(logger: LoggingCtx.LogApp, dispose: true);
                    });

                    services.AddDbContext<TodoDbContext>();

                    services.AddScoped<ITodoRepository, TodoRepository>();
                    services.AddScoped<ITodoService, TodoService>();
                })
                .UseConsoleLifetime();
    
            Program.Host = builder.Build();            

            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                try
                {
                    TodoDbContext dbContext = services.GetRequiredService<TodoDbContext>();

                    dbContext.Database.Migrate();

                    LoggingCtx.LogApp.Information("Migration Finished");
                }
                catch (Exception ex)
                {
                    LoggingCtx.LogApp.Fatal(ex, "Issue While Running");

                    Environment.Exit(1);
                }
            }            

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                // {
                //     DataContext = new MainWindowViewModel(),
                // };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}