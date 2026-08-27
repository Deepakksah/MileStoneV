using VideoOS.Vms.Types;

namespace VideoOS.TrayController.Communication.Recorder;

public class RecorderState
{
	public ServiceState serviceState { get; set; }

	public ConnectStatus connectStatus { get; set; }
}
