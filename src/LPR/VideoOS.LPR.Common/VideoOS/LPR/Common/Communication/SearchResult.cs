using System;
using System.Collections.Generic;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.LPR.Common.Communication;

public class SearchResult
{
	private EventLine[] _result;

	private Guid _searchId;

	private Guid _siteId;

	private bool _isFaulted;

	public ICollection<EventLine> Result => _result;

	public Guid SearchId => _searchId;

	public Guid SiteId => _siteId;

	public bool IsFaulted
	{
		get
		{
			return _isFaulted;
		}
		set
		{
			_isFaulted = value;
		}
	}

	public SearchResult(EventLine[] result, Guid searchId, Guid siteId)
	{
		_result = result;
		_searchId = searchId;
		_siteId = siteId;
	}
}
