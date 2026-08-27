using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Util;

public interface IServiceInfo
{
	string description { get; set; }

	bool enabled { get; set; }

	Guid instance { get; set; }

	string name { get; set; }

	bool trusted { get; set; }

	Guid type { get; set; }

	string[] uri { get; set; }

	string endpointdefinition { get; set; }

	List<Configuration.RegistrationServiceEndpoint> ServiceEndpoints { get; }
}
