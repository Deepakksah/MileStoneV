using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Config;

public class QueryConfigurationChanges : IQueryConfigurationChanges
{
	private class QueryThreadState
	{
		public int ConfigurationRefreshIntervalInMs = 5000;

		public volatile bool IsLoggedIn = true;

		public readonly AutoResetEvent StopEvent = new AutoResetEvent(initialState: false);
	}

	private class UserContextQueryInfo
	{
		public UserContext UserContextToUpdate;

		public readonly Dictionary<Guid, DateTime> NextCheck = new Dictionary<Guid, DateTime>();

		public bool ProcessedByQueryThread;
	}

	private Thread _queryThread;

	private QueryThreadState _queryThreadState;

	private readonly List<UserContextQueryInfo> _userContextQueryInfos = new List<UserContextQueryInfo>();

	private readonly object _instanceLock = new object();

	private readonly AutoResetEvent _exclusiveQueryAccess = new AutoResetEvent(initialState: true);

	private bool _userLoggedIn;

	public int ConfigurationRefreshIntervalInMs { get; set; }

	public bool Enabled { get; set; } = true;

	public int UserContextCount
	{
		get
		{
			lock (_instanceLock)
			{
				return _userContextQueryInfos.Count;
			}
		}
	}

	public void AddUserContext(UserContext userContext)
	{
		UserContextQueryInfo item = new UserContextQueryInfo
		{
			UserContextToUpdate = userContext
		};
		lock (_instanceLock)
		{
			foreach (UserContextQueryInfo userContextQueryInfo in _userContextQueryInfos)
			{
				if (userContextQueryInfo.UserContextToUpdate.InstanceGuid == userContext.InstanceGuid)
				{
					return;
				}
			}
			_userContextQueryInfos.Add(item);
		}
	}

	public void RemoveUserContext(UserContext userContext)
	{
		lock (_instanceLock)
		{
			_userContextQueryInfos.RemoveAll((UserContextQueryInfo userContextQueryInfo) => userContextQueryInfo.UserContextToUpdate == userContext);
		}
	}

	public void InvalidateData(UserContext userContext)
	{
		lock (_instanceLock)
		{
			foreach (UserContextQueryInfo userContextQueryInfo in _userContextQueryInfos)
			{
				if (userContextQueryInfo.UserContextToUpdate == userContext)
				{
					userContextQueryInfo.ProcessedByQueryThread = false;
				}
			}
		}
	}

	private void QueryThread(object state)
	{
		try
		{
			QueryThreadState queryThreadState = (QueryThreadState)state;
			TimeSpan timeout = TimeSpan.FromMilliseconds(queryThreadState.ConfigurationRefreshIntervalInMs);
			List<IChange> list = new List<IChange>();
			List<Guid> list2 = new List<Guid>();
			List<Guid> list3 = new List<Guid>();
			List<Guid> list4 = new List<Guid>();
			_exclusiveQueryAccess.WaitOne();
			while (queryThreadState.IsLoggedIn)
			{
				queryThreadState.StopEvent.WaitOne(timeout);
				if (!queryThreadState.IsLoggedIn)
				{
					break;
				}
				List<UserContextQueryInfo> list5;
				lock (_instanceLock)
				{
					list5 = new List<UserContextQueryInfo>(_userContextQueryInfos);
				}
				foreach (UserContextQueryInfo item in list5)
				{
					LoadFailoverIntervals(item, queryThreadState.ConfigurationRefreshIntervalInMs);
					DateTime utcNow = DateTime.UtcNow;
					list.Clear();
					list2.Clear();
					list3.Clear();
					SDKConfiguration sDKConfiguration = (SDKConfiguration)item.UserContextToUpdate.Configuration;
					foreach (KeyValuePair<Guid, DateTime> item2 in item.NextCheck)
					{
						Guid key = item2.Key;
						if (!(item2.Value <= utcNow))
						{
							continue;
						}
						try
						{
							if (!queryThreadState.IsLoggedIn)
							{
								return;
							}
							if (sDKConfiguration != null)
							{
								DateTime changesFrom = LastModified(key, sDKConfiguration);
								IChange[] array = sDKConfiguration.QueryChanges(changesFrom, key);
								if (array == null)
								{
									if (!list4.Contains(key))
									{
										list3.Add(key);
									}
									continue;
								}
								list.AddRange(array);
							}
							if (!list4.Contains(key))
							{
								list4.Add(key);
							}
						}
						catch (Exception ex)
						{
							if (ex is FaultException)
							{
								if (!list4.Contains(key))
								{
									list3.Add(key);
								}
								continue;
							}
						}
						list2.Add(key);
					}
					if (!queryThreadState.IsLoggedIn)
					{
						return;
					}
					if (list.Count > 0)
					{
						sDKConfiguration?.OnRecorderInfoChange(list.ToArray());
					}
					utcNow = DateTime.UtcNow;
					foreach (Guid item3 in list2)
					{
						UpdateNextCheckForUserContext(item, item3, utcNow.AddMilliseconds(queryThreadState.ConfigurationRefreshIntervalInMs));
					}
					if (list3.Count > 0)
					{
						foreach (Guid item4 in list3)
						{
							item.NextCheck.Remove(item4);
						}
					}
					DateTime dateTime = DateTime.MaxValue;
					foreach (DateTime value in item.NextCheck.Values)
					{
						if (value < dateTime)
						{
							dateTime = value;
						}
					}
					DateTime utcNow2 = DateTime.UtcNow;
					if (utcNow2 < dateTime)
					{
						timeout = TimeSpan.FromMilliseconds(queryThreadState.ConfigurationRefreshIntervalInMs);
						continue;
					}
					timeout = dateTime.Subtract(utcNow2);
					timeout = timeout.Add(TimeSpan.FromSeconds(1.0));
					if (timeout.Ticks < 0)
					{
						timeout = TimeSpan.FromMilliseconds(queryThreadState.ConfigurationRefreshIntervalInMs);
					}
					else if (timeout.Ticks / 10000 > int.MaxValue)
					{
						timeout = TimeSpan.FromMilliseconds(queryThreadState.ConfigurationRefreshIntervalInMs);
					}
				}
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			EnvironmentManager.Instance.Log(GetType().FullName, error: true, "QueryThread", "Unhandled exception in query thread - shutting down thread: {0}", new object[1] { ex3 });
		}
		finally
		{
			_exclusiveQueryAccess.Set();
		}
	}

	private static void UpdateNextCheckForUserContext(UserContextQueryInfo userContextQueryInfo, Guid systemId, DateTime nextCheckTime)
	{
		if (userContextQueryInfo.NextCheck.ContainsKey(systemId))
		{
			userContextQueryInfo.NextCheck[systemId] = nextCheckTime;
		}
	}

	private static void LoadFailoverIntervals(UserContextQueryInfo userContextQueryInfo, int configurationRefreshIntervalInMs)
	{
		if (userContextQueryInfo.ProcessedByQueryThread)
		{
			return;
		}
		userContextQueryInfo.NextCheck.Clear();
		SDKConfiguration sDKConfiguration = (SDKConfiguration)userContextQueryInfo.UserContextToUpdate.Configuration;
		if (sDKConfiguration != null)
		{
			Dictionary<Guid, TimeSpan> failoverCheckIntervals = sDKConfiguration.GetFailoverCheckIntervals(new TimeSpan(0, 0, 0, 0, configurationRefreshIntervalInMs));
			DateTime utcNow = DateTime.UtcNow;
			foreach (KeyValuePair<Guid, TimeSpan> item in failoverCheckIntervals)
			{
				DateTime value = ((item.Value.Ticks == 0L) ? utcNow.Add(TimeSpan.FromMilliseconds(configurationRefreshIntervalInMs)) : utcNow.Add(item.Value));
				userContextQueryInfo.NextCheck[item.Key] = value;
			}
		}
		userContextQueryInfo.ProcessedByQueryThread = true;
	}

	public bool StartQuery(bool updateLoginState)
	{
		if (updateLoginState)
		{
			_userLoggedIn = true;
		}
		if (Enabled && _userLoggedIn && _queryThreadState == null)
		{
			_queryThreadState = new QueryThreadState
			{
				ConfigurationRefreshIntervalInMs = ConfigurationRefreshIntervalInMs
			};
			_queryThread = new Thread(QueryThread)
			{
				Name = "QueryConfigurationChanges thread",
				IsBackground = true
			};
			_queryThread.Start(_queryThreadState);
			return true;
		}
		return false;
	}

	public void StopQuery(bool updateLoginState)
	{
		if (updateLoginState)
		{
			_userLoggedIn = false;
		}
		if (_queryThreadState != null)
		{
			_queryThreadState.IsLoggedIn = false;
			_queryThreadState.StopEvent.Set();
			_queryThreadState = null;
			if (_queryThread != null)
			{
				_queryThread.Join(10000);
			}
		}
	}

	private static DateTime LastModified(Guid systemId, SDKConfiguration configuration)
	{
		DateTime dateTime = configuration.GetLastHardwareUpdate(systemId);
		List<IRecorder> recorders = configuration.GetRecorders(systemId);
		if (recorders != null)
		{
			foreach (IRecorder item in recorders)
			{
				if (item.LastModified > dateTime)
				{
					dateTime = item.LastModified;
				}
				foreach (IHardwareInfo value in item.Hardware.Values)
				{
					if (value.LastModified > dateTime)
					{
						dateTime = value.LastModified;
					}
				}
			}
		}
		if (dateTime == DateTime.MinValue)
		{
			dateTime = DateTime.UtcNow;
		}
		return dateTime;
	}
}
