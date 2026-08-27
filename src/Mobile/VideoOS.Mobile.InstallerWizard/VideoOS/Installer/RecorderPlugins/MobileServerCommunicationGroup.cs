using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Mobile.InstallerWizard;

namespace VideoOS.Installer.RecorderPlugins;

public class MobileServerCommunicationGroup : ServerCertificateGroup
{
	public MobileServerCommunicationGroup(Func<LegacyEncryption> legacyEncryptionProvider)
		: base(ServerConfiguratorData.MobileServerCertificateGroupId, "MobileServerCommunicationTitle", "MobileServerCommunicationSubtitle", "MOBILESERVERENABLEENCRYPTION", "MOBILESERVERCERTIFICATETHUMBPRINT", "VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.MobileServerGroupPresenter, VideoOS.ServerConfigurator.CertificateConfigurator.Presenters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", legacyEncryptionProvider, new List<string> { "ValidFrom", "ValidUntil", "HasPrivateKey" })
	{
	}
}
