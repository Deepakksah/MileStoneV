using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Platform.Admin;

public abstract class TabPlugin
{
	public abstract Guid Id { get; }

	public virtual Image Icon => null;

	public abstract string Name { get; }

	public abstract Guid AssociatedKind { get; }

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public abstract TabUserControl GenerateUserControl(Item associatedItem);

	public abstract bool IsVisible(Item associatedItem);

	public virtual Dictionary<string, string> GetConfigurationReportProperties(Item item)
	{
		return null;
	}
}
