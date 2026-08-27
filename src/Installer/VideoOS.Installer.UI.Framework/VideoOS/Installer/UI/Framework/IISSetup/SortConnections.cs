using System.Collections.Generic;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public class SortConnections : IComparer<TCPUDPConnection>
{
	public virtual int CompareConnections(TCPUDPConnection first, TCPUDPConnection second)
	{
		int num = Utils.CompareIPEndPoints(first.Local, second.Local);
		if (num != 0)
		{
			return num;
		}
		if (first.Protocol == Protocol.TCP && second.Protocol == Protocol.TCP)
		{
			num = Utils.CompareIPEndPoints(first.Remote, second.Remote);
			if (num != 0)
			{
				return num;
			}
		}
		num = first.PID - second.PID;
		if (num != 0)
		{
			return num;
		}
		if (first.Protocol == second.Protocol)
		{
			return 0;
		}
		if (first.Protocol == Protocol.TCP)
		{
			return -1;
		}
		return 1;
	}

	public int Compare(TCPUDPConnection x, TCPUDPConnection y)
	{
		return CompareConnections(x, y);
	}
}
