using System;
using System.Globalization;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogServiceClientBindingParameters
{
	public TimeSpan OpenTimeout { get; }

	public TimeSpan CloseTimeout { get; }

	public TimeSpan SendTimeout { get; }

	public TimeSpan ReceiveTimeout { get; }

	public LogServiceClientBindingParameters()
	{
		OpenTimeout = TimeSpan.FromMinutes(10.0);
		CloseTimeout = TimeSpan.FromMinutes(10.0);
		SendTimeout = TimeSpan.FromMinutes(1.0);
		ReceiveTimeout = TimeSpan.FromMinutes(10.0);
	}

	public LogServiceClientBindingParameters(TimeSpan openTimeout, TimeSpan closeTimeout, TimeSpan sendTimeout, TimeSpan receiveTimeout)
	{
		OpenTimeout = openTimeout;
		CloseTimeout = closeTimeout;
		SendTimeout = sendTimeout;
		ReceiveTimeout = receiveTimeout;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{{ OpenTimeout: {0}, CloseTimeout: {1}, SendTimeout: {2}, ReceiveTimeout: {3} }}", OpenTimeout, CloseTimeout, SendTimeout, ReceiveTimeout);
	}
}
