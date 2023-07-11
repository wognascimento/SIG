using Microsoft.EntityFrameworkCore;
using System;

namespace HT
{
    public class DatabaseContext : DbContext
    {
        private DataBaseSettings BaseSettings  = DataBaseSettings.Instance;
       
        
        static DatabaseContext() => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                $"host={BaseSettings.Host};" +
                $"user id={BaseSettings.Username};" +
                $"password={BaseSettings.Password};" +
                $"database={BaseSettings.Database};" //+
                //$"Pooling=false;" +
                //$"Timeout=300;" +
                //$"CommandTimeout=300;"
                );
        }
    }
}
