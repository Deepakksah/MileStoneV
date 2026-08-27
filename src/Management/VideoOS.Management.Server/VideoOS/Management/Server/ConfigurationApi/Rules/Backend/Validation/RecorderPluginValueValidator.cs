using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class RecorderPluginValueValidator : IValueValidator
{
	private readonly IRecorderPluginProxy _recorderPluginProxy;

	public RecorderPluginValueValidator(IRecorderPluginProxy recorderPluginProxy)
	{
		_recorderPluginProxy = recorderPluginProxy;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType = null, IDictionary<string, RuleParameter> parameters = null)
	{
		return _recorderPluginProxy.QueryPluginById(requestContext, value.ToString()) != null;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		return null;
	}
}
