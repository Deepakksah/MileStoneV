using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IOutputDevice : IDriverDevice, IDisposable
{
	IOState State { get; set; }

	bool SetStateSupported { get; }

	bool TriggerSupported { get; }

	void Trigger();

	void Trigger(long duration);
}
