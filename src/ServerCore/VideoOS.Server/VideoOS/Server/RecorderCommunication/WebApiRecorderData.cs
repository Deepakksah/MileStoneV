using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.WebApi;

namespace VideoOS.Server.RecorderCommunication;

public sealed class WebApiRecorderData : RecorderDataBase
{
	public RecorderVersion RecorderVersion { get; private set; }

	public Uri RecorderPublicWebApiAddress { get; private set; }

	public WebApiRecorderData(Guid recorderId, Uri recorderAddress, Guid serviceId, RecordingServerServiceType serviceType, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus, RecorderVersion version)
		: base(recorderId, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus)
	{
		RecorderVersion = version;
		RecorderPublicWebApiAddress = recorderAddress;
	}
}
