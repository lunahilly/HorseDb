using System.ComponentModel.DataAnnotations;
using Horse.Models.Enums;

namespace Horse.Models.Update
{
	internal sealed class UpdateOwner : IValidatableObject
	{
		public string? GivenName { get; set; }
		public string? FamilyName { get; set; }
		public DateOnly? BirthDate { get; set; }
		public HumanSex Sex { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.GivenName is null && this.FamilyName is null && this.BirthDate is null &&
				this.Sex is HumanSex.None && this.Email is null && this.Phone is null)
			{
				yield return new ValidationResult("There is no new value");
			}
			if (this.Phone is not null && this.Phone.Length < 10 && this.Phone.Length > 14)
			{
				yield return new ValidationResult("Phone number lenght is not correct", [nameof(this.Phone)]);
			}
		}
	}
}
