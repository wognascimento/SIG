using HT.DataBase.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace HT.DataBase
{
    public class DatabaseContext : DbContext
    {
        private DataBaseSettings BaseSettings = DataBaseSettings.Instance;
        static DatabaseContext() => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                $"host={BaseSettings.Host};" +
                $"user id={BaseSettings.Username};" +
                $"password={BaseSettings.Password};" +
                $"database={BaseSettings.Database};");
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DataPlanejamentoModel>()
                .Property(x => x.data)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
        */

        public DbSet<ApontamentoModel> Apontamentos { get; set; }
        public DbSet<DataPlanejamentoModel> DataPlanejamentos { get; set; }
        public DbSet<FuncionarioAtivoModel> FuncionarioAtivos { get; set; }
    }
}
