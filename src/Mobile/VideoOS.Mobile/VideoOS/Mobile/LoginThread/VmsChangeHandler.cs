using System;
using VideoOS.Mobile.Utilities;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace VideoOS.Mobile.LoginThread;

public class VmsChangeHandler<TCommunication> : IVmsChangeHandler
{
	private readonly ItemManager<object, EventHandler> _eventHandlers = new ItemManager<object, EventHandler>();

	private object _changeReceiver;

	private bool _initialized;

	private bool _changeHappened;

	private TCommunication _communication;

	private readonly Func<TCommunication> _createCommunication;

	private readonly Func<TCommunication, MessageReceiver, string, object> _registerReceiver;

	private readonly Action<TCommunication, object> _unregisterReceiver;

	public IItemManagerAddRemove<object, EventHandler> EventHandlers => _eventHandlers;

	public string MessageId { get; private set; }

	public VmsChangeHandler(string messageId, Func<TCommunication> createCommunication, Func<TCommunication, MessageReceiver, string, object> registerReceiver, Action<TCommunication, object> unregisterReceiver)
	{
		MessageId = messageId;
		_createCommunication = createCommunication;
		_registerReceiver = registerReceiver;
		_unregisterReceiver = unregisterReceiver;
	}

	public void Initialize()
	{
		if (!_initialized)
		{
			_communication = _createCommunication();
			_changeReceiver = _registerReceiver(_communication, MessageReceiver, MessageId);
		}
		_initialized = true;
	}

	public void Denitialize()
	{
		if (_initialized)
		{
			_unregisterReceiver(_communication, _changeReceiver);
		}
		_initialized = false;
	}

	private object MessageReceiver(Message message, FQID destination, FQID sender)
	{
		_changeHappened = true;
		return null;
	}

	public void Process()
	{
		if (_changeHappened)
		{
			_changeHappened = false;
			_eventHandlers.IterateItems(delegate(object obj, EventHandler eventHandler)
			{
				eventHandler(this, null);
			});
		}
	}
}
