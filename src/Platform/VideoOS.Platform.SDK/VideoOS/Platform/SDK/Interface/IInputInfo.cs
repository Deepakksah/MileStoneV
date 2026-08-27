namespace VideoOS.Platform.SDK.Interface;

public interface IInputInfo : IDeviceInfo
{
	IInputSecurityInfo InputSecurity { get; set; }
}
