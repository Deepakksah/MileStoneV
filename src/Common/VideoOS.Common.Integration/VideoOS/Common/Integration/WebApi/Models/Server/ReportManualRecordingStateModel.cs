using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ReportManualRecordingStateModel : CommandModelBase
{
	public Guid DeviceId { get; set; }

	public bool IsManualRecording { get; set; }
}
