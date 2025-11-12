using Horse.Models;
using Microsoft.EntityFrameworkCore;

namespace Horse
{
	public sealed class HorseDbContext : DbContext
	{
		public required DbSet<Models.Horsie> Horses { get; set; }
		public required DbSet<HorseImage> HorseImages { get; set; }
		public required DbSet<Owner> Owners { get; set; }
		public required DbSet<HorseRace> HorsesRaces { get; set; }
		public required DbSet<HorseComment> HorseComments { get; set; }

		public required DbSet<Appointment>  Appointments { get; set; }

		public HorseDbContext(DbContextOptions<HorseDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Horsie>(static (h) =>
			{
				h.HasOne(static (h) => h.Owner);
				h.HasOne(static (h) => h.Race);
				h.HasOne(static (h) => h.HorseImage);
				h.HasMany(static (h) => h.Comments);
				h.HasMany(static (h) => h.Appointments);
			});

			modelBuilder.Entity<Owner>(static (o) =>
			{
				o.HasMany(static (o) => o.Horses);
			});

			modelBuilder.Entity<HorseImage>(static (hi) =>
			{
				hi.HasOne(static (hi) => hi.Horse);
			});

			modelBuilder.Entity<HorseComment>(static (c) =>
			{
				c.HasOne(static (c) => c.Horse);
			});

			modelBuilder.Entity<Appointment>(static (a) =>
			{
				a.HasOne(static (a) => a.Horse);
			});
		}
	}
}
