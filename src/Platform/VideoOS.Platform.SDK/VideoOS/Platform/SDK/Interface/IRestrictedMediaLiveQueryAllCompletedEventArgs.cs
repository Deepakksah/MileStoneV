namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveQueryAllCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaLive[] Results { get; }
}
