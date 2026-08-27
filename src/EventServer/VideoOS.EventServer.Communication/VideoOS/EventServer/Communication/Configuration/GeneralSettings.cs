using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GeneralSettings", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class GeneralSettings : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int DaysToKeepAllOtherAlarmsField;

	private int DaysToKeepClosedAlarmsField;

	private bool TraceField;

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
	public int DaysToKeepAllOtherAlarms
	{
		get
		{
			return DaysToKeepAllOtherAlarmsField;
		}
		set
		{
			DaysToKeepAllOtherAlarmsField = value;
		}
	}

	[DataMember]
	public int DaysToKeepClosedAlarms
	{
		get
		{
			return DaysToKeepClosedAlarmsField;
		}
		set
		{
			DaysToKeepClosedAlarmsField = value;
		}
	}

	[DataMember]
	public bool Trace
	{
		get
		{
			return TraceField;
		}
		set
		{
			TraceField = value;
		}
	}
}
