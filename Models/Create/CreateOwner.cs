using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Horse.Models.Enums;

namespace Horse.Models.Create
{
	internal sealed class CreateOwner : IValidatableObject
	{
		[Required] public string? GivenName { get; set; }
		[Required] public string? FamilyName { get; set; }
		public DateOnly? BirthDate { get; set; }
		//? zodat je niet hoeft te checken of hij nul is en dat staat hij op de page niet ingevuld en of de datum van vandaag
		[Required] public HumanSex Sex { get; set; }
		[Required] [EmailAddress] public string? Email { get; set; }
		//parameters ervoor voor validation form
		public string? Phone { get; set; }

		public bool IsValid
		{
			[MemberNotNullWhen(true, nameof(this.GivenName), nameof(this.FamilyName), nameof(this.Email))]
			get => this.GivenName is not null && this.FamilyName is not null && this.Email is not null;
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (this.Phone is not null && this.Phone.Length < 10 && this.Phone.Length > 14)
			{
				yield return new ValidationResult("Phone number lenght is not correct", [nameof(this.Phone)]);
			}
		}
		//custom validation voor phone, yield zorgt ervoor dat hij niet meteen stopt, maar verder gaat zodat meerdere dingen returned kunnen worden
	}
}
