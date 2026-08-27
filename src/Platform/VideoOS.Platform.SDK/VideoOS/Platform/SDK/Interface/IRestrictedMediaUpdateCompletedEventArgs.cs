namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaUpdateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaResult Result { get; }
}
