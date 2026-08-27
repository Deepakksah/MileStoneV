#define TRACE
using System;
using System.Diagnostics;
using System.Net;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MIPFactory
{
	private static object _lockObject = new object();

	public static void CheckAndInitialize(IGeminiRequestContext requestContext)
	{
		if (Configuration.Instance != null)
		{
			return;
		}
		lock (_lockObject)
		{
			if (Configuration.Instance == null)
			{
				ServerInformation serverInformation = ServerInformation.GetServerInformation(requestContext);
				if (!Guid.TryParse(serverInformation.Id, out var result))
				{
					Trace.WriteLine("MIPConfiguration: No Mgt Server Id ???");
				}
				Uri serverUri = GetServerUri(serverInformation);
				Configuration.Instance = new MIPConfiguration(result, serverInformation.Name, serverUri.Host, serverUri.Port);
				MIPEnvironmentManager mIPEnvironmentManager = new MIPEnvironmentManager();
				mIPEnvironmentManager.Initialize();
				EnvironmentManager.Instance = mIPEnvironmentManager;
				LoginSettingsCache.AddLoginSettings(new LoginSettings(serverUri, new NetworkCredential(), result, serverInformation.ComputerName, secureOnly: false));
			}
		}
	}

	private static Uri GetServerUri(ServerInformation serverInformation)
	{
		string uri = "http://" + serverInformation.ComputerName;
		if (serverInformation.Addresses.Length != 0)
		{
			ServerAddress[] addresses = serverInformation.Addresses;
			foreach (ServerAddress serverAddress in addresses)
			{
				if (serverAddress.Type == ServerAddressType.Server)
				{
					uri = serverAddress.URL;
					break;
				}
			}
		}
		try
		{
			return new UriBuilder(uri).Uri;
		}
		catch (UriFormatException)
		{
			return new Uri("http://localhost");
		}
	}
}
