using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ChangeType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ChangeType : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool HardwareField;

	private bool RecorderField;

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
	public bool Hardware
	{
		get
		{
			return HardwareField;
		}
		set
		{
			HardwareField = value;
		}
	}

	[DataMember]
	public bool Recorder
	{
		get
		{
			return RecorderField;
		}
		set
		{
			RecorderField = value;
		}
	}
}
