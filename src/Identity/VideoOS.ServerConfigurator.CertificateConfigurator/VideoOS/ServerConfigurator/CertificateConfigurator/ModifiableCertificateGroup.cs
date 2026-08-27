using System.Collections.Generic;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class ModifiableCertificateGroup : CertificateGroup
{
	public ModifiableCertificateGroup(CertificateGroup source)
		: base(source)
	{
	}

	public void SetSelectedCertificate(ICertificate newValue)
	{
		base.SelectedCertificate = newValue;
		base.SelectedCertificateIsValid = IsSelectedCertificateValid();
	}

	public void SetUseEncryption(bool newValue)
	{
		base.UseEncryption = newValue;
		base.SelectedCertificateIsValid = IsSelectedCertificateValid();
	}

	public void SetAvailableCertificates(IEnumerable<ICertificate> certificates)
	{
		base.ValidCertificates = certificates;
		base.SelectedCertificateIsValid = IsSelectedCertificateValid();
	}

	private bool IsSelectedCertificateValid()
	{
		if (!base.UseEncryption)
		{
			return true;
		}
		if (base.SelectedCertificate == null)
		{
			return false;
		}
		return base.ValidCertificates.Contains(base.SelectedCertificate.Thumbprint);
	}

	public void SetLock(bool lockValue)
	{
		base.Locked = lockValue;
	}

	public void SetLockEncryption(bool lockValue)
	{
		base.LockEncryption = lockValue;
	}
}
