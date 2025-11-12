using Horse.Models.Enums;

namespace Horse.Web.Components
{
	internal static class Extensions
	{
		public static string ToWeightState(this HorseWeightState @this) =>
			@this switch
			{
				HorseWeightState.GoodWeight  => "Good weight",
				HorseWeightState.Underweight => "Underweight",
				HorseWeightState.Overweight  => "Overweight",
				_                            => "No weight state",
			};

		public static string ToState(this State @this) =>
			@this switch
			{
				State.Active    => "Active",
				State.Inactive  => "Inactive",
				State.Pension   => "Pension",
				State.UnderCare => "Under care",
				_               => "No state",
			};

		public static string ToSexHuman(this HumanSex @this) =>
			@this switch
			{
				HumanSex.Female => "Female",
				HumanSex.Male   => "Male",
				_               => "None",
			};

		public static string ToSexHorse(this HorseSex @this) =>
			@this switch
			{
				HorseSex.Mare     => "Mare",
				HorseSex.Stallion => "Stallion",
				HorseSex.Gelding  => "Gelding",
				_                 => "None",
			};

		public static string ToAppointmentType(this AppointmentType @this) =>
			@this switch
			{
				AppointmentType.Veterinary => "Veterinary",
				AppointmentType.Physio     => "Physio",
				AppointmentType.Farrier    => "Farrier",
				AppointmentType.Other      => "Other",
				_                          => "None",
			};

		public static DateTime ToAmsterdam(this DateTime @this) =>
			@this.Add(TimeZone.Amsterdam.GetUtcOffset(@this));
	}
}
