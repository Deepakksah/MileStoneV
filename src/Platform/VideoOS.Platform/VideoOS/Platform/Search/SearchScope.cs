using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Search;

public abstract class SearchScope
{
	public abstract DateTime From { get; protected set; }

	public abstract DateTime To { get; protected set; }

	public abstract IEnumerable<Item> Items { get; protected set; }

	public event EventHandler Changed;

	protected void FireChangedEvent()
	{
		this.Changed?.Invoke(this, EventArgs.Empty);
	}

	public override bool Equals(object obj)
	{
		if (obj is SearchScope searchScope && From.Equals(searchScope.From) && To.Equals(searchScope.To) && Items.Count() == searchScope.Items.Count())
		{
			if (Items.Except(searchScope.Items).Any())
			{
				return !searchScope.Items.Except(Items).Any();
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (To.GetHashCode() * 397) ^ From.GetHashCode();
	}
}
