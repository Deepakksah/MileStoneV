using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetSystemLicenseInfoCompletedEventArgs : IGetSystemLicenseInfoCompletedEventArgs
{
	ISystemLicenseInfo IGetSystemLicenseInfoCompletedEventArgs.Result { get; }
}
