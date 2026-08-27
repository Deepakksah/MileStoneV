using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class PresetCollection : IList<Preset>, ICollection<Preset>, IEnumerable<Preset>, IEnumerable
{
	private List<Preset> _list = new List<Preset>();

	private Dictionary<string, Preset> _names = new Dictionary<string, Preset>();

	private ISystemInfo _systemInfo;

	private ICamera _camera;

	public Preset this[int index]
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

	public Preset this[string index] => _names[index];

	public int Count => _list.Count;

	bool ICollection<Preset>.IsReadOnly => true;

	public PresetCollection(ICamera camera, ISystemInfo systemInfo)
	{
		_systemInfo = systemInfo;
		_camera = camera;
	}

	public PresetCollection(ICamera camera, ISystemInfo systemInfo, ICameraInfo cameraInfo)
		: this(camera, systemInfo)
	{
		if (cameraInfo.PtzSettings.Presets != null)
		{
			IPresetInfo[] presets = cameraInfo.PtzSettings.Presets;
			foreach (IPresetInfo presetInfo in presets)
			{
				Preset preset = new Preset(camera, presetInfo);
				_list.Add(preset);
				_names[preset.Name] = preset;
			}
		}
	}

	public int IndexOf(Preset item)
	{
		return _list.IndexOf(item);
	}

	void IList<Preset>.Insert(int index, Preset item)
	{
		throw new NotSupportedException();
	}

	void IList<Preset>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	void ICollection<Preset>.Add(Preset item)
	{
		throw new NotSupportedException();
	}

	void ICollection<Preset>.Clear()
	{
		throw new NotSupportedException();
	}

	public bool Contains(Preset item)
	{
		return _list.Contains(item);
	}

	public bool Contains(string item)
	{
		return _names.ContainsKey(item);
	}

	void ICollection<Preset>.CopyTo(Preset[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	bool ICollection<Preset>.Remove(Preset item)
	{
		throw new NotSupportedException();
	}

	public IEnumerator<Preset> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
