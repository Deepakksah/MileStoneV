using System;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public class ResamplerWrapper : IAudioResampler, IControl, IDisposable
{
	private AudioResampler _resampler;

	private bool _disposed;

	public Action<byte[]> OnTransformedData
	{
		get
		{
			return _resampler.OnTransformedData;
		}
		set
		{
			_resampler.OnTransformedData = value;
		}
	}

	public ResamplerWrapper(IAudioSampleSettings sampleSettings)
	{
		_resampler = new AudioResampler(sampleSettings);
		_resampler.OutputSampleRate = 8000;
	}

	public void OnInputData(byte[] input)
	{
		_resampler.OnInputData(input);
	}

	public void SetOutputSampleRate(int outputRate)
	{
		_resampler.OutputSampleRate = outputRate;
	}

	public void Start()
	{
		_resampler.Start();
	}

	public void Stop()
	{
		_resampler.Stop();
	}

	~ResamplerWrapper()
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
				Disposing.DisposeMember(ref _resampler);
			}
			_disposed = true;
		}
	}
}
