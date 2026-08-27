using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace VideoOS.Platform.SDK.Config;

public class PingUtil
{
	public class PingData
	{
		public IPAddress IPAddress;

		public string Name;

		public bool Reached;

		public DateTime ValidUntil;
	}

	private static Dictionary<IPAddress, PingData> _pingDatas = new Dictionary<IPAddress, PingData>();

	public static int PingDataLifeTimeInSeconds = 15;

	public static bool IsReachable(string name)
	{
		if (!EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("UsePing") || !(EnvironmentManager.Instance.EnvironmentOptions["UsePing"] == "Yes"))
		{
			return true;
		}
		lock (_pingDatas)
		{
			foreach (PingData value in _pingDatas.Values)
			{
				if (value.ValidUntil > DateTime.Now && value.Name == name)
				{
					return value.Reached;
				}
			}
		}
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(name);
			for (int i = 0; i < hostAddresses.Length; i++)
			{
				if (IsReachable(hostAddresses[i], name))
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static bool IsReachable(IPAddress ipaddress, string name)
	{
		lock (_pingDatas)
		{
			if (_pingDatas.TryGetValue(ipaddress, out var value) && value.ValidUntil > DateTime.Now)
			{
				return value.Reached;
			}
		}
		bool flag = Reachable(ipaddress);
		lock (_pingDatas)
		{
			_pingDatas[ipaddress] = new PingData
			{
				IPAddress = ipaddress,
				Reached = flag,
				ValidUntil = DateTime.Now + TimeSpan.FromSeconds(PingDataLifeTimeInSeconds),
				Name = name
			};
			return flag;
		}
	}

	private static bool Reachable(IPAddress address)
	{
		try
		{
			Ping ping = new Ping();
			if (ping.Send(address).Status == IPStatus.Success)
			{
				ping.Dispose();
				return true;
			}
			ping.Dispose();
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
