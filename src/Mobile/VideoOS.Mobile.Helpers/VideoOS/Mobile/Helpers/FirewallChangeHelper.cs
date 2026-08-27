using System;
using NetFwTypeLib;

namespace VideoOS.Mobile.Helpers;

public static class FirewallChangeHelper
{
	public static bool CloseFirewall(int[] ports, out string errMessage, string programname = "")
	{
		ports = ports ?? throw new ArgumentNullException("ports");
		bool result = true;
		errMessage = string.Empty;
		try
		{
			INetFwMgr fwMgr = GetFwMgr();
			INetFwAuthorizedApplication app = GetApp();
			if (fwMgr != null && app != null)
			{
				if (!string.IsNullOrWhiteSpace(programname))
				{
					fwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(programname);
				}
				for (NET_FW_PROFILE_TYPE_ nET_FW_PROFILE_TYPE_ = NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN; nET_FW_PROFILE_TYPE_ < NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_TYPE_MAX; nET_FW_PROFILE_TYPE_++)
				{
					INetFwOpenPorts globallyOpenPorts = fwMgr.LocalPolicy.GetProfileByType(nET_FW_PROFILE_TYPE_).GloballyOpenPorts;
					int[] array = ports;
					foreach (int portNumber in array)
					{
						globallyOpenPorts.Remove(portNumber, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
					}
				}
			}
		}
		catch (Exception ex)
		{
			errMessage = ex.Message;
			result = false;
		}
		return result;
	}

	public static bool OpenFirewall(string servicename, int[] ports, out string errMessage, string programname = "")
	{
		ports = ports ?? throw new ArgumentNullException("ports");
		bool result = true;
		errMessage = string.Empty;
		try
		{
			INetFwMgr fwMgr = GetFwMgr();
			INetFwAuthorizedApplication app = GetApp();
			if (fwMgr != null && app != null)
			{
				if (!string.IsNullOrWhiteSpace(programname))
				{
					INetFwProfile currentProfile = fwMgr.LocalPolicy.CurrentProfile;
					app.ProcessImageFileName = programname;
					app.Name = servicename;
					app.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
					app.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY;
					app.Enabled = true;
					currentProfile.AuthorizedApplications.Add(app);
				}
				for (NET_FW_PROFILE_TYPE_ nET_FW_PROFILE_TYPE_ = NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_DOMAIN; nET_FW_PROFILE_TYPE_ < NET_FW_PROFILE_TYPE_.NET_FW_PROFILE_TYPE_MAX; nET_FW_PROFILE_TYPE_++)
				{
					INetFwOpenPorts globallyOpenPorts = fwMgr.LocalPolicy.GetProfileByType(nET_FW_PROFILE_TYPE_).GloballyOpenPorts;
					int[] array = ports;
					for (int i = 0; i < array.Length; i++)
					{
						int port = array[i];
						INetFwOpenPort netOpenPort = GetNetOpenPort();
						netOpenPort.Port = port;
						netOpenPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
						netOpenPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
						netOpenPort.Name = servicename + " - Port " + port;
						globallyOpenPorts.Add(netOpenPort);
					}
				}
			}
		}
		catch (Exception ex)
		{
			result = false;
			errMessage = ex.Message;
		}
		return result;
	}

	public static INetFwMgr GetFwMgr()
	{
		INetFwMgr result = null;
		try
		{
			Type type = Type.GetTypeFromCLSID(new Guid("304CE942-6E39-40D8-943A-B913C40C9CD4"));
			if (type == null)
			{
				type = Type.GetTypeFromProgID("HNetCfg.FwMgr");
			}
			result = Activator.CreateInstance(type) as INetFwMgr;
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static INetFwAuthorizedApplication GetApp()
	{
		Type type = Type.GetTypeFromCLSID(new Guid("EC9846B3-2762-4A6B-A214-6ACB603462D2"));
		if (type == null)
		{
			type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
		}
		return Activator.CreateInstance(type) as INetFwAuthorizedApplication;
	}

	public static INetFwOpenPort GetNetOpenPort()
	{
		Type type = Type.GetTypeFromCLSID(new Guid("0CA545C6-37AD-4A6C-BF92-9F7610067EF5"));
		if (type == null)
		{
			type = Type.GetTypeFromProgID("HNetCfg.INetFwOpenPort");
		}
		return Activator.CreateInstance(type) as INetFwOpenPort;
	}
}
