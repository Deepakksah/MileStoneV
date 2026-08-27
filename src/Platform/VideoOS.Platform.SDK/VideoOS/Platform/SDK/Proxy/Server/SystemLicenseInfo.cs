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
public class SystemLicenseInfo : ISystemLicenseInfo
{
	private bool _isTrial;

	private bool _isEssential;

	private bool hasLicenseField;

	private string sLCField;

	private string productSkuField;

	private bool expiresField;

	private DateTime expiryDateField;

	private string[] featuresField;

	private bool hasCarePlus;

	public bool IsTrialLicense
	{
		get
		{
			return _isTrial;
		}
		set
		{
			_isTrial = value;
		}
	}

	public bool IsEssential
	{
		get
		{
			return _isEssential;
		}
		set
		{
			_isEssential = value;
		}
	}

	public bool HasLicense
	{
		get
		{
			return hasLicenseField;
		}
		set
		{
			hasLicenseField = value;
		}
	}

	public string SLC
	{
		get
		{
			return sLCField;
		}
		set
		{
			sLCField = value;
		}
	}

	public string ProductSku
	{
		get
		{
			return productSkuField;
		}
		set
		{
			productSkuField = value;
		}
	}

	public bool Expires
	{
		get
		{
			return expiresField;
		}
		set
		{
			expiresField = value;
		}
	}

	public DateTime ExpiryDate
	{
		get
		{
			return expiryDateField;
		}
		set
		{
			expiryDateField = value;
		}
	}

	public string[] Features
	{
		get
		{
			return featuresField;
		}
		set
		{
			featuresField = value;
		}
	}

	public bool HasCarePlus
	{
		get
		{
			return hasCarePlus;
		}
		set
		{
			hasCarePlus = value;
		}
	}
}
