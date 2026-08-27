using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Svc;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServiceInfo", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceInfo : IExtensibleDataObject, IServiceInfo
{
	private ExtensionDataObject extensionDataField;

	private string descriptionField;

	private bool enabledField;

	private Guid instanceField;

	private string nameField;

	private bool trustedField;

	private Guid typeField;

	private string[] uriField;

	private string endpointdefinitionField;

	private ServiceEndpoint[] endpointsField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	[DataMember]
	public bool enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			enabledField = value;
		}
	}

	[DataMember]
	public Guid instance
	{
		get
		{
			return instanceField;
		}
		set
		{
			instanceField = value;
		}
	}

	[DataMember]
	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[DataMember]
	public bool trusted
	{
		get
		{
			return trustedField;
		}
		set
		{
			trustedField = value;
		}
	}

	[DataMember]
	public Guid type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	[DataMember]
	public string[] uri
	{
		get
		{
			return uriField;
		}
		set
		{
			uriField = value;
		}
	}

	[DataMember]
	public string endpointdefinition
	{
		get
		{
			return endpointdefinitionField;
		}
		set
		{
			endpointdefinitionField = value;
		}
	}

	[DataMember]
	public ServiceEndpoint[] endpoints
	{
		get
		{
			return endpointsField;
		}
		set
		{
			endpointsField = value;
		}
	}

	public List<Configuration.RegistrationServiceEndpoint> ServiceEndpoints
	{
		get
		{
			List<Configuration.RegistrationServiceEndpoint> list = new List<Configuration.RegistrationServiceEndpoint>();
			if (endpoints != null)
			{
				ServiceEndpoint[] array = endpoints;
				foreach (ServiceEndpoint serviceEndpoint in array)
				{
					list.Add(new Configuration.RegistrationServiceEndpoint
					{
						Authentication = serviceEndpoint.authentication,
						Description = serviceEndpoint.description,
						Name = serviceEndpoint.name,
						Uri = serviceEndpoint.uri
					});
				}
			}
			return list;
		}
	}
}
