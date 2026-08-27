using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PluginIntegration", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PluginIntegration : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool AllowIntegrationRegistrationField;

	private Guid IntegrationIdField;

	private string IntegrationNameField;

	private string IntegrationVersionField;

	private string ManufacturerNameField;

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
	public bool AllowIntegrationRegistration
	{
		get
		{
			return AllowIntegrationRegistrationField;
		}
		set
		{
			AllowIntegrationRegistrationField = value;
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
			IntegrationIdField = value;
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
			IntegrationNameField = value;
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
			IntegrationVersionField = value;
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
			ManufacturerNameField = value;
		}
	}
}
