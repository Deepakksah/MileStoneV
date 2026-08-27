using System;
using System.Collections.Generic;
using VideoOS.Platform.Search.FilterConfigurations;

namespace VideoOS.Platform.Search;

public abstract class SearchUserControlsPlugin
{
	public abstract Guid Id { get; protected set; }

	public abstract string Name { get; protected set; }

	public abstract IEnumerable<Guid> SearchResultUserControlTypes { get; protected set; }

	public abstract IEnumerable<Type> SearchFilterConfigurationTypes { get; protected set; }

	public virtual SearchResultUserControl CreateSearchResultUserControl(Guid searchResultUserControlType)
	{
		return null;
	}

	public virtual SearchFilterEditControl CreateSearchFilterEditControl(FilterConfigurationBase filterConfiguration)
	{
		return null;
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
