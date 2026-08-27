using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Management.Server;

public static class MessagePropertiesManager
{
	private static Func<MessageProperties> _current;

	public static MessageProperties IncomingMessageProperties
	{
		get
		{
			if (_current != null)
			{
				return _current();
			}
			if (OperationContext.Current == null)
			{
				throw new InvalidOperationException("OperationContext is not available");
			}
			return OperationContext.Current.IncomingMessageProperties;
		}
	}

	public static void SwitchCurrent(Func<MessageProperties> messageProperties)
	{
		_current = messageProperties;
	}
}
