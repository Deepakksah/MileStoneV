using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PluginIntegration", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class PluginIntegration : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid IntegrationIdField;

	[OptionalField]
	private string IntegrationNameField;

	[OptionalField]
	private string IntegrationVersionField;

	[OptionalField]
	private string ManufacturerNameField;

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
	public Guid IntegrationId
	{
		get
		{
			return IntegrationIdField;
		}
		set
		{
			if (!IntegrationIdField.Equals(value))
			{
				IntegrationIdField = value;
				RaisePropertyChanged("IntegrationId");
			}
		}
	}

	[DataMember]
	public string IntegrationName
	{
		get
		{
			return IntegrationNameField;
		}
		set
		{
			if ((object)IntegrationNameField != value)
			{
				IntegrationNameField = value;
				RaisePropertyChanged("IntegrationName");
			}
		}
	}

	[DataMember]
	public string IntegrationVersion
	{
		get
		{
			return IntegrationVersionField;
		}
		set
		{
			if ((object)IntegrationVersionField != value)
			{
				IntegrationVersionField = value;
				RaisePropertyChanged("IntegrationVersion");
			}
		}
	}

	[DataMember]
	public string ManufacturerName
	{
		get
		{
			return ManufacturerNameField;
		}
		set
		{
			if ((object)ManufacturerNameField != value)
			{
				ManufacturerNameField = value;
				RaisePropertyChanged("ManufacturerName");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
