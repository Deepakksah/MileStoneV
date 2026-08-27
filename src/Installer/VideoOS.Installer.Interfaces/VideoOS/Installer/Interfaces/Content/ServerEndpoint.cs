using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Interfaces.Content;

public class ServerEndpoint
{
	public Guid Id { get; }

	public string DisplayNameKey { get; }

	public Guid CertificateGroupId { get; }

	public string ApplyCertificateImplementation { get; }

	public string RegisterThisServerImplementationType { get; }

	public string SetSqlServerImplementationType { get; }

	public ServerEndpoint(Guid id, string displayNameKey, Guid certificateGroupId, string applyCertificateImplementation)
		: this(id, displayNameKey, certificateGroupId, applyCertificateImplementation, null, null)
	{
	}

	public ServerEndpoint(Guid id, string displayNameKey, Guid certificateGroupId, string applyCertificateImplementation, string registerThisServerImplementationType, string setSqlServerImplementationType)
	{
		if (certificateGroupId == Guid.Empty && applyCertificateImplementation != null)
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0} must be null when {1} is Guid.Empty", "applyCertificateImplementation", "certificateGroupId")));
		}
		if (certificateGroupId != Guid.Empty && applyCertificateImplementation == null)
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0} must be Guid.Empty when {1} is null", "certificateGroupId", "applyCertificateImplementation")));
		}
		if (applyCertificateImplementation == null && registerThisServerImplementationType == null && setSqlServerImplementationType == null)
		{
			throw new InvalidOperationException("All registrars are null");
		}
		if (!(id != Guid.Empty))
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0} cannot be Guid.Empty", "id")));
		}
		Id = id;
		DisplayNameKey = displayNameKey;
		CertificateGroupId = certificateGroupId;
		ApplyCertificateImplementation = applyCertificateImplementation;
		RegisterThisServerImplementationType = registerThisServerImplementationType;
		SetSqlServerImplementationType = setSqlServerImplementationType;
	}
}
