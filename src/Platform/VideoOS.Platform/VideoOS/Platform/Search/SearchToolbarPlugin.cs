using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Client;

namespace VideoOS.Platform.Search;

public abstract class SearchToolbarPlugin
{
	public abstract Guid Id { get; protected set; }

	public abstract string Name { get; protected set; }

	public IEnumerable<Guid> SupportedSearchAgentPluginIds { get; protected set; } = Enumerable.Empty<Guid>();

	public ToolbarPluginOverflowMode ToolbarPluginOverflowMode { get; protected set; } = ToolbarPluginOverflowMode.AlwaysInOverflow;

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public abstract SearchToolbarPluginInstance GenerateSearchToolbarPluginInstance();
}
