namespace VideoOS.Platform.SDK.Interface;

public interface ISpeakerSecurityInfo : IDeviceSecurityInfo
{
	bool Speak { get; set; }
}
