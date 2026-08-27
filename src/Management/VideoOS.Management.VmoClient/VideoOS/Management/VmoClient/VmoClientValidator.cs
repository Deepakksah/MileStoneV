using System;

namespace VideoOS.Management.VmoClient;

public class VmoClientValidator : IVmoClientValidator
{
	public bool IsValid(IVmoClient vmoClient, Guid siteId)
	{
		if (vmoClient == null)
		{
			throw new ArgumentNullException("vmoClient");
		}
		return vmoClient.ManagementServer.Id == siteId;
	}
}
