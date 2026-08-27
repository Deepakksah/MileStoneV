using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetCustomSettingDataUserCompletedEventArgs : IGetCustomSettingDataUserCompletedEventArgs
{
	ICustomSetting IGetCustomSettingDataUserCompletedEventArgs.Result { get; }
}
