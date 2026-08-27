using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Svc;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServiceEndpoint", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceEndpoint : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string authenticationField;

	private string descriptionField;

	private string nameField;

	private string uriField;

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
	public string authentication
	{
		get
		{
			return authenticationField;
		}
		set
		{
			authenticationField = value;
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
	public string uri
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
