using System;
using System.Collections;
using System.Net;

namespace VideoOS.EventServer.Util;

public class IPaddressResolve
{
	public static readonly IPaddressResolve instance;

	private Hashtable entries = new Hashtable();

	private long lifeTime;

	private readonly long FiveMinutes = 3000000000L;

	public static IPaddressResolve Instance => instance;

	static IPaddressResolve()
	{
		instance = new IPaddressResolve();
	}

	public IPHostEntry hostEntry(string address)
	{
		if (lifeTime < DateTime.Now.Ticks)
		{
			entries = new Hashtable();
			lifeTime = DateTime.Now.Ticks + FiveMinutes;
		}
		IPHostEntry iPHostEntry = (IPHostEntry)entries[address];
		if (iPHostEntry != null)
		{
			return iPHostEntry;
		}
		try
		{
			if (address.Split('.').Length == 4)
			{
				IPAddress iPAddress = IPAddress.Parse(address);
				iPHostEntry = new IPHostEntry();
				iPHostEntry.AddressList = new IPAddress[1] { iPAddress };
				iPHostEntry.HostName = address;
				lock (entries)
				{
					entries[address] = iPHostEntry;
				}
				return iPHostEntry;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			iPHostEntry = Dns.GetHostEntry(address);
		}
		catch (Exception)
		{
		}
		if (iPHostEntry == null)
		{
			return iPHostEntry;
		}
		lock (entries)
		{
			entries[address] = iPHostEntry;
			return iPHostEntry;
		}
	}
}
