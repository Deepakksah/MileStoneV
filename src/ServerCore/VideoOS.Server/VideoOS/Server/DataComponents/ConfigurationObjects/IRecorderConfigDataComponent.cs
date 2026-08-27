using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface IRecorderConfigDataComponent : IDisposable
{
	RecorderConfigData[] QueryRecorderData();

	RecorderConfigData QueryRecorderData(Guid recorderId);

	bool RecorderExists(Guid id);

	bool RecorderEnabled(Guid id);
}
