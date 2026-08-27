using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using VideoOS.Common.Integration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicensePageViewModel : INotifyPropertyChanged, IDisposable
{
	private string _licensePath = string.Empty;

	private CancellationTokenSource _cancellationTokenSource;

	private bool _disposed;

	private readonly ILicenseHelper _licenseHelper;

	private string _browseFilter = "Licence (*.lic) |*.lic";

	public ICommand Browse { get; }

	public string HeaderText => Resources.LicensePageHeader;

	public string InfoText => string.Format(CultureInfo.CurrentCulture, Resources.LicensePageInfo, OemResources.Instance.App.ProductNameWithoutCompany);

	public string BrowseLicenseText => Resources.LicensePageBrowseLicense;

	public string BrowseButtonText => Resources.LicensePageBrowseButton;

	public ContentLinkText WhereToBuyText
	{
		get
		{
			if (Uri.TryCreate(OemResources.Instance.App.ProductResellerUrl, UriKind.Absolute, out var result))
			{
				string item = string.Format(CultureInfo.CurrentCulture, Resources.LicensePageWhereToBuyReseller, OemResources.Instance.App.CompanyName);
				return new ContentLinkText(Resources.LicensePageWhereToBuy, (item, result));
			}
			return null;
		}
	}

	public string LicensePath
	{
		get
		{
			return _licensePath;
		}
		set
		{
			_licensePath = value;
			OnPropertyChanged("LicensePath");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public LicensePageViewModel()
		: this(new LicenseHelper())
	{
	}

	public LicensePageViewModel(ILicenseHelper licenseHelper)
	{
		_licenseHelper = licenseHelper ?? throw new ArgumentNullException("licenseHelper");
		Browse = new DelegateCommand(delegate
		{
			BrowseLicenseFile();
		});
	}

	public void LookupLicenseAndSet()
	{
		string text = _licenseHelper.LookupLicense();
		if (_licenseHelper.CheckLicense(text))
		{
			LicensePath = text;
		}
	}

	public void Cancel()
	{
		_cancellationTokenSource?.Cancel();
	}

	public void ReloadResources()
	{
		OnPropertyChanged("HeaderText");
		OnPropertyChanged("InfoText");
		OnPropertyChanged("BrowseLicenseText");
		OnPropertyChanged("BrowseButtonText");
		OnPropertyChanged("WhereToBuyText");
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void BrowseLicenseFile()
	{
		using FileDialog fileDialog = new OpenFileDialog();
		fileDialog.Filter = _browseFilter;
		if (fileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(fileDialog.FileName))
		{
			LicensePath = fileDialog.FileName;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (!_disposed && _cancellationTokenSource != null)
			{
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
			}
			_disposed = true;
		}
	}
}
