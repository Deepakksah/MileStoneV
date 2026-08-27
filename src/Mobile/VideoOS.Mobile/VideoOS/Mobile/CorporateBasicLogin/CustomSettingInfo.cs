using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CustomSettingInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid IdField;

	[OptionalField]
	private DateTime LastModifiedTimeField;

	[OptionalField]
	private string LastModifiedUserField;

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
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
		}
	}

	[DataMember]
	public DateTime LastModifiedTime
	{
		get
		{
			return LastModifiedTimeField;
		}
		set
		{
			if (!LastModifiedTimeField.Equals(value))
			{
				LastModifiedTimeField = value;
				RaisePropertyChanged("LastModifiedTime");
			}
		}
	}

	[DataMember]
	public string LastModifiedUser
	{
		get
		{
			return LastModifiedUserField;
		}
		set
		{
			if ((object)LastModifiedUserField != value)
			{
				LastModifiedUserField = value;
				RaisePropertyChanged("LastModifiedUser");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
