using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.IO;

namespace FinClever
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EntityFrameworkProfilerBootstrapper.PreStart();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                     webBuilder.UseUrls("https://localhost:5001", "https://192.168.0.248:5001");
                     webBuilder.UseIISIntegration();
                     webBuilder.UseShutdownTimeout(TimeSpan.FromSeconds(60));
                     webBuilder.UseStartup<Startup>();
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        var jobKey = new JobKey("CachingStockHistoryJob");
                        q.AddJob<CachingStockHistoryJob>(opts => opts.WithIdentity(jobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity("CachingStockHistoryJob-trigger")
                            .WithSimpleSchedule(x => x
                                .WithIntervalInHours(24)
                                .RepeatForever()));
                    });

                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                 });
     
    }
}

