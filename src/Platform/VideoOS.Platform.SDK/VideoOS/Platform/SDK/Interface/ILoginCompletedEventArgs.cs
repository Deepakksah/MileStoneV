namespace VideoOS.Platform.SDK.Interface;

public interface ILoginCompletedEventArgs : IAsyncCompletedEventArgs
{
	ILoginInfo Result { get; }
}
