namespace VideoOS.Platform.SDK.Interface;

public interface IGetClientProfileCompletedEventArgs : IAsyncCompletedEventArgs
{
	IClientProfile Result { get; }
}
