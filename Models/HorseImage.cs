using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Horse.Models
{
	[Table("horse_images")]
	[PrimaryKey(nameof(HorseImage.HorseId))]
	public sealed class HorseImage
	{
		[Column("horse_id")] public required int HorseId { get; init; }

		[Column("image")] public required string ImageUrl { get; init; }

		public Horsie? Horse { get; init; }
	}
}
