using System;
using VideoOS.RecorderWebservice.Shared.Status2;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public interface IRecorderStatusService2
{
	Status GetCurrentDeviceStatus(string token, Guid[] deviceIds);

	AttachAndConnectionState GetRecorderStatus(string token);

	VideoDeviceStatistics[] GetVideoDeviceStatistics(string token, Guid[] deviceIds);

	void Abort();
}
