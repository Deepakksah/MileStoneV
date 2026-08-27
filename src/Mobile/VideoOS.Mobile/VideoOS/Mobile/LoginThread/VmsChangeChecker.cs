using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.LoginThread;

public class VmsChangeChecker
{
	public enum NotificationType
	{
		Receiver,
		Communicator
	}

	private readonly List<Tuple<NotificationType, IVmsChangeHandler>> _handlers = new List<Tuple<NotificationType, IVmsChangeHandler>>();

	private readonly object _handlerLocker = new object();

	private bool _initialized;

	public IItemManagerAddRemove<object, EventHandler> GetHandler(NotificationType notificationType, string messageId)
	{
		lock (_handlerLocker)
		{
			Tuple<NotificationType, IVmsChangeHandler> tuple = _handlers.Find((Tuple<NotificationType, IVmsChangeHandler> tupple) => tupple.Item1 == notificationType && tupple.Item2.MessageId == messageId);
			if (tuple == null)
			{
				tuple = new Tuple<NotificationType, IVmsChangeHandler>(notificationType, CreateHandler(notificationType, messageId));
				_handlers.Add(tuple);
				if (!_initialized)
				{
					tuple.Item2.Initialize();
				}
			}
			return tuple.Item2.EventHandlers;
		}
	}

	private IVmsChangeHandler CreateHandler(NotificationType notificationType, string messageId)
	{
		return notificationType switch
		{
			NotificationType.Receiver => new VmsChangeHandlerReceiver(messageId), 
			NotificationType.Communicator => new VmsChangeHandlerCommunication(messageId), 
			_ => throw new NotImplementedException(), 
		};
	}

	public void Initialize()
	{
		Log.TraceBegin("Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\VmsConfigChangeChecker.cs", 173);
		_initialized = true;
		Iterate(delegate(IVmsChangeHandler iVmsChangeHandler)
		{
			iVmsChangeHandler.Initialize();
		});
		Log.TraceEnd("Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\VmsConfigChangeChecker.cs", 179);
	}

	public void Denitialize()
	{
		Log.TraceBegin("Denitialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\VmsConfigChangeChecker.cs", 184);
		_initialized = false;
		Iterate(delegate(IVmsChangeHandler iVmsChangeHandler)
		{
			iVmsChangeHandler.Denitialize();
		});
		Log.TraceEnd("Denitialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\LoginThread\\VmsConfigChangeChecker.cs", 190);
	}

	public void Process()
	{
		Iterate(delegate(IVmsChangeHandler iVmsChangeHandler)
		{
			iVmsChangeHandler.Process();
		});
	}

	private void Iterate(Action<IVmsChangeHandler> action)
	{
		lock (_handlerLocker)
		{
			_handlers.ForEachCatched(delegate(Tuple<NotificationType, IVmsChangeHandler> tuple)
			{
				action(tuple.Item2);
			});
		}
	}
}
