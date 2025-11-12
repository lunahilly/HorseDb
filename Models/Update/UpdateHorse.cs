using System.ComponentModel.DataAnnotations;
using Horse.Models.Enums;

namespace Horse.Models.Update
{
	internal sealed class UpdateHorse
	{
		[Required] public int RaceId { get; set; }
		[Required] public int OwnerId { get; set; }
		public string? Name { get; set; }
		public string? FullName { get; set; }
		public string? Chip { get; set; }
		public string? Color { get; set; }
		public HorseSex Sex { get; set; }
		public DateOnly? BirthDate { get; set; }
		public int? Height { get; set; }
		public int? Weight { get; set; }
		public HorseWeightState WeightState { get; set; }
		public State State { get; set; }
	}
}
