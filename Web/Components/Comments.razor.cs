using System.Diagnostics;
using Horse.Models;
using Horse.Models.Create;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Horse.Web.Components
{
	public sealed partial class Comments : ComponentBase
	{
		private const string formName = nameof(Comments);

		private List<HorseComment> horsie;

		[Parameter] public int Id { get; init; }

		[Inject] public required IDbContextFactory<HorseDbContext> ContextFactory { get; init; }

		[SupplyParameterFromForm(FormName = Comments.formName)]
		private CreateHorseComment CreateModel { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			var db = await this.ContextFactory.CreateDbContextAsync();
			await using (db)
			{
				this.horsie = await db.HorseComments
									  .Where(h => h.HorseId == this.Id)
									  .ToListAsync();
			}
		}

		private async Task AddComment()
		{
			Debug.Assert(this.CreateModel.IsValid, "Validation did not run");
			var db = await this.ContextFactory.CreateDbContextAsync();

			await using (db)
			{
				var newComment = db.HorseComments.Add(new HorseComment()
				{
					HorseId = this.Id,
					Text = this.CreateModel.Text,
					Date = DateOnly.FromDateTime(DateTime.Now),
				});
				await db.SaveChangesAsync();
				this.horsie.Add(newComment.Entity);
			}
		}
	}
}
