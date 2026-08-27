using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PositionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PositionInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private double PanField;

	[OptionalField]
	private double TiltField;

	[OptionalField]
	private double ZoomField;

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
	public double Pan
	{
		get
		{
			return PanField;
		}
		set
		{
			if (!PanField.Equals(value))
			{
				PanField = value;
				RaisePropertyChanged("Pan");
			}
		}
	}

	[DataMember]
	public double Tilt
	{
		get
		{
			return TiltField;
		}
		set
		{
			if (!TiltField.Equals(value))
			{
				TiltField = value;
				RaisePropertyChanged("Tilt");
			}
		}
	}

	[DataMember]
	public double Zoom
	{
		get
		{
			return ZoomField;
		}
		set
		{
			if (!ZoomField.Equals(value))
			{
				ZoomField = value;
				RaisePropertyChanged("Zoom");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
