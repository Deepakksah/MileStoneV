namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaCreateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaResult Result { get; }
}
