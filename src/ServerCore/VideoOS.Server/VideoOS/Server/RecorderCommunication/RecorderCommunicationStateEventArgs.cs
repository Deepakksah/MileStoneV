using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.RecorderCommunication;

public sealed class RecorderCommunicationStateEventArgs : EventArgs
{
	public Guid RecorderId { get; private set; }

	public Guid ServiceId { get; private set; }

	public RecorderStatuses Status { get; private set; }

	public RecorderConnectionStatus RecorderConnectionStatus { get; private set; }

	public RecorderAttachStatus AttachStatus { get; private set; }

	public RecorderStatuses PreviousStatus { get; private set; }

	public RecorderConnectionStatus PreviousRecorderConnectionStatus { get; private set; }

	public RecorderAttachStatus PreviousAttachStatus { get; private set; }

	public RecorderCommunicationStateEventArgs(Guid recorderId, Guid serviceId, RecorderStatuses recorderStatus, RecorderConnectionStatus commStatus, RecorderAttachStatus attachStatus, RecorderStatuses previousRecorderStatus, RecorderConnectionStatus previousCommStatus, RecorderAttachStatus previousAttachStatus)
	{
		RecorderId = recorderId;
		ServiceId = serviceId;
		Status = recorderStatus;
		RecorderConnectionStatus = commStatus;
		AttachStatus = attachStatus;
		PreviousStatus = previousRecorderStatus;
		PreviousRecorderConnectionStatus = previousCommStatus;
		PreviousAttachStatus = previousAttachStatus;
	}
}
