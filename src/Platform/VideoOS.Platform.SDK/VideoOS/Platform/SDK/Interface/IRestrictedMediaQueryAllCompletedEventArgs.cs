namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaQueryAllCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMedia[] Results { get; }
}
