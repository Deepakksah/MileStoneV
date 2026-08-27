using System;

namespace VideoOS.Installer.Common;

public class ManagementServerInfo
{
	public Uri IDPAddress { get; }

	public Uri HttpAddress { get; }

	public Uri HttpsAddress { get; }

	public ManagementServerInfo()
	{
	}

	public ManagementServerInfo(Uri idpAddress, Uri httpAddress, Uri httpsAddress)
	{
		IDPAddress = idpAddress;
		HttpAddress = httpAddress;
		HttpsAddress = httpsAddress;
	}
}
