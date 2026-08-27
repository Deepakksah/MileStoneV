using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LicenseInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LicenseInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DataField;

	[OptionalField]
	private Guid LicenseIdField;

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
	public string Data
	{
		get
		{
			return DataField;
		}
		set
		{
			if ((object)DataField != value)
			{
				DataField = value;
				RaisePropertyChanged("Data");
			}
		}
	}

	[DataMember]
	public Guid LicenseId
	{
		get
		{
			return LicenseIdField;
		}
		set
		{
			if (!LicenseIdField.Equals(value))
			{
				LicenseIdField = value;
				RaisePropertyChanged("LicenseId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
