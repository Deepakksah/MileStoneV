using System;
using System.Globalization;
using System.Windows.Input;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class EnabledComponent : ViewModelBase, IEnabledComponent
{
	private DelegateCommand _editCommand;

	private DelegateCommand _cancelEditCommand;

	private bool _isEditing;

	private string _previousStoredName;

	private DeviceConfiguration _configuration;

	public virtual bool Enabled
	{
		get
		{
			return _configuration.Enabled;
		}
		set
		{
			_configuration.Enabled = value;
			OnPropertyChanged("Enabled");
		}
	}

	public ICommand EditCommand => _editCommand;

	public ICommand CancelEditCommand => _cancelEditCommand;

	public virtual string Name
	{
		get
		{
			return _configuration.Name;
		}
		set
		{
			_configuration.Name = value;
			OnPropertyChanged("Name");
		}
	}

	public DeviceConfiguration Configuration => _configuration;

	public bool IsEditing
	{
		get
		{
			return _isEditing;
		}
		set
		{
			if (_isEditing != value)
			{
				if (value)
				{
					_previousStoredName = Name;
				}
				else if (string.IsNullOrEmpty(Name))
				{
					Name = _previousStoredName;
				}
				_isEditing = value;
				OnPropertyChanged("IsEditing");
			}
		}
	}

	public EnabledComponent()
	{
		_editCommand = new DelegateCommand(IsEditingAction);
		_cancelEditCommand = new DelegateCommand(CancelEditingAction);
	}

	public EnabledComponent(DeviceConfiguration configuration)
	{
		_configuration = configuration;
		_editCommand = new DelegateCommand(IsEditingAction);
		_cancelEditCommand = new DelegateCommand(CancelEditingAction);
	}

	private void IsEditingAction(object edit)
	{
		IsEditing = Convert.ToBoolean(edit, CultureInfo.InvariantCulture);
	}

	private void CancelEditingAction(object edit)
	{
		IsEditing = false;
		Name = _previousStoredName;
	}
}
