using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReportServer", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReportServer : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AddressField;

	private Guid IdField;

	private bool IncludedField;

	private string NameField;

	private string StateField;

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
	public string Address
	{
		get
		{
			return AddressField;
		}
		set
		{
			AddressField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public bool Included
	{
		get
		{
			return IncludedField;
		}
		set
		{
			IncludedField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public string State
	{
		get
		{
			return StateField;
		}
		set
		{
			StateField = value;
		}
	}
}
