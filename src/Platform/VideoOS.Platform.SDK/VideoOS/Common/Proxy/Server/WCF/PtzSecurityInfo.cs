using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PtzSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class PtzSecurityInfo : IExtensibleDataObject, IPtzSecurityInfo
{
	private ExtensionDataObject extensionDataField;

	private bool ManualControlField;

	private bool PresetControlField;

	private bool ReserveControlField;

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
	public bool ManualControl
	{
		get
		{
			return ManualControlField;
		}
		set
		{
			ManualControlField = value;
		}
	}

	[DataMember]
	public bool PresetControl
	{
		get
		{
			return PresetControlField;
		}
		set
		{
			PresetControlField = value;
		}
	}

	[DataMember]
	public bool ReserveControl
	{
		get
		{
			return ReserveControlField;
		}
		set
		{
			ReserveControlField = value;
		}
	}
}
