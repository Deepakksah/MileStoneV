using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IPushVideoSuccess
{
	OnFrameCompletion SuccessCallback { get; set; }
}
