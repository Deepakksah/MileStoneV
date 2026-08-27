using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ClientProfile", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfile : IClientProfile
{
	private ClientProfileSetting[] ClientProfileSettingsField;

	private string DescriptionField;

	private Guid IdField;

	private DateTime LastModifiedField;

	private string NameField;

	private DateTime SettingsLastModifiedField;

	[DataMember]
	public ClientProfileSetting[] ClientProfileSettings
	{
		get
		{
			return ClientProfileSettingsField;
		}
		set
		{
			ClientProfileSettingsField = value;
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
			DescriptionField = value;
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
			IdField = value;
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
			LastModifiedField = value;
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
			NameField = value;
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
			SettingsLastModifiedField = value;
		}
	}

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
}
