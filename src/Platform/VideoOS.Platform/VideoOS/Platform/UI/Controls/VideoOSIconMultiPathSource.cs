using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconMultiPathSource : VideoOSIconSourceBase
{
	private ICollection<VideoOSIconMultiPathPathInfo> __paths = new ObservableCollection<VideoOSIconMultiPathPathInfo>();

	public ICollection<VideoOSIconMultiPathPathInfo> Paths
	{
		get
		{
			return __paths;
		}
		set
		{
			if (__paths != value)
			{
				__paths = value;
				OnPropertyChanged("Paths");
			}
		}
	}
}
