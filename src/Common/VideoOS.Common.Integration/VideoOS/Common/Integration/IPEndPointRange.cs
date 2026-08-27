using System;
using System.Net;
using VideoOS.Utilities.Network;

namespace VideoOS.Common.Integration;

public class IPEndPointRange
{
	private IPAddress _startIP;

	private IPAddress _endIP;

	private int _startPort;

	private int _endPort;

	private IPEndPoint _currentIPEndPoint;

	private bool _restarted = true;

	public IPAddress StartIPAddress
	{
		get
		{
			return _startIP;
		}
		set
		{
			_startIP = value;
			Reset();
		}
	}

	public IPAddress EndIPAddress
	{
		get
		{
			return _endIP;
		}
		set
		{
			_endIP = value;
			Reset();
		}
	}

	public int StartPort
	{
		get
		{
			return _startPort;
		}
		set
		{
			_startPort = value;
			Reset();
		}
	}

	public int EndPort
	{
		get
		{
			return _endPort;
		}
		set
		{
			_endPort = value;
			Reset();
		}
	}

	public IPEndPoint CurrentIPEndPoint
	{
		get
		{
			if (_restarted)
			{
				return null;
			}
			return new IPEndPoint(_currentIPEndPoint.Address, _currentIPEndPoint.Port);
		}
	}

	public IPAddress CurrentIPAddress
	{
		get
		{
			return _currentIPEndPoint.Address;
		}
		set
		{
			if (IsWithinRange(value))
			{
				_currentIPEndPoint.Address = value;
			}
			else
			{
				_currentIPEndPoint.Address = _startIP;
			}
			_restarted = false;
		}
	}

	public IPEndPointRange(IPAddress startIP, IPAddress endIP, int portStart, int portEnd)
	{
		_startIP = startIP;
		_endIP = endIP;
		_startPort = portStart;
		_endPort = portEnd;
		Reset();
	}

	public IPEndPointRange(IPAddress startIP, IPAddress endIP)
		: this(startIP, endIP, 0, 0)
	{
	}

	private void Reset()
	{
		_currentIPEndPoint = new IPEndPoint(_startIP, _startPort);
		_restarted = true;
	}

	public IPEndPoint GetNextIPEndPoint()
	{
		if (_restarted)
		{
			_restarted = false;
		}
		else if (_currentIPEndPoint.Port + 1 > _endPort)
		{
			_currentIPEndPoint.Port = _startPort;
			IncrementIPAddress();
		}
		else
		{
			_currentIPEndPoint.Port++;
		}
		return new IPEndPoint(_currentIPEndPoint.Address, _currentIPEndPoint.Port);
	}

	public IPAddress GetNextIPAddress()
	{
		if (_restarted)
		{
			_restarted = false;
		}
		else
		{
			IncrementIPAddress();
		}
		return new IPAddress(_currentIPEndPoint.Address.GetAddressBytes());
	}

	public IPAddress GetPreviousIPAddress()
	{
		if (_restarted)
		{
			_restarted = false;
		}
		else
		{
			DecrementIPAddress();
		}
		return new IPAddress(_currentIPEndPoint.Address.GetAddressBytes());
	}

	private void IncrementIPAddress()
	{
		if (_currentIPEndPoint.Address.Equals(_endIP))
		{
			_currentIPEndPoint.Address = _startIP;
		}
		else
		{
			_currentIPEndPoint.Address = IPHelper.IncrementIPAddress(_currentIPEndPoint.Address);
		}
	}

	private void DecrementIPAddress()
	{
		if (_currentIPEndPoint.Address.Equals(_startIP))
		{
			_currentIPEndPoint.Address = _endIP;
		}
		else
		{
			_currentIPEndPoint.Address = IPHelper.DecrementIPAddress(_currentIPEndPoint.Address);
		}
	}

	public bool IsWithinRange(IPAddress address)
	{
		bool result = false;
		if (IPHelper.IPAddressCompare(address, _startIP) >= 0 && IPHelper.IPAddressCompare(address, _endIP) <= 0)
		{
			result = true;
		}
		return result;
	}

	public bool IsRangeOverlapping(IPEndPointRange range)
	{
		if (range == null)
		{
			throw new ArgumentNullException("range");
		}
		bool result = false;
		if (_startIP.AddressFamily == range.StartIPAddress.AddressFamily)
		{
			if (IPHelper.IPAddressCompare(range.EndIPAddress, _startIP) >= 0 && IPHelper.IPAddressCompare(range.EndIPAddress, _endIP) <= 0)
			{
				result = true;
			}
			else if (IPHelper.IPAddressCompare(range.StartIPAddress, _startIP) <= 0 && IPHelper.IPAddressCompare(range.EndIPAddress, _endIP) >= 0)
			{
				result = true;
			}
			else if (IPHelper.IPAddressCompare(range.StartIPAddress, _startIP) >= 0 && IPHelper.IPAddressCompare(range.StartIPAddress, _endIP) <= 0)
			{
				result = true;
			}
		}
		return result;
	}
}
