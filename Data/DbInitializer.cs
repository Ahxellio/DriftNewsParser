using DriftNews.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriftNewsParser.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _db;
        public DbInitializer(ApplicationDbContext db, ILogger<DbInitializer> Logger)
        {
            _db = db;
        }
        public async Task InitializeAsync() 
        {
            if (await _db.DriversRDS.AnyAsync()) return;

        }
    }
}
