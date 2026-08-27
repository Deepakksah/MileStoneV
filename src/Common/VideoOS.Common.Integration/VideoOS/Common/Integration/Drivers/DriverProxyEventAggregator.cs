using System;

namespace VideoOS.Common.Integration.Drivers;

public class DriverProxyEventAggregator : IDriverProxyEvents
{
	public static DriverProxyEventAggregator Instance { get; } = new DriverProxyEventAggregator();

	public event EventHandler<DriverProxyEventArgs> DriverProxyUnavailable;

	public void OnDriverProxyUnavailable(object sender, DriverProxyEventArgs args)
	{
		this.DriverProxyUnavailable?.Invoke(sender, args);
	}

	private DriverProxyEventAggregator()
	{
	}
}
