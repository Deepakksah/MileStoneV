using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetCustomSettingDataGlobalCompletedEventArgs : IGetCustomSettingDataGlobalCompletedEventArgs
{
	ICustomSetting IGetCustomSettingDataGlobalCompletedEventArgs.Result { get; }
}
