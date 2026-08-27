using System;

namespace VideoOS.Management.Server;

public interface ILicenseInfo
{
	DateTime LastModified { get; set; }

	byte[] Data { get; set; }

	string DataAsString { get; set; }

	Guid Id { get; set; }
}
