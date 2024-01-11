using AngleSharp;
using DriftNews.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriftNewsParser.Data
{
    public static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            Microsoft.Extensions.Configuration.IConfiguration Configuration) => services.AddDbContext<ApplicationDbContext>(opt=>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("PSQL"));
            })
            .AddTransient<DbInitializer>()
            ;

    }
}
