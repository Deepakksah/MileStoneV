using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderData
{
	Guid RecorderId { get; }

	Guid ServiceId { get; }

	RecordingServerServiceType ServiceType { get; }

	RecorderConnectionStatus ConnectionStatus { get; set; }

	RecorderAttachStatus AttachStatus { get; set; }

	RecorderStatuses RecorderStatus { get; set; }
}
