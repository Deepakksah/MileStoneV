using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public static class HostNameHelper
{
	public static string GetHostName(string ipAddress)
	{
		try
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
			if (hostEntry != null)
			{
				return hostEntry.HostName;
			}
		}
		catch (SocketException ex)
		{
			Log.Debug(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return null;
	}

	public static List<string> GetHostNames(string host)
	{
		List<string> list = new List<string>();
		string hostName = GetHostName(host);
		if (string.IsNullOrEmpty(hostName))
		{
			list.Add(host);
		}
		else
		{
			string[] array = hostName.Split('.');
			string text = string.Empty;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				text += text2;
				list.Add(text);
				text += '.';
			}
		}
		return list;
	}
}
