using System;
using VideoOS.Common.Interfaces.Recorder;

namespace VideoOS.Common.Integration;

public interface IPluginFactory
{
	IDataSecurityExtension CreateDataSecurityExtension(Guid deviceId);
}
