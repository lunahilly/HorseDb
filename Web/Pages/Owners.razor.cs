using System.Diagnostics;
using Horse.Models;
using Horse.Models.Create;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Horse.Web.Pages
{
	public partial class Owners : ComponentBase
	{
		private const string formName = nameof(Owners);
		//kleine letter naam omdat hij private is

		[SupplyParameterFromForm(FormName = Owners.formName)]
		//form name zodat we weten dat deze model wordt ingevuld met dit form, belangrijk als je meerdere formulieren in page hebt
		private CreateOwner CreateModel { get; set; } = new();

		[Inject] public required NavigationManager NavigationManager { get; init; }
		//required ervoor ipv ? && public ipv private

		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }

		[Inject] public required IJSRuntime JsRuntime { get; init; }

		private List<Owner> data = [];
		//get & set weg, onnodig en scheelt performance

		private bool hidden = true;
		private int delete;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.data = await db.Owners
									.Include(static (o) => o.Horses)
									.OrderBy(static (o) => o.Id)
									.ToListAsync();
			}
		}

		private async Task AddOwner()
		{
			Debug.Assert(this.CreateModel.IsValid, "Validation did not run");
			//In development krijg je een melding als er iets niet klopt, dit is in production gone. 2de parameter is de foutmelding

			var db = await this.DbFactory.CreateDbContextAsync();
			//consistency niet deze context noemen en de ander db

			await using (db)
			{
				var entry = db.Owners.Add(new Owner()
				{
					GivenName = this.CreateModel.GivenName,
					FamilyName = this.CreateModel.FamilyName,
					Sex = this.CreateModel.Sex,
					BirthDate = this.CreateModel.BirthDate,
					Email = this.CreateModel.Email,
					Phone = string.IsNullOrWhiteSpace(this.CreateModel.Phone) ? null : this.CreateModel.Phone,
					//checked of het null of whitespace is anders geeft hij gewoon de value van create model mee
				});
				await db.SaveChangesAsync();
				this.data.Add(entry.Entity);
			}

			//if statement weg doordat er al gevalidate wordt in de form voordat hij verstuurd wordt
		}

		private async Task ShowDialog(string name, int del, string id)
		{
			await this.JsRuntime.InvokeAsync<object>("OpenDialog", name, id);
			this.delete = del;
		}

		private async Task ShowErrorDialog() =>
			await this.JsRuntime.InvokeVoidAsync("CloseDialog");

		private async Task CloseDialog(string id) =>
			await this.JsRuntime.InvokeVoidAsync("CloseDialog" , id);

		private async Task DeleteOwner(int ownerId)
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				var owner = await db.Owners.Include(static (o) => o.Horses)
									.FirstOrDefaultAsync((o) => o.Id == ownerId);
				if (owner?.Horses is { Count: < 1 })
				{
					db.Owners.Remove(owner);
					await db.SaveChangesAsync();
					this.data.Remove(owner);
				}
				else
				{
					await this.CloseDialog("DialogOwner");
					await this.ShowErrorDialog();
				}
			}
		}

		private void ToggleHidden() => this.hidden = !this.hidden;
	}
}
