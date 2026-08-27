using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReport", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReport : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private IntercommunicationReportDateTimeResult[] DateTimeResultsField;

	private IntercommunicationReportDoubleResult[] DoubleResultsField;

	private IntercommunicationReportServer[] ServersField;

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
	public IntercommunicationReportDateTimeResult[] DateTimeResults
	{
		get
		{
			return DateTimeResultsField;
		}
		set
		{
			DateTimeResultsField = value;
		}
	}

	[DataMember]
	public IntercommunicationReportDoubleResult[] DoubleResults
	{
		get
		{
			return DoubleResultsField;
		}
		set
		{
			DoubleResultsField = value;
		}
	}

	[DataMember]
	public IntercommunicationReportServer[] Servers
	{
		get
		{
			return ServersField;
		}
		set
		{
			ServersField = value;
		}
	}
}
