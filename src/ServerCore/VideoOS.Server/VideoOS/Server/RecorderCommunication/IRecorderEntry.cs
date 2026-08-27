using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderEntry : IDisposable
{
	Guid ServiceId { get; set; }

	Guid RecorderId { get; set; }

	string RecorderUri { get; set; }

	RecorderStatuses Status { get; set; }

	RecorderConnectionStatus ConnectionStatus { get; set; }

	RecorderAttachStatus AttachStatus { get; set; }

	RecordingServerServiceType ServiceType { get; set; }

	bool IsConfigurationCompatible { get; }

	bool SubscriptionOnGlobalEvents { get; set; }

	IRecorderSend Sender { get; }

	IRecorderReceive Reciever { get; }

	IServiceConnectionStatus ServiceConnectionStatus { get; }

	bool IsConnectionAlive();

	void ReleaseResources();

	void UpdateStatusData(string infoType, string data);
}
