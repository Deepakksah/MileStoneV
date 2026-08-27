using System;

namespace VideoOS.Server.RecorderCommunication;

public sealed class WebApiRecorderEntryFactory : IRecorderEntryFactory
{
	public IRecorderEntry CreateRecorder(IRecorderData recorderData)
	{
		WebApiRecorderData webApiRecorderData = recorderData as WebApiRecorderData;
		if (recorderData == null)
		{
			throw new ArgumentNullException("recorderData");
		}
		return WebApiRecorderEntryBase.CreateRecorderEntry(webApiRecorderData.RecorderId, webApiRecorderData.RecorderPublicWebApiAddress, webApiRecorderData.ServiceId, webApiRecorderData.ServiceType, webApiRecorderData.ConnectionStatus, webApiRecorderData.AttachStatus, webApiRecorderData.RecorderStatus, webApiRecorderData.RecorderVersion);
	}
}
