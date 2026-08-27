using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework.PageActions;

public class LanguagePageActions : PageSetupActionsBase
{
	public override string PageName => "LanguagePage";

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (CryptoConfig.AllowOnlyFipsAlgorithms && ProductCompliance.ProductComplianceVersion < ProductComplianceVersion.V1)
		{
			Func<string> func = () => Resources.ErrorMessage_FipsEnabledOnUpgradeTitle;
			Func<ContentLinkText> lookupErrorPageBody = () => new ContentLinkText(Resources.ErrorMessage_FipsEnabledOnUpgrade, (Resources.ErrorMessage_FipsEnabledOnUpgradeLinkText, new Uri("https://docs.microsoft.com/en-us/windows/security/threat-protection/security-policy-settings/system-cryptography-use-fips-compliant-algorithms-for-encryption-hashing-and-signing")));
			throw new ComplianceException(func(), new DetailedErrorException(func, lookupErrorPageBody, enablePreviousButton: false));
		}
		return base.BeforeLeavePage(parameters);
	}
}
