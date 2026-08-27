using System;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public class PluginEventTypeFilter : BaseEventTypeFilter
{
	private IEventTypeProxy _eventTypeProxy;

	public PluginEventTypeFilter(IEventTypeProxy eventTypeProxy)
		: base(new string[2] { "MIP", "MIPDevice" })
	{
		_eventTypeProxy = eventTypeProxy;
	}

	public override string GetIdGenerator(IGeminiRequestContext requestContext, Guid eventGroupId)
	{
		return _eventTypeProxy.GetPluginEventTypeIdGenerator(requestContext, eventGroupId).ToString();
	}
}
