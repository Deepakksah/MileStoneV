using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Search.Results;

namespace VideoOS.Platform.Search;

public class SearchResultData
{
	private ICollection<ResultProperty> _cachedResultProperties;

	private SemaphoreSlim _cachedResultPropertiesSemaphore = new SemaphoreSlim(1);

	private readonly IDictionary<DateTime, ICollection<BoundingShape>> _cachedBoundingShapes = new Dictionary<DateTime, ICollection<BoundingShape>>();

	private SemaphoreSlim _cachedBoundingShapesSemaphore = new SemaphoreSlim(1);

	private ICollection<SearchResultDataMetadataObjects> _cachedMetadataObjects;

	private SemaphoreSlim _cachedMetadataObjectsSemaphore = new SemaphoreSlim(1);

	private DateTime _beginTimeWithPadding = DateTime.MinValue;

	private DateTime _endTimeWithPadding = DateTime.MinValue;

	public SearchDefinition SearchDefinition { get; public set; }

	public string Title { get; set; }

	public string WarningText { get; set; }

	public DateTime BeginTimeWithPadding
	{
		get
		{
			if (!(_beginTimeWithPadding == DateTime.MinValue))
			{
				return _beginTimeWithPadding;
			}
			return BeginTime;
		}
		set
		{
			_beginTimeWithPadding = value;
		}
	}

	public DateTime BeginTime { get; set; }

	public DateTime TriggerTime { get; set; }

	public DateTime EndTime { get; set; }

	public DateTime EndTimeWithPadding
	{
		get
		{
			if (!(_endTimeWithPadding == DateTime.MinValue))
			{
				return _endTimeWithPadding;
			}
			return EndTime;
		}
		set
		{
			_endTimeWithPadding = value;
		}
	}

	public Guid Id { get; }

	public Item Item { get; set; }

	public IEnumerable<Item> RelatedItems { get; set; }

	public IEnumerable<SearchFilter> IgnoredSearchFilters { get; set; }

	public Guid SearchResultUserControlType { get; set; } = SearchResultUserControlTypes.Video;

	public SearchResultData(Guid id)
	{
		Id = id;
	}

	public async Task<ICollection<ResultProperty>> GetCachedPropertiesAsync(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			await _cachedResultPropertiesSemaphore.WaitAsync(cancellationToken);
			if (_cachedResultProperties == null)
			{
				ICollection<ResultProperty> collection = await GetPropertiesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!cancellationToken.IsCancellationRequested)
				{
					_cachedResultProperties = collection ?? new Collection<ResultProperty>();
				}
			}
		}
		finally
		{
			_cachedResultPropertiesSemaphore.Release();
		}
		return _cachedResultProperties ?? new Collection<ResultProperty>();
	}

	protected virtual Task<ICollection<ResultProperty>> GetPropertiesAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult((ICollection<ResultProperty>)new Collection<ResultProperty>());
	}

	public async Task<ICollection<BoundingShape>> GetCachedBoundingShapesAsync(DateTime time, CancellationToken cancellationToken)
	{
		await _cachedBoundingShapesSemaphore.WaitAsync(cancellationToken);
		ICollection<BoundingShape> value;
		try
		{
			if (!_cachedBoundingShapes.TryGetValue(time, out value))
			{
				value = await GetBoundingShapesAsync(time, cancellationToken);
				_cachedBoundingShapes.Add(time, value ?? new List<BoundingShape>());
			}
		}
		finally
		{
			_cachedBoundingShapesSemaphore.Release();
		}
		return value;
	}

	protected virtual Task<ICollection<BoundingShape>> GetBoundingShapesAsync(DateTime time, CancellationToken cancellationToken)
	{
		return Task.FromResult((ICollection<BoundingShape>)new Collection<BoundingShape>());
	}

	public async Task<ICollection<SearchResultDataMetadataObjects>> GetCachedMetadataObjectsAsync(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			await _cachedMetadataObjectsSemaphore.WaitAsync(cancellationToken);
			if (_cachedMetadataObjects == null)
			{
				ICollection<SearchResultDataMetadataObjects> collection = await GetMetadataObjectsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!cancellationToken.IsCancellationRequested)
				{
					_cachedMetadataObjects = collection ?? new Collection<SearchResultDataMetadataObjects>();
				}
			}
		}
		finally
		{
			_cachedMetadataObjectsSemaphore.Release();
		}
		return _cachedMetadataObjects ?? new Collection<SearchResultDataMetadataObjects>();
	}

	protected virtual Task<ICollection<SearchResultDataMetadataObjects>> GetMetadataObjectsAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult((ICollection<SearchResultDataMetadataObjects>)new Collection<SearchResultDataMetadataObjects>());
	}

	public void ClearCachedData()
	{
		_cachedResultPropertiesSemaphore.Wait();
		_cachedResultProperties = null;
		_cachedResultPropertiesSemaphore.Release();
		_cachedBoundingShapesSemaphore.Wait();
		_cachedBoundingShapes.Clear();
		_cachedBoundingShapesSemaphore.Release();
		_cachedMetadataObjectsSemaphore.Wait();
		_cachedMetadataObjects = null;
		_cachedMetadataObjectsSemaphore.Release();
	}

	[Obsolete("Previously used to fire the DataChanged event, but this event is no longer used/supported and therefore this method does nothing now.", true)]
	protected void FireDataChangedEvent()
	{
	}
}
