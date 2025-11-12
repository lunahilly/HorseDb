using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Horse.Models.Enums;

namespace Horse.Models.Create
{
	internal sealed class CreateHorsie : IValidatableObject
	{
		public int Id { get; set; }
		public int? RaceId { get; set; }
		[Required] public int? OwnerId { get; set; }
		[Required] public string? Name { get; set; }
		[Required] public string? FullName { get; set; }
		[Required] public string? Chip { get; set; }
		public string? Color { get; set; }
		[Required] public HorseSex Sex { get; set; }
		public DateOnly? Birthdate { get; set; }
		public int? Height { get; set; }
		public int? Weight { get; set; }
		[Required] public HorseWeightState WeightState { get; set; }
		[Required] public State State { get; set; }

		public bool IsValid
		{
			[MemberNotNullWhen(true, nameof(this.OwnerId), nameof(this.Name),
							   nameof(this.FullName), nameof(this.Chip))]
			get => this.OwnerId is not null && this.Name is not null &&
				   this.FullName is not null && this.Chip is not null;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var enumerable = this.Chip;
			if (enumerable == null)
			{
				yield break;
			}

			foreach (char c in enumerable)
			{
				if (!char.IsDigit(c))
				{
					yield return new ValidationResult("Invalid chip number", [nameof(enumerable)]);
				}
			}

			if (enumerable.Length != 15)
			{
				yield return new ValidationResult("Chip must be 15 characters", [nameof(enumerable)]);
			}
		}
	}
}
