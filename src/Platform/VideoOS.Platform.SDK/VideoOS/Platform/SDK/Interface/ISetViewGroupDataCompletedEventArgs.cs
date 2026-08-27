namespace VideoOS.Platform.SDK.Interface;

public interface ISetViewGroupDataCompletedEventArgs : IAsyncCompletedEventArgs
{
	SetViewGroupDataResultType Result { get; }
}
