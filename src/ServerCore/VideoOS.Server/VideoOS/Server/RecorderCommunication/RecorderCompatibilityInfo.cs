using System;

namespace VideoOS.Server.RecorderCommunication;

public sealed class RecorderCompatibilityInfo
{
	public Guid Id { get; private set; }

	public Guid ServiceId { get; private set; }

	public bool Connected { get; private set; }

	public bool IsConfigurationCompatible { get; private set; }

	public RecorderCompatibilityInfo(Guid recorderId, Guid serviceId, bool connected, bool isConfigurationCompatible)
	{
		Id = recorderId;
		ServiceId = serviceId;
		Connected = connected;
		IsConfigurationCompatible = isConfigurationCompatible;
	}
}
