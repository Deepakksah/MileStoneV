using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "SystemLicenseInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SystemLicenseInfo : ISystemLicenseInfo
{
	private bool ExpiresField;

	private DateTime ExpiryDateField;

	private string[] FeaturesField;

	private bool HasLicenseField;

	private bool IsTrialLicenseField;

	private string ProductSkuField;

	private string SLCField;

	private string SLCEnvironmentField;

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

	public bool HasCarePlus
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public bool IsEssential
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}
}
