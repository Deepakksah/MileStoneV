using System;
using System.Collections.Generic;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Util;

public static class Migration
{
	public static bool NewEventMigrate = true;

	private static object _msgObj = null;

	private static object _newEventRef = null;

	public static void Initialize()
	{
		if (EnvironmentManager.Instance != null)
		{
			if (NewEventMigrate && _msgObj == null)
			{
				_msgObj = EnvironmentManager.Instance.RegisterReceiver(BaseEventHandler, new MessageIdFilter("Server.EventCommand"));
			}
			InitCheckForMigration();
		}
	}

	private static object BaseEventHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			if (message.Data is EventData)
			{
				Message message2 = new Message("Server.NewEventCommand", message.RelatedFQID, message.Data, message.Reason);
				EnvironmentManager.Instance.SendMessage(message2, dest, source);
			}
			else
			{
				EnvironmentManager instance = EnvironmentManager.Instance;
				object[] args = new Message[1] { message };
				instance.Log(error: false, "EventCommand handling", "Unknown class in the Data field", args);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("BaseEventHandler", exception);
		}
		return null;
	}

	public static void InitCheckForMigration()
	{
		if (_newEventRef == null)
		{
			_newEventRef = EnvironmentManager.Instance.RegisterReceiver(NewEventsHandler, new MessageIdFilter("Server.NewEventsIndication"), EnvironmentManager.MessageReceiverHint.AvoidAsync);
		}
	}

	private static object NewEventsHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			if (message.Data is IEnumerable<object> enumerable)
			{
				foreach (object item in enumerable)
				{
					EnvironmentManager.Instance.PostMessage(new Message("Server.NewEventIndication")
					{
						Data = item
					});
				}
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("NewEventsHandler: Event not forwarded to NewEventIndication receivers", exception);
		}
		return null;
	}
}
