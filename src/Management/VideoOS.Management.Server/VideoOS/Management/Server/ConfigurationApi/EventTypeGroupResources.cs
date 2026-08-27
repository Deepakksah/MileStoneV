using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

public class EventTypeGroupResources
{
	public string ResourceId;

	public GeneratorType GeneratorType;

	public string GeneratorSubType;

	public EventTypeGroupResources(string resourceId, GeneratorType generatorType, string generatorSubType)
	{
		ResourceId = resourceId;
		GeneratorType = generatorType;
		GeneratorSubType = generatorSubType;
	}
}
