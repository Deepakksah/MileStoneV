using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.EventServer.Communication.AlarmClient;
using VideoOS.LPR.Common.Communication.Info;
using VideoOS.LPR.Common.Communication.VideoSources;
using VideoOS.LPR.Common.Config;
using VideoOS.LPR.Common.Loggers;
using VideoOS.LPR.Common.Utils;
using VideoOS.LPR.Common.WCF.Client;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.LPR.Common.Communication;

public class ServerController : IDisposable
{
	private readonly ServerId _serverId;

	private readonly PluginCommandClientWrapper _pluginCommandClient;

	private readonly AlarmClient _alarmClient;

	private readonly Logger _auditLogger;

	private Timer _countryModuleTimer = new Timer(5000.0);

	private Timer _videoSourceTimer = new Timer(5000.0);

	private Timer _listsTimer = new Timer(5000.0);

	private Timer _licensePlateStylesTimer = new Timer(5000.0);

	private readonly object _timerLock = new object();

	public Guid SiteId => _serverId.Id;

	public event EventHandler SearchListsResultEvent;

	public ServerController(ServerId serverId, Uri serverAddress)
	{
		_serverId = serverId;
		_pluginCommandClient = new PluginCommandClientWrapper(serverId, serverAddress);
		_alarmClient = new AlarmCommandClientManager().GetAlarmCommandClient(serverId);
		_auditLogger = new Logger(serverId);
		_countryModuleTimer.AutoReset = false;
		_countryModuleTimer.Elapsed += _countryModuleTimer_Elapsed;
		_videoSourceTimer.AutoReset = false;
		_videoSourceTimer.Elapsed += _videoSourceTimer_Elapsed;
		_listsTimer.AutoReset = false;
		_listsTimer.Elapsed += _listsTimer_Elapsed;
		_licensePlateStylesTimer.AutoReset = false;
		_licensePlateStylesTimer.Elapsed += LicensePlateStylesTimerElapsed;
		Task.Run(delegate
		{
			_auditLogger.Initialize();
		});
	}

	private void Start(Timer timer)
	{
		lock (_timerLock)
		{
			timer?.Start();
		}
	}

	private void LicensePlateStylesTimerElapsed(object sender, ElapsedEventArgs e)
	{
		_pluginCommandClient.CallWithRetryAsync((PluginCommandClient client, string token) => client.GetCountryModulesWithLicensePlateStylesAsync(token, new GetCountryModulesWithLicensePlateStylesParams()), "LicensePlateStylesTimerElapsed").ContinueWith(delegate(Task<GetCountryModulesWithLicensePlateStylesResp> t)
		{
			this.SearchListsResultEvent?.Invoke(this, new SearchListsResultEventArgs<GetCountryModulesWithLicensePlateStylesResp>(t, _serverId.Id));
			if (t.IsFaulted)
			{
				t.Exception.Handle((Exception x) => true);
				Start(_licensePlateStylesTimer);
			}
		}, TaskScheduler.Current);
	}

	private void _listsTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		GetListsAsync().ContinueWith(delegate(Task<GetLicensePlateListsResp> t)
		{
			this.SearchListsResultEvent?.Invoke(this, new SearchListsResultEventArgs<GetLicensePlateListsResp>(t, _serverId.Id));
			if (t.IsFaulted)
			{
				t.Exception.Handle((Exception x) => true);
				Start(_listsTimer);
			}
		}, TaskScheduler.Current);
	}

	private void _videoSourceTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		Task.Factory.StartNew(() => GetVideoSourcesUnsafe()).ContinueWith(delegate(Task<GetVideoSourcesResponse> t)
		{
			this.SearchListsResultEvent?.Invoke(this, new SearchListsResultEventArgs<GetVideoSourcesResponse>(t, _serverId.Id));
			if (t.IsFaulted)
			{
				t.Exception.Handle((Exception x) => true);
				Start(_videoSourceTimer);
			}
		}, TaskScheduler.Current);
	}

	private void _countryModuleTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		_pluginCommandClient.CallWithRetryAsync((PluginCommandClient client, string token) => client.GetAllCountryCodesAsync(token, new GetAllCountryCodesParams()), "_countryModuleTimer_Elapsed").ContinueWith(delegate(Task<GetAllCountryCodesResp> t)
		{
			this.SearchListsResultEvent?.Invoke(this, new SearchListsResultEventArgs<GetAllCountryCodesResp>(t, _serverId.Id));
			if (t.IsFaulted)
			{
				t.Exception.Handle((Exception x) => true);
				Start(_countryModuleTimer);
			}
		}, TaskScheduler.Current);
	}

	public void Close()
	{
		_auditLogger.Close();
		_alarmClient.Close();
		_pluginCommandClient.Close();
		Dispose(disposing: true);
	}

	private Task<GetLicensePlateListsResp> GetListsAsync()
	{
		return _pluginCommandClient.CallWithRetry((PluginCommandClient wcfClient, string token) => wcfClient.GetMatchListsAsync(token, new GetLicensePlateListsParams()), "GetListsAsync");
	}

	public Task<LicensePlateInfo[]> SearchListAsync(Guid listId, string listName, string searchFilter, int startIndex, int count)
	{
		return Task.Run(async delegate
		{
			try
			{
				GetLicensePlatesParams parameters = new GetLicensePlatesParams
				{
					ListId = listId,
					SearchFilter = searchFilter,
					StartIndex = startIndex,
					Count = count
				};
				GetLicensePlatesResp getLicensePlatesResp = await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.GetMatchListItemsAsync(token, parameters), "SearchListAsync").ConfigureAwait(continueOnCapturedContext: false);
				if (getLicensePlatesResp != null && (getLicensePlatesResp.LicensePlateInfos != null || getLicensePlatesResp.LicensePlates != null))
				{
					_auditLogger.LogSearchLicensePlateList(listName, searchFilter);
					if (getLicensePlatesResp.LicensePlateInfos != null)
					{
						return getLicensePlatesResp.LicensePlateInfos;
					}
					return getLicensePlatesResp.LicensePlates.Select((LicensePlate x) => new LicensePlateInfo
					{
						LicensePlate = new LicensePlate
						{
							RegistrationNumber = x.RegistrationNumber
						},
						Properties = Array.Empty<LicensePlateProperty>()
					}).ToArray();
				}
			}
			catch (ServiceFaultException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed search list {0} on site {1}. Exception: {2}", listName, _serverId.Id, ex.Message));
				throw;
			}
			return (LicensePlateInfo[])null;
		});
	}

	public Task<int> AddLicensePlatesAsync(Guid listId, string listName, IEnumerable<LicensePlateInfo> licensePlateInfos)
	{
		return Task.Run(async delegate
		{
			try
			{
				LicensePlateInfo[] materializedLicensePlateInfos = licensePlateInfos.ToArray();
				AddLicensePlatesParams parameters = new AddLicensePlatesParams
				{
					ListId = listId,
					LicensePlateInfos = materializedLicensePlateInfos,
					LicensePlates = materializedLicensePlateInfos.Select((LicensePlateInfo x) => x.LicensePlate).ToArray()
				};
				AddLicensePlatesResp addLicensePlatesResp = await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.AddMatchListItemsAsync(token, parameters), "AddLicensePlatesAsync").ConfigureAwait(continueOnCapturedContext: false);
				LicensePlateInfo[] array = materializedLicensePlateInfos;
				foreach (LicensePlateInfo licensePlateInfo in array)
				{
					_auditLogger.LogAddLicensePlate(listName, licensePlateInfo.LicensePlate.RegistrationNumber);
				}
				return addLicensePlatesResp.Count;
			}
			catch (ServiceFaultException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to add plates from list {0} on site {1}. Exception: {2}", listName, _serverId.Id, ex.Message));
				throw;
			}
		});
	}

	public Task<int> DeleteLicensePlatesAsync(Guid listId, string listName, IEnumerable<LicensePlate> plates)
	{
		return Task.Run(async delegate
		{
			try
			{
				LicensePlate[] platesToDelete = plates.ToArray();
				DeleteLicensePlatesParams parameters = new DeleteLicensePlatesParams
				{
					ListId = listId,
					LicensePlates = platesToDelete
				};
				DeleteLicensePlatesResp deleteLicensePlatesResp = await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.DeleteMatchListItemsAsync(token, parameters), "DeleteLicensePlatesAsync").ConfigureAwait(continueOnCapturedContext: false);
				LicensePlate[] array = platesToDelete;
				foreach (LicensePlate licensePlate in array)
				{
					_auditLogger.LogDeleteLicensePlate(listName, licensePlate.RegistrationNumber);
				}
				return deleteLicensePlatesResp.Count;
			}
			catch (ServiceFaultException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to delete plates from list {0} on site {1}. Exception: {2}", listName, _serverId.Id, ex.Message));
				throw;
			}
		});
	}

	public Task<bool> ClearLicensePlatesAsync(Guid listId, string listName)
	{
		return Task.Run(async delegate
		{
			try
			{
				ClearLicensePlatesParams parameters = new ClearLicensePlatesParams
				{
					ListId = listId
				};
				await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.ClearMatchListItemsAsync(token, parameters), "ClearLicensePlatesAsync").ConfigureAwait(continueOnCapturedContext: false);
				_auditLogger.LogClearLicensePlates(listName);
				return true;
			}
			catch (ServiceFaultException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to clear all plates from list {0} on site {1}. Exception: {2}", listName, _serverId.Id, ex.Message));
				throw;
			}
		});
	}

	public Task<int> GetLicensePlateCountAsync(Guid listId, string listName, string searchFilter)
	{
		return Task.Run(async delegate
		{
			try
			{
				GetLicensePlateCountParams parameters = new GetLicensePlateCountParams
				{
					ListId = listId,
					SearchFilter = searchFilter
				};
				return (await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.GetMatchListItemsCountAsync(token, parameters), "GetLicensePlateCountAsync").ConfigureAwait(continueOnCapturedContext: false)).Count;
			}
			catch (ServiceFaultException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to clear all plates from list {0} on site {1}. Exception: {2}", listName, _serverId.Id, ex.Message));
				throw;
			}
		});
	}

	public GetVideoSourcesResponse GetVideoSources()
	{
		try
		{
			return GetVideoSourcesUnsafe();
		}
		catch (ServiceFaultException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to get video sources from {0}. Exception: {1}", _serverId.Id, ex.Message));
		}
		return new GetVideoSourcesResponse
		{
			VideoSourceInfos = new List<VideoSourceInfo>()
		};
	}

	private GetVideoSourcesResponse GetVideoSourcesUnsafe()
	{
		List<VideoSourceInfo> list = new List<VideoSourceInfo>();
		GetVideoSourcesResp getVideoSourcesResp = null;
		try
		{
			getVideoSourcesResp = _pluginCommandClient.CallWithRetry((PluginCommandClient wcfClient, string token) => wcfClient.GetVideoSources(token, new VideoOS.LPR.Common.WCF.DataContracts.GetVideoSourcesParams()), "GetVideoSourcesUnsafe");
		}
		catch (Exception)
		{
		}
		if (getVideoSourcesResp != null && getVideoSourcesResp.VideoSources != null)
		{
			string[] videoSources = getVideoSourcesResp.VideoSources;
			for (int num = 0; num < videoSources.Length; num++)
			{
				VideoSourceItem videoSourceItem = VideoSourceItem.DeserializeItem(videoSources[num]);
				list.Add(new VideoSourceInfo
				{
					Id = videoSourceItem.FQID.ObjectId,
					Name = videoSourceItem.Name,
					SiteId = _serverId.Id,
					SourceCameraId = videoSourceItem.Config.CameraFQID.ObjectId
				});
			}
		}
		return new GetVideoSourcesResponse
		{
			VideoSourceInfos = list
		};
	}

	public void GetSearchLists()
	{
		lock (_timerLock)
		{
			if (_countryModuleTimer != null)
			{
				_countryModuleTimer.Start();
			}
			if (_listsTimer != null)
			{
				_listsTimer.Start();
			}
			if (_videoSourceTimer != null)
			{
				_videoSourceTimer.Start();
			}
			if (_licensePlateStylesTimer != null)
			{
				_licensePlateStylesTimer.Start();
			}
		}
	}

	public Task<SearchResult> SearchLPREvents(SearchInfo searchInfo)
	{
		TaskCompletionSource<SearchResult> taskCompletionSource = new TaskCompletionSource<SearchResult>();
		Task.Run(delegate
		{
			Task<GetEventLines2Resp>.Factory.FromAsync(_alarmClient.BeginGetEventLines2(new GetEventLines2Params
			{
				SearchId = searchInfo.SearchId,
				SearchGroup = searchInfo.SearchGroup,
				SearchOrders = searchInfo.SearchOrders,
				Page = searchInfo.Page,
				PageSize = searchInfo.PageSize,
				TruePaging = false
			}, null, null), _alarmClient.EndGetEventLines2).ContinueWith(delegate(Task<GetEventLines2Resp> t)
			{
				if (!t.IsFaulted)
				{
					CompensateForDbTimestampResolution(t.Result.Result);
					taskCompletionSource.SetResult(new SearchResult(t.Result.Result, t.Result.SearchId, _serverId.Id));
					_auditLogger.LogSearchLicensePlateEvents(searchInfo.GenerateAuditLogString());
				}
				else
				{
					LogException(t.Exception);
					t.Exception.Handle((Exception x) => true);
					taskCompletionSource.SetResult(new SearchResult(null, searchInfo.SearchId, _serverId.Id)
					{
						IsFaulted = true
					});
				}
			}, TaskScheduler.Current);
		}).ContinueWith(delegate(Task t)
		{
			if (t.Exception != null)
			{
				LogException(t.Exception);
			}
			t.Exception.Handle((Exception x) => true);
			taskCompletionSource.SetResult(new SearchResult(Array.Empty<EventLine>(), searchInfo.SearchId, _serverId.Id));
		}, TaskContinuationOptions.OnlyOnFaulted);
		return taskCompletionSource.Task;
	}

	public Task<int> GetLicensePlateEventCountAsync(VideoOS.EventServer.Communication.AlarmClient.SearchGroup searchGroup)
	{
		TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
		GetEventCountParams parameters = new GetEventCountParams
		{
			SearchGroup = searchGroup
		};
		Task<GetEventCountResp>.Factory.FromAsync(_alarmClient.BeginGetEventCount(parameters, null, null), _alarmClient.EndGetEventCount).ContinueWith(delegate(Task<GetEventCountResp> t)
		{
			if (!t.IsFaulted)
			{
				if (t.Result != null)
				{
					taskCompletionSource.SetResult(t.Result.Count);
				}
				else
				{
					taskCompletionSource.SetResult(0);
				}
			}
			else
			{
				LogException(t.Exception);
				t.Exception.Handle((Exception x) => true);
				if (t.Exception.InnerException != null)
				{
					taskCompletionSource.SetException(t.Exception.InnerException);
				}
				else
				{
					taskCompletionSource.SetException(t.Exception);
				}
			}
		}, TaskScheduler.Current);
		return taskCompletionSource.Task;
	}

	public Task<BaseEvent[]> GetEventsAsync(Guid[] eventIds)
	{
		TaskCompletionSource<BaseEvent[]> taskCompletionSource = new TaskCompletionSource<BaseEvent[]>();
		GetEventsParams parameters = new GetEventsParams
		{
			EventIds = eventIds
		};
		Task<GetEventsResp>.Factory.FromAsync(_alarmClient.BeginGetEvents(parameters, null, null), _alarmClient.EndGetEvents).ContinueWith(delegate(Task<GetEventsResp> t)
		{
			try
			{
				taskCompletionSource.SetResult(t.Result.Events);
			}
			catch (AggregateException ex)
			{
				LogException(ex.InnerException);
				taskCompletionSource.SetException(ex.InnerException);
			}
			catch (Exception ex2)
			{
				LogException(ex2);
				taskCompletionSource.SetException(ex2);
			}
		}, TaskScheduler.Current);
		return taskCompletionSource.Task;
	}

	public async Task<List<KeyValuePair<string, string>>> LookupLicensePlatesAsync(string registrationNumber, MatchListItemType matchListItemType, Guid listId)
	{
		List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>();
		LookupLicensePlateInfosParams parameters = new LookupLicensePlateInfosParams
		{
			LicensePlateLookups = new LicensePlateLookupGroup[1]
			{
				new LicensePlateLookupGroup
				{
					ListId = listId,
					LicensePlates = new LicensePlate[1]
					{
						new LicensePlate
						{
							RegistrationNumber = registrationNumber,
							MatchListItemType = matchListItemType
						}
					}
				}
			}
		};
		try
		{
			LicensePlateLookupResultGroup licensePlateLookupResultGroup = null;
			try
			{
				licensePlateLookupResultGroup = (await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient wcfClient, string token) => wcfClient.LookupMatchListItemInfosAsync(token, parameters), "LookupLicensePlatesAsync").ConfigureAwait(continueOnCapturedContext: false)).LicensePlateLookupResultGroups.FirstOrDefault((LicensePlateLookupResultGroup x) => x.LicensePlateList.Id == listId);
			}
			catch (ServiceFaultException ex)
			{
				if (!(ex.InnerException is ActionNotSupportedException))
				{
					throw;
				}
			}
			if (licensePlateLookupResultGroup != null)
			{
				IEnumerable<LicensePlateProperty> licensePlateProperties = null;
				if (licensePlateLookupResultGroup.LicensePlateInfos.Length != 0)
				{
					licensePlateProperties = licensePlateLookupResultGroup.LicensePlateInfos.First().Properties;
				}
				properties = ListPropertiesHelper.BuildPropertyKeyValuePairsForUI(licensePlateLookupResultGroup.LicensePlateList.PropertyDefinitions, licensePlateProperties);
			}
		}
		catch (Exception ex2)
		{
			LogException(ex2);
		}
		return properties;
	}

	public Task<GetLicensePlateEventsResult> GetLicensePlateEventsAsync(SearchInfo searchInfo)
	{
		TaskCompletionSource<GetLicensePlateEventsResult> tcs = new TaskCompletionSource<GetLicensePlateEventsResult>();
		GetEventLines2Params parameters = new GetEventLines2Params
		{
			SearchId = searchInfo.SearchId,
			SearchGroup = searchInfo.SearchGroup,
			SearchOrders = searchInfo.SearchOrders,
			Page = searchInfo.Page,
			PageSize = searchInfo.PageSize,
			TruePaging = true
		};
		Task<GetEventLines2Resp>.Factory.FromAsync(_alarmClient.BeginGetEventLines2(parameters, null, null), _alarmClient.EndGetEventLines2).ContinueWith((Func<Task<GetEventLines2Resp>, Task>)async delegate(Task<GetEventLines2Resp> task)
		{
			_ = 3;
			try
			{
				List<LicensePlateEvent> licensePlateEvents = new List<LicensePlateEvent>();
				GetEventLines2Resp eventLines = await task.ConfigureAwait(continueOnCapturedContext: false);
				if (eventLines != null && eventLines.Result != null)
				{
					CompensateForDbTimestampResolution(eventLines.Result);
					if (eventLines.Result.Any((EventLine eventLine2) => eventLine2.MessageId == Guid.Empty))
					{
						Dictionary<string, Guid> dictionary = (await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient client, string token) => client.GetMatchListsAsync(token, new GetLicensePlateListsParams()), "GetLicensePlateEventsAsync").ConfigureAwait(continueOnCapturedContext: false)).LicensePlateLists.ToDictionary((LicensePlateList l) => l.Name, (LicensePlateList l) => l.Id);
						foreach (EventLine item in eventLines.Result.Where((EventLine eventLine2) => eventLine2.MessageId == Guid.Empty))
						{
							if (dictionary.TryGetValue(item.Message, out var value))
							{
								item.MessageId = value;
							}
						}
					}
					Dictionary<Guid, LicensePlate[]> source = (from eventLine2 in eventLines.Result
						group eventLine2 by eventLine2.MessageId).ToDictionary((IGrouping<Guid, EventLine> group) => group.Key, (IGrouping<Guid, EventLine> group) => group.Select((EventLine eventLine2) => new LicensePlate
					{
						RegistrationNumber = eventLine2.ObjectValue
					}).ToArray());
					LookupLicensePlateInfosParams lookupPlateParams = new LookupLicensePlateInfosParams
					{
						LicensePlateLookups = source.Select((KeyValuePair<Guid, LicensePlate[]> l) => new LicensePlateLookupGroup
						{
							ListId = l.Key,
							LicensePlates = l.Value
						}).ToArray()
					};
					Dictionary<Tuple<string, Guid>, Tuple<LicensePlateInfo, LicensePlateList>> platesAndLists = null;
					try
					{
						platesAndLists = (await _pluginCommandClient.CallWithRetryAsync((PluginCommandClient client, string token) => client.LookupMatchListItemInfosAsync(token, lookupPlateParams), "GetLicensePlateEventsAsync").ConfigureAwait(continueOnCapturedContext: false)).LicensePlateLookupResultGroups.SelectMany((LicensePlateLookupResultGroup g) => g.LicensePlateInfos.Select((LicensePlateInfo p) => new Tuple<LicensePlateInfo, LicensePlateList>(p, g.LicensePlateList))).ToDictionary((Tuple<LicensePlateInfo, LicensePlateList> t) => new Tuple<string, Guid>(t.Item1.LicensePlate.RegistrationNumber, t.Item2.Id));
					}
					catch (ServiceFaultException ex)
					{
						if (!(ex.InnerException is ActionNotSupportedException))
						{
							throw;
						}
					}
					Guid[] eventIds = eventLines.Result.Select((EventLine l) => l.Id).ToArray();
					Dictionary<Guid, BaseEvent> fullEvents = null;
					try
					{
						fullEvents = (await GetEventsAsync(eventIds).ConfigureAwait(continueOnCapturedContext: false)).ToDictionary((BaseEvent f) => f.EventHeader.ID);
					}
					catch (ActionNotSupportedException)
					{
					}
					EventLine[] result = eventLines.Result;
					foreach (EventLine eventLine in result)
					{
						Guid messageId = eventLine.MessageId;
						BaseEvent value2 = null;
						AnalyticsEvent analyticsEvent = null;
						if (fullEvents != null && fullEvents.TryGetValue(eventLine.Id, out value2))
						{
							analyticsEvent = value2 as AnalyticsEvent;
						}
						LicensePlateInfo licensePlateInfo = null;
						LicensePlateList licensePlateList = null;
						if (platesAndLists != null && platesAndLists.TryGetValue(new Tuple<string, Guid>(eventLine.ObjectValue, messageId), out var value3))
						{
							licensePlateInfo = value3.Item1;
							licensePlateList = value3.Item2;
						}
						licensePlateEvents.Add(new LicensePlateEvent(eventLine, licensePlateInfo, licensePlateList, analyticsEvent));
					}
				}
				if (!eventLines.TruePaging)
				{
					licensePlateEvents = licensePlateEvents.Skip(searchInfo.Page * searchInfo.PageSize).ToList();
				}
				tcs.SetResult(new GetLicensePlateEventsResult(_serverId.Id, licensePlateEvents, errorOccurred: false));
			}
			catch (Exception ex3)
			{
				LogException(ex3);
				tcs.SetResult(new GetLicensePlateEventsResult(_serverId.Id, null, errorOccurred: true));
			}
		}, TaskScheduler.Current);
		return tcs.Task;
	}

	private static void CompensateForDbTimestampResolution(IEnumerable<EventLine> eventLines)
	{
		if (eventLines == null)
		{
			return;
		}
		foreach (EventLine eventLine in eventLines)
		{
			eventLine.Timestamp = eventLine.Timestamp.AddMilliseconds(3.0);
		}
	}

	private static void LogException(Exception ex)
	{
		string text = string.Empty;
		Exception ex2 = ex;
		while (true)
		{
			text = text + "Message = " + ex2.Message + " ";
			if (ex2.InnerException == null)
			{
				break;
			}
			ex2 = ex2.InnerException;
		}
		EnvironmentManager.Instance.Log(error: true, "LPR.ServerController", string.Format(CultureInfo.InvariantCulture, "Failed to search LPR events, Exception: {0}", text));
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		lock (_timerLock)
		{
			if (_countryModuleTimer != null)
			{
				_countryModuleTimer.Elapsed -= _countryModuleTimer_Elapsed;
				_countryModuleTimer.Close();
				_countryModuleTimer.Dispose();
				_countryModuleTimer = null;
			}
			if (_videoSourceTimer != null)
			{
				_videoSourceTimer.Elapsed -= _videoSourceTimer_Elapsed;
				_videoSourceTimer.Close();
				_videoSourceTimer.Dispose();
				_videoSourceTimer = null;
			}
			if (_listsTimer != null)
			{
				_listsTimer.Elapsed -= _listsTimer_Elapsed;
				_listsTimer.Close();
				_listsTimer.Dispose();
				_listsTimer = null;
			}
			if (_licensePlateStylesTimer != null)
			{
				_licensePlateStylesTimer.Elapsed -= LicensePlateStylesTimerElapsed;
				_licensePlateStylesTimer.Close();
				_licensePlateStylesTimer.Dispose();
				_licensePlateStylesTimer = null;
			}
		}
	}
}
