namespace VideoOS.Platform.SDK.Interface;

public interface IOutputInfo : IDeviceInfo
{
	IOutputSecurityInfo OutputSecurity { get; set; }
}
