using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ImmerVisionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ImmerVisionInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string LensProfileDataField;

	[OptionalField]
	private string LensProfileNameField;

	[OptionalField]
	private string LensProfileRplField;

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
	public string LensProfileData
	{
		get
		{
			return LensProfileDataField;
		}
		set
		{
			if ((object)LensProfileDataField != value)
			{
				LensProfileDataField = value;
				RaisePropertyChanged("LensProfileData");
			}
		}
	}

	[DataMember]
	public string LensProfileName
	{
		get
		{
			return LensProfileNameField;
		}
		set
		{
			if ((object)LensProfileNameField != value)
			{
				LensProfileNameField = value;
				RaisePropertyChanged("LensProfileName");
			}
		}
	}

	[DataMember]
	public string LensProfileRpl
	{
		get
		{
			return LensProfileRplField;
		}
		set
		{
			if ((object)LensProfileRplField != value)
			{
				LensProfileRplField = value;
				RaisePropertyChanged("LensProfileRpl");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
