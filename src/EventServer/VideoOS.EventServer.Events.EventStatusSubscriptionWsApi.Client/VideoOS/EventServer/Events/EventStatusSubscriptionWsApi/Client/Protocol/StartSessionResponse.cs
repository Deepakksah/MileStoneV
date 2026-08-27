using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class StartSessionResponse : ResponseBase
{
	public Guid SessionId { get; }

	[DefaultValue(30)]
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public int InactiveTimeoutSeconds { get; }

	public StartSessionResponse(ulong commandId, int status, Guid sessionId, int inactiveTimeoutSeconds)
		: base(commandId, status)
	{
		SessionId = sessionId;
		InactiveTimeoutSeconds = inactiveTimeoutSeconds;
	}
}
