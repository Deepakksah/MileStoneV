using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class SubDayModel
{
	public ScheduleTypes.SubdayType SubDayType { get; set; }

	public int StartTimeOfDay { get; set; }

	public int EndTimeOfDay { get; set; }

	public int Frequency { get; set; }
}
