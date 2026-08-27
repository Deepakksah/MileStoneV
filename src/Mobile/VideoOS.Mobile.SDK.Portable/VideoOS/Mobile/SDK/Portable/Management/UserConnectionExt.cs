using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class UserConnectionExt
{
	public UserConnection Connection { get; set; }

	public Camera CameraParams { get; set; }

	public UserConnectionExt()
	{
	}

	public UserConnectionExt(UserConnection connection, Camera camera)
	{
		Connection = connection;
		CameraParams = camera;
	}
}
