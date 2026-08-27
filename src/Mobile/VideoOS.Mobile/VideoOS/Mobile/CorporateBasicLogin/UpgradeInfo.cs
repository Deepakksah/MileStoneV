using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "UpgradeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UpgradeInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string HostnameField;

	[OptionalField]
	private string MessageField;

	[OptionalField]
	private string PathField;

	[OptionalField]
	private int PortField;

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
	public string Hostname
	{
		get
		{
			return HostnameField;
		}
		set
		{
			if ((object)HostnameField != value)
			{
				HostnameField = value;
				RaisePropertyChanged("Hostname");
			}
		}
	}

	[DataMember]
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			if ((object)MessageField != value)
			{
				MessageField = value;
				RaisePropertyChanged("Message");
			}
		}
	}

	[DataMember]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			if ((object)PathField != value)
			{
				PathField = value;
				RaisePropertyChanged("Path");
			}
		}
	}

	[DataMember]
	public int Port
	{
		get
		{
			return PortField;
		}
		set
		{
			if (!PortField.Equals(value))
			{
				PortField = value;
				RaisePropertyChanged("Port");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
