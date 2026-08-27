namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public interface IPushVideo
{
	int QueueSize { get; }

	int MaxQueueSize { get; set; }
}
