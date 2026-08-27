namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveUpdateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaLiveResult Result { get; }
}
