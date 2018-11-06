using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NpgsqlTypes;

// ReSharper disable StringLiteralTypo UnusedAutoPropertyAccessor.Global UnusedMember.Global
namespace EFCore.PG_688
{
    class Program
    {
        static void Main()
        {
            using (var ctx = new SomeContext())
            {
                try
                {
                    ctx.Database.EnsureCreated();
                    Console.WriteLine(ctx.SomeModel.Count());
                }
                finally
                {
                    ctx.Database.EnsureDeleted();
                }
            }
        }
    }

    public class SomeModel
    {
        public int Id { get; set; }
        public NpgsqlRange<LocalTime> Period { get; set; }
    }

    public class SomeContext : DbContext
    {
        public DbSet<SomeModel> SomeModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.UseNpgsql(
                "Host=localhost;Port=5432",
                x => x.UseNodaTime().MapRange<LocalTime>("timerange", "time"));

        protected override void OnModelCreating(ModelBuilder builder)
            => builder.ForNpgsqlHasRange("timerange", "time");
    }
}