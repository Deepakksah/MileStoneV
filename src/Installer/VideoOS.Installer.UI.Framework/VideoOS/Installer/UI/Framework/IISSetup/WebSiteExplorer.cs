using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Web.Administration;
using Microsoft.Win32;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public static class WebSiteExplorer
{
	public static IEnumerable<SiteInfo> GetWebSites()
	{
		List<SiteInfo> list = new List<SiteInfo>();
		try
		{
			using ServerManager serverManager = new ServerManager();
			SiteCollection sites = serverManager.Sites;
			if (sites != null && sites.Count > 0)
			{
				foreach (Site item in sites)
				{
					int num = 0;
					int httpsPort = 0;
					int state = (int)item.State;
					IPHostEntry iPHostEntry = new IPHostEntry();
					iPHostEntry.HostName = "localhost";
					foreach (Binding binding in item.Bindings)
					{
						if (binding.EndPoint != null && binding.Protocol.ToUpperInvariant() == Uri.UriSchemeHttp.ToUpperInvariant())
						{
							string text = binding.EndPoint.Address.ToString();
							num = binding.EndPoint.Port;
							if (text.Length > 0 && text != "0.0.0.0")
							{
								try
								{
									iPHostEntry = Dns.GetHostEntry(text);
								}
								catch
								{
								}
								if (iPHostEntry == null)
								{
									iPHostEntry = new IPHostEntry();
									iPHostEntry.HostName = text;
								}
							}
						}
						else if (binding.EndPoint != null && binding.Protocol.ToUpperInvariant() == Uri.UriSchemeHttps.ToUpperInvariant())
						{
							httpsPort = binding.EndPoint.Port;
						}
					}
					if (num > 0)
					{
						list.Add(new SiteInfo(item.Id.ToString(CultureInfo.InvariantCulture), item.Name, iPHostEntry.HostName, num, httpsPort, state));
					}
				}
			}
		}
		catch (COMException ex)
		{
			if (ex.ErrorCode == -2147221164)
			{
				LogManager.Write("No websites were found, it does not seem like IIS is installed.");
				return new List<SiteInfo>();
			}
			LogManager.Write(ex);
			return new List<SiteInfo>();
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			if (IisInstalled())
			{
				throw new UIFrameworkErrorException(Resources.IISSetup_ErrorConfiguring, enablePreviousButtong: false);
			}
			return new List<SiteInfo>();
		}
		return list;
	}

	public static bool IisInstalled()
	{
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\InetStp\\Components", RegistryKeyPermissionCheck.ReadSubTree);
			if (registryKey == null)
			{
				RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\InetStp\\Components", RegistryKeyPermissionCheck.ReadSubTree);
				if (registryKey2 != null)
				{
					return CheckIISManagerConsole(registryKey2);
				}
				return false;
			}
			return CheckIISManagerConsole(registryKey);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			LogManager.Write("IIS is not installed.");
			return false;
		}
	}

	private static bool CheckIISManagerConsole(RegistryKey baseKey)
	{
		object value = baseKey.GetValue("ManagementConsole");
		if (value != null)
		{
			int num = Convert.ToInt32(value);
			LogManager.Write("IIS Manager console version = " + num);
			return num == 1;
		}
		value = baseKey.GetValue("LegacySnapin");
		if (value != null)
		{
			int num2 = Convert.ToInt32(value);
			LogManager.Write("IIS Manager console 6 version = " + num2);
			return num2 == 1;
		}
		return false;
	}

	public static bool GetWebsiteHttpsBinding(string websiteId, out int httpsPort)
	{
		try
		{
			using ServerManager serverManager = new ServerManager();
			Site site = serverManager.Sites.FirstOrDefault((Site s) => s.Id == long.Parse(websiteId, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture));
			if (site != null)
			{
				Binding binding = site.Bindings.FirstOrDefault((Binding b) => b.Protocol.ToUpperInvariant() == Uri.UriSchemeHttps.ToUpperInvariant());
				if (binding != null)
				{
					httpsPort = binding.EndPoint.Port;
					return site.State == ObjectState.Stopped;
				}
			}
		}
		catch (Exception)
		{
		}
		httpsPort = 443;
		return true;
	}
}
