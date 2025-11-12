using Horse.Models;
using Horse.Models.Structs;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Horse.Web.Pages
{
	public partial class Home : ComponentBase
	{
		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }

		private List<Horsie> horses = [];
		private List<Owner> owners = [];

		private Statistics stats;

		private int totalHorses;
		private double? averageWeight;
		private int averageAge;
		private double horsesOwner;
		private int? totalWeight;
		private int count;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.horses = await db.Horses
									  .ToListAsync();
				this.owners = await db.Owners
									  .Include(static (o) => o.Horses)
									  .ToListAsync();

				this.stats = await db.Horses
									 .GroupBy(static (h) => h.Id)
									 .Select(static (h) => new Statistics()
										  {
											  totalHorseCount = h.Count(),
											  avarageWeight = h.Average(static (h) => h.Weight),
											  totalWeight = h.Sum(static (h) => h.Weight),
											  averageAge = (int)h
															   .Where(static (h) => h.BirthDate != null)
															   .Average(static (h) =>
																			DateOnly.FromDateTime(DateTime.UtcNow)
																					.Year -
																			h.BirthDate.Value.Year),
										  }
									  )
									 .FirstOrDefaultAsync();
			}

			var averageHorses = new List<int>();
			foreach (var owner in this.owners)
			{
				if (owner.Horses != null)
				{
					averageHorses.Add(owner.Horses.Count);
				}
			}

			this.horsesOwner = averageHorses.Average();
		}
	}
}
