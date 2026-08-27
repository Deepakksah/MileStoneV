using System;

namespace VideoOS.Server.DataComponents;

public interface ISystemBuilderLicenseRequestComponent : IDisposable
{
	SystemBuilderLicenseRequest GetSystemBuilderLicenseRequest();
}
