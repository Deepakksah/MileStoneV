using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReportDateTimeResult", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReportDateTimeResult : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private IntercommunicationReportDateTimeResultData[] DataField;

	private Guid IdField;

	private string NameField;

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
	public IntercommunicationReportDateTimeResultData[] Data
	{
		get
		{
			return DataField;
		}
		set
		{
			DataField = value;
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
}
