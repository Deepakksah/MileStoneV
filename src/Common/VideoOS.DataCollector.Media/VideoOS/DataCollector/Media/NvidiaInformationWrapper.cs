using System;
using System.Collections.Generic;
using VideoOS.Platform.MediaProcessing;

namespace VideoOS.DataCollector.Media;

public class NvidiaInformationWrapper : INvidiaInformationWrapper, IDisposable
{
	private bool disposed;

	private NvidiaInformation _nInfo;

	public IEnumerable<string> NvidiaIds => _nInfo.GetDeviceIds();

	public NvidiaInformationWrapper()
	{
		_nInfo = new NvidiaInformation();
	}

	public uint GetDecoderLoad(string arg)
	{
		return _nInfo.GetDeviceDecoderLoad(arg);
	}

	public uint GetDeviceLoad(string arg)
	{
		return _nInfo.GetDeviceGPULoad(arg);
	}

	public uint GetMemoryUsage(string arg)
	{
		return _nInfo.GetDeviceMemoryUsage(arg);
	}

	public void Dispose()
	{
		Dispose(managed: true);
		GC.SuppressFinalize(this);
		disposed = true;
	}

	private void Dispose(bool managed)
	{
		if (managed && _nInfo != null && !disposed)
		{
			_nInfo.Dispose();
		}
	}

	public bool HasHACapability()
	{
		return _nInfo.HasHACapability();
	}
}
