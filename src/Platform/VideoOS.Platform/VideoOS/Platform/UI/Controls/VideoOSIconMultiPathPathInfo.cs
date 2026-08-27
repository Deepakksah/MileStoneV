using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Shapes;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconMultiPathPathInfo : INotifyPropertyChanged
{
	private Path __path;

	private bool __autoColor;

	public Path Path
	{
		get
		{
			return __path;
		}
		set
		{
			if (__path != value)
			{
				__path = value;
				OnPropertyChanged("Path");
			}
		}
	}

	public bool AutoColor
	{
		get
		{
			return __autoColor;
		}
		set
		{
			if (__autoColor != value)
			{
				__autoColor = value;
				OnPropertyChanged("AutoColor");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
