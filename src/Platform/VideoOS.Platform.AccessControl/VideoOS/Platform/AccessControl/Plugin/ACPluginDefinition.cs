using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACPluginDefinition
{
	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public virtual string VersionString => "1.0.0.0";

	public virtual string Manufacturer => "Your Company name goes here";

	public abstract List<ACPlugin> ACPlugins { get; }

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
