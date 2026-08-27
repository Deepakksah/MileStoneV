namespace VideoOS.Management.Server;

public class ServiceChannelSubscriber
{
	public static void NotifyAsyncronously(ServiceChannelNotification notification)
	{
		notification.Send(null);
	}
}
