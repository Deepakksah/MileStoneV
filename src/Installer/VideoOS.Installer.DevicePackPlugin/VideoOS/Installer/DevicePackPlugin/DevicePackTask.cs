using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.MSI;

namespace VideoOS.Installer.DevicePackPlugin;

public class DevicePackTask : MSIInstallerTask, IInstallationAdapter
{
	public override bool RunInFinalizeMode => false;

	public override bool RunInHardwareManufacturerMode => true;

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		_parameters = installParameters;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}
