using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Common.Audio;

public sealed class AudioRecorder : IAudioRecorder, IDisposable
{
	private WaveInEvent _waveInEvent;

	private bool _recording;

	private bool _initialized;

	private static readonly TimeSpan DefaultBufferTime = TimeSpan.FromMilliseconds(64.0);

	private const int DefaultOutgoingWaveFormatChannels = 1;

	private const int DefaultOutgoingWaveFormatBitsPerSample = 16;

	private const int DefaultRecordingFrequency = 8000;

	public const double OutgoingAudioMeterAmplifyingFactor = 1.333;

	private readonly TimeSpan _bufferTime;

	private bool _disposed;

	public override int OutgoingAudioWaveFormatSamplesPerSecond
	{
		get
		{
			return base.OutgoingAudioWaveFormatSamplesPerSecond;
		}
		set
		{
			if (new int[2] { 8000, 16000 }.Contains(value))
			{
				base.OutgoingAudioWaveFormatSamplesPerSecond = value;
			}
		}
	}

	public AudioRecorder()
		: this(DefaultBufferTime, 1, 16, 8000)
	{
	}

	public AudioRecorder(TimeSpan bufferTime, int outgoingWaveFormatChannels, int outgoingWaveFormatBitsPerSample, int outgoingAudioWaveFormatSamplesPerSecond)
	{
		_bufferTime = bufferTime;
		OutgoingWaveFormatChannels = outgoingWaveFormatChannels;
		OutgoingWaveFormatBitsPerSample = outgoingWaveFormatBitsPerSample;
		OutgoingAudioWaveFormatSamplesPerSecond = outgoingAudioWaveFormatSamplesPerSecond;
		if (OutgoingAudioWaveFormatSamplesPerSecond == 0)
		{
			OutgoingAudioWaveFormatSamplesPerSecond = 8000;
		}
	}

	~AudioRecorder()
	{
		Dispose(disposing: true);
	}

	private void WaveInEventOnDataAvailable(object sender, WaveInEventArgs e)
	{
		if (_initialized)
		{
			FireAudioDataReadyEvent(e.Buffer);
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override bool Initialize()
	{
		if (_initialized)
		{
			return true;
		}
		if (VerifyRecordingDevicesAreAvailable())
		{
			InitializeWaveInEvent();
			_initialized = true;
		}
		return _initialized;
	}

	private static bool VerifyRecordingDevicesAreAvailable()
	{
		if (WaveIn.DeviceCount < 1)
		{
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager.Instance.Log(error: false, "AudioRecorder", "No recording devices are available.");
			}
			return false;
		}
		return true;
	}

	private void InitializeWaveInEvent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		WaveFormat waveFormat = new WaveFormat(OutgoingAudioWaveFormatSamplesPerSecond, OutgoingWaveFormatBitsPerSample, OutgoingWaveFormatChannels);
		_waveInEvent = new WaveInEvent
		{
			BufferMilliseconds = Convert.ToInt32(_bufferTime.TotalMilliseconds),
			WaveFormat = waveFormat
		};
		_waveInEvent.DataAvailable += WaveInEventOnDataAvailable;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override void StartRecording()
	{
		if (!_recording)
		{
			if (!_initialized)
			{
				throw new MIPException("AudioRecorder is not initialized");
			}
			_waveInEvent.StartRecording();
			_recording = true;
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override void StopRecording()
	{
		if (_recording)
		{
			_waveInEvent.StopRecording();
			_recording = false;
		}
	}

	public override bool IsRecording()
	{
		return _recording;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override void Free()
	{
		if (IsRecording())
		{
			StopRecording();
		}
		WaveInEvent waveInEvent = _waveInEvent;
		if (waveInEvent != null)
		{
			waveInEvent.Dispose();
		}
		_initialized = false;
	}

	public override int CalcMeterValue(byte[] Data, int DataLength)
	{
		double num = 0.0;
		double num2 = 0.0;
		for (int i = 0; i < DataLength / OutgoingWaveFormatBitsPerSample; i += 5)
		{
			double num3 = 0.0;
			for (int j = 0; j < OutgoingWaveFormatBitsPerSample / 8; j++)
			{
				num3 += (double)(Data[i * OutgoingWaveFormatBitsPerSample + j] << j * 8);
			}
			if (num3 > (double)(1 << OutgoingWaveFormatBitsPerSample - 1))
			{
				num3 -= (double)(1 << OutgoingWaveFormatBitsPerSample);
			}
			num += num3 * num3;
			num2 += 1.0;
		}
		int num4 = (int)(Math.Sqrt(num / num2) / (double)(1 << OutgoingWaveFormatBitsPerSample - 1) * 100.0 * 1.333);
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num4 > 100)
		{
			num4 = 100;
		}
		return num4;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_disposed = true;
			WaveInEvent waveInEvent = _waveInEvent;
			if (waveInEvent != null)
			{
				waveInEvent.Dispose();
			}
		}
	}
}
