using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class UserDefinedEventCollection : IList<UserDefinedEvent>, ICollection<UserDefinedEvent>, IEnumerable<UserDefinedEvent>, IEnumerable
{
	private List<UserDefinedEvent> _list = new List<UserDefinedEvent>();

	private Dictionary<Guid, UserDefinedEvent> _events = new Dictionary<Guid, UserDefinedEvent>();

	private ISystemInfo _systemInfo;

	public Dictionary<Guid, InputEvent> InputEvents = new Dictionary<Guid, InputEvent>();

	public UserDefinedEvent this[int index]
	{
		get
		{
			return _list[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public UserDefinedEvent this[Guid id]
	{
		get
		{
			if (_events.TryGetValue(id, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public int Count => _list.Count;

	bool ICollection<UserDefinedEvent>.IsReadOnly => true;

	public UserDefinedEventCollection()
	{
	}

	public UserDefinedEventCollection(ISystemInfo systemInfo)
	{
		_systemInfo = systemInfo;
		if (_systemInfo.ConfigurationInfo != null)
		{
			CreateUserDefinedEventCollection();
		}
	}

	public UserDefinedEventCollection(ISystemInfo systemInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		if (systemInfo == null)
		{
			throw new ArgumentNullException("systemInfo");
		}
		if (recorder == null)
		{
			throw new ArgumentNullException("recorder");
		}
		if (recorderInfo == null)
		{
			throw new ArgumentNullException("recorderInfo");
		}
		_systemInfo = systemInfo;
		if (_systemInfo.ConfigurationInfo != null)
		{
			IEventTypeInfo[] eventTypes = _systemInfo.ConfigurationInfo.EventTypes;
			foreach (IEventTypeInfo eventTypeInfo in eventTypes)
			{
				UserDefinedEvent userDefinedEvent = new UserDefinedEvent(_systemInfo, recorder, eventTypeInfo);
				if (userDefinedEvent.Recorder != null && userDefinedEvent.Recorder.Guid == recorderInfo.RecorderId && userDefinedEvent.EventType == "Manual")
				{
					AddEvent(userDefinedEvent);
				}
			}
		}
		if (recorderInfo.Inputs != null)
		{
			IInputInfo[] inputs = recorderInfo.Inputs;
			foreach (IInputInfo inputInfo in inputs)
			{
				InputEvents[inputInfo.DeviceId] = new InputEvent(inputInfo.DeviceId, inputInfo.Name, inputInfo.RecorderId);
			}
		}
	}

	private void CreateUserDefinedEventCollection()
	{
		IEventTypeInfo[] eventTypes = _systemInfo.ConfigurationInfo.EventTypes;
		foreach (IEventTypeInfo eventTypeInfo in eventTypes)
		{
			UserDefinedEvent even = new UserDefinedEvent(_systemInfo, null, eventTypeInfo);
			AddEvent(even);
		}
		IRecorderInfo[] recorders = _systemInfo.ConfigurationInfo.Recorders;
		for (int i = 0; i < recorders.Length; i++)
		{
			IInputInfo[] inputs = recorders[i].Inputs;
			foreach (IInputInfo inputInfo in inputs)
			{
				InputEvents[inputInfo.DeviceId] = new InputEvent(inputInfo.DeviceId, inputInfo.Name, inputInfo.RecorderId);
			}
		}
	}

	public InputEvent InputEvent(Guid id)
	{
		if (InputEvents.TryGetValue(id, out var value))
		{
			return value;
		}
		return null;
	}

	public void AddEvent(UserDefinedEvent even)
	{
		if (_events.Remove(even.Guid))
		{
			foreach (UserDefinedEvent item in _list)
			{
				if (item.Guid == even.Guid)
				{
					_list.Remove(item);
					break;
				}
			}
		}
		_list.Add(even);
		_events[even.Guid] = even;
	}

	public void AddRange(UserDefinedEventCollection collection)
	{
		foreach (UserDefinedEvent item in collection)
		{
			_list.Add(item);
			_events[item.Guid] = item;
		}
	}

	public int IndexOf(UserDefinedEvent item)
	{
		return _list.IndexOf(item);
	}

	void IList<UserDefinedEvent>.Insert(int index, UserDefinedEvent item)
	{
		throw new NotSupportedException();
	}

	void IList<UserDefinedEvent>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	public void Remove(UserDefinedEvent item)
	{
		_list.Remove(item);
		_events.Remove(item.Guid);
	}

	void ICollection<UserDefinedEvent>.Add(UserDefinedEvent item)
	{
		throw new NotSupportedException();
	}

	void ICollection<UserDefinedEvent>.Clear()
	{
		throw new NotSupportedException();
	}

	public bool Contains(UserDefinedEvent item)
	{
		return _list.Contains(item);
	}

	public bool ContainsKey(Guid id)
	{
		return _events.ContainsKey(id);
	}

	void ICollection<UserDefinedEvent>.CopyTo(UserDefinedEvent[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	bool ICollection<UserDefinedEvent>.Remove(UserDefinedEvent item)
	{
		throw new NotSupportedException();
	}

	public IEnumerator<UserDefinedEvent> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
