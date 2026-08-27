using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.Business.Components;

public class SessionMsgIndicator
{
	private static SessionMsgIndicator _theData;

	private Mutex _dataLock = new Mutex(initiallyOwned: true);

	private int _totalMsgCount;

	private DateTime _latestMsgCreateTime = DateTime.MinValue;

	private HashSet<Guid> _sessionsWithMessages = new HashSet<Guid>();

	public static SessionMsgIndicator Current => _theData;

	public int TotalMsgCount
	{
		get
		{
			_dataLock.WaitOne(-1);
			try
			{
				return _totalMsgCount;
			}
			finally
			{
				_dataLock.ReleaseMutex();
			}
		}
	}

	public DateTime LatestMsgCreateTime
	{
		get
		{
			_dataLock.WaitOne(-1);
			try
			{
				return _latestMsgCreateTime;
			}
			finally
			{
				_dataLock.ReleaseMutex();
			}
		}
	}

	private static void InitData()
	{
		_theData = new SessionMsgIndicator();
		try
		{
		}
		finally
		{
			_theData._dataLock.ReleaseMutex();
		}
	}

	static SessionMsgIndicator()
	{
		InitData();
	}

	public void ClearData()
	{
		_dataLock.WaitOne(-1);
		try
		{
			InitData();
		}
		finally
		{
			_dataLock.ReleaseMutex();
		}
	}

	public bool HasSessionMsgs(Guid sessionId)
	{
		_dataLock.WaitOne(-1);
		try
		{
			return _sessionsWithMessages.Contains(sessionId);
		}
		finally
		{
			_dataLock.ReleaseMutex();
		}
	}

	public void UpdateData(SessionMsgIndicatorData newData)
	{
		_dataLock.WaitOne(-1);
		try
		{
			_totalMsgCount = newData.TotalMsgCount;
			_latestMsgCreateTime = newData.LatestMsgCreateTime;
			_sessionsWithMessages = newData.SessionsWithMessages;
		}
		finally
		{
			_dataLock.ReleaseMutex();
		}
	}

	public SessionMsgIndicatorData GetAllCacheData()
	{
		SessionMsgIndicatorData sessionMsgIndicatorData = new SessionMsgIndicatorData();
		_dataLock.WaitOne(-1);
		try
		{
			sessionMsgIndicatorData.LatestMsgCreateTime = _latestMsgCreateTime;
			sessionMsgIndicatorData.TotalMsgCount = _totalMsgCount;
			foreach (Guid sessionsWithMessage in _sessionsWithMessages)
			{
				sessionMsgIndicatorData.SessionsWithMessages.Add(sessionsWithMessage);
			}
			return sessionMsgIndicatorData;
		}
		finally
		{
			_dataLock.ReleaseMutex();
		}
	}
}
