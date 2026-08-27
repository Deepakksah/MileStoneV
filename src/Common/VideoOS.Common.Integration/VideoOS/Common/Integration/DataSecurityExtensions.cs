using System;
using System.Collections.Generic;
using VideoOS.Common.Interfaces.Recorder;

namespace VideoOS.Common.Integration;

public class DataSecurityExtensions : IDataSecurityExtension
{
	private List<IDataSecurityExtension> dataSecurityExtensions;

	public DataSecurityExtensions(List<IDataSecurityExtension> dataSecurityExtensions)
	{
		this.dataSecurityExtensions = dataSecurityExtensions;
	}

	public void ConnectionEstablished()
	{
		foreach (IDataSecurityExtension dataSecurityExtension in dataSecurityExtensions)
		{
			dataSecurityExtension.ConnectionEstablished();
		}
	}

	public void ConnectionTerminated()
	{
		foreach (IDataSecurityExtension dataSecurityExtension in dataSecurityExtensions)
		{
			dataSecurityExtension.ConnectionTerminated();
		}
	}

	public bool IsAllowedLive(Guid[] roleIds, DateTime time)
	{
		bool flag = true;
		foreach (IDataSecurityExtension dataSecurityExtension in dataSecurityExtensions)
		{
			flag = flag && dataSecurityExtension.IsAllowedLive(roleIds, time);
		}
		return flag;
	}

	public bool IsAllowedBrowse(Guid[] roleIds, DateTime startTime, DateTime stopTime)
	{
		bool flag = true;
		foreach (IDataSecurityExtension dataSecurityExtension in dataSecurityExtensions)
		{
			flag = flag && dataSecurityExtension.IsAllowedBrowse(roleIds, startTime, stopTime);
		}
		return flag;
	}
}
