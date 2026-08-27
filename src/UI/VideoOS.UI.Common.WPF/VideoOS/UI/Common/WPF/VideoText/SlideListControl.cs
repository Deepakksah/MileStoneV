using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;
using VideoOS.UI.Common.WPF.VideoText.resources;

namespace VideoOS.UI.Common.WPF.VideoText;

public partial class SlideListControl : UserControl, IComponentConnector
{
	private static readonly int _maxCountOfSlides = 25;

	public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(SlideType), typeof(SlideListControl), new PropertyMetadata(SlideListControlPropertyChangedCallback));

	public static readonly DependencyProperty SlidesProperty = DependencyProperty.Register("Slides", typeof(ObservableCollection<SlideViewModel>), typeof(SlideListControl), new PropertyMetadata(new ObservableCollection<SlideViewModel>(), SlideListControlPropertyChangedCallback));

	public static readonly DependencyProperty SlidesAreReadOnlyProperty = DependencyProperty.Register("SlidesAreReadOnly", typeof(bool), typeof(SlideListControl), new PropertyMetadata(SlideListControlPropertyChangedCallback));

	public static readonly DependencyProperty SelectedSlideProperty = DependencyProperty.Register("SelectedSlide", typeof(SlideViewModel), typeof(SlideListControl), new PropertyMetadata(SlideListControlPropertyChangedCallback));

	public SlideType Type
	{
		get
		{
			return (SlideType)GetValue(TypeProperty);
		}
		set
		{
			SetValue(TypeProperty, value);
		}
	}

	public ObservableCollection<SlideViewModel> Slides
	{
		get
		{
			return (ObservableCollection<SlideViewModel>)GetValue(SlidesProperty);
		}
		set
		{
			SetValue(SlidesProperty, value);
		}
	}

	public bool SlidesAreReadOnly
	{
		get
		{
			return (bool)GetValue(SlidesAreReadOnlyProperty);
		}
		set
		{
			SetValue(SlidesAreReadOnlyProperty, value);
		}
	}

	public SlideViewModel SelectedSlide
	{
		get
		{
			return (SlideViewModel)GetValue(SelectedSlideProperty);
		}
		set
		{
			SetValue(SelectedSlideProperty, value);
		}
	}

	public string SlideListName { get; set; }

	public event EventHandler<SlideSelectedEventArgs> SlideSelected;

	public SlideListControl()
	{
		InitializeComponent();
		base.DataContextChanged += SlideListControl_DataContextChanged;
	}

	private void SlideListControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		_textColumn.Header = SlideListName;
	}

	public void ClearSelection()
	{
		_slidesList.SelectedItem = null;
	}

	public void SelectFirstItem()
	{
		if (Slides.Count > 0)
		{
			_slidesList.SelectedItem = Slides[0];
		}
	}

	private void UpdateEnableState()
	{
		if (!SlidesAreReadOnly && Slides.Count > 0)
		{
			btnClear.IsEnabled = true;
		}
		else
		{
			btnClear.IsEnabled = false;
		}
		if (!SlidesAreReadOnly && Slides.Count < _maxCountOfSlides)
		{
			btnAdd.IsEnabled = true;
		}
		else
		{
			btnAdd.IsEnabled = false;
		}
		if (!SlidesAreReadOnly && Slides.Count > 0 && _slidesList.SelectedItem != null)
		{
			if (Slides.First() != _slidesList.SelectedItem)
			{
				btnUp.IsEnabled = true;
			}
			else
			{
				btnUp.IsEnabled = false;
			}
			if (Slides.Last() != _slidesList.SelectedItem)
			{
				btnDown.IsEnabled = true;
			}
			else
			{
				btnDown.IsEnabled = false;
			}
		}
		else
		{
			btnUp.IsEnabled = false;
			btnDown.IsEnabled = false;
		}
	}

	private void btnAdd_Click(object sender, RoutedEventArgs e)
	{
		if (_slidesList.SelectedItem != null)
		{
			SlideViewModel slideViewModel = new SlideViewModel((_slidesList.SelectedItem as SlideViewModel).SlideData.Clone() as SlideData);
			slideViewModel.ResetText();
			BuildActionsForRow(slideViewModel);
			Slides.Add(slideViewModel);
		}
		else
		{
			SlideViewModel slideViewModel2 = new SlideViewModel();
			slideViewModel2.IsReadOnly = SlidesAreReadOnly;
			BuildActionsForRow(slideViewModel2);
			Slides.Add(slideViewModel2);
		}
		_slidesList.SelectedItem = Slides[Slides.Count - 1];
		UpdateSlideNames();
	}

	private void btnUp_Click(object sender, RoutedEventArgs e)
	{
		int num = Slides.IndexOf(_slidesList.SelectedItem);
		Slides.Move(num, num - 1);
		_slidesList.SelectedItem = Slides[num - 1];
		UpdateEnableState();
		UpdateSlideNames();
	}

	private void btnDown_Click(object sender, RoutedEventArgs e)
	{
		int num = Slides.IndexOf(_slidesList.SelectedItem);
		Slides.Move(num, num + 1);
		_slidesList.SelectedItem = Slides[num + 1];
		UpdateEnableState();
		UpdateSlideNames();
	}

	private void _slidesList_SelectionChanged(object sender, RoutedEventArgs e)
	{
		UpdateEnableState();
		FireSlideSelected();
	}

	private void UpdateSlideNames()
	{
		int num = 1;
		foreach (SlideViewModel slide in Slides)
		{
			slide.DisplayName = Strings.SlideName + num;
			num++;
		}
	}

	private void FireSlideSelected()
	{
		if (this.SlideSelected != null)
		{
			this.SlideSelected(this, new SlideSelectedEventArgs(Type, _slidesList.SelectedItem as SlideViewModel));
		}
	}

	private void btnClear_Click(object sender, RoutedEventArgs e)
	{
		if (VideoOSMessageBox.Show(Window.GetWindow(this), Strings.DeleteSlidesTitle, Strings.DeleteSlidesTitle, Strings.DeleteSlidesContent, VideoOSMessageBox.Buttons.OKCancel, VideoOSMessageBox.ResultButtons.Cancel, null, showRememberChoiceCheckBox: false, new VideoOSMessageBox.Options
		{
			OKButtonText = Strings.DeleteButton
		}).Button == VideoOSMessageBox.ResultButtons.OK)
		{
			Slides.Clear();
		}
	}

	private void BuildActionsForRow(SlideViewModel slideViewModel)
	{
		VideoOSActionActivate videoOSActionActivate = new VideoOSActionActivate
		{
			IconSource = new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Delete
			},
			IsEnabled = !slideViewModel.IsReadOnly,
			ToolTip = Strings.RemoveSlide,
			AutomationId = "RowDeleteButton"
		};
		videoOSActionActivate.Activated += delegate
		{
			if (VideoOSMessageBox.Show(Window.GetWindow(this), Strings.DeleteSlideTitle, Strings.DeleteSlideTitle, Strings.DeleteSlideContent, VideoOSMessageBox.Buttons.OKCancel, VideoOSMessageBox.ResultButtons.Cancel, null, showRememberChoiceCheckBox: false, new VideoOSMessageBox.Options
			{
				OKButtonText = Strings.DeleteButton
			}).Button == VideoOSMessageBox.ResultButtons.OK)
			{
				Slides.Remove(slideViewModel);
				UpdateEnableState();
				UpdateSlideNames();
			}
		};
		slideViewModel.RowActions = new ObservableCollection<VideoOSActionBase> { videoOSActionActivate };
	}

	private static void SlideListControlPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (!(d is SlideListControl slideListControl))
		{
			return;
		}
		if (e.Property.Name == "Slides")
		{
			if (slideListControl.Slides == null)
			{
				slideListControl.Slides = new ObservableCollection<SlideViewModel>();
			}
			else
			{
				slideListControl.Slides = e.NewValue as ObservableCollection<SlideViewModel>;
			}
			foreach (SlideViewModel slide in slideListControl.Slides)
			{
				slideListControl.BuildActionsForRow(slide);
			}
			slideListControl.UpdateEnableState();
			slideListControl.UpdateSlideNames();
			slideListControl._slidesList.ItemsSource = slideListControl.Slides;
		}
		else if (e.Property.Name == "SelectedSlide")
		{
			if (slideListControl.Slides.Contains(slideListControl.SelectedSlide))
			{
				slideListControl._slidesList.SelectedItem = slideListControl.SelectedSlide;
			}
		}
		else if (e.Property.Name == "Type")
		{
			slideListControl.Type = (SlideType)e.NewValue;
		}
		else if (e.Property.Name == "SlidesAreReadOnly")
		{
			slideListControl.UpdateEnableState();
		}
	}
}
