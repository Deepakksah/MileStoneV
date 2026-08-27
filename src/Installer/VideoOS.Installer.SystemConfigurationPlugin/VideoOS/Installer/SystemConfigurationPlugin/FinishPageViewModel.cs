using System;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class FinishPageViewModel : ViewModelBase
{
	private string _webClientHeader = "";

	private string _webClientAddress = "";

	private string _webClientLabel = "";

	private string _mobileClientAddress = "";

	private string _mobilClientLabel = "";

	private bool _openClient = true;

	private string _clientLabel = "";

	private string _downloadManagerLabel = "";

	private bool _openDownloadManager = true;

	private ViewModelBase _contentViewModel;

	private string _header = "";

	private string _headerInfo = "";

	private bool _openDownloadManagerVisibility = true;

	private bool _openClientVisibility = true;

	private bool _summaryVisibility = true;

	private bool _webMobileClientVisibility;

	private bool _prepareFinalizeVisibility;

	private bool _prepareFinalize;

	private string _prepareFinalizeLabel = string.Empty;

	private string _prepareFinalizeLabelExplanation = string.Empty;

	private bool _removeInstallLogVisibility;

	private bool _removeInstallLog;

	private string _removeInstallLogLabel = string.Empty;

	public string Header
	{
		get
		{
			return _header;
		}
		set
		{
			_header = value;
			OnPropertyChanged("Header");
		}
	}

	public string HeaderInfo
	{
		get
		{
			return _headerInfo;
		}
		set
		{
			_headerInfo = value;
			OnPropertyChanged("HeaderInfo");
		}
	}

	public string WebClientHeader
	{
		get
		{
			return _webClientHeader;
		}
		set
		{
			_webClientHeader = value;
			OnPropertyChanged("WebClientHeader");
		}
	}

	public string WebClientLabel
	{
		get
		{
			return _webClientLabel;
		}
		set
		{
			_webClientLabel = value;
			OnPropertyChanged("WebClientLabel");
		}
	}

	public string WebClientAddress
	{
		get
		{
			return _webClientAddress;
		}
		set
		{
			_webClientAddress = value;
			OnPropertyChanged("WebClientAddress");
		}
	}

	public bool WebMobileClientVisibility
	{
		get
		{
			return _webMobileClientVisibility;
		}
		set
		{
			_webMobileClientVisibility = value;
			OnPropertyChanged("WebMobileClientVisibility");
		}
	}

	public string MobileClientLabel
	{
		get
		{
			return _mobilClientLabel;
		}
		set
		{
			_mobilClientLabel = value;
			OnPropertyChanged("MobileClientLabel");
		}
	}

	public string MobileClientAddress
	{
		get
		{
			return _mobileClientAddress;
		}
		set
		{
			_mobileClientAddress = value;
			OnPropertyChanged("MobileClientAddress");
		}
	}

	public ViewModelBase ContentViewModel
	{
		get
		{
			return _contentViewModel;
		}
		set
		{
			_contentViewModel = value;
			OnPropertyChanged("ContentViewModel");
		}
	}

	public bool SummaryVisibility
	{
		get
		{
			return _summaryVisibility;
		}
		set
		{
			_summaryVisibility = value;
			OnPropertyChanged("SummaryVisibility");
		}
	}

	public bool OpenClientVisibility
	{
		get
		{
			return _openClientVisibility;
		}
		set
		{
			_openClientVisibility = value;
			OnPropertyChanged("OpenClientVisibility");
		}
	}

	public bool OpenClient
	{
		get
		{
			return _openClient;
		}
		set
		{
			_openClient = value;
		}
	}

	public string ClientLabel
	{
		get
		{
			return _clientLabel;
		}
		set
		{
			_clientLabel = value;
			OnPropertyChanged("ClientLabel");
		}
	}

	public bool OpenDownloadManagerVisibility
	{
		get
		{
			return _openDownloadManagerVisibility;
		}
		set
		{
			_openDownloadManagerVisibility = value;
			OnPropertyChanged("OpenDownloadManagerVisibility");
		}
	}

	public bool OpenDownloadManager
	{
		get
		{
			return _openDownloadManager;
		}
		set
		{
			_openDownloadManager = value;
		}
	}

	public string DownloadManagerLabel
	{
		get
		{
			return _downloadManagerLabel;
		}
		set
		{
			_downloadManagerLabel = value;
			OnPropertyChanged("DownloadManagerLabel");
		}
	}

	public bool PrepareFinalizeVisibility
	{
		get
		{
			return _prepareFinalizeVisibility;
		}
		set
		{
			_prepareFinalizeVisibility = value;
			OnPropertyChanged("PrepareFinalizeVisibility");
		}
	}

	public bool PrepareFinalize
	{
		get
		{
			return _prepareFinalize;
		}
		set
		{
			_prepareFinalize = value;
			OnPropertyChanged("PrepareFinalize");
		}
	}

	public string PrepareFinalizeLabel
	{
		get
		{
			return _prepareFinalizeLabel;
		}
		set
		{
			_prepareFinalizeLabel = value;
			OnPropertyChanged("PrepareFinalizeLabel");
		}
	}

	public string PrepareFinalizeLabelExplanation
	{
		get
		{
			return _prepareFinalizeLabelExplanation;
		}
		set
		{
			_prepareFinalizeLabelExplanation = value;
			OnPropertyChanged("PrepareFinalizeLabelExplanation");
		}
	}

	public bool RemoveInstallLogVisibility
	{
		get
		{
			return _removeInstallLogVisibility;
		}
		set
		{
			_removeInstallLogVisibility = value;
			OnPropertyChanged("RemoveInstallLogVisibility");
		}
	}

	public bool RemoveInstallLog
	{
		get
		{
			return _removeInstallLog;
		}
		set
		{
			_removeInstallLog = value;
			OnPropertyChanged("RemoveInstallLog");
		}
	}

	public string RemoveInstallLogLabel
	{
		get
		{
			return _removeInstallLogLabel;
		}
		set
		{
			_removeInstallLogLabel = value;
			OnPropertyChanged("RemoveInstallLogLabel");
		}
	}

	public FinishPageViewModel(ViewModelBase contentViewModel)
	{
		if (contentViewModel == null)
		{
			throw new ArgumentNullException("contentViewModel");
		}
		_contentViewModel = contentViewModel;
	}
}
