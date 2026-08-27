namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public enum ConditionType
{
	None,
	WithinTimeProfile,
	Always,
	OutsideTimeProfile,
	TimeBetween,
	DaysOfWeek,
	WithinWindow,
	FailoverActive,
	FailoverInactive
}
