using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IExternalPluginsDataComponent : IDisposable
{
	IEnumerable<ExternalPluginsData> QueryExternalServerPlugins();

	IEnumerable<ExternalPluginsData> QueryExternalSmartClientPlugins();

	ExternalPluginsData QueryExternalSmartClientPlugin(Guid pluginId);
}
