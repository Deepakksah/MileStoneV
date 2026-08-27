using System;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class MultipleCertificateSelectionViewModel
{
	public enum Mode
	{
		SameCertificateForAllServices,
		IndividualCertificateForEachService
	}

	public class ServiceViewModel
	{
		private MultipleCertificateSelectionViewModel parent;

		public bool EncryptionEnabled { get; set; } = true;

		public CertificateSelection2 CertificateSelection { get; set; } = CertificateSelection2.NoSelection;

		public bool IsSelectionValid { get; set; }

		public bool EncryptionEnabledResolved => ViewModelResolved.EncryptionEnabled;

		public CertificateSelection2 CertificateSelectionResolved => ViewModelResolved.CertificateSelection;

		public bool IsSelectionValidResolved => ViewModelResolved.IsSelectionValid;

		private ServiceViewModel ViewModelResolved
		{
			get
			{
				if (parent.SelectedMode != Mode.IndividualCertificateForEachService)
				{
					return parent.AllServicesViewModel;
				}
				return this;
			}
		}

		public ServiceViewModel(MultipleCertificateSelectionViewModel parent)
		{
			this.parent = parent;
		}
	}

	public Mode SelectedMode { get; set; }

	public ServiceViewModel AllServicesViewModel { get; }

	public ServiceViewModel PrimaryServiceViewModel { get; }

	public ServiceViewModel SecondaryServiceViewModel { get; }

	public bool IsSelectionValid
	{
		get
		{
			switch (SelectedMode)
			{
			case Mode.SameCertificateForAllServices:
				return AllServicesViewModel.IsSelectionValidResolved;
			case Mode.IndividualCertificateForEachService:
				if (PrimaryServiceViewModel.IsSelectionValidResolved)
				{
					return SecondaryServiceViewModel.IsSelectionValidResolved;
				}
				return false;
			default:
				throw new NotImplementedException("Missing implementation for custom mode.");
			}
		}
	}

	public MultipleCertificateSelectionViewModel()
	{
		AllServicesViewModel = new ServiceViewModel(this);
		PrimaryServiceViewModel = new ServiceViewModel(this);
		SecondaryServiceViewModel = new ServiceViewModel(this);
	}

	public void ResolveSelectedMode()
	{
		if (PrimaryServiceViewModel.EncryptionEnabled == SecondaryServiceViewModel.EncryptionEnabled && PrimaryServiceViewModel.CertificateSelection == SecondaryServiceViewModel.CertificateSelection)
		{
			SelectedMode = Mode.SameCertificateForAllServices;
			AllServicesViewModel.EncryptionEnabled = PrimaryServiceViewModel.EncryptionEnabled;
			AllServicesViewModel.CertificateSelection = PrimaryServiceViewModel.CertificateSelection;
		}
		else
		{
			SelectedMode = Mode.IndividualCertificateForEachService;
		}
	}
}
