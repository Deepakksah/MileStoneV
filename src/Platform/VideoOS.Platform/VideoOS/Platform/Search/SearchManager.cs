using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Search.Errors;

namespace VideoOS.Platform.Search;

public class SearchManager : ISearchManager, IDisposable
{
	private class JobData : IDisposable
	{
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		private readonly ManualResetEvent _continueEvent = new ManualResetEvent(initialState: false);

		private EventHandler<SearchAgentResultReadyEventArgs> _searchResultReadyEventHandler;

		private EventHandler<SearchAgentErrorEventArgs> _searchErrorEventHandler;

		private bool _disposed;

		public CancellationTokenSource CancellationTokenSource => _cancellationTokenSource;

		public ManualResetEvent ContinueEvent => _continueEvent;

		public SearchDefinition SearchDefinition { get; }

		public Guid SessionId { get; set; }

		public DateTime From { get; set; }

		public DateTime To { get; set; }

		public IEnumerable<Item> Items { get; set; }

		public SearchSortOrder SortOrder { get; set; }

		public EventHandler<SearchAgentResultReadyEventArgs> SearchResultReadyEventHandler
		{
			set
			{
				if (!object.Equals(_searchResultReadyEventHandler, value))
				{
					if (_searchResultReadyEventHandler != null)
					{
						SearchDefinition.SearchResultReady -= _searchResultReadyEventHandler;
					}
					_searchResultReadyEventHandler = value;
					if (_searchResultReadyEventHandler != null)
					{
						SearchDefinition.SearchResultReady += _searchResultReadyEventHandler;
					}
				}
			}
		}

		public EventHandler<SearchAgentErrorEventArgs> SearchErrorEventHandler
		{
			set
			{
				if (!object.Equals(_searchErrorEventHandler, value))
				{
					if (_searchErrorEventHandler != null)
					{
						SearchDefinition.ErrorOccurred -= _searchErrorEventHandler;
					}
					_searchErrorEventHandler = value;
					if (_searchErrorEventHandler != null)
					{
						SearchDefinition.ErrorOccurred += _searchErrorEventHandler;
					}
				}
			}
		}

		public JobData(SearchDefinition searchDefinition)
		{
			if (searchDefinition == null)
			{
				throw new ArgumentNullException("searchDefinition");
			}
			SearchDefinition = searchDefinition;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				_disposed = true;
				_cancellationTokenSource.Dispose();
				_continueEvent.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	private readonly List<SearchAgentPlugin> _searchAgentPlugins = new List<SearchAgentPlugin>();

	private readonly ConcurrentDictionary<SearchDefinition, JobData> _runningJobs = new ConcurrentDictionary<SearchDefinition, JobData>();

	private readonly ConcurrentDictionary<Task, JobData> _runningTasks = new ConcurrentDictionary<Task, JobData>();

	private readonly TimeSpan START_TASK_TIMEOUT = TimeSpan.FromMinutes(10.0);

	private bool _disposed;

	public ReadOnlyCollection<SearchFilterCategory> SearchFilterCategories { get; }

	public event EventHandler<SearchResultReadyEventArgs> SearchResultReady;

	public event EventHandler<SearchErrorEventArgs> ErrorOccurred;

	public event EventHandler<SearchDoneEventArgs> SearchDone;

	public SearchManager()
	{
		foreach (PluginDefinition allPluginDefinition in EnvironmentManager.Instance.AllPluginDefinitions)
		{
			if (allPluginDefinition.SearchAgentPlugins != null)
			{
				_searchAgentPlugins.AddRange(allPluginDefinition.SearchAgentPlugins.Where((SearchAgentPlugin x) => x != null));
			}
		}
		SearchFilterCategories = GetSearchFilterCategoriesFromPlugins();
	}

	public SearchManager(IEnumerable<SearchAgentPlugin> searchAgentPlugins)
	{
		if (searchAgentPlugins == null)
		{
			throw new ArgumentNullException("searchAgentPlugins");
		}
		_searchAgentPlugins.AddRange(searchAgentPlugins.Where((SearchAgentPlugin x) => x != null).ToList());
		SearchFilterCategories = GetSearchFilterCategoriesFromPlugins();
	}

	private ReadOnlyCollection<SearchFilterCategory> GetSearchFilterCategoriesFromPlugins()
	{
		List<SearchFilterCategory> list = new List<SearchFilterCategory>();
		foreach (SearchAgentPlugin searchAgentPlugin in _searchAgentPlugins)
		{
			try
			{
				if (searchAgentPlugin.SearchFilterCategory != null)
				{
					searchAgentPlugin.SearchFilterCategory.SearchAgentPluginId = searchAgentPlugin.Id;
					list.Add(searchAgentPlugin.SearchFilterCategory);
				}
			}
			catch (Exception ex)
			{
				string text = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", GetType().Name, "SearchManager");
				EnvironmentManager instance = EnvironmentManager.Instance;
				object[] args = new string[2] { searchAgentPlugin.Name, ex.Message };
				instance.Log(error: true, text, "Error getting SearchFilterCategories from SearchAgentPlugin=\"{0}\" with exception=\"{1}\"", args);
			}
		}
		return new ReadOnlyCollection<SearchFilterCategory>(list);
	}

	public SearchDefinition CreateSearchDefinition(SearchFilterCategory searchFilterCategory, SearchScope searchScope)
	{
		SearchAgentPlugin searchAgentPlugin = _searchAgentPlugins.Single((SearchAgentPlugin x) => x.Id == searchFilterCategory.SearchAgentPluginId);
		SearchDefinition searchDefinition = searchAgentPlugin.CreateSearchDefinition(searchScope);
		searchDefinition.SearchAgentPluginId = searchAgentPlugin.Id;
		return searchDefinition;
	}

	public void DestroySearchDefinition(SearchDefinition searchDefinition)
	{
		if (searchDefinition == null)
		{
			throw new ArgumentNullException("searchDefinition");
		}
		searchDefinition.SearchCriteria.Close();
		searchDefinition.Close();
	}

	public void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items)
	{
		StartSearch(definition, from, to, items, SearchSortOrder.Unspecified, Guid.NewGuid());
	}

	public void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, Guid sessionId)
	{
		StartSearch(definition, from, to, items, SearchSortOrder.Unspecified, sessionId);
	}

	public void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder)
	{
		StartSearch(definition, from, to, items, sortOrder, Guid.NewGuid());
	}

	public void StartSearch(SearchDefinition definition, DateTime from, DateTime to, IEnumerable<Item> items, SearchSortOrder sortOrder, Guid sessionId)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		try
		{
			CancelSearch(new SearchDefinition[1] { definition });
			JobData jobData = new JobData(definition)
			{
				SessionId = sessionId,
				From = from,
				To = to,
				Items = items,
				SortOrder = sortOrder
			};
			Task task = Task.Factory.StartNew(delegate(object state)
			{
				JobData jobData2 = (JobData)state;
				jobData2.SearchResultReadyEventHandler = delegate(object s, SearchAgentResultReadyEventArgs e)
				{
					FirePartialResultReady(e.SearchResults, (SearchDefinition)s, e.SessionId);
				};
				jobData2.SearchErrorEventHandler = delegate(object s, SearchAgentErrorEventArgs e)
				{
					FireErrorOccurred(e.SearchError, (SearchDefinition)s, e.SessionId);
				};
				try
				{
					jobData.ContinueEvent.WaitOne(START_TASK_TIMEOUT);
					jobData2.SearchDefinition.CallSearch(jobData2.SessionId, jobData2.From, jobData2.To, jobData2.Items, jobData2.SortOrder, jobData2.CancellationTokenSource.Token);
				}
				finally
				{
					jobData2.SearchResultReadyEventHandler = null;
					jobData2.SearchErrorEventHandler = null;
					FireSearchDone(jobData2.SearchDefinition, jobData2.SessionId);
				}
			}, jobData, jobData.CancellationTokenSource.Token);
			task.ContinueWith(delegate(Task previousTask)
			{
				if (_runningTasks.TryRemove(previousTask, out var value))
				{
					if (previousTask.IsFaulted)
					{
						FireErrorOccurred(new UnknownSearchError(previousTask.Exception.InnerException), value.SearchDefinition, value.SessionId);
					}
					_runningJobs.TryRemove(value.SearchDefinition, value);
					value.Dispose();
				}
			}, jobData.CancellationTokenSource.Token);
			_runningTasks.TryAdd(task, jobData);
			_runningJobs.AddOrUpdate(definition, jobData, (SearchDefinition def, JobData oldJob) => jobData);
			jobData.ContinueEvent.Set();
		}
		catch (Exception innerException)
		{
			throw new MIPException("SearchManager:StartSearch failed.", innerException);
		}
	}

	protected virtual void FirePartialResultReady(IEnumerable<SearchResultData> searchResults, SearchDefinition searchDefinition, Guid sessionId)
	{
		if (searchResults == null)
		{
			throw new ArgumentNullException("searchResults");
		}
		if (searchDefinition == null)
		{
			throw new ArgumentNullException("searchDefinition");
		}
		if (_runningJobs.TryGetValue(searchDefinition, out var value) && value.SessionId == sessionId && !value.CancellationTokenSource.IsCancellationRequested)
		{
			SearchResultReadyEventArgs e = new SearchResultReadyEventArgs
			{
				SearchResults = searchResults,
				SearchDefinition = searchDefinition,
				SessionId = sessionId
			};
			this.SearchResultReady?.Invoke(this, e);
		}
	}

	protected virtual void FireErrorOccurred(SearchError searchError, SearchDefinition searchDefinition, Guid sessionId)
	{
		if (searchDefinition == null)
		{
			throw new ArgumentNullException("searchDefinition");
		}
		if (_runningJobs.TryGetValue(searchDefinition, out var value) && value.SessionId == sessionId && !value.CancellationTokenSource.IsCancellationRequested)
		{
			this.ErrorOccurred?.Invoke(this, new SearchErrorEventArgs
			{
				SearchError = searchError,
				SearchDefinition = searchDefinition,
				SessionId = sessionId
			});
		}
	}

	protected virtual void FireSearchDone(SearchDefinition searchDefinition, Guid sessionId)
	{
		if (searchDefinition == null)
		{
			throw new ArgumentNullException("searchDefinition");
		}
		if (_runningJobs.TryGetValue(searchDefinition, out var value) && value.SessionId == sessionId && !value.CancellationTokenSource.IsCancellationRequested)
		{
			this.SearchDone?.Invoke(this, new SearchDoneEventArgs
			{
				SearchDefinition = value.SearchDefinition
			});
		}
	}

	public bool CancelSearch(IEnumerable<SearchDefinition> definitions)
	{
		if (definitions == null)
		{
			throw new ArgumentNullException("definitions");
		}
		bool flag = false;
		foreach (SearchDefinition definition in definitions)
		{
			if (_runningJobs.TryRemove(definition, out var value))
			{
				value.SearchResultReadyEventHandler = null;
				value.SearchErrorEventHandler = null;
				value.CancellationTokenSource.Cancel();
				flag = (byte)((flag ? 1u : 0u) | 1u) != 0;
				this.SearchDone?.Invoke(this, new SearchDoneEventArgs
				{
					SearchDefinition = value.SearchDefinition
				});
			}
		}
		return flag;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
