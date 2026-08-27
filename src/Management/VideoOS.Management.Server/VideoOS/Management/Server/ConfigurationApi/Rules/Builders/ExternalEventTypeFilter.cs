using VideoOS.Management.Server.BusinessObjects.EventTypes;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class ExternalEventTypeFilter : BaseEventTypeFilter
{
	private string _generatorSubType;

	public ExternalEventTypeFilter(string generatorSubType)
		: base(new string[1] { "External" })
	{
		_generatorSubType = generatorSubType;
	}

	public override bool Filter(EventTypeInfo eventTypeInfo)
	{
		return eventTypeInfo.GeneratorSubType == _generatorSubType;
	}
}
