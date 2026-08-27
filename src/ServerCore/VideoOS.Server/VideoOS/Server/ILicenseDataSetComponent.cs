using System.Threading;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server;

public interface ILicenseDataSetComponent
{
	ReaderWriterLockSlim LicenseCacheLock { get; }

	void WriteLicenseDataSetChangesToDatabase(ILicenseSnapshot licenseSnapshot);

	ILicenseSnapshot LoadLicenseDataSetByEnabled(bool enabled);

	ILicenseSnapshot LoadLicenseDataSet(out bool isObtainedFromCache);
}
