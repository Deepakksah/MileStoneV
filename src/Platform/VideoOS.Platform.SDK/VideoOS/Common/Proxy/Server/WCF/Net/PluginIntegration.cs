using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "PluginIntegration", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PluginIntegration
{
	private bool AllowIntegrationRegistrationField;

	private Guid IntegrationIdField;

	private string IntegrationNameField;

	private string IntegrationVersionField;

	private string ManufacturerNameField;

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
