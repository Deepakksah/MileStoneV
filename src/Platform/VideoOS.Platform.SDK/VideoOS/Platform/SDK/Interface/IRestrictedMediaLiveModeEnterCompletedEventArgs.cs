namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveModeEnterCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaLiveResult Result { get; }
}
