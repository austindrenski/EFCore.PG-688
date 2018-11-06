using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NpgsqlTypes;

// ReSharper disable StringLiteralTypo
namespace EFCore.PG_688
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SomeContext())
            {
                Console.WriteLine(ctx.SomeModel.Count());
            }
        }
    }

    public class SomeModel
    {
        public NpgsqlRange<LocalTime> Period { get; set; }
    }

    public class SomeContext : DbContext
    {
        public DbSet<SomeModel> SomeModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.UseNpgsql(
                "Host=localhost;Port=5432",
                x => x.UseNodaTime().MapRange<LocalTime>("timerange"));

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ForNpgsqlHasRange("timerange", "time");
    }
}