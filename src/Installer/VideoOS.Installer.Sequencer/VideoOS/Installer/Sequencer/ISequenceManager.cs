using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Sequencer.Prerequisite;

namespace VideoOS.Installer.Sequencer;

public interface ISequenceManager
{
	Dictionary<string, List<IPackageParameter>> PackageParameters { get; set; }

	List<string> InstalledPackages { get; }

	List<string> UninstalledPackages { get; }

	List<string> NotExecutedInstallPackages { get; }

	List<string> NotExecutedUninstallPackages { get; }

	Dictionary<string, string> FailedInstallPackages { get; }

	Dictionary<string, string> FailedUninstallationPackages { get; }

	List<string> PackagesToUninstall { get; set; }

	List<string> PackagesToInstall { get; set; }

	string WinTargetDir { get; }

	bool NeedReboot { get; }

	void Install();

	IAsyncResult BeginInstall(AsyncCallback callback, object asyncState);

	IAsyncResult BeginModify(AsyncCallback callback, object asyncState);

	void EndInstall(IAsyncResult asyncResult);

	void Cancel();

	IAsyncResult BeginCancel(AsyncCallback callback, object asyncState);

	void EndCancel(IAsyncResult asyncResult);

	void Rollback();

	IAsyncResult BeginRollback(AsyncCallback callback, object asyncState);

	void Uninstall();

	IAsyncResult BeginUninstall(AsyncCallback callback, object asyncState);

	void SetPrerequisitesToInstall(Dictionary<string, ProductInstallInfo> doInstall, int websitePort);

	void EndUninstall(IAsyncResult asyncResult);

	void EndModify(IAsyncResult asyncResult);

	string GetPrerequisiteCode(string prererequisiteDisplayName);

	void ServerSideRegistration(string exeLocation, string parentProcessName);
}
