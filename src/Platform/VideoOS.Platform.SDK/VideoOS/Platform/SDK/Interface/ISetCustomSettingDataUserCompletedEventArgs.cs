namespace VideoOS.Platform.SDK.Interface;

public interface ISetCustomSettingDataUserCompletedEventArgs : IAsyncCompletedEventArgs
{
	SetCustomSettingResultType Result { get; }
}
