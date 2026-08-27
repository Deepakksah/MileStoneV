namespace VideoOS.Platform.SDK.Interface;

public interface ISpeakerInfo : IMediaDeviceInfo, IDeviceInfo
{
	ISpeakerSecurityInfo SpeakerSecurity { get; set; }
}
