using Horse.Models;
using Horse.Models.Enums;
using Horse.Models.Update;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Horse.Web.Pages
{
	public sealed partial class ShowOwner : ComponentBase
	{
		private const string formName = nameof(ShowOwner);
		[CascadingParameter] public required Owner? Owner { get; set; }

		[Parameter] public int Id { get; init; }

		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }

		[Inject] public required NavigationManager NavigationManager { get; init; }

		[SupplyParameterFromForm(FormName = ShowOwner.formName)]
		private UpdateOwner UpdateModel { get; set; } = new();

		private bool hidden = true;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.Owner = await db.Owners
									 .Include(static (o) => o.Horses)
									 .FirstOrDefaultAsync((o) => o.Id == this.Id);
			}
		}

		private async Task UpdateOwner()
		{
			this.UpdateModel.GivenName = !string.IsNullOrWhiteSpace(this.UpdateModel.GivenName)
				? this.UpdateModel.GivenName
				: this.Owner?.GivenName;
			this.UpdateModel.FamilyName = !string.IsNullOrWhiteSpace(this.UpdateModel.FamilyName)
				? this.UpdateModel.FamilyName
				: this.Owner?.FamilyName;
			this.UpdateModel.BirthDate ??= this.Owner?.BirthDate;
			this.UpdateModel.Sex = this.UpdateModel.Sex != HumanSex.None ? this.UpdateModel.Sex : this.Owner!.Sex;
			this.UpdateModel.Email = !string.IsNullOrWhiteSpace(this.UpdateModel.Email)
				? this.UpdateModel.Email
				: this.Owner?.Email;
			this.UpdateModel.Phone = !string.IsNullOrWhiteSpace(this.UpdateModel.Phone)
				? this.UpdateModel.Phone
				: this.Owner?.Phone;

			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				await db.Owners
						.Where(id => id.Id == this.Id)
						.ExecuteUpdateAsync(builder => builder
													  .SetProperty(static (h) => h.GivenName,
																   this.UpdateModel.GivenName)
													  .SetProperty(static (h) => h.FamilyName,
																   this.UpdateModel.FamilyName)
													  .SetProperty(static (h) => h.BirthDate,
																   this.UpdateModel.BirthDate)
													  .SetProperty(static (h) => h.Sex, this.UpdateModel.Sex)
													  .SetProperty(static (h) => h.Email, this.UpdateModel.Email)
													  .SetProperty(static (h) => h.Phone, this.UpdateModel.Phone));
				this.NavigationManager.Refresh(true);
			}
		}

		private bool BirthdayToday()
		{
			if (Owner.BirthDate != null && Owner.BirthDate.Value.DayOfYear == DateTime.Today.DayOfYear)
			{
				return true;
			}

			return false;
		}

		private void ToggleHidden() => this.hidden = !this.hidden;
	}
}
