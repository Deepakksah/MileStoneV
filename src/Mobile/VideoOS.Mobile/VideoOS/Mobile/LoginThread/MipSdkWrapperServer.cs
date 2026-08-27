using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Platform;
using VideoOS.Platform.SDK;

namespace VideoOS.Mobile.LoginThread;

public class MipSdkWrapperServer
{
	public static readonly MipSdkWrapperServer Instance = new MipSdkWrapperServer();

	public UserContext UserContext { get; private set; }

	private MipSdkWrapperServer()
	{
	}

	public bool Login(ILoginData loginData, out Exception exception)
	{
		exception = null;
		if (UserContext == null)
		{
			UserContext = MipSdkWrapperExt.LoginMipMultiUserContextDelegate(loginData.Username, loginData.Password, loginData.AdUser, ClientTypes.MobileServer, loginData.ServerUri, out exception);
		}
		if (exception != null)
		{
			Log.Trace("Login", "Server login");
			Log.Exception(exception);
		}
		if (UserContext == null)
		{
			return false;
		}
		bool num = MultiUserEnvironment.IsServerConnected(UserContext, UserContext.Configuration.ServerFQID.ServerId.Uri);
		if (!num)
		{
			Log.Error("MipSdkWrapperServer.Login()", "MultiUserEnvironment.IsServerConnected returned FALSE");
			exception = new EndpointNotFoundCommunicationMIPException("Server not connected");
			return num;
		}
		loginData.ServerFqid = UserContext.Configuration.ServerFQID;
		MultiUserEnvironment.ReloadConfiguration(UserContext, UserContext.Configuration.ServerFQID);
		MultiUserEnvironment.ReloadConfiguration(VideoOS.Platform.Configuration.Instance.ServerFQID.ServerId.UserContext, UserContext.Configuration.ServerFQID);
		return num;
	}

	public void Logout()
	{
		MipSdkWrapperExt.ClearUserContext(UserContext);
		UserContext = null;
	}
}
