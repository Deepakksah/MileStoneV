using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.HyperlinkListView;

public class HyperlinkListViewData : INotifyPropertyChanged
{
	private object _tag;

	private string _link;

	public object Tag
	{
		get
		{
			return _tag;
		}
		set
		{
			_tag = value;
			NotifyPropertyChanged("Tag");
		}
	}

	public string Link
	{
		get
		{
			return _link;
		}
		set
		{
			_link = value;
			NotifyPropertyChanged("Link");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public HyperlinkListViewData(string link)
	{
		_link = link;
	}

	private void NotifyPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
