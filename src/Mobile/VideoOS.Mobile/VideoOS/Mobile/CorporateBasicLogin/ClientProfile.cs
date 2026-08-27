using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ClientProfile", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfile : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private ClientProfileSetting[] ClientProfileSettingsField;

	[OptionalField]
	private string DescriptionField;

	[OptionalField]
	private Guid IdField;

	[OptionalField]
	private DateTime LastModifiedField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private DateTime SettingsLastModifiedField;

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
	public ClientProfileSetting[] ClientProfileSettings
	{
		get
		{
			return ClientProfileSettingsField;
		}
		set
		{
			if (ClientProfileSettingsField != value)
			{
				ClientProfileSettingsField = value;
				RaisePropertyChanged("ClientProfileSettings");
			}
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			if ((object)DescriptionField != value)
			{
				DescriptionField = value;
				RaisePropertyChanged("Description");
			}
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
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			if (!LastModifiedField.Equals(value))
			{
				LastModifiedField = value;
				RaisePropertyChanged("LastModified");
			}
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
		}
	}

	[DataMember]
	public DateTime SettingsLastModified
	{
		get
		{
			return SettingsLastModifiedField;
		}
		set
		{
			if (!SettingsLastModifiedField.Equals(value))
			{
				SettingsLastModifiedField = value;
				RaisePropertyChanged("SettingsLastModified");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
