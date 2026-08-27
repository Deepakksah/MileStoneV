using System;
using System.Net;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public class TCPUDPConnection
{
	private int _dwState;

	private static TCPUDPConnections _conns;

	private bool _IsResolveIP = true;

	private Protocol _Protocol;

	private string _State = string.Empty;

	private IPEndPoint _OldLocalHostName;

	private IPEndPoint _OldRemoteHostName;

	private string _LocalAddress = string.Empty;

	private string _RemoteAddress = string.Empty;

	private IPEndPoint _Local;

	private IPEndPoint _Remote;

	private int _dwOwningPid;

	private int _OldProcessID = -1;

	private string _ProcessName = string.Empty;

	private DateTime _WasActiveAt = DateTime.MinValue;

	private object _Tag;

	public int iState
	{
		get
		{
			return _dwState;
		}
		set
		{
			if (_dwState != value)
			{
				_dwState = value;
				_State = Utils.StateToStr(value);
			}
		}
	}

	public bool IsResolveIP
	{
		get
		{
			return _IsResolveIP;
		}
		set
		{
			_IsResolveIP = value;
		}
	}

	public Protocol Protocol
	{
		get
		{
			return _Protocol;
		}
		set
		{
			_Protocol = value;
		}
	}

	public string State => _State;

	public string LocalAddress
	{
		get
		{
			if (_OldLocalHostName == _Local)
			{
				if (_LocalAddress.Trim() == string.Empty)
				{
					SaveHostName(IsLocalHostName: true);
				}
			}
			else
			{
				SaveHostName(IsLocalHostName: true);
			}
			return _LocalAddress;
		}
	}

	public string RemoteAddress
	{
		get
		{
			if (_OldRemoteHostName == _Remote)
			{
				if (_RemoteAddress.Trim() == string.Empty)
				{
					SaveHostName(IsLocalHostName: false);
				}
			}
			else
			{
				SaveHostName(IsLocalHostName: false);
			}
			return _RemoteAddress;
		}
	}

	public IPEndPoint Local
	{
		get
		{
			return _Local;
		}
		set
		{
			if (_Local != value)
			{
				_Local = value;
			}
		}
	}

	public IPEndPoint Remote
	{
		get
		{
			return _Remote;
		}
		set
		{
			if (_Remote != value)
			{
				_Remote = value;
			}
		}
	}

	public int PID
	{
		get
		{
			return _dwOwningPid;
		}
		set
		{
			if (_dwOwningPid != value)
			{
				_dwOwningPid = value;
			}
		}
	}

	public string ProcessName
	{
		get
		{
			if (_OldProcessID == _dwOwningPid)
			{
				if (_ProcessName.Trim() == string.Empty)
				{
					SaveProcessID();
				}
			}
			else
			{
				SaveProcessID();
			}
			return _ProcessName;
		}
	}

	public DateTime WasActiveAt
	{
		get
		{
			return _WasActiveAt;
		}
		public set
		{
			_WasActiveAt = value;
		}
	}

	public object Tag
	{
		get
		{
			return _Tag;
		}
		set
		{
			_Tag = value;
		}
	}

	public TCPUDPConnection(TCPUDPConnections conns)
	{
		_conns = conns;
	}

	public string GetHostName(IPEndPoint HostAddress)
	{
		return Utils.GetHostName(HostAddress, _conns.LocalHostName);
	}

	private void SaveHostName(bool IsLocalHostName)
	{
		if (IsLocalHostName)
		{
			_LocalAddress = GetHostName(_Local);
			_OldLocalHostName = _Local;
		}
		else
		{
			_RemoteAddress = GetHostName(_Remote);
			_OldRemoteHostName = _Remote;
		}
	}

	private void SaveProcessID()
	{
		_ProcessName = Utils.GetProcessNameByPID(_dwOwningPid);
		_OldProcessID = _dwOwningPid;
	}
}
