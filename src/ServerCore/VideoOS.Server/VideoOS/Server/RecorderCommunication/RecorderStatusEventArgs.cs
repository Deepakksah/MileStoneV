using System;

namespace VideoOS.Server.RecorderCommunication;

public sealed class RecorderStatusEventArgs : EventArgs
{
	public Guid RecorderId { get; private set; }

	public Guid ServiceId { get; private set; }

	public RecorderStatusEventArgs(Guid recorderId, Guid serviceId)
	{
		RecorderId = recorderId;
		ServiceId = serviceId;
	}
}
