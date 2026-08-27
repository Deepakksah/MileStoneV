using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("ProductInfo: {ProductName}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ProductInfo
{
	[DataMember]
	public string ProductName { get; set; }

	[DataMember]
	public Guid ProductLine { get; set; }

	[DataMember]
	public Guid VendorId { get; set; }

	[DataMember]
	public Guid SubProduct { get; set; }

	[DataMember]
	public string MajorVersion { get; set; }

	[DataMember]
	public string MinorVersion { get; set; }

	[DataMember]
	public string ServiceVersion { get; set; }

	[DataMember]
	public string BuildConfiguration { get; set; }

	[DataMember]
	public string BuildNumber { get; set; }

	[DataMember]
	public DateTime BuildDate { get; set; }

	[DataMember]
	public int ProductCode { get; set; }

	[DataMember]
	public string SLC { get; set; }
}
