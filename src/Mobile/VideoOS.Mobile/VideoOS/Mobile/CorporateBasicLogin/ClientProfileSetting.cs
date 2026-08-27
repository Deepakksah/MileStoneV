using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ClientProfileSetting", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfileSetting : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string KeyField;

	[OptionalField]
	private bool LockedField;

	[OptionalField]
	private string ValueField;

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
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			if ((object)KeyField != value)
			{
				KeyField = value;
				RaisePropertyChanged("Key");
			}
		}
	}

	[DataMember]
	public bool Locked
	{
		get
		{
			return LockedField;
		}
		set
		{
			if (!LockedField.Equals(value))
			{
				LockedField = value;
				RaisePropertyChanged("Locked");
			}
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			if ((object)ValueField != value)
			{
				ValueField = value;
				RaisePropertyChanged("Value");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
