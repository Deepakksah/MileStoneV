using System;

namespace VideoOS.Installer.Interfaces.Content;

public class ServerToServerCommunicationCertificateGroup : ServerCertificateGroup
{
	public ServerToServerCommunicationCertificateGroup()
		: this(null)
	{
	}

	public ServerToServerCommunicationCertificateGroup(Func<LegacyEncryption> legacyEncryptionProvider)
		: base(ServerCertificateGroup.ServerToServerCommunicationGroupId, "ServerToServerCommunicationTitle", "ServerToServerCommunicationSubtitle", "SERVERTOSERVERENABLEENCRYPTION", "SERVERTOSERVERCERTIFICATETHUMBPRINT", "VideoOS.ServerConfigurator.CertificateConfigurator.Presenters.ServerToServerGroupPresenter, VideoOS.ServerConfigurator.CertificateConfigurator.Presenters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89", legacyEncryptionProvider)
	{
	}
}
