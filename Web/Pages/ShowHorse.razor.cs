using Horse.Models;
using Horse.Models.Enums;
using Horse.Models.Update;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Horse.Web.Pages
{
	public sealed partial class ShowHorse : ComponentBase
	{
		private const string formName = "Update" + nameof(ShowHorse);

		private Horsie horsie = null!;

		[Parameter] public int Id { get; init; }

		[Inject] public required NavigationManager NavigationManager { get; init; }

		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }

		[SupplyParameterFromForm(FormName = ShowHorse.formName)]
		private UpdateHorse UpdateModel { get; set; } = new();

		private List<HorseRace> races = [];
		private List<Owner> owners = [];

		private bool hidden = true;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.horsie = await db.Horses
									  .Include(static (h) => h.Owner)
									  .Include(static (h) => h.Race)
									  .Include(static (h) => h.HorseImage)
									  .Include(static (h) => h.Comments)
									  .Include(static (h) => h.Appointments)
									  .FirstOrDefaultAsync(h => h.Id == this.Id) ??
							  throw new InvalidOperationException();
				this.owners = await db.Owners
									  .ToListAsync();
				this.races = await db.HorsesRaces
									 .ToListAsync();
			}

			this.UpdateModel = new UpdateHorse()
			{
				BirthDate = this.horsie.BirthDate,
			};
		}



		private async Task UpdateHorse()
		{
			this.UpdateModel.RaceId = this.UpdateModel.RaceId != 0 ? this.UpdateModel.RaceId : this.horsie.RaceId;
			this.UpdateModel.OwnerId = this.UpdateModel.OwnerId != 0 ? this.UpdateModel.OwnerId : this.horsie.OwnerId;
			this.UpdateModel.Name = !string.IsNullOrWhiteSpace((this.UpdateModel.Name))
				? this.UpdateModel.Name
				: this.horsie.Name;
			this.UpdateModel.FullName = !string.IsNullOrWhiteSpace((this.UpdateModel.FullName))
				? this.UpdateModel.Name
				: this.horsie.FullName;
			this.UpdateModel.Chip = !string.IsNullOrWhiteSpace((this.UpdateModel.Chip))
				? this.UpdateModel.Chip
				: this.horsie.Chip;
			this.UpdateModel.Sex = HorseSex.None != this.UpdateModel.Sex ? this.UpdateModel.Sex : this.horsie.Sex;
			this.UpdateModel.BirthDate ??= this.horsie.BirthDate;
			this.UpdateModel.Color = string.IsNullOrWhiteSpace(this.UpdateModel.Color)
				? this.UpdateModel.Color
				: this.horsie.Color;
			this.UpdateModel.Weight ??= this.horsie.Weight;
			this.UpdateModel.Height ??= this.horsie.Height;
			this.UpdateModel.WeightState = this.UpdateModel.WeightState != HorseWeightState.None
				? this.UpdateModel.WeightState
				: this.horsie.WeightState;
			this.UpdateModel.State = State.None != this.UpdateModel.State ? this.UpdateModel.State : this.horsie.State;

			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				await db.Horses
						.Where(id => id.Id == this.Id)
						.ExecuteUpdateAsync(builder => builder
													  .SetProperty(static (h) => h.RaceId, this.UpdateModel.RaceId)
													  .SetProperty(static (h) => h.OwnerId, this.UpdateModel.OwnerId)
													  .SetProperty(static (h) => h.Name, this.UpdateModel.Name)
													  .SetProperty(static (h) => h.FullName, this.UpdateModel.FullName)
													  .SetProperty(static (h) => h.Chip, this.UpdateModel.Chip)
													  .SetProperty(static (h) => h.Sex, this.UpdateModel.Sex)
													  .SetProperty(static (h) => h.BirthDate,
																   this.UpdateModel.BirthDate)
													  .SetProperty(static (h) => h.Color, this.UpdateModel.Color)
													  .SetProperty(static (h) => h.Weight, this.UpdateModel.Weight)
													  .SetProperty(static (h) => h.Height, this.UpdateModel.Height)
													  .SetProperty(static (h) => h.WeightState,
																   this.UpdateModel.WeightState)
													  .SetProperty(static (h) => h.State, this.UpdateModel.State));
				this.NavigationManager.Refresh(true);
			}
		}

		private void ToggleHidden() => this.hidden = !this.hidden;
	}
}
