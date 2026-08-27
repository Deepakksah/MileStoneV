using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSActionBase : INotifyPropertyChanged
{
	private VideoOSIconSourceBase __iconSource;

	private VideoOSIconSourceBase __inMenuIconSource;

	private string __text;

	private string __inMenuText;

	private string __toolTip;

	private string __inMenuToolTip;

	private bool __isEnabled = true;

	private bool __isVisible = true;

	private object __tag;

	private int __overflowOrder = int.MaxValue;

	private static IMultiValueConverter FallbackValueConverter { get; } = new LambdaConverter2<object, object>((object value, object fallbackValue) => value ?? fallbackValue, "FallbackValueConverter");

	private static IMultiValueConverter ContextMenuItemToolTipConverter { get; } = new LambdaConverter4<string, string, string, string>(delegate(string inMenuText, string text, string inMenuToolTip, string toolTip)
	{
		string objA = inMenuText ?? text;
		string text2 = inMenuToolTip ?? toolTip;
		return (!object.Equals(objA, text2)) ? text2 : null;
	}, "ContextMenuItemToolTipConverter");

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return __iconSource;
		}
		set
		{
			if (__iconSource != value)
			{
				__iconSource = value;
				OnPropertyChanged("IconSource");
			}
		}
	}

	public VideoOSIconSourceBase InMenuIconSource
	{
		get
		{
			return __inMenuIconSource;
		}
		set
		{
			if (__inMenuIconSource != value)
			{
				__inMenuIconSource = value;
				OnPropertyChanged("InMenuIconSource");
			}
		}
	}

	public string Text
	{
		get
		{
			return __text;
		}
		set
		{
			if (!(__text == value))
			{
				__text = value;
				OnPropertyChanged("Text");
			}
		}
	}

	public string InMenuText
	{
		get
		{
			return __inMenuText;
		}
		set
		{
			if (!(__inMenuText == value))
			{
				__inMenuText = value;
				OnPropertyChanged("InMenuText");
			}
		}
	}

	public string ToolTip
	{
		get
		{
			return __toolTip;
		}
		set
		{
			if (!(__toolTip == value))
			{
				__toolTip = value;
				OnPropertyChanged("ToolTip");
			}
		}
	}

	public string InMenuToolTip
	{
		get
		{
			return __inMenuToolTip;
		}
		set
		{
			if (!(__inMenuToolTip == value))
			{
				__inMenuToolTip = value;
				OnPropertyChanged("InMenuToolTip");
			}
		}
	}

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

	public object Tag
	{
		get
		{
			return __tag;
		}
		set
		{
			if (__tag != value)
			{
				__tag = value;
				OnPropertyChanged("Tag");
			}
		}
	}

	public int OverflowOrder
	{
		get
		{
			return __overflowOrder;
		}
		set
		{
			if (__overflowOrder != value)
			{
				__overflowOrder = value;
				OnPropertyChanged("OverflowOrder");
			}
		}
	}

	public string AutomationId { get; set; } = string.Empty;

	public event PropertyChangedEventHandler PropertyChanged;

	public VideoOSActionBase()
	{
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public virtual object AsContextMenuItem()
	{
		VideoOSContextMenuItem obj = new VideoOSContextMenuItem
		{
			Tag = this
		};
		obj.SetBinding(binding: new MultiBinding
		{
			Bindings = 
			{
				(BindingBase)new Binding("InMenuIconSource")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("IconSource")
				{
					Mode = BindingMode.OneWay,
					Source = this
				}
			},
			Converter = FallbackValueConverter
		}, dp: VideoOSContextMenuItem.IconSourceProperty);
		obj.SetBinding(binding: new MultiBinding
		{
			Bindings = 
			{
				(BindingBase)new Binding("InMenuText")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("Text")
				{
					Mode = BindingMode.OneWay,
					Source = this
				}
			},
			Converter = FallbackValueConverter
		}, dp: VideoOSContextMenuItem.DataProperty);
		obj.SetBinding(UIElement.IsEnabledProperty, new Binding("IsEnabled")
		{
			Mode = BindingMode.OneWay,
			Source = this
		});
		obj.SetBinding(UIElement.VisibilityProperty, new Binding("IsVisible")
		{
			Mode = BindingMode.OneWay,
			Source = this,
			Converter = new BoolToVisibilityConverter()
		});
		obj.SetBinding(binding: new MultiBinding
		{
			Bindings = 
			{
				(BindingBase)new Binding("InMenuText")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("Text")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("InMenuToolTip")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("ToolTip")
				{
					Mode = BindingMode.OneWay,
					Source = this
				}
			},
			Converter = ContextMenuItemToolTipConverter
		}, dp: FrameworkElement.ToolTipProperty);
		AutomationProperties.SetAutomationId(obj, AutomationId);
		return obj;
	}
}
