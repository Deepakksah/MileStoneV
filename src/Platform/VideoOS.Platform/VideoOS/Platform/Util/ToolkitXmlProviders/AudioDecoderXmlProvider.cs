using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public class AudioDecoderXmlProvider : MediaProcessingToolkitXmlProvider
{
	private const string ChannelsName = "channels";

	private const string ChannelsKey = "channels_key";

	private const string ChannelsUpdateKey = "channels_update_key";

	private const string SamplingRateName = "sampling_rate";

	private const string SamplingRateKey = "sampling_rate_key";

	private const string SamplingRateUpdateKey = "sampling_rate_update_key";

	private const string BitsPerSampleName = "bits_per_sample";

	private const string BitsPerSampleKey = "bits_per_sample_key";

	private const string BitsPerSampleUpdateKey = "bits_per_sample_update_key";

	public uint Channels { get; set; }

	public uint SamplingRate { get; set; }

	public uint BitsPerSample { get; set; }

	public AudioDecoderXmlProvider()
	{
		Channels = 0u;
		SamplingRate = 0u;
		BitsPerSample = 0u;
	}

	public AudioDecoderXmlProvider(InternalMediaService.PcmAudioConnectParams connectParams)
	{
		Channels = connectParams.Channels;
		SamplingRate = connectParams.SampleRate;
		BitsPerSample = connectParams.BitsPerSample;
	}

	protected override void WriteTransformationStart(XmlWriter xw)
	{
		xw.WriteStartElement("audio_decoder");
		xw.WriteAttributeString("channels", Channels.ToString(CultureInfo.InvariantCulture));
		xw.WriteAttributeString("channels_update_key", "channels_key");
		xw.WriteAttributeString("sampling_rate", SamplingRate.ToString(CultureInfo.InvariantCulture));
		xw.WriteAttributeString("sampling_rate_update_key", "sampling_rate_key");
		xw.WriteAttributeString("bits_per_sample", BitsPerSample.ToString(CultureInfo.InvariantCulture));
		xw.WriteAttributeString("bits_per_sample_update_key", "bits_per_sample_key");
	}

	protected override void WriteTransformationEnd(XmlWriter xw)
	{
		xw.WriteEndElement();
	}

	public override void UpdateConfiguration(Action<string, string> configurationUpdater)
	{
		if (Channels != 0)
		{
			configurationUpdater("channels_key", Channels.ToString(CultureInfo.InvariantCulture));
		}
		if (SamplingRate != 0)
		{
			configurationUpdater("sampling_rate_key", SamplingRate.ToString(CultureInfo.InvariantCulture));
		}
		if (BitsPerSample == 8 || BitsPerSample == 16)
		{
			configurationUpdater("bits_per_sample_key", BitsPerSample.ToString(CultureInfo.InvariantCulture));
		}
	}
}
