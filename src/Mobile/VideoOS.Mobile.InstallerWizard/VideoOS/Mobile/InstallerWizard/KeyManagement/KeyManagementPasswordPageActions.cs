using System;
using System.Collections.Generic;
using System.Security;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Security;

namespace VideoOS.Mobile.InstallerWizard.KeyManagement;

public class KeyManagementPasswordPageActions : PageSetupActionsBase
{
	public override string PageName => "MobileServerKeyManagementPasswordPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (parameters.ContainsKey("HARDWARE-MANUFACTURER-MODE") && bool.Parse(parameters["HARDWARE-MANUFACTURER-MODE"]))
		{
			return false;
		}
		return base.BeforeActivatePage(parameters);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		GlobalDataWrapper globalDataWrapper = new GlobalDataWrapper();
		if (globalDataWrapper.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return base.BeforeLeavePage(parameters);
		}
		if (globalDataWrapper.GetValueAsBoolean("QUIET-MODE"))
		{
			SecureString secureString = string.Empty.EncryptString();
			if (parameters.TryGetValue("KEYMANAGEMENTPASSWORD", out var value) && value != null)
			{
				secureString = EncryptHelper.DecryptPassword(value, unicodePassword: true).EncryptString();
			}
			PasswordValidationResult passwordValidationResult = new PasswordValidator().ValidateNewPassword(secureString, secureString);
			if (passwordValidationResult != PasswordValidationResult.Ok && passwordValidationResult != PasswordValidationResult.PasswordIsEmpty)
			{
				throw new InvalidOperationException(FormattableString.Invariant($"New System Password: {passwordValidationResult}"));
			}
		}
		return base.BeforeLeavePage(parameters);
	}
}
