using VideoOS.ConfigurationAPI;
using VideoOS.Platform.JsonHandling;

namespace VideoOS.Management.Server.RestApi;

public interface IConfigurationItemSerializer
{
	string SerializeParameters();

	string SerializeInvokeResultWithFilter(IRestApiRequestParameters restApiRequestParameters, string methodId, bool includeError, bool includeProgress, out bool badRequest);

	bool SerializeInvokeResultErrors(JsonBuilder jsonBuilder);

	string TaskListInJson();

	string Resources(IRestApiRequestContext context, ConfigurationItem[] items);

	string Definitions(IRestApiRequestContext context, string methodId);

	string Serialize(IRestApiRequestParameters restApiRequestParameters);
}
