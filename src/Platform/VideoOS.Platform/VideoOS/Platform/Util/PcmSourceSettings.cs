using System;

namespace VideoOS.Platform.Util;

public class PcmSourceSettings : IPcmSourceSettings
{
	public enum ChannelsType
	{
		Mono = 1,
		Stereo
	}

	public enum BitsPerSampleType
	{
		OneByteInt = 8,
		TwoBytesInt = 0x10
	}

	private const uint DefaultSamplingRate = 8000u;

	private ChannelsType _channels = ChannelsType.Mono;

	private BitsPerSampleType _bitsPerSample = BitsPerSampleType.TwoBytesInt;

	private uint _samplingRate = 8000u;

	private readonly Action<PcmSourceSettings> _onChange;

	public ChannelsType Channels
	{
		get
		{
			return _channels;
		}
		set
		{
			_channels = value;
			OnChangeInternal();
		}
	}

	public BitsPerSampleType BitsPerSample
	{
		get
		{
			return _bitsPerSample;
		}
		set
		{
			_bitsPerSample = value;
			OnChangeInternal();
		}
	}

	public uint SamplingRate
	{
		get
		{
			return _samplingRate;
		}
		set
		{
			_samplingRate = value;
			OnChangeInternal();
		}
	}

	public PcmSourceSettings(Action<PcmSourceSettings> onChange)
	{
		_onChange = onChange;
	}

	private void OnChangeInternal()
	{
		if (_onChange != null)
		{
			_onChange(this);
		}
	}

	public void InitFromAnother(PcmSourceSettings other)
	{
		if (other != null)
		{
			if (BitsPerSample != other.BitsPerSample)
			{
				BitsPerSample = other.BitsPerSample;
			}
			if (Channels != other.Channels)
			{
				Channels = other.Channels;
			}
			if (SamplingRate != other.SamplingRate)
			{
				SamplingRate = other.SamplingRate;
			}
		}
	}
}
