namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public interface IPushStreamSub<T> where T : PushStreamData, new()
{
	ParceDeviceState GetDeviceState { get; set; }

	void SendFrame(T frame, bool blocking = false);
}
