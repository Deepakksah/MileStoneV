using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public interface IBaseRequestStreamResponseDownload<T> where T : StreamParams
{
	T StreamParams { get; }

	int SourceWidth { get; }

	int SourceHeight { get; }
}
