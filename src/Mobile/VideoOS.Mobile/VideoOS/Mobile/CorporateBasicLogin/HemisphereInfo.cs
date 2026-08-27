using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HemisphereInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class HemisphereInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private double CenterXField;

	[OptionalField]
	private double CenterYField;

	[OptionalField]
	private double RadiusXField;

	[OptionalField]
	private double RadiusYField;

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
	public double CenterX
	{
		get
		{
			return CenterXField;
		}
		set
		{
			if (!CenterXField.Equals(value))
			{
				CenterXField = value;
				RaisePropertyChanged("CenterX");
			}
		}
	}

	[DataMember]
	public double CenterY
	{
		get
		{
			return CenterYField;
		}
		set
		{
			if (!CenterYField.Equals(value))
			{
				CenterYField = value;
				RaisePropertyChanged("CenterY");
			}
		}
	}

	[DataMember]
	public double RadiusX
	{
		get
		{
			return RadiusXField;
		}
		set
		{
			if (!RadiusXField.Equals(value))
			{
				RadiusXField = value;
				RaisePropertyChanged("RadiusX");
			}
		}
	}

	[DataMember]
	public double RadiusY
	{
		get
		{
			return RadiusYField;
		}
		set
		{
			if (!RadiusYField.Equals(value))
			{
				RadiusYField = value;
				RaisePropertyChanged("RadiusY");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
