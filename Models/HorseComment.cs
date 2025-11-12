using System.ComponentModel.DataAnnotations.Schema;

namespace Horse.Models
{
	[Table("horse_comments")]
	public sealed class HorseComment
	{
		[Column("id")] public int? Id { get; init; }

		[Column("horse_id")] public required int HorseId { get; init; }

		[Column("date")] public DateOnly Date { get; init; }

		[Column("comment")] public required string Text { get; init; }

		public Horsie? Horse {get; init; }
	}
}
