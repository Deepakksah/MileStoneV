using System;

namespace VideoOS.Platform.Search;

public abstract class SearchAgentPlugin
{
	public abstract Guid Id { get; protected set; }

	public abstract string Name { get; protected set; }

	public abstract SearchFilterCategory SearchFilterCategory { get; protected set; }

	public abstract SearchDefinition CreateSearchDefinition(SearchScope searchScope);

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
