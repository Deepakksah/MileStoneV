using System;
using System.Collections.Generic;

namespace VideoOS.ServerConfigurator.Interfaces;

public class CertificateGroup
{
	private bool _useEncryption;

	private ICertificate _selectedCertificate;

	public Guid Id { get; private set; }

	public string TitleKey { get; }

	public string SubtitleKey { get; }

	public bool UseEncryption
	{
		get
		{
			return _useEncryption;
		}
		protected set
		{
			if (Locked && value != _useEncryption)
			{
				throw new InvalidOperationException("Encryption state cannot be changed on a locked certificate group");
			}
			_useEncryption = value;
		}
	}

	public ICertificate SelectedCertificate
	{
		get
		{
			return _selectedCertificate;
		}
		protected set
		{
			if (Locked && value?.Thumbprint != _selectedCertificate?.Thumbprint)
			{
				throw new InvalidOperationException("Certificate cannot be changed on a locked certificate group");
			}
			_selectedCertificate = value;
		}
	}

	public IEnumerable<ICertificate> ValidCertificates { get; protected set; }

	public bool SelectedCertificateIsValid { get; protected set; }

	public ImplementationDefinition Presenter { get; private set; }

	public IEnumerable<string> FilterStrings { get; private set; }

	public bool Locked { get; protected set; }

	public bool LockEncryption { get; protected set; }

	public void SetPresenterFileLocation(string fileLocation)
	{
		Presenter.SetFileLocation(fileLocation);
	}

	protected CertificateGroup(CertificateGroup source)
		: this(source?.Id ?? Guid.Empty, source?.TitleKey, source?.SubtitleKey, source?.UseEncryption ?? false, source?.SelectedCertificate, source?.Presenter.AssemblyQualifiedName, source?.FilterStrings)
	{
		ValidCertificates = source?.ValidCertificates;
		SetPresenterFileLocation(source?.Presenter.FileLocation);
	}

	public CertificateGroup(Guid id, string titleKey, string subtitleKey, bool useEncryption, ICertificate selectedCertificate, string presenterImplementationType, IEnumerable<string> filterStrings)
	{
		Id = id;
		TitleKey = titleKey;
		SubtitleKey = subtitleKey;
		UseEncryption = useEncryption;
		SelectedCertificate = selectedCertificate;
		Presenter = new ImplementationDefinition(presenterImplementationType);
		FilterStrings = filterStrings ?? new List<string>();
	}

	public void Merge(CertificateGroup mergeInstance)
	{
		if (mergeInstance == null)
		{
			throw new ArgumentNullException("mergeInstance");
		}
		if (Id != mergeInstance.Id)
		{
			throw new InvalidOperationException(FormattableString.Invariant($"Instances with different group Id's cannot be merged. This Id = {Id}, mergeInstance.Id = {mergeInstance.Id}"));
		}
		Presenter = mergeInstance.Presenter;
		SelectedCertificate = mergeInstance.SelectedCertificate ?? SelectedCertificate;
		UseEncryption = mergeInstance.UseEncryption || UseEncryption;
	}
}
