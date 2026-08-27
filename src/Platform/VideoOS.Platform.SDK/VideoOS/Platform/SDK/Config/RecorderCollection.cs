using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class RecorderCollection : IList<IRecorder>, ICollection<IRecorder>, IEnumerable<IRecorder>, IEnumerable
{
	private List<IRecorder> _list = new List<IRecorder>();

	private Dictionary<Guid, IRecorder> _recorders = new Dictionary<Guid, IRecorder>();

	private ISystemInfo _systemInfo;

	public IRecorder this[int index]
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

	public IRecorder this[Guid guid]
	{
		get
		{
			if (_recorders.TryGetValue(guid, out var value))
			{
				return value;
			}
			EnvironmentManager instance = EnvironmentManager.Instance;
			Guid guid2 = guid;
			instance.Log(error: true, "RecorderCollection", "Unknown recorder Guid: " + guid2.ToString());
			return null;
		}
	}

	public int Count => _list.Count;

	bool ICollection<IRecorder>.IsReadOnly => true;

	public RecorderCollection()
	{
	}

	public RecorderCollection(ISystemInfo systemInfo)
	{
		_systemInfo = systemInfo;
		if (_systemInfo.ConfigurationInfo != null)
		{
			CreateRecorderCollection();
		}
	}

	public void CreateRecorderCollection()
	{
		IRecorderInfo[] recorders = _systemInfo.ConfigurationInfo.Recorders;
		foreach (IRecorderInfo recorderInfo in recorders)
		{
			IRecorder recorder = new Recorder(_systemInfo, recorderInfo);
			if (_recorders.TryGetValue(recorder.Guid, out var value))
			{
				_list.Remove(value);
			}
			_list.Add(recorder);
			_recorders[recorder.Guid] = recorder;
		}
	}

	public int IndexOf(IRecorder item)
	{
		return _list.IndexOf(item);
	}

	void IList<IRecorder>.Insert(int index, IRecorder item)
	{
		throw new NotSupportedException();
	}

	void IList<IRecorder>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	public bool ContainsKey(Guid guid)
	{
		return _recorders.ContainsKey(guid);
	}

	public void Add(IRecorder recorder)
	{
		_list.Add(recorder);
		_recorders[recorder.Guid] = recorder;
	}

	void ICollection<IRecorder>.Clear()
	{
		throw new NotSupportedException();
	}

	public bool Contains(IRecorder item)
	{
		return _list.Contains(item);
	}

	void ICollection<IRecorder>.CopyTo(IRecorder[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	bool ICollection<IRecorder>.Remove(IRecorder item)
	{
		throw new NotSupportedException();
	}

	public IEnumerator<IRecorder> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
