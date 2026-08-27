using System;
using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class AudioSamplesRepeater : IAudioResampler, IControl, IDisposable
{
	private bool _disposed;

	public Action<byte[]> OnTransformedData { get; set; }

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public void OnInputData(byte[] input)
	{
		OnTransformedData?.Invoke(input);
	}

	public void SetOutputSampleRate(int outputRate)
	{
	}

	~AudioSamplesRepeater()
	{
		Dispose(disposing: true);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				OnTransformedData = null;
			}
			_disposed = true;
		}
	}
}
