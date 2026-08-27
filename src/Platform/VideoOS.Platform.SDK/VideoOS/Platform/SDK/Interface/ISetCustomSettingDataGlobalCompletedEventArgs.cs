namespace VideoOS.Platform.SDK.Interface;

public interface ISetCustomSettingDataGlobalCompletedEventArgs : IAsyncCompletedEventArgs
{
	SetCustomSettingResultType Result { get; }
}
