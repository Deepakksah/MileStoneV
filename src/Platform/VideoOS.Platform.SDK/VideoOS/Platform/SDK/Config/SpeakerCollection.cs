using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class SpeakerCollection
{
	private List<Speaker> _list = new List<Speaker>();

	private Dictionary<Guid, Speaker> _speakers = new Dictionary<Guid, Speaker>();

	private ISystemInfo _systemInfo;

	public Speaker this[Guid id]
	{
		get
		{
			if (_speakers.TryGetValue(id, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public SpeakerCollection()
	{
	}

	public SpeakerCollection(ISystemInfo systemInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		_systemInfo = systemInfo;
		ISpeakerInfo[] speakers = recorderInfo.Speakers;
		foreach (ISpeakerInfo speakerInfo in speakers)
		{
			Speaker speaker = new Speaker(_systemInfo, recorder, speakerInfo);
			if (!_speakers.ContainsKey(speaker.Guid))
			{
				_list.Add(speaker);
				_speakers[speaker.Guid] = speaker;
			}
		}
	}

	public void Remove(Speaker item)
	{
		_list.Remove(item);
		_speakers.Remove(item.Guid);
	}

	public bool ContainsKey(Guid id)
	{
		return _speakers.ContainsKey(id);
	}

	public void Add(Speaker obj)
	{
		_list.Add(obj);
		_speakers[obj.Guid] = obj;
	}

	public void AddRange(SpeakerCollection collection)
	{
		if (collection == null)
		{
			return;
		}
		foreach (Speaker item in collection)
		{
			_list.Add(item);
			_speakers[item.Guid] = item;
		}
	}

	public IEnumerator<Speaker> GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
