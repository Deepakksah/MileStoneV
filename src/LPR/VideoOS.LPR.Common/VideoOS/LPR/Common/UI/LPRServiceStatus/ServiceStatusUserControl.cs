using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.UI.LPRServiceStatus;

public partial class ServiceStatusUserControl : UserControl, IComponentConnector
{
	private GetStatusResp _status;

	public GetStatusResp Status
	{
		get
		{
			return _status;
		}
		set
		{
			_status = value;
			UpdateUI();
		}
	}

	public ServiceStatusUserControl()
	{
		InitializeComponent();
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		UpdateUI();
	}

	private void _videoSourcesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		_videoSourcesListView.UnselectAll();
	}

	private void UpdateUI()
	{
		if (base.IsLoaded)
		{
			LPRServiceStatusHelper lPRServiceStatusHelper = new LPRServiceStatusHelper(_status, CultureInfo.CurrentUICulture);
			_statusLabel.Content = lPRServiceStatusHelper.StatusText;
			_uptimeLabel.Content = lPRServiceStatusHelper.UptimeText;
			_cpuLabel.Content = lPRServiceStatusHelper.CpuUsageText;
			_memoryLabel.Content = lPRServiceStatusHelper.MemoryAvailableText;
			_detectedLicensePlatesCountLabel.Content = lPRServiceStatusHelper.LicensePlatesDetectedText;
			_videoSourcesListView.ItemsSource = lPRServiceStatusHelper.VideoSourceStatusModels;
		}
	}
}
