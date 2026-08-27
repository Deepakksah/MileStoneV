using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRecorderPluginDataComponent : IDisposable
{
	RecorderPluginData QueryPluginById(string id);

	IEnumerable<RecorderPluginData> QueryPlugins();
}
