using System;
using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.Installer.RecorderPlugins;

public class ImageServerCommunicationGroup : ServerCertificateGroup
{
	public ImageServerCommunicationGroup(Func<LegacyEncryption> legacyEncryptionProvider)
		: base(ServerConfiguratorData.ImageServerCertificateGroupId, "ImageServerCommunicationTitle", "ImageServerCommunicationSubtitle", "IMAGESERVERENABLEENCRYPTION", "IMAGESERVERCERTIFICATETHUMBPRINT", "VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.ImageServerGroupPresenter, VideoOS.ServerConfigurator.CertificateConfigurator.Presenters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", legacyEncryptionProvider)
	{
	}
}
