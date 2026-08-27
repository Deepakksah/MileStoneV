namespace VideoOS.Management.Server;

public class NotificationManager
{
	private static NotificationManager _notificationManager = new NotificationManager();

	public static NotificationManager Instance => _notificationManager;

	public static void Notify(EventObject eventObject)
	{
		GeminiTrace.Verbose("Event has been recieved for notification: '{0}'.", eventObject.EventTypeName);
		EnQueue(eventObject);
		try
		{
			ServiceChannelNotification notification = new ServiceChannelNotification(eventObject);
			GeminiHttpApplication.ThrowIfShutdownReceived();
			ServiceChannelNotification.Save();
			ServiceChannelSubscriber.NotifyAsyncronously(notification);
		}
		finally
		{
			DeQueue(eventObject);
		}
	}

	private static void EnQueue(EventObject eventObject)
	{
	}

	private static void DeQueue(EventObject eventObject)
	{
	}

	public static void Shutdown()
	{
	}
}
