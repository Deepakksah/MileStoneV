using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace VideoOS.Installer.UI.Framework.IISSetup;

public class TCPUDPConnections : IEnumerable<TCPUDPConnection>, IEnumerable
{
	public delegate void ItemAddedEvent(object sender, TCPUDPConnection item);

	public delegate void ItemChangedEvent(object sender, TCPUDPConnection item, int Pos);

	public delegate void ItemInsertedEvent(object sender, TCPUDPConnection item, int Position);

	public delegate void ItemDeletedEvent(object sender, TCPUDPConnection item, int Position);

	private List<TCPUDPConnection> _list;

	private Timer _timer;

	private int _DeadConnsMultiplier = 10;

	private int _TimerCounter = -1;

	private string _LocalHostName = string.Empty;

	private DateTime _LastRefreshDateTime = DateTime.MinValue;

	private SortConnections _connComp = new SortConnections();

	public int DeadConnsMultiplier
	{
		get
		{
			return _DeadConnsMultiplier;
		}
		set
		{
			_DeadConnsMultiplier = value;
		}
	}

	public Timer Timer => _timer;

	public DateTime LastRefreshDateTime => _LastRefreshDateTime;

	public bool AutoRefresh
	{
		get
		{
			return _timer.Enabled;
		}
		set
		{
			_timer.Enabled = value;
		}
	}

	public int Count => _list.Count;

	public TCPUDPConnection this[int index]
	{
		get
		{
			return _list[index];
		}
		set
		{
			_list[index] = value;
		}
	}

	public string LocalHostName => _LocalHostName;

	public event ItemAddedEvent ItemAdded;

	public event ItemChangedEvent ItemChanged;

	public event ItemInsertedEvent ItemInserted;

	public event ItemDeletedEvent ItemDeleted;

	public TCPUDPConnections()
	{
		_LocalHostName = Utils.GetLocalHostName();
		_list = new List<TCPUDPConnection>();
		_timer = new Timer();
		_timer.Interval = 1000.0;
		_timer.Elapsed += _timer_Elapsed;
		_timer.Start();
	}

	private void ItemAddedEventHandler(TCPUDPConnection item)
	{
		if (this.ItemAdded != null)
		{
			this.ItemAdded(this, item);
		}
	}

	private void ItemChangedEventHandler(TCPUDPConnection item, int Pos)
	{
		if (this.ItemChanged != null)
		{
			this.ItemChanged(this, item, Pos);
		}
	}

	private void ItemInsertedEventHandler(TCPUDPConnection item, int Position)
	{
		if (this.ItemInserted != null)
		{
			this.ItemInserted(this, item, Position);
		}
	}

	private void ItemDeletedEventHandler(TCPUDPConnection item, int Position)
	{
		if (this.ItemDeleted != null)
		{
			this.ItemDeleted(this, item, Position);
		}
	}

	private void _timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		Refresh();
	}

	public void Refresh()
	{
		lock (this)
		{
			_LastRefreshDateTime = DateTime.Now;
			GetTcpConnections();
			GetUdpConnections();
			_TimerCounter++;
			if (_DeadConnsMultiplier == _TimerCounter)
			{
				CheckForClosedConnections();
				_TimerCounter = -1;
			}
		}
	}

	public void StopAutoRefresh()
	{
		_timer.Stop();
	}

	public void StartAutoRefresh()
	{
		_timer.Start();
	}

	public void Add(TCPUDPConnection item)
	{
		int Pos = 0;
		TCPUDPConnection tCPUDPConnection = IndexOf(item, out Pos);
		if (tCPUDPConnection == null)
		{
			item.WasActiveAt = DateTime.Now;
			if (Pos > -1)
			{
				Insert(Pos, item);
				return;
			}
			_list.Add(item);
			ItemAddedEventHandler(item);
			return;
		}
		_list[Pos].WasActiveAt = DateTime.Now;
		if (tCPUDPConnection.iState != item.iState || tCPUDPConnection.PID != item.PID)
		{
			tCPUDPConnection.iState = item.iState;
			tCPUDPConnection.PID = item.PID;
			ItemChangedEventHandler(tCPUDPConnection, Pos);
		}
	}

	public void Sort()
	{
		_list.Sort(_connComp);
	}

	public TCPUDPConnection IndexOf(TCPUDPConnection item, out int Pos)
	{
		int num = -1;
		foreach (TCPUDPConnection item2 in _list)
		{
			num++;
			int num2 = _connComp.CompareConnections(item, item2);
			if (num2 == 0)
			{
				Pos = num;
				return item2;
			}
			if (num2 <= 0 && num2 < 0)
			{
				Pos = num;
				return null;
			}
		}
		Pos = -1;
		return null;
	}

	public void CheckForClosedConnections()
	{
		int num = (int)_timer.Interval * _DeadConnsMultiplier;
		for (int num2 = _list.Count - 1; num2 >= 0; num2--)
		{
			TCPUDPConnection tCPUDPConnection = this[num2];
			if (Math.Abs((int)(_LastRefreshDateTime - tCPUDPConnection.WasActiveAt).TotalMilliseconds) > num)
			{
				Remove(num2);
			}
		}
	}

	public void Remove(int index)
	{
		TCPUDPConnection item = this[index];
		_list.RemoveAt(index);
		ItemDeletedEventHandler(item, index);
	}

	public void Insert(int index, TCPUDPConnection item)
	{
		_list.Insert(index, item);
		ItemInsertedEventHandler(item, index);
	}

	public void GetTcpConnections()
	{
		int ipVersion = 2;
		int dwOutBufLen = 20000;
		byte[] array = new byte[dwOutBufLen];
		if (IPHlpAPI32Wrapper.GetExtendedTcpTable(array, out dwOutBufLen, sort: true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) != 0)
		{
			array = new byte[dwOutBufLen];
			if (IPHlpAPI32Wrapper.GetExtendedTcpTable(array, out dwOutBufLen, sort: true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) != 0)
			{
				return;
			}
		}
		int num = 0;
		int num2 = Convert.ToInt32(array[num]);
		num += 4;
		for (int i = 0; i < num2; i++)
		{
			TCPUDPConnection tCPUDPConnection = new TCPUDPConnection(this);
			int iState = Convert.ToInt32(array[num]);
			tCPUDPConnection.iState = iState;
			num += 4;
			tCPUDPConnection.Protocol = Protocol.TCP;
			tCPUDPConnection.Local = Utils.BufferToIPEndPoint(array, ref num, IsRemote: false);
			tCPUDPConnection.Remote = Utils.BufferToIPEndPoint(array, ref num, IsRemote: true);
			tCPUDPConnection.PID = Utils.BufferToInt(array, ref num);
			Add(tCPUDPConnection);
		}
	}

	public void GetUdpConnections()
	{
		int ipVersion = 2;
		int dwOutBufLen = 20000;
		byte[] array = new byte[dwOutBufLen];
		if (IPHlpAPI32Wrapper.GetExtendedUdpTable(array, out dwOutBufLen, sort: true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0) != 0)
		{
			array = new byte[dwOutBufLen];
			if (IPHlpAPI32Wrapper.GetExtendedUdpTable(array, out dwOutBufLen, sort: true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0) != 0)
			{
				return;
			}
		}
		int num = 0;
		int num2 = Convert.ToInt32(array[num]);
		num += 4;
		for (int i = 0; i < num2; i++)
		{
			TCPUDPConnection tCPUDPConnection = new TCPUDPConnection(this);
			tCPUDPConnection.Protocol = Protocol.UDP;
			tCPUDPConnection.Local = Utils.BufferToIPEndPoint(array, ref num, IsRemote: false);
			tCPUDPConnection.PID = Utils.BufferToInt(array, ref num);
			Add(tCPUDPConnection);
		}
	}

	public IEnumerator<TCPUDPConnection> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
