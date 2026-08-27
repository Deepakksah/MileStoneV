using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IPushVideoFail
{
	long TotalFailCount { get; }

	OnFrameCompletion FailCallback { get; set; }
}
