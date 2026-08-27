using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.VideoChannel.Encoding;
using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.Utilities;

public class AudioParamsHelper
{
	private class AudioEncoderSettingsWrapper : IAudioEncoderSettingsExt, IAudioEncoderSettings, IAudioSampleSettings
	{
		public AudioEncoderMf.AudioEncoderTypes EncoderType { get; set; }

		public int SampleRate => (int)PcmSourceSettings.SamplingRate;

		public int BitsPerSample => (int)PcmSourceSettings.BitsPerSample;

		public int ChannelsCount => (int)PcmSourceSettings.Channels;

		public int AvrBytesPerSec { get; set; }

		public PcmSourceSettings PcmSourceSettings { get; }

		public AudioEncoderSettingsWrapper(PcmSourceSettings pcmSourceSettings)
		{
			PcmSourceSettings = pcmSourceSettings;
		}
	}

	private static readonly Dictionary<Tuple<int, List<int>>, int> _samplingRatesIndexReplacements = new Dictionary<Tuple<int, List<int>>, int>();

	private static readonly int[] _samplingRates = new int[9] { 8000, 11025, 12000, 16000, 22050, 24000, 32000, 44100, 48000 };

	public const int SamplingRateSwitchStep = 10;

	public const int MaxChannelsCount = 2;

	public const int ChannelsSwitchStep = 5;

	public const int MaxPercentageValue = 100;

	public static readonly int MinPercentageValue = 100 - 10 * _samplingRates.Length;

	public const int BitsPerSampleSwitchStep = 2;

	public static IEnumerable<int> SamplingRates { get; } = _samplingRates;

	public static PcmSourceSettings GetPcmSourceSettings(int percentage)
	{
		float innerStep;
		return GetPcmSourceSettings(percentage, out innerStep);
	}

	private static PcmSourceSettings GetPcmSourceSettings(int percentage, out float innerStep)
	{
		PcmSourceSettings pcmSourceSettings = new PcmSourceSettings(null);
		percentage = Math.Min(percentage, 99);
		percentage = Math.Max(percentage, MinPercentageValue);
		int num = percentage - MinPercentageValue;
		int percentageDiffStepIndex = num / 10;
		int num2 = num % 10;
		int channelsValue = num2 / 5;
		int num3 = num2 % 5;
		innerStep = ((float)num3 + 1f) / 5f;
		Tuple<int, List<int>> tuple = _samplingRatesIndexReplacements.Keys.SingleOrDefault((Tuple<int, List<int>> t) => t.Item1 == percentageDiffStepIndex && t.Item2.Contains(channelsValue + 1));
		pcmSourceSettings.SamplingRate = (uint)((tuple != null) ? _samplingRates[_samplingRatesIndexReplacements[tuple]] : _samplingRates[percentageDiffStepIndex]);
		pcmSourceSettings.Channels = (PcmSourceSettings.ChannelsType)(channelsValue + 1);
		pcmSourceSettings.BitsPerSample = ((num3 < 2) ? PcmSourceSettings.BitsPerSampleType.OneByteInt : PcmSourceSettings.BitsPerSampleType.TwoBytesInt);
		return pcmSourceSettings;
	}

	public static IAudioEncoderSettingsExt GetMp3AudioEncoderSettings(int percentage, bool pcm = false)
	{
		float innerStep;
		PcmSourceSettings pcmSourceSettings = GetPcmSourceSettings(percentage, out innerStep);
		if (!pcm)
		{
			pcmSourceSettings.BitsPerSample = PcmSourceSettings.BitsPerSampleType.TwoBytesInt;
		}
		return new AudioEncoderSettingsWrapper(pcmSourceSettings)
		{
			EncoderType = AudioEncoderMf.AudioEncoderTypes.Mp3,
			AvrBytesPerSec = (int)((float)pcmSourceSettings.SamplingRate / 2f * innerStep * (float)pcmSourceSettings.Channels)
		};
	}
}
