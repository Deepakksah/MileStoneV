using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ClientProfile : IClientProfile
{
	private Guid idField;

	private string nameField;

	private string descriptionField;

	private DateTime lastModifiedField;

	private DateTime settingsLastModifiedField;

	private ClientProfileSetting[] clientProfileSettingsField;

	IClientProfileSetting[] IClientProfile.ClientProfileSettings
	{
		get
		{
			return ClientProfileSettings;
		}
		set
		{
			ClientProfileSettings = (ClientProfileSetting[])value;
		}
	}

	public Guid Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public DateTime LastModified
	{
		get
		{
			return lastModifiedField;
		}
		set
		{
			lastModifiedField = value;
		}
	}

	public DateTime SettingsLastModified
	{
		get
		{
			return settingsLastModifiedField;
		}
		set
		{
			settingsLastModifiedField = value;
		}
	}

	public ClientProfileSetting[] ClientProfileSettings
	{
		get
		{
			return clientProfileSettingsField;
		}
		set
		{
			clientProfileSettingsField = value;
		}
	}
}
