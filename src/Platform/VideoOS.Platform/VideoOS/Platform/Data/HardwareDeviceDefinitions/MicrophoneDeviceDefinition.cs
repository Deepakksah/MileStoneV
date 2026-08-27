using System.Collections.Generic;

namespace VideoOS.Platform.Data.HardwareDeviceDefinitions;

public class MicrophoneDeviceDefinition : DeviceDefinition
{
	private static readonly IDictionary<MediaContent, string> AcceptedCodecs = new Dictionary<MediaContent, string>
	{
		{
			MediaContent.Audio,
			"Audio"
		},
		{
			MediaContent.Pcm,
			"Pcm"
		}
	};

	public MicrophoneDeviceDefinition()
		: base(AcceptedCodecs)
	{
	}
}
