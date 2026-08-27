using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.Incident;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Filtering;
using VideoOS.IM.Common.GlobalDefinitions.Utilities;
using VideoOS.IM.Common.Service.Abstractions;
using VideoOS.IM.Common.Service.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.RetentionTimes.Decorators;
using VideoOS.IM.Server.Service.Time;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.RetentionTimes;

public sealed class IncidentRetentionTimeCleanupService : IIncidentRetentionTimeCleanupService
{
	[NotNull]
	private ADtoDAL _dDtoDAL;

	[NotNull]
	private IRetentionTimeIncidentDecorator _retentionTimeIncidentDecorator;

	[NotNull]
	private IDtoGlobalSettingsManager _dtoGlobalSettingsManager;

	[NotNull]
	private ITimeProvider _timeProvider;

	[NotNull]
	private TimeSpan _interval;

	[NotNull]
	private readonly CancellationTokenSource _isStopped = new CancellationTokenSource();

	private DateTime _startingIncidentRetentionTime;

	[NotNull]
	private readonly ReaderWriterLockSlim _initializedLock = new ReaderWriterLockSlim();

	[NotNull]
	private readonly ReaderWriterLockSlim _incidentRetentionTimeRunningLock = new ReaderWriterLockSlim();

	private bool _isInitialized;

	private bool _isIncidentRetentionTimeRunning;

	private static readonly Lazy<IIncidentRetentionTimeCleanupService> LazyInstance = new Lazy<IIncidentRetentionTimeCleanupService>(() => new IncidentRetentionTimeCleanupService());

	public static IIncidentRetentionTimeCleanupService Instance => LazyInstance.Value;

	private int DaysToKeepIncidentsFromDb
	{
		get
		{
			_dDtoDAL.RefreshGlobalSettingsManager();
			if (_dtoGlobalSettingsManager?.ManagerOptionInstance == null)
			{
				return Constants.DaysToKeepIncidentsDefault;
			}
			if (_dtoGlobalSettingsManager.ManagerOptionInstance.IncidentExtendedPropsConfig.DaysToKeepIncidents.HasValue)
			{
				return _dtoGlobalSettingsManager.ManagerOptionInstance.IncidentExtendedPropsConfig.DaysToKeepIncidents.Value;
			}
			return Constants.DaysToKeepIncidentsDefault;
		}
	}

	public bool IsInitialized
	{
		get
		{
			_initializedLock.EnterReadLock();
			try
			{
				return _isInitialized;
			}
			finally
			{
				_initializedLock.ExitReadLock();
			}
		}
		private set
		{
			_initializedLock.EnterWriteLock();
			try
			{
				_isInitialized = value;
			}
			finally
			{
				_initializedLock.ExitWriteLock();
			}
		}
	}

	public bool IsIncidentRetentionTimeRunning
	{
		get
		{
			_incidentRetentionTimeRunningLock.EnterReadLock();
			try
			{
				if (!IsInitialized)
				{
					throw new InvalidOperationException("InitApplyIncidentRetentionAfter should be called first.");
				}
				return _isIncidentRetentionTimeRunning;
			}
			finally
			{
				_incidentRetentionTimeRunningLock.ExitReadLock();
			}
		}
		private set
		{
			_incidentRetentionTimeRunningLock.EnterWriteLock();
			try
			{
				_isIncidentRetentionTimeRunning = value;
			}
			finally
			{
				_incidentRetentionTimeRunningLock.ExitWriteLock();
			}
		}
	}

	public IncidentRetentionTimeCleanupService()
	{
		_dDtoDAL = (ADtoDAL)GlobalWebApiAppDALInstanceSelector.GetDtoDALImplementationForBackground();
	}

	public void InitApplyIncidentRetentionAfter([NotNull] IRetentionTimeIncidentDecorator retentionTimeIncidentDecorator, [NotNull] ITimeProvider timeProvider, [NotNull] IDtoGlobalSettingsManager dtoGlobalSettingsManager)
	{
		if (IsInitialized)
		{
			throw new InvalidOperationException("InitApplyIncidentRetentionAfter has already been called..");
		}
		IsInitialized = true;
		retentionTimeIncidentDecorator.NullCheck("", "InitApplyIncidentRetentionAfter", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\RetentionTimes\\IncidentRetentionTimeCleanupService.cs", 144);
		dtoGlobalSettingsManager.NullCheck("", "InitApplyIncidentRetentionAfter", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\RetentionTimes\\IncidentRetentionTimeCleanupService.cs", 145);
		timeProvider.NullCheck("", "InitApplyIncidentRetentionAfter", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\RetentionTimes\\IncidentRetentionTimeCleanupService.cs", 146);
		_retentionTimeIncidentDecorator = retentionTimeIncidentDecorator;
		_dtoGlobalSettingsManager = dtoGlobalSettingsManager;
		_timeProvider = timeProvider;
		_startingIncidentRetentionTime = _timeProvider.LocalTime;
		if (_dtoGlobalSettingsManager.ManagerOptionInstance != null)
		{
			if (!_dtoGlobalSettingsManager.ManagerOptionInstance.IncidentExtendedPropsConfig.StartingIncidentRetentionTime.HasValue)
			{
				_dtoGlobalSettingsManager.ManagerOptionInstance.IncidentExtendedPropsConfig.StartingIncidentRetentionTime = _startingIncidentRetentionTime;
				_dDtoDAL.SaveChangedGlobalSettingsToDb(_dtoGlobalSettingsManager?.ManagerOptionInstance);
			}
			else
			{
				_startingIncidentRetentionTime = _dtoGlobalSettingsManager.ManagerOptionInstance.IncidentExtendedPropsConfig.StartingIncidentRetentionTime.Value;
			}
		}
	}

	public Task StartApplyingRetentionTimeCleanupAsync(TimeSpan? interval = null)
	{
		if (!IsInitialized)
		{
			throw new InvalidOperationException("InitApplyIncidentRetentionAfter should be called first.");
		}
		if (IsIncidentRetentionTimeRunning)
		{
			throw new InvalidOperationException("StartApplyingRetentionTimeCleanupAsync is currently running. 'StopApplyingRetentionTimeCleanup' must be called before trying to starting the process again");
		}
		IsIncidentRetentionTimeRunning = true;
		_interval = interval ?? new TimeSpan(1, 0, 0, 0);
		return Task.Run((Func<Task>)DeleteIncidentsBasedOnRetentionTimeBackgroundAsync);
	}

	public void StopApplyingRetentionTimeCleanup()
	{
		_isStopped.Cancel();
	}

	private async Task DeleteIncidentsBasedOnRetentionTimeBackgroundAsync()
	{
		while (!_isStopped.IsCancellationRequested)
		{
			EstablishNewConnectionWithDb();
			IEnumerable<long> source = await GetIncidentsIdToBeDeletedAsync();
			await _dDtoDAL.GetIncidentDAL().DeleteRangeAsync(source.ToHashSet(), _isStopped.Token);
			await Task.Delay(_interval);
		}
		IsIncidentRetentionTimeRunning = false;
	}

	private void EstablishNewConnectionWithDb()
	{
		_dDtoDAL = (ADtoDAL)GlobalWebApiAppDALInstanceSelector.GetDtoDALImplementationForBackground();
	}

	private async Task<IEnumerable<long>> GetIncidentsIdToBeDeletedAsync()
	{
		List<DtoIncident> list = (await GetIncidentsWhereRetentionTimeCanBeAppliedAsync()).ToList();
		_retentionTimeIncidentDecorator.Decorate(list, DaysToKeepIncidentsFromDb);
		return from x in list
			where x.TimeCreated.HasValue && x.TimeCreated <= _timeProvider.LocalTime
			select x.Id;
	}

	private async Task<IEnumerable<DtoIncident>> GetIncidentsWhereRetentionTimeCanBeAppliedAsync()
	{
		DtoPaging incidentFromStartingRetentionTimeFilter = GetIncidentFromStartingRetentionTimeFilter();
		DtoIncidentListResponse dtoIncidentListResponse = await _dDtoDAL.GetIncidentDAL().GetListAsync(incidentFromStartingRetentionTimeFilter, _isStopped.Token);
		if (dtoIncidentListResponse != null && dtoIncidentListResponse.Success)
		{
			return dtoIncidentListResponse.Value;
		}
		return Enumerable.Empty<DtoIncident>();
	}

	private DtoPaging GetIncidentFromStartingRetentionTimeFilter()
	{
		return new DtoPaging
		{
			CountPerPage = int.MaxValue,
			Filters = new Dictionary<string, string> { 
			{
				"TimeCreated",
				_startingIncidentRetentionTime.ToString(CultureInfo.InvariantCulture)
			} },
			FiltersType = new Dictionary<string, string> { 
			{
				"TimeCreated",
				EFilterTypes.gtoe.ToString()
			} }
		};
	}
}
