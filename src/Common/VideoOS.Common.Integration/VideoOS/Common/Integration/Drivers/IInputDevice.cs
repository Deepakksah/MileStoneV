using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IInputDevice : IDriverDevice, IDisposable
{
	IOState State { get; }
}
