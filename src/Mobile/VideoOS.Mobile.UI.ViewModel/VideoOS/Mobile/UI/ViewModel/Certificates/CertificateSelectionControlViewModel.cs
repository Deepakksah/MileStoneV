using System;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Mobile.Helpers.Certificates;

namespace VideoOS.Mobile.UI.ViewModel.Certificates;

public class CertificateSelectionControlViewModel
{
	private const StoreName fixedStoreName = StoreName.My;

	private const StoreLocation fixedStoreLocation = StoreLocation.LocalMachine;

	private ICertificateSelectionControlModel _certificateSelectionControlModel;

	private CertificateData _selectedCertificate;

	public CertificateData SelectedCertificate
	{
		get
		{
			return _selectedCertificate;
		}
		set
		{
			_selectedCertificate = value;
		}
	}

	public bool SelectCertificateButtonEnabled => EncryptionCheckBoxChecked;

	public bool CertificateDetailsButtonEnabled
	{
		get
		{
			if (EncryptionCheckBoxChecked)
			{
				return SelectedCertificate != null;
			}
			return false;
		}
	}

	public bool EncryptionCheckBoxChecked { get; set; }

	public CertificateSelectionType SelectionType
	{
		get
		{
			if (SelectedCertificate == null)
			{
				return CertificateSelectionType.NoSelection;
			}
			return CertificateSelectionType.Certificate;
		}
	}

	public bool IsSelectionValid
	{
		get
		{
			if (!EncryptionCheckBoxChecked || SelectionType != CertificateSelectionType.Certificate)
			{
				return !EncryptionCheckBoxChecked;
			}
			return true;
		}
	}

	public X509Certificate2Collection AvailableCertificates { get; private set; }

	public CertificateSelectionControlViewModel()
		: this(new Certificate())
	{
	}

	public CertificateSelectionControlViewModel(ICertificateSelectionControlModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		_certificateSelectionControlModel = model;
		SelectedCertificate = null;
		EncryptionCheckBoxChecked = true;
	}

	public X509Certificate2Collection GetCertificatesCollection()
	{
		X509Certificate2Collection x509Certificate2Collection = _certificateSelectionControlModel.GetCertificates(StoreName.My, StoreLocation.LocalMachine).FilterServerValidCertificates().ToX509Collection();
		x509Certificate2Collection.MoveItemToLastPosition(SelectedCertificate?.Certificate?.Thumbprint);
		return x509Certificate2Collection;
	}
}
