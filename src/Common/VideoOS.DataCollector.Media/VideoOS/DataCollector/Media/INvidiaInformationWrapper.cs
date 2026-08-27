using System;
using System.Collections.Generic;

namespace VideoOS.DataCollector.Media;

public interface INvidiaInformationWrapper : IDisposable
{
	IEnumerable<string> NvidiaIds { get; }

	uint GetMemoryUsage(string arg);

	uint GetDeviceLoad(string arg);

	uint GetDecoderLoad(string arg);

	bool HasHACapability();
}
