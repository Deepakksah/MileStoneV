using VideoOS.Mobile.VideoChannel.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Classes;

public class MfLib
{
	private static MfLib _instance;

	private int _refCountMF;

	public static MfLib Instance => _instance ?? (_instance = new MfLib());

	public void StartMfNoSockets()
	{
		if (_refCountMF == 0)
		{
			MFHelper.MFStartup(1u);
		}
		_refCountMF++;
	}

	public void ShutdownMf()
	{
		if (_refCountMF > 0)
		{
			_refCountMF--;
			if (_refCountMF == 0)
			{
				MFHelper.MFShutdown();
			}
		}
	}
}
