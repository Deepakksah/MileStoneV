using System;
using System.Text;

namespace VideoOS.Management.Server;

public class LicenseRow : ILicenseInfo
{
	public DateTime LastModified { get; set; }

	public byte[] Data { get; set; }

	public string DataAsString
	{
		get
		{
			return Encoding.UTF8.GetString(Data);
		}
		set
		{
			Data = Encoding.UTF8.GetBytes(value);
		}
	}

	public Guid Id { get; set; }
}
