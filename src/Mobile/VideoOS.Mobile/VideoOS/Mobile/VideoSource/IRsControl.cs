namespace VideoOS.Mobile.VideoSource;

public interface IRsControl
{
	bool CreateRsChannel();

	bool InitRsChannel();

	bool CloseRsChannel();
}
