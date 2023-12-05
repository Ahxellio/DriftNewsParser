using DriftNewsParser.Data;
using DriftNewsParser.Infrastructure;
using DriftNewsParser.Models;
using DriftNewsParser.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DriftNewsParser
{
    public partial class App : Application
    {
        private static IHost _Host;
        public static IHost Host => _Host ??= 
            Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Host.Services;
        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration)
            .AddServices()
            .AddViewModels()
            ;
        [STAThread]
        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            using (var scope = Services.CreateScope())
                await scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync();
            base.OnStartup(e);
            await host.StartAsync();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            using var host = Host;
            base.OnExit(e);
            host.StopAsync();
        }
    }
}
