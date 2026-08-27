using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class SiteHomeNotUniqueException : VmoServiceException
{
	protected override int ErrorCodeDefault => 61100;

	public SiteHomeNotUniqueException()
		: base(FormatMessage())
	{
	}

	private static string FormatMessage()
	{
		return VmoResources.VMO61100;
	}
}
