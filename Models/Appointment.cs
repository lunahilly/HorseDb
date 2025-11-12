using System.ComponentModel.DataAnnotations.Schema;
using Horse.Models.Enums;

namespace Horse.Models
{
	[Table("appointments")]
	public sealed class Appointment
	{
		[Column("id")] public int Id { get; init; }

		[Column("horse_id")] public required int HorseId { get; init; }

		[Column("type")] public required AppointmentType Type { get; init; }

		[Column("date")] public required DateTime Date { get; init; }

		[Column("description")] public string? Description { get; init; }

		public Horsie? Horse { get; init; }
	}
}
