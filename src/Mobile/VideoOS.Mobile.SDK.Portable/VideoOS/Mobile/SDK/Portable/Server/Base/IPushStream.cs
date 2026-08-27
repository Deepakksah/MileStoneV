namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IPushStream<T> where T : PushStreamData, new()
{
	void EnqueueFrame(T frame);
}
