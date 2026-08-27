using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public partial class WpfMessageBoxData : INotifyPropertyChanged
{
	private string _text;

	private string _detailsText;

	private WpfMessageBoxButton _controlButtons;

	private BitmapSource _attentionImageSource;

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			if (_text != value)
			{
				_text = value;
				NotifyPropertyChanged("Text");
			}
		}
	}

	public string DetailsText
	{
		get
		{
			return _detailsText;
		}
		set
		{
			if (_detailsText != value)
			{
				_detailsText = value;
				NotifyPropertyChanged("DetailsText");
			}
		}
	}

	public WpfMessageBoxButton ControlButtons
	{
		get
		{
			return _controlButtons;
		}
		set
		{
			if (_controlButtons != value)
			{
				_controlButtons = value;
				NotifyPropertyChanged("ControlButtons");
			}
		}
	}

	public BitmapSource AttentionImageSource
	{
		get
		{
			return _attentionImageSource;
		}
		set
		{
			if (_attentionImageSource != value)
			{
				_attentionImageSource = value;
				NotifyPropertyChanged("AttentionImageSource");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public WpfMessageBoxData(string text, string detailsText, WpfMessageBoxButton button, BitmapSource imageSource)
	{
		_text = text;
		_detailsText = detailsText;
		_controlButtons = button;
		_attentionImageSource = imageSource;
	}

	private void NotifyPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
