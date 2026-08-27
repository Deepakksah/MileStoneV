using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IProductInfo
{
	string ProductName { get; set; }

	Guid ProductLine { get; set; }

	Guid VendorId { get; set; }

	Guid SubProduct { get; set; }

	string MajorVersion { get; set; }

	string MinorVersion { get; set; }

	string ServiceVersion { get; set; }

	string BuildConfiguration { get; set; }

	string BuildNumber { get; set; }

	DateTime BuildDate { get; set; }

	int ProductCode { get; set; }

	string SLC { get; set; }
}
