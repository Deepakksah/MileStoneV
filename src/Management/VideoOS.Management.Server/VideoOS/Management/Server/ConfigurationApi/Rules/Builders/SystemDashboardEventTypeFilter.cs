using VideoOS.Management.Server.BusinessObjects.EventTypes;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class SystemDashboardEventTypeFilter : BaseEventTypeFilter
{
	private string _generatorSubType;

	public SystemDashboardEventTypeFilter(string generatorSubType)
		: base(new string[1] { "SystemDashboard" })
	{
		_generatorSubType = generatorSubType;
	}

	public override bool Filter(EventTypeInfo eventTypeInfo)
	{
		return eventTypeInfo.GeneratorSubType == _generatorSubType;
	}
}
