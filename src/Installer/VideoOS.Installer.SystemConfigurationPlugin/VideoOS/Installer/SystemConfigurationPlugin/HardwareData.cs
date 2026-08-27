using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareData : EnabledComponent, IEnabledComponent, INotifyPropertyChanged
{
	private bool _expandComponents;

	private DelegateCommand _expandComponentsCommand;

	private bool _editMode;

	private HardwareConfiguration _configuration;

	private bool _enabled;

	private bool _error;

	private string _errorInfo;

	public HardwareConfiguration HardwareConfiguration => _configuration;

	public ICommand ExpandComponentsCommand => _expandComponentsCommand;

	public bool EditMode
	{
		get
		{
			return _editMode;
		}
		set
		{
			_editMode = value;
			OnPropertyChanged("EditMode");
		}
	}

	public bool ExpandComponents
	{
		get
		{
			return _expandComponents;
		}
		set
		{
			if (_expandComponents != value)
			{
				_expandComponents = value;
				OnPropertyChanged("ExpandComponents");
				if (value)
				{
					this.ComponentExpandedEvent(this, new EventArgs());
				}
			}
		}
	}

	public bool Error
	{
		get
		{
			return _error;
		}
		set
		{
			_error = value;
		}
	}

	public string ErrorInfo
	{
		get
		{
			return _errorInfo;
		}
		set
		{
			_errorInfo = value;
			OnPropertyChanged("ErrorInfo");
		}
	}

	public override string Name
	{
		get
		{
			return _configuration.Name;
		}
		set
		{
			if (!(_configuration.Name != value))
			{
				return;
			}
			_configuration.Name = value;
			OnPropertyChanged("Name");
			foreach (HardwareComponent hardwareComponent in HardwareComponents)
			{
				hardwareComponent.RenameComponents(value);
			}
			foreach (HardwareComponent item in HardwareComponentsNotShown)
			{
				item.RenameComponents(value);
			}
		}
	}

	public override bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled != value)
			{
				_enabled = value;
				OnPropertyChanged("Enabled");
			}
		}
	}

	public ObservableCollection<HardwareComponent> HardwareComponents { get; } = new ObservableCollection<HardwareComponent>();

	public ObservableCollection<HardwareComponent> HardwareComponentsNotShown { get; } = new ObservableCollection<HardwareComponent>();

	public event EventHandler ComponentExpandedEvent = delegate
	{
	};

	public HardwareData(HardwareConfiguration configuration)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		_enabled = configuration.Validated;
		_error = !configuration.Validated;
		_errorInfo = Resources.HardwareSelectionInvalidHardwareInfo;
		_configuration = configuration;
		if (configuration.Devices != null)
		{
			AddDevice<CameraConfiguration, CameraComponent>(configuration, HardwareComponents);
			AddDevice<MicrophoneConfiguration, MicrophoneComponent>(configuration, HardwareComponents);
			AddDevice<SpeakerConfiguration, SpeakerComponent>(configuration, HardwareComponents);
			AddDevice<MetadataConfiguration, MetadataComponent>(configuration, HardwareComponentsNotShown);
			AddDevice<InputConfiguration, InputComponent>(configuration, HardwareComponentsNotShown);
			AddDevice<OutputConfiguration, OutputComponent>(configuration, HardwareComponentsNotShown);
		}
		_expandComponentsCommand = new DelegateCommand(ExpandComponentsOnCommand);
	}

	private void ExpandComponentsOnCommand(object state)
	{
		ExpandComponents = !_expandComponents;
	}

	private static void AddDevice<T, H>(HardwareConfiguration configuration, ObservableCollection<HardwareComponent> hardwareCollection) where T : DeviceConfiguration where H : HardwareComponent
	{
		IEnumerable<DeviceConfiguration> source = configuration.Devices.Where((DeviceConfiguration deviceConfiguration) => deviceConfiguration is T);
		if (source.Count() > 0)
		{
			HardwareComponent item = Activator.CreateInstance(typeof(H), source.Select((DeviceConfiguration c) => new EnabledComponent(c))) as HardwareComponent;
			hardwareCollection.Add(item);
		}
	}
}
