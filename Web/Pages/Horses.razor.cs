using System.Diagnostics;
using Horse.Models;
using Horse.Models.Create;
using Horse.Models.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Horse.Web.Pages
{
	public partial class Horses : ComponentBase
	{
		private const string formName = nameof(Horse);

		[SupplyParameterFromForm(FormName = Horses.formName)]
		private CreateHorsie CreateModel { get; set; } = new();

		[Inject] public required NavigationManager NavigationManager { get; init; }
		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }
		[Inject] public required IJSRuntime JsRuntime { get; init; }

		private List<Horsie> horse = [];
		private List<HorseRace> race = [];
		private List<Owner> owner = [];
		private string raceid = "";

		private bool hidden = true;
		private bool raceAdded;
		private int delete;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.horse = await db.Horses
									 .Include(static (h) => h.Owner)
									 .Include(static (h) => h.Race)
									 .OrderBy(static (h) => h.Id)
									 .ToListAsync();
				this.race = await db.HorsesRaces
									.ToListAsync();
				this.owner = await db.Owners
									 .ToListAsync();
			}
		}
		private async Task AddHorsie()
		{
			Debug.Assert(this.CreateModel.IsValid, "Validation did not run");
			var db = await this.DbFactory.CreateDbContextAsync();
			var parsed = int.TryParse(this.raceid, out var result);
			this.CreateModel.RaceId = result;

			if (!parsed)
			{
				db.HorsesRaces.Add(new HorseRace()
				{
					Name = this.raceid,
				});
				await db.SaveChangesAsync();
				this.raceAdded = true;
				this.race = await db.HorsesRaces
									.ToListAsync();
			}

			await using (db)
			{
				var entry = db.Horses.Add(new Horsie()
				{
					RaceId = !this.raceAdded ? (int)this.CreateModel.RaceId! : this.race.LastOrDefault()!.Id,
					OwnerId = (int)this.CreateModel.OwnerId,
					Name = this.CreateModel.Name,
					FullName = this.CreateModel.FullName,
					Chip = this.CreateModel.Chip,
					Sex = this.CreateModel.Sex,
					BirthDate = this.CreateModel.Birthdate,
					WeightState = this.CreateModel.WeightState != HorseWeightState.None
						? this.CreateModel.WeightState
						: HorseWeightState.GoodWeight,
					State = this.CreateModel.State,
					Height = this.CreateModel.Height,
					Weight = this.CreateModel.Weight,
					Color = string.IsNullOrWhiteSpace(this.CreateModel.Color) ? null : this.CreateModel.Color,
				});
				await db.SaveChangesAsync();
				this.horse.Add(entry.Entity);
			}
		}

		private async Task DeleteHorsie(int horseId)
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				await db.HorseComments
						.Where(comment => comment.HorseId == horseId)
						.ExecuteDeleteAsync();
				await db.Horses
						.Where(id => id.Id == horseId)
						.ExecuteDeleteAsync();
				await db.SaveChangesAsync();
				var index = this.horse.FindIndex((h) => h.Id == horseId);
				this.horse.RemoveAt(index);
			}

			this.CloseDialog("DialogHorses");
		}

		private void ToggleHidden() => this.hidden = !this.hidden;

		private async Task ShowDialog(string name, int del, string id)
		{
			await this.JsRuntime.InvokeAsync<object>("OpenDialog", name, id);
			this.delete = del;
		}

		private async Task CloseDialog(string id) =>
			await this.JsRuntime.InvokeVoidAsync("CloseDialog" , id);
	}
}
