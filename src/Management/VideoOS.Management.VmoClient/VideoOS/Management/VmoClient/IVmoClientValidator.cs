using System;

namespace VideoOS.Management.VmoClient;

public interface IVmoClientValidator
{
	bool IsValid(IVmoClient vmoClient, Guid siteId);
}
