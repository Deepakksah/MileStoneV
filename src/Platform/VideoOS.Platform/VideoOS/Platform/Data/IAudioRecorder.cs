using System;

namespace VideoOS.Platform.Data;

public class IAudioRecorder
{
	public delegate void AudioDataReadyEventHandler(object sender, AudioDataEventArgs e);

	public class AudioDataEventArgs : EventArgs
	{
		protected byte[] _audioData;

		public virtual byte[] AudioData
		{
			get
			{
				return _audioData;
			}
			set
			{
				_audioData = value;
			}
		}

		public AudioDataEventArgs(byte[] audioData)
		{
			_audioData = audioData;
		}
	}

	public virtual int OutgoingWaveFormatChannels { get; set; }

	public virtual int OutgoingWaveFormatBitsPerSample { get; set; }

	public virtual int OutgoingAudioWaveFormatSamplesPerSecond { get; set; }

	public event AudioDataReadyEventHandler AudioDataReadyEvent;

	public virtual void FireAudioDataReadyEvent(byte[] audioData)
	{
		if (this.AudioDataReadyEvent != null)
		{
			this.AudioDataReadyEvent(this, new AudioDataEventArgs(audioData));
		}
	}

	public virtual bool Initialize()
	{
		return true;
	}

	public virtual void Free()
	{
	}

	public virtual void StartRecording()
	{
	}

	public virtual void StopRecording()
	{
	}

	public virtual bool IsRecording()
	{
		return false;
	}

	public virtual int CalcMeterValue(byte[] data, int dataLength)
	{
		return 0;
	}
}
