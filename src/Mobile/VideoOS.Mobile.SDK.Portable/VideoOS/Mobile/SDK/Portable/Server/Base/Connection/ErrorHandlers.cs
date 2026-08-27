using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ErrorHandlers : IErrorHandlers
{
	private readonly Dictionary<ErrorCodes, List<EventHandler>> _eventHandlers = new Dictionary<ErrorCodes, List<EventHandler>>();

	public void SubscribeToError(ErrorCodes errorCode, EventHandler errorHandler)
	{
		if (!_eventHandlers.ContainsKey(errorCode))
		{
			_eventHandlers.Add(errorCode, new List<EventHandler>());
		}
		_eventHandlers[errorCode].Add(errorHandler);
	}

	public void UnsubscribeFromError(ErrorCodes errorCode, EventHandler errorHandler)
	{
		List<EventHandler> handlers = GetHandlers(errorCode);
		if (handlers != null && handlers.Contains(errorHandler))
		{
			handlers.Remove(errorHandler);
		}
	}

	private List<EventHandler> GetHandlers(ErrorCodes errorCode)
	{
		List<EventHandler> result = null;
		if (_eventHandlers.ContainsKey(errorCode))
		{
			result = _eventHandlers[errorCode];
		}
		return result;
	}

	public void ProcessError(ErrorCodes errorCode, CommunicationEntry request, CommunicationEntry response)
	{
		List<EventHandler> handlers = GetHandlers(errorCode);
		if (handlers == null)
		{
			return;
		}
		ErrorHandlersArgs e = new ErrorHandlersArgs(errorCode, request, response);
		foreach (EventHandler item in handlers)
		{
			try
			{
				item(this, e);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "ErrorHandlers.ProcessError", ex.Message);
			}
		}
	}
}
