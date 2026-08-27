using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmObjectState", Namespace = "http://videoos.net/2/CentralServerStatusCommand")]
public class AlarmObjectState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int InProgressAlarmsField;

	private int NewAlarmsField;

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
	public int InProgressAlarms
	{
		get
		{
			return InProgressAlarmsField;
		}
		set
		{
			InProgressAlarmsField = value;
		}
	}

	[DataMember]
	public int NewAlarms
	{
		get
		{
			return NewAlarmsField;
		}
		set
		{
			NewAlarmsField = value;
		}
	}
}
