using System.ComponentModel.DataAnnotations.Schema;
using Horse.Models.Enums;

namespace Horse.Models
{
	[Table("owners")]
	public sealed class Owner
	{
		[Column("id")] public int Id { get; init; }

		[Column("given_name")] public required string GivenName { get; init; }

		[Column("family_name")] public required string FamilyName { get; init; }

		[Column("birth_date")] public DateOnly? BirthDate { get; init; }

		[Column("sex")] public required HumanSex Sex {get; init; }

		[Column("email")] public required string Email { get; init; }

		[Column("phone")] public string? Phone { get; init; }
		public List<Horsie>? Horses { get; init; }
	}
}
