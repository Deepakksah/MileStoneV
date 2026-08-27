using VideoOS.Vms.Types;

namespace VideoOS.TrayController.Communication.Failover;

public class FailoverState
{
	public ServiceState serviceState { get; set; }

	public ConnectStatus connectStatus { get; set; }

	public FailoverRecorderState failoverRecorderState { get; set; }
}
