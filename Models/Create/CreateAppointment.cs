using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Horse.Models.Enums;

namespace Horse.Models.Create
{
	internal sealed class CreateAppointment
	{
		[Required] public int HorseId { get; set; }

		[Required] public AppointmentType  Type { get; set; }

		[Required] public DateTime? Date { get; set; }

		public string Description { get; set; }

		public bool IsValid
		{
			[MemberNotNullWhen(true, nameof(this.Date), nameof(this.HorseId))]
			get => this.Date is not null && this.HorseId != 0;
		}
	}
}
