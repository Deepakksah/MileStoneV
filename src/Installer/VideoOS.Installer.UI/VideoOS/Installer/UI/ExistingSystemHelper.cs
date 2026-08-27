using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public static class ExistingSystemHelper
{
	public static bool CheckExistingSystems(bool allowTrialCheck)
	{
		string detectedProName = string.Empty;
		string detectedVersion = string.Empty;
		InstallerPrerequisitesTypeSystemDetections installerPrerequisitesTypeSystemDetections = InstallerManager.Instance.DetectSystems(ref detectedProName, ref detectedVersion);
		if (!allowTrialCheck && installerPrerequisitesTypeSystemDetections != null && installerPrerequisitesTypeSystemDetections.CheckOnlyTrial)
		{
			LogManager.Write("CheckExistingSystems(): Skipped because only for trial version.", TraceEventType.Warning);
			return true;
		}
		if (!string.IsNullOrEmpty(detectedProName) && installerPrerequisitesTypeSystemDetections != null)
		{
			if (installerPrerequisitesTypeSystemDetections.Notification == NotificationType.Error)
			{
				throw new Exception(string.Format(Resources.ErrorMessage_ExistingSystemsExistText, VersionFormat.GetNeutralProductName(), detectedProName));
			}
			if (installerPrerequisitesTypeSystemDetections.Notification == NotificationType.Warning)
			{
				string text = string.Empty;
				if (installerPrerequisitesTypeSystemDetections.Text == TextType.MoveConfig)
				{
					text = string.Format(Resources.WarningMessage_ExistingSystemsMoveConfigText, detectedProName, VersionFormat.GetNeutralProductName());
				}
				else if (installerPrerequisitesTypeSystemDetections.Text == TextType.UninstallOldVersionDependent)
				{
					if (string.Compare(detectedProName, VersionFormat.GetNeutralProductName(), StringComparison.InvariantCultureIgnoreCase) != 0)
					{
						bool flag = false;
						if (string.IsNullOrEmpty(detectedVersion))
						{
							LogManager.Write("Version of the existing system could not be detected. Show original message.", TraceEventType.Warning);
						}
						else if (installerPrerequisitesTypeSystemDetections.DependentVersion.Split(';').Contains(detectedVersion))
						{
							flag = true;
							text = string.Format(allowTrialCheck ? Resources.WarningMessage_ExistingSystemsUninstallOldVersionDependentTrialText : Resources.WarningMessage_ExistingSystemsUninstallOldVersionDependentText, detectedProName, VersionFormat.GetNeutralProductName());
						}
						else
						{
							LogManager.Write($"Version of the existing system ('{detectedVersion}') is not included in the list products with special message. Show original message.", TraceEventType.Warning);
						}
						if (!flag)
						{
							text = string.Format(allowTrialCheck ? Resources.WarningMessage_ExistingSystemsUninstallOldTrialext : Resources.WarningMessage_ExistingSystemsUninstallOldText, detectedProName, VersionFormat.GetNeutralProductName());
						}
					}
				}
				else if (string.Compare(detectedProName, VersionFormat.GetNeutralProductName(), StringComparison.InvariantCultureIgnoreCase) != 0)
				{
					text = string.Format(allowTrialCheck ? Resources.WarningMessage_ExistingSystemsUninstallOldTrialext : Resources.WarningMessage_ExistingSystemsUninstallOldText, detectedProName, VersionFormat.GetNeutralProductName());
				}
				if (!string.IsNullOrEmpty(text))
				{
					LogManager.Write(text, TraceEventType.Warning);
					return MessageHelper.ShowInstallerMessageBox(Resources.ErrorMessage_ExistingSystemsExistCaption, text, InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) == DialogResult.Yes;
				}
			}
		}
		return true;
	}
}
