namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaDeleteCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaResult Result { get; }
}
