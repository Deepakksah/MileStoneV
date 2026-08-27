using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ProductInfo : IProductInfo
{
	private string productNameField;

	private Guid productLineField;

	private Guid vendorIdField;

	private Guid subProductField;

	private string majorVersionField;

	private string minorVersionField;

	private string serviceVersionField;

	private string buildConfigurationField;

	private string buildNumberField;

	private DateTime buildDateField;

	private int ProductCodeField;

	private string SLCField;

	public string ProductName
	{
		get
		{
			return productNameField;
		}
		set
		{
			productNameField = value;
		}
	}

	public Guid ProductLine
	{
		get
		{
			return productLineField;
		}
		set
		{
			productLineField = value;
		}
	}

	public Guid VendorId
	{
		get
		{
			return vendorIdField;
		}
		set
		{
			vendorIdField = value;
		}
	}

	public Guid SubProduct
	{
		get
		{
			return subProductField;
		}
		set
		{
			subProductField = value;
		}
	}

	public string MajorVersion
	{
		get
		{
			return majorVersionField;
		}
		set
		{
			majorVersionField = value;
		}
	}

	public string MinorVersion
	{
		get
		{
			return minorVersionField;
		}
		set
		{
			minorVersionField = value;
		}
	}

	public string ServiceVersion
	{
		get
		{
			return serviceVersionField;
		}
		set
		{
			serviceVersionField = value;
		}
	}

	public string BuildConfiguration
	{
		get
		{
			return buildConfigurationField;
		}
		set
		{
			buildConfigurationField = value;
		}
	}

	public string BuildNumber
	{
		get
		{
			return buildNumberField;
		}
		set
		{
			buildNumberField = value;
		}
	}

	public DateTime BuildDate
	{
		get
		{
			return buildDateField;
		}
		set
		{
			buildDateField = value;
		}
	}

	public int ProductCode
	{
		get
		{
			return ProductCodeField;
		}
		set
		{
			ProductCodeField = value;
		}
	}

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
}
