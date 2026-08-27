using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ProductInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ProductInfo : IProductInfo
{
	private string BuildConfigurationField;

	private DateTime BuildDateField;

	private string BuildNumberField;

	private string MajorVersionField;

	private string MinorVersionField;

	private int ProductCodeField;

	private Guid ProductLineField;

	private string ProductNameField;

	private string SLCField;

	private string ServiceVersionField;

	private Guid SubProductField;

	private Guid VendorIdField;

	[DataMember]
	public string BuildConfiguration
	{
		get
		{
			return BuildConfigurationField;
		}
		set
		{
			BuildConfigurationField = value;
		}
	}

	[DataMember]
	public DateTime BuildDate
	{
		get
		{
			return BuildDateField;
		}
		set
		{
			BuildDateField = value;
		}
	}

	[DataMember]
	public string BuildNumber
	{
		get
		{
			return BuildNumberField;
		}
		set
		{
			BuildNumberField = value;
		}
	}

	[DataMember]
	public string MajorVersion
	{
		get
		{
			return MajorVersionField;
		}
		set
		{
			MajorVersionField = value;
		}
	}

	[DataMember]
	public string MinorVersion
	{
		get
		{
			return MinorVersionField;
		}
		set
		{
			MinorVersionField = value;
		}
	}

	[DataMember]
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

	[DataMember]
	public Guid ProductLine
	{
		get
		{
			return ProductLineField;
		}
		set
		{
			ProductLineField = value;
		}
	}

	[DataMember]
	public string ProductName
	{
		get
		{
			return ProductNameField;
		}
		set
		{
			ProductNameField = value;
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
	public string ServiceVersion
	{
		get
		{
			return ServiceVersionField;
		}
		set
		{
			ServiceVersionField = value;
		}
	}

	[DataMember]
	public Guid SubProduct
	{
		get
		{
			return SubProductField;
		}
		set
		{
			SubProductField = value;
		}
	}

	[DataMember]
	public Guid VendorId
	{
		get
		{
			return VendorIdField;
		}
		set
		{
			VendorIdField = value;
		}
	}
}
