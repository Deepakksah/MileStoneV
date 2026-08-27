namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IStreamingDeviceCapabilities
{
	bool LiveEnabled { get; }

	bool PlaybackEnabled { get; }

	bool BookmarkAddEnabled { get; }

	bool BookmarkDeleteEnabled { get; }

	bool BookmarkEditEnabled { get; }

	bool BookmarkViewEnabled { get; }
}
