using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class RecorderPluginData
{
	public Guid RecorderPluginId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public static RecorderPluginData QueryPluginById(IGeminiRequestContext requestContext, string id)
	{
		using IRecorderPluginDataComponent recorderPluginDataComponent = requestContext.CreateRecorderPluginDataComponent();
		return recorderPluginDataComponent.QueryPluginById(id);
	}

	public static IEnumerable<RecorderPluginData> QueryPlugins(IGeminiRequestContext requestContext)
	{
		using IRecorderPluginDataComponent recorderPluginDataComponent = requestContext.CreateRecorderPluginDataComponent();
		return recorderPluginDataComponent.QueryPlugins();
	}
}
