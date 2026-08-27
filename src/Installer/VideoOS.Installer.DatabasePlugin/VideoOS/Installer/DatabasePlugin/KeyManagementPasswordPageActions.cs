using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.Properties;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Security;

namespace VideoOS.Installer.DatabasePlugin;

public class KeyManagementPasswordPageActions : PageSetupActionsBase
{
	public SystemPasswordMode SystemPasswordMode;

	private IEncryptionKeyReader _encryptionKeyReader;

	private IGlobalData _globalData;

	private IPasswordValidator _passwordValidator;

	public override string PageName => "KeyManagementPasswordWizardPage";

	public IEncryptionKeyReader EncryptionKeyReader
	{
		get
		{
			return _encryptionKeyReader ?? (_encryptionKeyReader = KeyManagementPasswordHelper.CreateKeyManager(LogManager.InnerLog));
		}
		set
		{
			_encryptionKeyReader = value;
		}
	}

	public IGlobalData GlobalData
	{
		get
		{
			return _globalData ?? (_globalData = new GlobalDataWrapper());
		}
		public set
		{
			_globalData = value;
		}
	}

	public IPasswordValidator PasswordValidator
	{
		get
		{
			if (_passwordValidator == null)
			{
				_passwordValidator = new PasswordValidator();
			}
			return _passwordValidator;
		}
		set
		{
			_passwordValidator = value;
		}
	}

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
		try
		{
			SystemPasswordMode = KeyManagementPasswordHelper.GetSystemPasswordMode(EncryptionKeyReader, GlobalData, LogManager.InnerLog);
		}
		catch (KeyStorageException ex) when (ex.InnerException is UnauthorizedAccessException)
		{
			throw new UIFrameworkErrorException(Resources.AccessDenied_ErrorOccurred);
		}
		catch (KeyStorageException ex2)
		{
			throw new UIFrameworkErrorException(Resources.KeyStorageExceptionGenericMessage + " " + ex2.Message, enablePreviousButtong: true);
		}
		bool flag = SystemPasswordMode != SystemPasswordMode.NoPasswordRequired;
		return base.BeforeActivatePage(parameters) && flag;
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return base.BeforeLeavePage(parameters);
		}
		if (GlobalData.GetValueAsBoolean("QUIET-MODE"))
		{
			SecureString secureString = string.Empty.EncryptString();
			if (parameters.TryGetValue("SYSTEMPASSWORD", out var value) && value != null)
			{
				secureString = EncryptHelper.DecryptPassword(value, unicodePassword: true).EncryptString();
			}
			switch (SystemPasswordMode)
			{
			case SystemPasswordMode.EnterExistingPassword:
			{
				string text = KeyManagementPasswordHelper.VerifyExistingPassword(EncryptionKeyReader, secureString, acceptEmptyPassword: true, LogManager.InnerLog);
				if (!string.IsNullOrEmpty(text))
				{
					throw new UnauthorizedException(FormattableString.Invariant($"Existing System Password : {text}"));
				}
				break;
			}
			case SystemPasswordMode.CreateNewPassword:
			{
				PasswordValidationResult passwordValidationResult = PasswordValidator.ValidateNewPassword(secureString, secureString);
				if (passwordValidationResult != PasswordValidationResult.Ok && passwordValidationResult != PasswordValidationResult.PasswordIsEmpty)
				{
					throw new UnauthorizedException(FormattableString.Invariant($"New System Password : {passwordValidationResult}"));
				}
				break;
			}
			default:
				throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}. Value was {1}", "SystemPasswordMode", SystemPasswordMode)));
			case SystemPasswordMode.NoPasswordRequired:
				break;
			}
		}
		return base.BeforeLeavePage(parameters);
	}
}
