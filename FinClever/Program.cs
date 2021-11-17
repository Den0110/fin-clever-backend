using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinClever
{
    public class Program
    {
        public static void Main(string[] args)
        {

            using (var context = new MyDbContext())
            {
                var account = new Account()
                {
                    Name = "test_account",
                    Type = "debit_card"
                };

                context.Accounts.Add(account);
                context.SaveChanges();

                Console.WriteLine($"id: {account.Id}, name: {account.Name}, type: {account.Type}");
                Console.ReadLine();
            }

        }

        /* public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
     }*/
    }
}
