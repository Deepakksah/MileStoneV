namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkGetCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmark Result { get; }
}
