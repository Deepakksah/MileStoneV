using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class CertificateGroupViewModel : INotifyPropertyChanged
{
	private readonly ICertificateCmdModel _commandModel;

	private readonly Guid _groupId;

	private bool _useEncryption;

	private CertificateViewModel _selectedCertificate;

	private CommandHandler _CertificateDetailsCommand;

	public Guid GroupId => _groupId;

	public string Title { get; }

	public string Subtitle { get; }

	public bool UseEncryption
	{
		get
		{
			return _useEncryption;
		}
		set
		{
			_commandModel.SetUseEncryption(value, _groupId);
		}
	}

	public ObservableCollection<ServerViewModel> Servers { get; }

	public ObservableCollection<CertificateViewModel> AvailableCertificates { get; }

	public CertificateViewModel SelectedCertificate
	{
		get
		{
			return _selectedCertificate;
		}
		set
		{
			_commandModel.SetCertificate(value?.UnderlyingSource?.Thumbprint, _groupId);
		}
	}

	public bool CertificateIsValid { get; private set; }

	public bool IsUnlocked { get; private set; }

	public bool IsEncryptionUnlocked { get; private set; }

	public bool CertificateIsValidOrEmpty
	{
		get
		{
			if (!CertificateIsValid)
			{
				return SelectedCertificate?.UnderlyingSource == null;
			}
			return true;
		}
	}

	public ICommand CertificateDetailsCommand
	{
		get
		{
			if (_CertificateDetailsCommand == null)
			{
				_CertificateDetailsCommand = new CommandHandler(delegate
				{
					ShowCertificateDetails();
				});
				_CertificateDetailsCommand.CanExecute = SelectedCertificate?.UnderlyingSource != null;
				PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
				{
					if (e.PropertyName == "SelectedCertificate")
					{
						_CertificateDetailsCommand.CanExecute = SelectedCertificate?.UnderlyingSource != null;
					}
				};
			}
			return _CertificateDetailsCommand;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public CertificateGroupViewModel(string title, string subtitle, ICertificateCmdModel commandModel, Guid groupId, bool useEncryption, IEnumerable<ServerViewModel> servers, IEnumerable<CertificateViewModel> availableCertificates, CertificateViewModel selectedCertificate, bool certificateIsValid, bool locked, bool lockEncryption)
	{
		Title = title;
		Subtitle = subtitle;
		_commandModel = commandModel;
		_groupId = groupId;
		_useEncryption = useEncryption;
		Servers = new ObservableCollection<ServerViewModel>(servers);
		AvailableCertificates = new ObservableCollection<CertificateViewModel>(availableCertificates);
		_selectedCertificate = selectedCertificate;
		CertificateIsValid = certificateIsValid;
		IsUnlocked = !locked;
		IsEncryptionUnlocked = !lockEncryption && !locked;
	}

	public void Update(CertificateGroupViewModel viewModel)
	{
		if (_useEncryption != viewModel.UseEncryption)
		{
			_useEncryption = viewModel.UseEncryption;
			OnPropertyChanged("UseEncryption");
		}
		if (_selectedCertificate != viewModel.SelectedCertificate)
		{
			_selectedCertificate = viewModel.SelectedCertificate;
			OnPropertyChanged("SelectedCertificate");
			OnPropertyChanged("CertificateIsValidOrEmpty");
		}
		if (CertificateIsValid != viewModel.CertificateIsValid)
		{
			CertificateIsValid = viewModel.CertificateIsValid;
			OnPropertyChanged("CertificateIsValid");
			OnPropertyChanged("CertificateIsValidOrEmpty");
		}
		if (IsUnlocked != viewModel.IsUnlocked)
		{
			IsUnlocked = viewModel.IsUnlocked;
			OnPropertyChanged("IsUnlocked");
		}
		if (IsEncryptionUnlocked != viewModel.IsEncryptionUnlocked)
		{
			IsEncryptionUnlocked = viewModel.IsEncryptionUnlocked;
			OnPropertyChanged("IsEncryptionUnlocked");
		}
		Servers.UpdateKeyedItems(viewModel.Servers, (ServerViewModel vm) => vm.ServerEndpointId);
		AvailableCertificates.UpdateKeyedItems(viewModel.AvailableCertificates, (CertificateViewModel vm) => vm.Thumbprint);
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void ShowCertificateDetails()
	{
		if (System.Windows.Application.Current != null)
		{
			Window parent = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault((Window x) => x.IsActive);
			SelectedCertificate.ShowCertificateDetails(parent);
		}
		else if (Form.ActiveForm != null)
		{
			Form activeForm = Form.ActiveForm;
			SelectedCertificate.ShowCertificateDetails(activeForm);
		}
		else
		{
			SelectedCertificate.ShowCertificateDetails();
		}
	}
}
