using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Horse.Models.Create
{
	internal sealed class CreateHorseComment
	{
		[Required] public string? Text { get; set; }

		public bool IsValid
		{
			[MemberNotNullWhen(true, nameof(this.Text))]
			get => this.Text is not null;
		}
	}
}
