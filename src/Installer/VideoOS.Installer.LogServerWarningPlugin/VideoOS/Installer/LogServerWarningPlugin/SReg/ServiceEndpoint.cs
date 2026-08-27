using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Installer.LogServerWarningPlugin.SReg;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServiceEndpoint", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceEndpoint : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string authenticationField;

	[OptionalField]
	private string descriptionField;

	[OptionalField]
	private string nameField;

	[OptionalField]
	private string uriField;

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
	public string authentication
	{
		get
		{
			return authenticationField;
		}
		set
		{
			if ((object)authenticationField != value)
			{
				authenticationField = value;
				RaisePropertyChanged("authentication");
			}
		}
	}

	[DataMember]
	public string description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			if ((object)descriptionField != value)
			{
				descriptionField = value;
				RaisePropertyChanged("description");
			}
		}
	}

	[DataMember]
	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			if ((object)nameField != value)
			{
				nameField = value;
				RaisePropertyChanged("name");
			}
		}
	}

	[DataMember]
	public string uri
	{
		get
		{
			return uriField;
		}
		set
		{
			if ((object)uriField != value)
			{
				uriField = value;
				RaisePropertyChanged("uri");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
