using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IServiceInfo
{
	string description { get; set; }

	bool enabled { get; set; }

	Guid instance { get; set; }

	string name { get; set; }

	bool trusted { get; set; }

	Guid type { get; set; }

	string[] uri { get; set; }

	IServiceEndpoint[] endpoints { get; set; }
}
