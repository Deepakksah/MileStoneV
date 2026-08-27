using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class CertificateSelectionControlViewModel
{
	private CertificateSelection2 _certificateSelection = CertificateSelection2.NoSelection;

	private CertificateData _selectedCertificateData;

	private const StoreName fixedStoreName = StoreName.My;

	private const StoreLocation fixedStoreLocation = StoreLocation.LocalMachine;

	private ICertificateSelectionControlModel _certificateSelectionControlModel;

	public CertificateSelection2 CertificateSelection
	{
		get
		{
			return _certificateSelection;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_certificateSelection = value;
			_selectedCertificateData = null;
		}
	}

	public bool EncryptionCheckBoxChecked { get; set; }

	public CertificateData SelectedCertificateData
	{
		get
		{
			if (CertificateSelection == CertificateSelection2.NoSelection)
			{
				return null;
			}
			if (_selectedCertificateData == null)
			{
				_selectedCertificateData = AvailableCertificates.FirstOrDefault((CertificateData c) => c.CertificateHash.SequenceEqual(CertificateSelection.CertificateHash));
			}
			return _selectedCertificateData;
		}
	}

	public bool SelectCertificateButtonEnabled => EncryptionCheckBoxChecked;

	public bool CertificateDetailsButtonEnabled
	{
		get
		{
			if (EncryptionCheckBoxChecked)
			{
				return SelectionType == CertificateSelectionType.Certificate;
			}
			return false;
		}
	}

	public CertificateSelectionType SelectionType
	{
		get
		{
			if (!(CertificateSelection == CertificateSelection2.NoSelection))
			{
				if (SelectedCertificateData != null)
				{
					return CertificateSelectionType.Certificate;
				}
				return CertificateSelectionType.CertificateNotFound;
			}
			return CertificateSelectionType.NoSelection;
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

	public CertificateData[] AvailableCertificates { get; private set; }

	public CertificateSelectionControlViewModel()
		: this(new CertificateSelectionControlModel())
	{
	}

	public CertificateSelectionControlViewModel(ICertificateSelectionControlModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		_certificateSelectionControlModel = model;
		AvailableCertificates = _certificateSelectionControlModel.GetCertificates(StoreName.My, StoreLocation.LocalMachine).FilterServerValidCertificates().ToArray();
		EncryptionCheckBoxChecked = true;
		CertificateSelection = CertificateSelection2.NoSelection;
	}
}
