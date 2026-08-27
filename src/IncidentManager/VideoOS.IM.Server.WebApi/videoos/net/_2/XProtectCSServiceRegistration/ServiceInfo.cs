using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace videoos.net._2.XProtectCSServiceRegistration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServiceInfo", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string descriptionField;

	private bool enabledField;

	private string endpointdefinitionField;

	private ServiceEndpoint[] endpointsField;

	private Guid instanceField;

	private string nameField;

	private bool trustedField;

	private Guid typeField;

	private string[] uriField;

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
}
