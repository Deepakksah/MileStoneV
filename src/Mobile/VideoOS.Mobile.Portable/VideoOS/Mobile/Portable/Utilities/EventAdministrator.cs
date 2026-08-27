using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public class EventAdministrator<TKey, TEvent, TClient> : IEventAdministratorIn<TEvent>, IEventAdministratorOut<TKey, TEvent, TClient>
{
	private class ClientData
	{
		public ClientEvent<TKey, TEvent> OnEventFunc;

		public TClient Client;
	}

	private Dictionary<TKey, ClientData> _dictClients;

	public InputEvent<TEvent> OnEvent { get; private set; }

	public TClient this[TKey key]
	{
		get
		{
			lock (_dictClients)
			{
				return _dictClients[key].Client;
			}
		}
		set
		{
			lock (_dictClients)
			{
				_dictClients[key].Client = value;
			}
		}
	}

	public List<TKey> Keys
	{
		get
		{
			lock (_dictClients)
			{
				return _dictClients.Keys.ToList();
			}
		}
	}

	public EventAdministrator()
	{
		_dictClients = new Dictionary<TKey, ClientData>();
		OnEvent = OnNewEvent;
	}

	public void AddClient(ref TKey tKey, ClientEvent<TKey, TEvent> OnEvent, TClient tClient)
	{
		ClientData clientData = new ClientData();
		clientData.OnEventFunc = OnEvent;
		clientData.Client = tClient;
		lock (_dictClients)
		{
			_dictClients.Add(tKey, clientData);
		}
	}

	public TClient RemoveClient(TKey tKey)
	{
		TClient client = _dictClients[tKey].Client;
		lock (_dictClients)
		{
			_dictClients.Remove(tKey);
			return client;
		}
	}

	public bool Exist(TKey tKey)
	{
		lock (_dictClients)
		{
			return _dictClients.ContainsKey(tKey);
		}
	}

	public void OnNewEvent(TEvent tEvent)
	{
		try
		{
			Dictionary<TKey, ClientData> dictionary;
			lock (_dictClients)
			{
				dictionary = new Dictionary<TKey, ClientData>(_dictClients);
			}
			foreach (KeyValuePair<TKey, ClientData> item in dictionary)
			{
				try
				{
					item.Value.OnEventFunc(item.Key, tEvent);
				}
				catch (Exception ex)
				{
					Logger.Instance.Log(LogContent.Error, "EventAdministrator.OnNewEvent " + item.Key, ex.Message);
				}
			}
		}
		catch (Exception ex2)
		{
			Logger.Instance.Log(LogContent.Error, "EventAdministrator.OnNewEvent", ex2.Message);
		}
	}
}
