using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetConfigurationCompletedEventArgs : IGetConfigurationCompletedEventArgs
{
	IConfigurationInfo IGetConfigurationCompletedEventArgs.Result { get; }
}
