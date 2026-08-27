namespace VideoOS.Platform.SDK.Config;

public interface ISpeaker : IStreamDevice, IDevice
{
	SpeakerSecurity SpeakerAccess { get; }
}
