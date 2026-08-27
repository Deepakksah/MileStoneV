using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IpixInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class IpixInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool CeilingMountedField;

	[OptionalField]
	private HemisphereInfo HemisphereField;

	[OptionalField]
	private PositionInfo HomepositionField;

	[OptionalField]
	private bool IpixEnabledField;

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
	public bool CeilingMounted
	{
		get
		{
			return CeilingMountedField;
		}
		set
		{
			if (!CeilingMountedField.Equals(value))
			{
				CeilingMountedField = value;
				RaisePropertyChanged("CeilingMounted");
			}
		}
	}

	[DataMember]
	public HemisphereInfo Hemisphere
	{
		get
		{
			return HemisphereField;
		}
		set
		{
			if (HemisphereField != value)
			{
				HemisphereField = value;
				RaisePropertyChanged("Hemisphere");
			}
		}
	}

	[DataMember]
	public PositionInfo Homeposition
	{
		get
		{
			return HomepositionField;
		}
		set
		{
			if (HomepositionField != value)
			{
				HomepositionField = value;
				RaisePropertyChanged("Homeposition");
			}
		}
	}

	[DataMember]
	public bool IpixEnabled
	{
		get
		{
			return IpixEnabledField;
		}
		set
		{
			if (!IpixEnabledField.Equals(value))
			{
				IpixEnabledField = value;
				RaisePropertyChanged("IpixEnabled");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
