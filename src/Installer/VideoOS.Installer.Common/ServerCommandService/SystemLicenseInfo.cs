using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemLicenseInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SystemLicenseInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ExpiresField;

	private DateTime ExpiryDateField;

	private string[] FeaturesField;

	private bool HasLicenseField;

	private bool IsTrialLicenseField;

	private string ProductSkuField;

	private string SLCField;

	private string SLCEnvironmentField;

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
	public bool Expires
	{
		get
		{
			return ExpiresField;
		}
		set
		{
			ExpiresField = value;
		}
	}

	[DataMember]
	public DateTime ExpiryDate
	{
		get
		{
			return ExpiryDateField;
		}
		set
		{
			ExpiryDateField = value;
		}
	}

	[DataMember]
	public string[] Features
	{
		get
		{
			return FeaturesField;
		}
		set
		{
			FeaturesField = value;
		}
	}

	[DataMember]
	public bool HasLicense
	{
		get
		{
			return HasLicenseField;
		}
		set
		{
			HasLicenseField = value;
		}
	}

	[DataMember]
	public bool IsTrialLicense
	{
		get
		{
			return IsTrialLicenseField;
		}
		set
		{
			IsTrialLicenseField = value;
		}
	}

	[DataMember]
	public string ProductSku
	{
		get
		{
			return ProductSkuField;
		}
		set
		{
			ProductSkuField = value;
		}
	}

	[DataMember]
	public string SLC
	{
		get
		{
			return SLCField;
		}
		set
		{
			SLCField = value;
		}
	}

	[DataMember]
	public string SLCEnvironment
	{
		get
		{
			return SLCEnvironmentField;
		}
		set
		{
			SLCEnvironmentField = value;
		}
	}
}
