using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ChangeType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ChangeType : IExtensibleDataObject, IChangeType
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
