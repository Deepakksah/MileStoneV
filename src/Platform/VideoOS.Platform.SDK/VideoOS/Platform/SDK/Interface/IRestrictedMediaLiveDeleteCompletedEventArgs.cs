namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveDeleteCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaLiveResult Result { get; }
}
