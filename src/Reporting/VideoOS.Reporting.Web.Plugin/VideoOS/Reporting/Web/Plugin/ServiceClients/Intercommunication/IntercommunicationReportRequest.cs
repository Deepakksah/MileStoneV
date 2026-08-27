using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReportRequest", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReportRequest : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DataField;

	private int IntervalField;

	private string ReportTypeField;

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
	public string Data
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
	public int Interval
	{
		get
		{
			return IntervalField;
		}
		set
		{
			IntervalField = value;
		}
	}

	[DataMember]
	public string ReportType
	{
		get
		{
			return ReportTypeField;
		}
		set
		{
			ReportTypeField = value;
		}
	}
}
