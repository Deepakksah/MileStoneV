using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class SystemDashboardRecordingServerParameterValueValidator : IValueValidator
{
	private readonly IRecordingServerProxy _recordingServerProxy;

	public SystemDashboardRecordingServerParameterValueValidator(IRecordingServerProxy recordingServerProxy)
	{
		_recordingServerProxy = recordingServerProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if ((Guid.TryParse(itemId, out var result) && SourceGuid.AnySource.Equals(result)) || _recordingServerProxy.QueryRecordingServer(requestContext, itemId) != null)
		{
			return "RecordingServer";
		}
		return null;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		return GetItemType(requestContext, value.ToString(), null) == expectedType;
	}
}
