using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.VideoText.resources;

namespace VideoOS.UI.Common.WPF.VideoText;

public class SlideViewModel : INotifyPropertyChanged
{
	private ICollection<VideoOSActionBase> _rowActions;

	private SlideData _slideData = new SlideData();

	private SlideColor _foregroundDefault;

	private SlideColor _backgroundDefault;

	private bool _isReadOnly;

	public ObservableCollection<SlideColor> DefinedBrushes { get; private set; }

	public ObservableCollection<int> DefinedDurations { get; private set; }

	public SlideData SlideData
	{
		get
		{
			return _slideData;
		}
		set
		{
			_slideData = value;
		}
	}

	public bool IsValid => Text != Strings.VideoTextDefault;

	public bool IsReadOnly
	{
		get
		{
			return _isReadOnly;
		}
		set
		{
			if (_isReadOnly != value)
			{
				_isReadOnly = value;
				FirePropertyChangedEvent("IsReadOnly");
			}
		}
	}

	public string DisplayName
	{
		get
		{
			return _slideData.DisplayName;
		}
		set
		{
			if (_slideData.DisplayName != value)
			{
				_slideData.DisplayName = value;
				FirePropertyChangedEvent("DisplayName");
			}
		}
	}

	public string Text
	{
		get
		{
			return _slideData.Text;
		}
		set
		{
			if (_slideData.Text != value)
			{
				_slideData.Text = value;
				FirePropertyChangedEvent("Text");
			}
		}
	}

	public FontFamily FontFamily
	{
		get
		{
			return new FontFamily(_slideData.FontFamilyName);
		}
		set
		{
			if (_slideData.FontFamilyName != value.Source)
			{
				_slideData.FontFamilyName = value.Source;
				FirePropertyChangedEvent("FontFamily");
			}
		}
	}

	public double FontSize
	{
		get
		{
			return _slideData.FontSize;
		}
		set
		{
			if (_slideData.FontSize != value)
			{
				_slideData.FontSize = value;
				FirePropertyChangedEvent("FontSize");
			}
		}
	}

	public SlideColor BackgroundBrush
	{
		get
		{
			Color color = (Color)ColorConverter.ConvertFromString(_slideData.BackgroundColor);
			return FindDefinedBrush(color, _backgroundDefault);
		}
		set
		{
			_slideData.BackgroundColor = value.SolidColorBrush.Color.ToString(CultureInfo.InvariantCulture);
			FirePropertyChangedEvent("BackgroundBrush");
		}
	}

	public SlideColor ForegroundBrush
	{
		get
		{
			Color color = (Color)ColorConverter.ConvertFromString(_slideData.ForegroundColor);
			return FindDefinedBrush(color, _foregroundDefault);
		}
		set
		{
			_slideData.ForegroundColor = value.SolidColorBrush.Color.ToString(CultureInfo.InvariantCulture);
			FirePropertyChangedEvent("ForegroundBrush");
		}
	}

	public int Duration
	{
		get
		{
			return _slideData.Duration;
		}
		set
		{
			if (_slideData.Duration != value)
			{
				_slideData.Duration = value;
				FirePropertyChangedEvent("Duration");
			}
		}
	}

	public ICollection<VideoOSActionBase> RowActions
	{
		get
		{
			return _rowActions;
		}
		set
		{
			if (_rowActions != value)
			{
				_rowActions = value;
				FirePropertyChangedEvent("RowActions");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public SlideViewModel()
	{
		DefinedDurations = BuildDefinedDurations();
		DefinedBrushes = BuildDefinedBrushes();
		Text = Strings.VideoTextDefault;
		FontFamily = Fonts.SystemFontFamilies.ToList()[0];
		FontSize = 12.0;
		BackgroundBrush = FindDefinedBrush(Colors.Black);
		ForegroundBrush = FindDefinedBrush(Colors.White);
		Duration = 5;
	}

	public SlideViewModel(SlideData slideData)
		: this()
	{
		_slideData = slideData;
	}

	public void ResetText()
	{
		Text = Strings.VideoTextDefault;
	}

	private void FirePropertyChangedEvent(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	private static ObservableCollection<int> BuildDefinedDurations()
	{
		ObservableCollection<int> observableCollection = new ObservableCollection<int>();
		for (int i = 1; i <= 10; i++)
		{
			observableCollection.Add(i);
		}
		return observableCollection;
	}

	private ObservableCollection<SlideColor> BuildDefinedBrushes()
	{
		_foregroundDefault = new SlideColor
		{
			SolidColorBrush = Brushes.White,
			ColorName = Strings.White
		};
		_backgroundDefault = new SlideColor
		{
			SolidColorBrush = Brushes.Black,
			ColorName = Strings.Black
		};
		return new ObservableCollection<SlideColor>
		{
			_foregroundDefault,
			new SlideColor
			{
				SolidColorBrush = Brushes.DarkGray,
				ColorName = Strings.DarkGray
			},
			_backgroundDefault,
			new SlideColor
			{
				SolidColorBrush = Brushes.Pink,
				ColorName = Strings.Pink
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.Red,
				ColorName = Strings.Red
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.DarkRed,
				ColorName = Strings.DarkRed
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.LightGreen,
				ColorName = Strings.LightGreen
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.Green,
				ColorName = Strings.Green
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.DarkGreen,
				ColorName = Strings.DarkGreen
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.LightBlue,
				ColorName = Strings.LightBlue
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.Blue,
				ColorName = Strings.Blue
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.DarkBlue,
				ColorName = Strings.DarkBlue
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.LightYellow,
				ColorName = Strings.LightYellow
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.Yellow,
				ColorName = Strings.Yellow
			},
			new SlideColor
			{
				SolidColorBrush = Brushes.Orange,
				ColorName = Strings.Orange
			}
		};
	}

	private SlideColor FindDefinedBrush(Color color, SlideColor defaultBrush)
	{
		return FindDefinedBrush(color) ?? defaultBrush;
	}

	private SlideColor FindDefinedBrush(Color color)
	{
		foreach (SlideColor definedBrush in DefinedBrushes)
		{
			if (color == definedBrush.SolidColorBrush.Color)
			{
				return definedBrush;
			}
		}
		return null;
	}
}
