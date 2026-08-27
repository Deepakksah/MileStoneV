using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util;

[CollectionDataContract(Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ArrayOfEndpointDefinition : List<EndpointDefinition>
{
	public ArrayOfEndpointDefinition()
	{
	}

	public ArrayOfEndpointDefinition(List<Configuration.RegistrationServiceEndpoint> ep)
	{
		foreach (Configuration.RegistrationServiceEndpoint item in ep)
		{
			Add(new EndpointDefinition(item.Uri, item.Authentication, item.Name, item.Description));
		}
	}
}
