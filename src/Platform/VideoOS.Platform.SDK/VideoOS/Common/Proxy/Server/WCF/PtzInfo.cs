using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PtzInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class PtzInfo : IExtensibleDataObject, IPtzInfo
{
	private ExtensionDataObject extensionDataField;

	private CapabilityInfo[] CapabilitiesField;

	private bool EditPresetField;

	private bool IsCenterOnPositionInViewSupportedField;

	private bool IsPtzCenterAndZoomToRectangleSupportedField;

	private bool IsPtzDiagonalSupportedField;

	private bool IsPtzHomeSupportedField;

	private PresetInfo[] PresetsField;

	private bool PtzEnabledField;

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
	public CapabilityInfo[] Capabilities
	{
		get
		{
			return CapabilitiesField;
		}
		set
		{
			CapabilitiesField = value;
		}
	}

	[DataMember]
	public bool EditPreset
	{
		get
		{
			return EditPresetField;
		}
		set
		{
			EditPresetField = value;
		}
	}

	[DataMember]
	public bool IsCenterOnPositionInViewSupported
	{
		get
		{
			return IsCenterOnPositionInViewSupportedField;
		}
		set
		{
			IsCenterOnPositionInViewSupportedField = value;
		}
	}

	[DataMember]
	public bool IsPtzCenterAndZoomToRectangleSupported
	{
		get
		{
			return IsPtzCenterAndZoomToRectangleSupportedField;
		}
		set
		{
			IsPtzCenterAndZoomToRectangleSupportedField = value;
		}
	}

	[DataMember]
	public bool IsPtzDiagonalSupported
	{
		get
		{
			return IsPtzDiagonalSupportedField;
		}
		set
		{
			IsPtzDiagonalSupportedField = value;
		}
	}

	[DataMember]
	public bool IsPtzHomeSupported
	{
		get
		{
			return IsPtzHomeSupportedField;
		}
		set
		{
			IsPtzHomeSupportedField = value;
		}
	}

	[DataMember]
	public PresetInfo[] Presets
	{
		get
		{
			return PresetsField;
		}
		set
		{
			PresetsField = value;
		}
	}

	[DataMember]
	public bool PtzEnabled
	{
		get
		{
			return PtzEnabledField;
		}
		set
		{
			PtzEnabledField = value;
		}
	}

	IPresetInfo[] IPtzInfo.Presets
	{
		get
		{
			return Presets;
		}
		set
		{
			Presets = (PresetInfo[])value;
		}
	}

	ICapabilityInfo[] IPtzInfo.Capabilities
	{
		get
		{
			return Capabilities;
		}
		set
		{
			Capabilities = (CapabilityInfo[])value;
		}
	}
}
