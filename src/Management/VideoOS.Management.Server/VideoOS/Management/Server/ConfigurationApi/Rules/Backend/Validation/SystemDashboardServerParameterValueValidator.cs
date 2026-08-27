using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class SystemDashboardServerParameterValueValidator : IValueValidator
{
	private readonly IRecordingServerProxy _recordingServerProxy;

	private readonly IServerInformationProxy _serverInformationProxy;

	private Guid _managementServerId = Guid.Empty;

	public SystemDashboardServerParameterValueValidator(IRecordingServerProxy recordingServerProxy, IServerInformationProxy serverInformationProxy)
	{
		_recordingServerProxy = recordingServerProxy;
		_serverInformationProxy = serverInformationProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if (_managementServerId == Guid.Empty)
		{
			_managementServerId = new Guid(_serverInformationProxy.GetServerInformation(requestContext).Id);
		}
		if (string.IsNullOrEmpty(itemId) || (Guid.TryParse(itemId, out var result) && (SourceGuid.Server.Equals(result) || _managementServerId.Equals(result))))
		{
			return "System";
		}
		if ((Guid.TryParse(itemId, out result) && SourceGuid.AnySource.Equals(result)) || _recordingServerProxy.QueryRecordingServer(requestContext, itemId) != null)
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
