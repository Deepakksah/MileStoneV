using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.Content;

public class ServerCertificateGroup
{
	public static readonly Guid ServerToServerCommunicationGroupId = new Guid("84430EB7-847C-422D-AA00-7915CD0D7A65");

	public Guid Id { get; }

	public string TitleKey { get; }

	public string SubtitleKey { get; }

	public string EnableEncryptionParameterKey { get; }

	public string CertificateThumbprintParameterKey { get; }

	public string PresenterImplementation { get; }

	public Func<LegacyEncryption> LegacyEncryptionProvider { get; }

	public IEnumerable<string> FilterStrings { get; }

	public ServerCertificateGroup(Guid id, string titleKey, string subtitleKey, string enableEncryptionParameterKey, string certificateThumbprintParameterKey, string presenterImplementation)
		: this(id, titleKey, subtitleKey, enableEncryptionParameterKey, certificateThumbprintParameterKey, presenterImplementation, null, new List<string>())
	{
	}

	public ServerCertificateGroup(Guid id, string titleKey, string subtitleKey, string enableEncryptionParameterKey, string certificateThumbprintParameterKey, string presenterImplementation, Func<LegacyEncryption> legacyEncryptionProvider)
		: this(id, titleKey, subtitleKey, enableEncryptionParameterKey, certificateThumbprintParameterKey, presenterImplementation, legacyEncryptionProvider, new List<string>())
	{
	}

	public ServerCertificateGroup(Guid id, string titleKey, string subtitleKey, string enableEncryptionParameterKey, string certificateThumbprintParameterKey, string presenterImplementation, Func<LegacyEncryption> legacyEncryptionProvider, IEnumerable<string> filterStrings)
	{
		if (string.IsNullOrEmpty(enableEncryptionParameterKey))
		{
			throw new ArgumentNullException("enableEncryptionParameterKey");
		}
		Id = id;
		TitleKey = titleKey;
		SubtitleKey = subtitleKey;
		EnableEncryptionParameterKey = enableEncryptionParameterKey;
		CertificateThumbprintParameterKey = certificateThumbprintParameterKey;
		PresenterImplementation = presenterImplementation;
		LegacyEncryptionProvider = legacyEncryptionProvider;
		FilterStrings = filterStrings;
	}
}
