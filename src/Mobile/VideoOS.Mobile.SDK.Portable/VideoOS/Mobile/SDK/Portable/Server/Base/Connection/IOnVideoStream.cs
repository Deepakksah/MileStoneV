namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public interface IOnVideoStream
{
	void CreatedVideo(IStreamReconnection iVideo);

	void DestroyedVideo(IStreamReconnection iVideo);
}
