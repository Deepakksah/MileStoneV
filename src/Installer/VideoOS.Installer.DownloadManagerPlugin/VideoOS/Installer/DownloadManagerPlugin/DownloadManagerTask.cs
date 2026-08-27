using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.DownloadManagerPlugin;

public class DownloadManagerTask : MSIInstallerTask, IInstallationAdapter
{
	public override bool RunInFinalizeMode => false;

	public override bool RunInHardwareManufacturerMode => true;

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Defined by interface")]
	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Defined by interface")]
	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Defined by interface")]
	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Defined by interface")]
	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		_parameters = installParameters;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Defined by interface")]
	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}
