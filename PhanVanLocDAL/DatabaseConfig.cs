using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PhanVanLocDAL
{
    public static class DatabaseConfig
    {
        private static IConfiguration? _configuration;
        
        private static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    
                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        public static string ConnectionString =>
        Configuration.GetConnectionString("DefaultConnection") ??
        "Server=DESKTOP-5VBL21U\\VANLOC;Database=HotelManagement;Integrated Security=True;TrustServerCertificate=True;";


        public static void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}

