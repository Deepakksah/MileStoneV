using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Platform.Data.HardwareDeviceDefinitions;

public class PcmMicrophoneDeviceDefinition : MicrophoneDeviceDefinition
{
	public const uint DefaultSampleRate = 8000u;

	public const uint DefaultBitsPerSample = 16u;

	public const uint DefaultNumberOfChannels = 1u;

	private readonly ISet<uint> _bitsPerSampleSet = new HashSet<uint>();

	public uint SampleRate { get; set; }

	public Tuple<uint, uint> SampleRateRange { get; set; }

	public uint BitsPerSample { get; set; }

	public ISet<uint> BitsPerSampleSet => _bitsPerSampleSet;

	public uint NumberOfChannels { get; set; }

	public Tuple<uint, uint> NumberOfChannelsRange { get; set; }

	public PcmMicrophoneDeviceDefinition()
	{
		base.Codec = MediaContent.Pcm;
		base.CodecSet.Add(MediaContent.Pcm);
		SampleRate = 8000u;
		SampleRateRange = new Tuple<uint, uint>(8000u, 8000u);
		BitsPerSample = 16u;
		BitsPerSampleSet.Add(16u);
		NumberOfChannels = 1u;
		NumberOfChannelsRange = new Tuple<uint, uint>(1u, 1u);
	}

	public override void GenerateDeviceParameters(string prefix, IDictionary<string, string> parameters)
	{
		base.GenerateDeviceParameters(prefix, parameters);
		parameters.Add(prefix + "SampleRate", SampleRate.ToString(CultureInfo.InvariantCulture));
		parameters.Add(prefix + "SampleRateRange", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", SampleRateRange.Item1, SampleRateRange.Item2));
		parameters.Add(prefix + "BitsPerSample", BitsPerSample.ToString(CultureInfo.InvariantCulture));
		parameters.Add(prefix + "BitsPerSampleList", BitsPerSampleSet.Select((uint bps) => bps.ToString(CultureInfo.InvariantCulture)).Aggregate((string s1, string s2) => s1 + "," + s2));
		parameters.Add(prefix + "NumberOfChannels", NumberOfChannels.ToString(CultureInfo.InvariantCulture));
		parameters.Add(prefix + "NumberOfChannelsRange", string.Format(CultureInfo.InvariantCulture, "{0}-{1}", NumberOfChannelsRange.Item1, NumberOfChannelsRange.Item2));
	}
}
