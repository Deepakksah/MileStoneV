using System;

namespace VideoOS.Server.DataComponents;

public interface IMIPLicenseRequestComponent : IDisposable
{
	string QueryMIPLicenseRequest();
}
