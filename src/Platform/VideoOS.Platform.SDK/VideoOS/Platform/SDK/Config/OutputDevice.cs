using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class OutputDevice : IDevice
{
	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public Guid Guid { get; set; }

	public bool Disabled { get; set; }

	public string DBExportName { get; private set; }

	public PositioningInformation PositioningInformation => null;

	public bool CanActivate { get; set; }

	public OutputDevice(IRecorder recorder, IOutputInfo outputInfo)
	{
		Recorder = recorder;
		Guid = outputInfo.DeviceId;
		Name = outputInfo.Name;
		CanActivate = outputInfo.OutputSecurity.Activate;
		DBExportName = null;
	}

	public void OutputActivate()
	{
		Recorder.OutputActivate(Guid.ToString(), throwOnError: false);
	}

	public void OutputDeactivate()
	{
		Recorder.OutputDeactive(Guid.ToString(), throwOnError: false);
	}

	public IOState OutputGetState()
	{
		return Recorder.OutputGetState(Guid.ToString(), throwOnError: false);
	}

	public void OutputTrigger(int duration)
	{
		Recorder.OuputTrigger(Guid.ToString(), duration, throwOnError: false);
	}
}
