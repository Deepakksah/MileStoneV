using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceInfo
{
	[DataMember(IsRequired = true)]
	public Guid type;

	[DataMember(IsRequired = true, Order = 1)]
	public Guid instance;

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public ArrayOfString uri;

	[DataMember(EmitDefaultValue = false, Order = 3)]
	public string name;

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public string description;

	[DataMember(IsRequired = true, Order = 5)]
	public bool trusted;

	[DataMember(EmitDefaultValue = true, Order = 6)]
	public string endpointDefinitions;

	[DataMember(EmitDefaultValue = true, Order = 7)]
	public ArrayOfEndpointDefinition endpoints;

	public List<Configuration.RegistrationServiceEndpoint> ServiceEndpoints
	{
		get
		{
			List<Configuration.RegistrationServiceEndpoint> list = new List<Configuration.RegistrationServiceEndpoint>();
			if (endpoints != null)
			{
				foreach (EndpointDefinition endpoint in endpoints)
				{
					string text = endpoint.ServicePath;
					if (uri != null)
					{
						text = endpoint.GetEndpointUri(uri[0]);
					}
					list.Add(new Configuration.RegistrationServiceEndpoint
					{
						Authentication = endpoint.AuthenticationType,
						Description = endpoint.Description,
						Name = endpoint.Name,
						Uri = text
					});
				}
			}
			return list;
		}
	}
}
