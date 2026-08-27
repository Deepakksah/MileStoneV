using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Installer.LogServerWarningPlugin.SReg;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServiceInfo", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string descriptionField;

	[OptionalField]
	private bool enabledField;

	[OptionalField]
	private string endpointdefinitionField;

	[OptionalField]
	private ServiceEndpoint[] endpointsField;

	[OptionalField]
	private Guid instanceField;

	[OptionalField]
	private string nameField;

	[OptionalField]
	private bool trustedField;

	[OptionalField]
	private Guid typeField;

	[OptionalField]
	private string[] uriField;

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
	public bool enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			if (!enabledField.Equals(value))
			{
				enabledField = value;
				RaisePropertyChanged("enabled");
			}
		}
	}

	[DataMember]
	public string endpointdefinition
	{
		get
		{
			return endpointdefinitionField;
		}
		set
		{
			if ((object)endpointdefinitionField != value)
			{
				endpointdefinitionField = value;
				RaisePropertyChanged("endpointdefinition");
			}
		}
	}

	[DataMember]
	public ServiceEndpoint[] endpoints
	{
		get
		{
			return endpointsField;
		}
		set
		{
			if (endpointsField != value)
			{
				endpointsField = value;
				RaisePropertyChanged("endpoints");
			}
		}
	}

	[DataMember]
	public Guid instance
	{
		get
		{
			return instanceField;
		}
		set
		{
			if (!instanceField.Equals(value))
			{
				instanceField = value;
				RaisePropertyChanged("instance");
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
	public bool trusted
	{
		get
		{
			return trustedField;
		}
		set
		{
			if (!trustedField.Equals(value))
			{
				trustedField = value;
				RaisePropertyChanged("trusted");
			}
		}
	}

	[DataMember]
	public Guid type
	{
		get
		{
			return typeField;
		}
		set
		{
			if (!typeField.Equals(value))
			{
				typeField = value;
				RaisePropertyChanged("type");
			}
		}
	}

	[DataMember]
	public string[] uri
	{
		get
		{
			return uriField;
		}
		set
		{
			if (uriField != value)
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
