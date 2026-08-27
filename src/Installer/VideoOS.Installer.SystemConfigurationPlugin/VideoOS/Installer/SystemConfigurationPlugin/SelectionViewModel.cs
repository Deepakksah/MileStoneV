using System;
using System.Collections.ObjectModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class SelectionViewModel : ViewModelBase
{
	private bool _enabled = true;

	private bool _hardwareSelection = true;

	private string _noCameraMessage;

	private bool _hardwareToShow;

	private string _header = "";

	private string _headerInfo = "";

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

	public bool HardwareToShow
	{
		get
		{
			return _hardwareToShow;
		}
		set
		{
			_hardwareToShow = value;
			OnPropertyChanged("HardwareToShow");
		}
	}

	public ObservableCollection<HardwareData> Hardware { get; }

	public bool HardwareSelection
	{
		get
		{
			return _hardwareSelection;
		}
		set
		{
			_hardwareSelection = value;
			OnPropertyChanged("HardwareSelection");
		}
	}

	public string NoCameraMessage
	{
		get
		{
			return _noCameraMessage;
		}
		set
		{
			_noCameraMessage = value;
			_hardwareToShow = false;
			OnPropertyChanged("NoCameraMessage");
		}
	}

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled == value)
			{
				return;
			}
			foreach (HardwareData item in Hardware)
			{
				item.Enabled = value;
			}
			_enabled = value;
			OnPropertyChanged("Enabled");
		}
	}

	public SelectionViewModel()
	{
		_hardwareToShow = false;
		Hardware = new ObservableCollection<HardwareData>();
	}

	public void SetHardware(HardwareData hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		hardware.ComponentExpandedEvent += Hardware_ComponentExpandedEvent;
		Hardware.Add(hardware);
		HardwareToShow = true;
	}

	private void Hardware_ComponentExpandedEvent(object sender, EventArgs e)
	{
		foreach (HardwareData item in Hardware)
		{
			if (item != sender)
			{
				item.ExpandComponents = false;
			}
		}
	}
}
