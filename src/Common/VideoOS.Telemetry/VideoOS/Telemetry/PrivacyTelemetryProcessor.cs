using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace VideoOS.Telemetry;

public class PrivacyTelemetryProcessor : ITelemetryProcessor
{
	private readonly ITelemetryProcessor _next;

	public PrivacyTelemetryProcessor(ITelemetryProcessor next)
	{
		_next = next ?? throw new ArgumentNullException("next");
	}

	public void Process(ITelemetry item)
	{
		if (item != null)
		{
			item.Context.Cloud.RoleInstance = string.Empty;
			TelemetryContextExtensions.GetInternalContext(item.Context).NodeName = string.Empty;
			item.Context.Location.Ip = "0.0.0.0";
		}
		_next.Process(item);
	}
}
