using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationReportDoubleResultData", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public class IntercommunicationReportDoubleResultData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double KeyField;

	private int ValueField;

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
	public double Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
		}
	}

	[DataMember]
	public int Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			ValueField = value;
		}
	}
}
