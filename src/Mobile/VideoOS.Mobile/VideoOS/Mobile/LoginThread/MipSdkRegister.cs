using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Registration;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.LoginThread;

public class MipSdkRegister
{
	private const string F22B2E2D793075 = "5AC5173927744AD49773F1AECD77D27E7C1619812E814B19";

	public ILoginData LoginData { get; private set; }

	public ILoginConfiguration LoginConfiguration { get; private set; }

	public ILoginConfigSpecific LoginConfigSpecific { get; private set; }

	public MipSdkRegister(ILoginData data, ILoginConfiguration configuration, ILoginConfigSpecific specific)
	{
		LoginData = data;
		LoginConfiguration = configuration;
		LoginConfigSpecific = specific;
	}

	public bool RegisterMobileService()
	{
		try
		{
			ServerData currentServerData = GetServerData();
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = ServiceUtil.GetRegisteredServiceUriInfo(LoginConfigSpecific.ServerTypeId, LoginData.ServerFqid.ServerId);
			Guid[] instanceIds = (from svc in registeredServiceUriInfo
				where svc.Instance != LoginData.XpmoInstance
				where ServerData.FromXml(svc.ServiceData).ComputerName.Equals(currentServerData.ComputerName, StringComparison.CurrentCultureIgnoreCase)
				select svc.Instance).ToArray();
			UnregisterServices(instanceIds);
			VideoOS.Platform.Configuration.ServiceURIInfo serviceURIInfo = registeredServiceUriInfo.FirstOrDefault((VideoOS.Platform.Configuration.ServiceURIInfo svc) => svc.Instance == LoginData.XpmoInstance);
			if (serviceURIInfo.Instance == default(Guid))
			{
				RegisterService(currentServerData, GetServerUri());
			}
			else
			{
				ServerData serverData = ServerData.FromXml(serviceURIInfo.ServiceData);
				if (!currentServerData.Equals(serverData))
				{
					if (!serverData.ComputerName.Equals(currentServerData.ComputerName, StringComparison.CurrentCultureIgnoreCase) || serverData.HttpPort != currentServerData.HttpPort || serverData.HttpSecurePort != currentServerData.HttpSecurePort)
					{
						UnregisterServices(new Guid[1] { serviceURIInfo.Instance });
						RegisterService(currentServerData, GetServerUri());
					}
					else
					{
						if (!serviceURIInfo.UriArray.First().Equals(GetServerUri().ToString(), StringComparison.CurrentCultureIgnoreCase))
						{
							Log.Info("MipSdkRegister.RegisterMobileService()", "Registered server uri (" + serviceURIInfo.UriArray.First() + ") differ from default registration (" + GetServerUri().ToString() + ")");
						}
						ServiceUtil.SetServiceData(LoginData.ServerFqid.ServerId, serviceURIInfo.Instance, currentServerData.ToXml());
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkRegister.RegisterMobileService()", ex.Message);
			return false;
		}
		return true;
	}

	private void UnregisterServices(IEnumerable<Guid> instanceIds)
	{
		try
		{
			foreach (Guid instanceId in instanceIds)
			{
				ServiceUtil.UnRegisterServiceUri(LoginData.ServerFqid.ServerId, instanceId);
			}
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkRegister.UnregisterServices()", ex.Message);
		}
	}

	private void RegisterService(ServerData serverData, Uri uri)
	{
		try
		{
			ServiceUtil.RegisterServiceUri(LoginConfigSpecific.ServerTypeId, LoginData.ServerFqid.ServerId, LoginData.XpmoInstance, uri, Environment.MachineName, LoginConfigSpecific.ServerDescription, serverData.ToXml());
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkRegister.RegisterService()", ex.Message);
		}
	}

	private Uri GetServerUri()
	{
		string scheme = (LoginConfiguration.IsSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
		string hostName = Dns.GetHostEntry("localhost").HostName;
		int portNumber = (LoginConfiguration.IsSecure ? LoginConfiguration.HttpSecureMetaChannelPort : LoginConfiguration.HttpMetaChannelPort);
		return new UriBuilder(scheme, hostName, portNumber).Uri;
	}

	private ServerData GetServerData()
	{
		PBE pBE = new PBE(Encoding.UTF8.GetBytes("5AC5173927744AD49773F1AECD77D27E7C1619812E814B19"));
		return new ServerData
		{
			ComputerName = Dns.GetHostEntry("localhost").HostName,
			HttpPort = LoginConfiguration.HttpMetaChannelPort,
			HttpSecurePort = LoginConfiguration.HttpSecureMetaChannelPort,
			ServerVersion = LoginConfigSpecific.ServerVersion,
			S = pBE.GetEncryptedValue(LoginConfiguration.SystemInfoUsername),
			T = pBE.GetEncryptedValue(LoginConfiguration.SystemInfoPassword)
		};
	}
}
