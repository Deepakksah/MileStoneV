using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverProxyEvents
{
	event EventHandler<DriverProxyEventArgs> DriverProxyUnavailable;
}
