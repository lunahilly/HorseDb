using System.Diagnostics;
using Horse.Models;
using Horse.Models.Create;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Horse.Web.Components
{
	public sealed partial class Appointments : ComponentBase
	{
		[Parameter] public int HorseId { get; set; }

		private const string formNameAppointment = "Appointment" + nameof(Appointments);
		[Inject] public required IDbContextFactory<HorseDbContext> DbFactory { get; init; }

		[SupplyParameterFromForm(FormName = Appointments.formNameAppointment)]
		private CreateAppointment CreateAppointment { get; set; } = new();

		private List<Appointment> appointments = [];
		private List<Horsie> horses = [];

		private bool hideUpcoming;
		private bool hidePast = true;

		protected override async Task OnInitializedAsync()
		{
			var db = await this.DbFactory.CreateDbContextAsync();
			await using (db)
			{
				this.horses = await db.Horses
									  .ToListAsync();
				if (this.HorseId == 0)
				{
					this.appointments = await db.Appointments
												.Include(static (a) => a.Horse)
												.ToListAsync();
				}
				else
				{
					this.appointments = await db.Appointments
												.Include(static (a) => a.Horse)
												.ToListAsync();
					this.appointments = this.appointments.Where(a => a.HorseId == this.HorseId).ToList();
				}
			}
		}

		private async Task AddAppointment()
		{
			Debug.Assert(this.CreateAppointment.IsValid, "Validation did not run");
			var db = await this.DbFactory.CreateDbContextAsync();

			await using (db)
			{
				var fuck = this.CreateAppointment.Date.GetValueOrDefault().ToUniversalTime();
				var newAppointment = db.Appointments.Add(new Appointment()
				{
					HorseId = this.CreateAppointment.HorseId,
					Type = this.CreateAppointment.Type,
					Date = fuck,
					Description = this.CreateAppointment.Description,
				});
				await db.SaveChangesAsync();
				this.appointments.Add(newAppointment.Entity);
			}
		}

		private void ToggleUpcoming() => this.hideUpcoming = !this.hideUpcoming;

		private void TogglePast() => this.hidePast = !this.hidePast;
	}
}
