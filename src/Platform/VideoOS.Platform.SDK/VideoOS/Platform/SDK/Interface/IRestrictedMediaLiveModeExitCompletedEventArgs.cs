namespace VideoOS.Platform.SDK.Interface;

public interface IRestrictedMediaLiveModeExitCompletedEventArgs : IAsyncCompletedEventArgs
{
	IRestrictedMediaResult Result { get; }
}
