using System.ComponentModel.DataAnnotations.Schema;

namespace Horse.Models
{
	[Table("horse_races")]
	public sealed class HorseRace
	{
		[Column("id")] public int Id { get; init; }

		[Column("name")] public required string Name { get; init; }
	}
}
