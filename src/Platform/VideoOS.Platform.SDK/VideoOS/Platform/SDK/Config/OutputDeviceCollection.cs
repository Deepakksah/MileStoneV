using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class OutputDeviceCollection
{
	private List<OutputDevice> _list = new List<OutputDevice>();

	private Dictionary<Guid, OutputDevice> _outputs = new Dictionary<Guid, OutputDevice>();

	public OutputDevice this[Guid id] => _outputs[id];

	public OutputDeviceCollection()
	{
	}

	public OutputDeviceCollection(ISystemInfo systemInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		if (systemInfo.ConfigurationInfo != null)
		{
			IOutputInfo[] outputs = recorderInfo.Outputs;
			foreach (IOutputInfo outputInfo in outputs)
			{
				OutputDevice outputDevice = new OutputDevice(recorder, outputInfo);
				AddOutput(outputDevice);
			}
		}
	}

	public void AddOutput(OutputDevice outputDevice)
	{
		_list.Add(outputDevice);
		_outputs[outputDevice.Guid] = outputDevice;
	}

	public void AddRange(OutputDeviceCollection collection)
	{
		if (collection == null)
		{
			return;
		}
		foreach (OutputDevice item in collection)
		{
			_list.Add(item);
			_outputs[item.Guid] = item;
		}
	}

	public void Remove(OutputDevice item)
	{
		_list.Remove(item);
		_outputs.Remove(item.Guid);
	}

	public bool ContainsKey(Guid id)
	{
		return _outputs.ContainsKey(id);
	}

	public IEnumerator<OutputDevice> GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
