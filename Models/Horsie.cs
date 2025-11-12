using System.ComponentModel.DataAnnotations.Schema;
using Horse.Models.Enums;

namespace Horse.Models
{
	[Table("horses")]
	public sealed class Horsie
	{
		[Column("race_id")] public required int RaceId { get; init; }

		[Column("owner_id")] public required int OwnerId { get; init; }

		[Column("id")] public int Id { get; init; }

		[Column("name")] public required string Name { get; init; }

		[Column("full_name")] public required string FullName { get; init; }

		[Column("chip")] public required string Chip { get; init; }

		[Column("sex")] public required HorseSex Sex { get; init; }

		[Column("birth_date")] public DateOnly? BirthDate { get; init; }

		[Column("color")] public string? Color { get; init; }

		[Column("height")] public int? Height { get; init; }

		[Column("weight")]  public int? Weight { get; init; }

		[Column("weight_state")] public HorseWeightState WeightState { get; init; }

		[Column("state")] public required State State { get; init; }

		public Owner? Owner { get; init; }
		public HorseImage? HorseImage { get; init; }

		public HorseRace? Race { get; init; }

		public List<HorseComment>? Comments { get; init; }
		public List<Appointment>? Appointments { get; init; }
	}
}
