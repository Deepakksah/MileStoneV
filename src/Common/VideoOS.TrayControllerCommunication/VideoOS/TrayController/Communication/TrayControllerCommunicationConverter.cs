using VideoOS.Vms.Types;

namespace VideoOS.TrayController.Communication;

public static class TrayControllerCommunicationConverter
{
	public static ServiceState ToServiceState(ServiceState_Communication status)
	{
		return status switch
		{
			ServiceState_Communication.Indeterminate => ServiceState.Indeterminate, 
			ServiceState_Communication.ContinuePending => ServiceState.ContinuePending, 
			ServiceState_Communication.Paused => ServiceState.Paused, 
			ServiceState_Communication.PausePending => ServiceState.PausePending, 
			ServiceState_Communication.Running => ServiceState.Running, 
			ServiceState_Communication.StartPending => ServiceState.StartPending, 
			ServiceState_Communication.Stopped => ServiceState.Stopped, 
			ServiceState_Communication.StopPending => ServiceState.StopPending, 
			_ => ServiceState.Indeterminate, 
		};
	}

	public static ServiceState_Communication ToServiceState_Communication(ServiceState status)
	{
		return status switch
		{
			ServiceState.Indeterminate => ServiceState_Communication.Indeterminate, 
			ServiceState.ContinuePending => ServiceState_Communication.ContinuePending, 
			ServiceState.Paused => ServiceState_Communication.Paused, 
			ServiceState.PausePending => ServiceState_Communication.PausePending, 
			ServiceState.Running => ServiceState_Communication.Running, 
			ServiceState.StartPending => ServiceState_Communication.StartPending, 
			ServiceState.Stopped => ServiceState_Communication.Stopped, 
			ServiceState.StopPending => ServiceState_Communication.StopPending, 
			_ => ServiceState_Communication.Indeterminate, 
		};
	}

	public static ConnectStatus ToConnectStatus(ConnectStatus_Communication status)
	{
		return status switch
		{
			ConnectStatus_Communication.Disconnected => ConnectStatus.Disconnected, 
			ConnectStatus_Communication.Reconnecting => ConnectStatus.Reconnecting, 
			ConnectStatus_Communication.Connecting => ConnectStatus.Connecting, 
			ConnectStatus_Communication.Connected => ConnectStatus.Connected, 
			ConnectStatus_Communication.ConnectedAttachPending => ConnectStatus.ConnectedAttachPending, 
			ConnectStatus_Communication.SslCommunicationError => ConnectStatus.SslCommunicationError, 
			_ => ConnectStatus.Reconnecting, 
		};
	}

	public static ConnectStatus_Communication ToConnectStatus_Communication(ConnectStatus status)
	{
		return status switch
		{
			ConnectStatus.Disconnected => ConnectStatus_Communication.Disconnected, 
			ConnectStatus.Reconnecting => ConnectStatus_Communication.Reconnecting, 
			ConnectStatus.Connecting => ConnectStatus_Communication.Connecting, 
			ConnectStatus.Connected => ConnectStatus_Communication.Connected, 
			ConnectStatus.ConnectedAttachPending => ConnectStatus_Communication.ConnectedAttachPending, 
			ConnectStatus.SslCommunicationError => ConnectStatus_Communication.SslCommunicationError, 
			_ => ConnectStatus_Communication.Reconnecting, 
		};
	}
}
