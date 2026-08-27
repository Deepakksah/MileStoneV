namespace VideoOS.Platform.SDK.Interface;

public interface IMicrophoneInfo : IMediaDeviceInfo, IDeviceInfo
{
	IMicrophoneSecurityInfo MicrophoneSecurity { get; set; }
}
