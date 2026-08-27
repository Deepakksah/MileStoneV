using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSIconSourceBase : INotifyPropertyChanged
{
	private bool __followFlowDirection;

	public bool FollowFlowDirection
	{
		get
		{
			return __followFlowDirection;
		}
		set
		{
			if (value != __followFlowDirection)
			{
				__followFlowDirection = value;
				OnPropertyChanged("FollowFlowDirection");
			}
		}
	}

	public object BuiltInIconSourcePropertyChanged { get; }

	public event PropertyChangedEventHandler PropertyChanged;

	public VideoOSIconSourceBase()
	{
	}

	public void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
