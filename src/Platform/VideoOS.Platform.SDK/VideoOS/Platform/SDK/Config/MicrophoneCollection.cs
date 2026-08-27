using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MicrophoneCollection
{
	private List<IMicrophone> _list = new List<IMicrophone>();

	private Dictionary<Guid, IMicrophone> _microphones = new Dictionary<Guid, IMicrophone>();

	private ISystemInfo _systemInfo;

	public IMicrophone this[Guid id]
	{
		get
		{
			if (_microphones.TryGetValue(id, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public MicrophoneCollection()
	{
	}

	public MicrophoneCollection(ISystemInfo systemInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		_systemInfo = systemInfo;
		IMicrophoneInfo[] microphones = recorderInfo.Microphones;
		foreach (IMicrophoneInfo microphoneInfo in microphones)
		{
			Microphone microphone = new Microphone(_systemInfo, recorder, microphoneInfo, recorderInfo);
			_list.Add(microphone);
			_microphones[microphone.Guid] = microphone;
		}
	}

	public void Add(IMicrophone item)
	{
		_list.Add(item);
		_microphones[item.Guid] = item;
	}

	public void Remove(IMicrophone item)
	{
		_list.Remove(item);
		_microphones.Remove(item.Guid);
	}

	public void AddRange(MicrophoneCollection collection)
	{
		if (collection == null)
		{
			return;
		}
		foreach (IMicrophone item in collection)
		{
			_list.Add(item);
			_microphones[item.Guid] = item;
		}
	}

	public bool ContainsKey(Guid id)
	{
		return _microphones.ContainsKey(id);
	}

	public IEnumerator<IMicrophone> GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
