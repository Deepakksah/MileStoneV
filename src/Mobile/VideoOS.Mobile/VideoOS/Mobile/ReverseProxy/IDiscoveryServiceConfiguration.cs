using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.ReverseProxy;

public interface IDiscoveryServiceConfiguration
{
	IEnumerable<Uri> Uris { get; }

	TimeSpan WaitForResponseTimeoutMs { get; }

	TimeSpan UpdateFrequencyMs { get; }
}
