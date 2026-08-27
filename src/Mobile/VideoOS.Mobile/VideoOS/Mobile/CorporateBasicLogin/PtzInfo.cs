using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PtzInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PtzInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private CapabilityInfo[] CapabilitiesField;

	[OptionalField]
	private bool EditPresetField;

	[OptionalField]
	private bool IsCenterOnPositionInViewSupportedField;

	[OptionalField]
	private bool IsPtzCenterAndZoomToRectangleSupportedField;

	[OptionalField]
	private bool IsPtzDiagonalSupportedField;

	[OptionalField]
	private bool IsPtzHomeSupportedField;

	[OptionalField]
	private PresetInfo[] PresetsField;

	[OptionalField]
	private bool PtzEnabledField;

	[Browsable(false)]
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
			if (CapabilitiesField != value)
			{
				CapabilitiesField = value;
				RaisePropertyChanged("Capabilities");
			}
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
			if (!EditPresetField.Equals(value))
			{
				EditPresetField = value;
				RaisePropertyChanged("EditPreset");
			}
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
			if (!IsCenterOnPositionInViewSupportedField.Equals(value))
			{
				IsCenterOnPositionInViewSupportedField = value;
				RaisePropertyChanged("IsCenterOnPositionInViewSupported");
			}
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
			if (!IsPtzCenterAndZoomToRectangleSupportedField.Equals(value))
			{
				IsPtzCenterAndZoomToRectangleSupportedField = value;
				RaisePropertyChanged("IsPtzCenterAndZoomToRectangleSupported");
			}
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
			if (!IsPtzDiagonalSupportedField.Equals(value))
			{
				IsPtzDiagonalSupportedField = value;
				RaisePropertyChanged("IsPtzDiagonalSupported");
			}
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
			if (!IsPtzHomeSupportedField.Equals(value))
			{
				IsPtzHomeSupportedField = value;
				RaisePropertyChanged("IsPtzHomeSupported");
			}
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
			if (PresetsField != value)
			{
				PresetsField = value;
				RaisePropertyChanged("Presets");
			}
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
			if (!PtzEnabledField.Equals(value))
			{
				PtzEnabledField = value;
				RaisePropertyChanged("PtzEnabled");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
