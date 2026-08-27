using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareComponent : ViewModelBase
{
	private string _name = "";

	public virtual string GetComponentName => "";

	public string ComponentName
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			OnPropertyChanged("ComponentName");
		}
	}

	public ObservableCollection<EnabledComponent> Components { get; } = new ObservableCollection<EnabledComponent>();

	public HardwareComponent(IEnumerable<EnabledComponent> hardwareComponents)
	{
		if (hardwareComponents == null)
		{
			throw new ArgumentNullException("hardwareComponents");
		}
		foreach (EnabledComponent hardwareComponent in hardwareComponents)
		{
			Components.Add(hardwareComponent);
		}
	}

	public void RenameComponents(string hardwareName)
	{
		int num = 1;
		foreach (EnabledComponent component in Components)
		{
			component.Name = FormattableString.Invariant($"{hardwareName} - {GetComponentName} {num}");
			num++;
		}
	}
}
