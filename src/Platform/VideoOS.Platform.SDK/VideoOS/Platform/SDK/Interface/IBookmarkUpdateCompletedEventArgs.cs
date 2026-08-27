namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkUpdateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmark Result { get; }
}
