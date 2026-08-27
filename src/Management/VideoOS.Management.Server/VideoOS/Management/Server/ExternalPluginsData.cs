using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class ExternalPluginsData
{
	public Guid PluginId { get; set; }

	public string Data { get; set; }

	public static IEnumerable<ExternalPluginsData> QueryExternalServerPlugins(IGeminiRequestContext requestContext)
	{
		using IExternalPluginsDataComponent externalPluginsDataComponent = requestContext.CreateExternalPluginsDataComponent();
		return externalPluginsDataComponent.QueryExternalServerPlugins();
	}

	public static IEnumerable<ExternalPluginsData> QueryExternalSmartClientPlugins(IGeminiRequestContext requestContext)
	{
		using IExternalPluginsDataComponent externalPluginsDataComponent = requestContext.CreateExternalPluginsDataComponent();
		return externalPluginsDataComponent.QueryExternalSmartClientPlugins();
	}

	public static ExternalPluginsData QueryExternalSmartClientPlugin(IGeminiRequestContext requestContext, Guid pluginId)
	{
		using IExternalPluginsDataComponent externalPluginsDataComponent = requestContext.CreateExternalPluginsDataComponent();
		return externalPluginsDataComponent.QueryExternalSmartClientPlugin(pluginId);
	}
}
