using System;
using System.Windows;

namespace VideoOS.Common.Integration.Communication;

public class ResponseReceivedWeakEventManager : WeakEventManager
{
	private static ResponseReceivedWeakEventManager CurrentManager
	{
		get
		{
			Type typeFromHandle = typeof(ResponseReceivedWeakEventManager);
			ResponseReceivedWeakEventManager responseReceivedWeakEventManager = (ResponseReceivedWeakEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
			if (responseReceivedWeakEventManager == null)
			{
				responseReceivedWeakEventManager = new ResponseReceivedWeakEventManager();
				WeakEventManager.SetCurrentManager(typeFromHandle, responseReceivedWeakEventManager);
			}
			return responseReceivedWeakEventManager;
		}
	}

	private ResponseReceivedWeakEventManager()
	{
	}

	public static void AddHandler(IServerCommandResponse source, EventHandler<ResponseReceivedEventArgs> handler)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		CurrentManager.ProtectedAddHandler(source, handler);
	}

	public static void RemoveHandler(IServerCommandResponse source, EventHandler<ResponseReceivedEventArgs> handler)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		CurrentManager.ProtectedRemoveHandler(source, handler);
	}

	protected override ListenerList NewListenerList()
	{
		return new ListenerList<ResponseReceivedEventArgs>();
	}

	protected override void StartListening(object source)
	{
		((IServerCommandResponse)source).ResponseReceived += OnResponseReceived;
	}

	protected override void StopListening(object source)
	{
		((IServerCommandResponse)source).ResponseReceived -= OnResponseReceived;
	}

	private void OnResponseReceived(object sender, ResponseReceivedEventArgs e)
	{
		DeliverEvent(sender, e);
	}
}
