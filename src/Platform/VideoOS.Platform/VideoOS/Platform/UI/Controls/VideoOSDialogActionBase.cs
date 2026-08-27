using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSDialogActionBase : INotifyPropertyChanged
{
	public enum Placements
	{
		Left,
		Right
	}

	private bool __isEnabled = true;

	private bool __isVisible = true;

	private Placements __placement = Placements.Right;

	public bool IsEnabled
	{
		get
		{
			return __isEnabled;
		}
		set
		{
			if (__isEnabled != value)
			{
				__isEnabled = value;
				OnPropertyChanged("IsEnabled");
			}
		}
	}

	public bool IsVisible
	{
		get
		{
			return __isVisible;
		}
		set
		{
			if (__isVisible != value)
			{
				__isVisible = value;
				OnPropertyChanged("IsVisible");
			}
		}
	}

	public Placements Placement
	{
		get
		{
			return __placement;
		}
		set
		{
			if (__placement != value)
			{
				__placement = value;
				OnPropertyChanged("Placement");
			}
		}
	}

	public string AutomationId { get; set; } = string.Empty;

	public abstract bool CanResize { get; }

	public virtual double DesiredWidthPadding => 0.0;

	public event PropertyChangedEventHandler PropertyChanged;

	public VideoOSDialogActionBase()
	{
	}

	public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
