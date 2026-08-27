using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "UpdateEventsExpirationParams", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class UpdateEventsExpirationParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int DaysToKeepAccessControlEventsField;

	private int DaysToKeepEventServerEventsField;

	private int DaysToKeepLPREventsField;

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
	public int DaysToKeepAccessControlEvents
	{
		get
		{
			return DaysToKeepAccessControlEventsField;
		}
		set
		{
			DaysToKeepAccessControlEventsField = value;
		}
	}

	[DataMember]
	public int DaysToKeepEventServerEvents
	{
		get
		{
			return DaysToKeepEventServerEventsField;
		}
		set
		{
			DaysToKeepEventServerEventsField = value;
		}
	}

	[DataMember]
	public int DaysToKeepLPREvents
	{
		get
		{
			return DaysToKeepLPREventsField;
		}
		set
		{
			DaysToKeepLPREventsField = value;
		}
	}
}
