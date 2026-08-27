using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces;

public interface IInstallationAdapter
{
	bool RunInHardwareManufacturerMode { get; }

	bool RunInFinalizeMode { get; }

	string DisplayName { get; set; }

	bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters);

	void Initialize(List<IPackageParameter> installParameters);

	void Finalize(List<IPackageParameter> installParameters);

	void Install(string fileName, List<IPackageParameter> installParameters);

	void Uninstall(string fileName, List<IPackageParameter> installParameters);
}
