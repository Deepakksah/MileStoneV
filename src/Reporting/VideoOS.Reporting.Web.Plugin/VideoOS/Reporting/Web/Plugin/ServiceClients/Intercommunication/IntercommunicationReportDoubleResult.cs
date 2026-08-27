using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReportDoubleResult", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReportDoubleResult : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private IntercommunicationReportDoubleResultData[] DataField;

	private Guid IdField;

	private string NameField;

	private int StepSizeField;

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
	public IntercommunicationReportDoubleResultData[] Data
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

	[DataMember]
	public int StepSize
	{
		get
		{
			return StepSizeField;
		}
		set
		{
			StepSizeField = value;
		}
	}
}
