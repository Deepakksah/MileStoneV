using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.VideoText;

public partial class VideoTextWindow : VideoOSDialog, IComponentConnector
{
	private TextBox _preview;

	private bool _helpButtonVisible;

	public bool HelpButtonVisible
	{
		set
		{
			if (_helpButtonVisible != value)
			{
				_helpButtonVisible = value;
				_buttonHelp.IsVisible = _helpButtonVisible;
			}
		}
	}

	public event EventHandler HelpButtonClickedEvent;

	private void FireHelpButtonClickedEvent()
	{
		if (this.HelpButtonClickedEvent != null)
		{
			this.HelpButtonClickedEvent(this, new EventArgs());
		}
	}

	public VideoTextWindow()
	{
		InitializeComponent();
		base.DataContextChanged += VideoTextWindow_DataContextChanged;
	}

	private void VideoTextWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		_preview = PreviewGenerator.CreateTextBox();
		SetupBindings();
		_previewContentHolder.Children.Add(_preview);
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
		_preSlides.SlideSelected += _preSlides_SlideSelected;
		_postSlides.SlideSelected += _postSlides_SlideSelected;
	}

	private void Button_OK(object sender, EventArgs e)
	{
		base.DialogResult = true;
		Close();
	}

	private void Button_Cancel(object sender, EventArgs e)
	{
		base.DialogResult = false;
		Close();
	}

	private void Button_Help(object sender, EventArgs e)
	{
		FireHelpButtonClickedEvent();
	}

	private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
	{
		FireHelpButtonClickedEvent();
	}

	private void SetupBindings()
	{
		Binding binding = new Binding("Slide.BackgroundBrush.SolidColorBrush");
		_preview.SetBinding(Control.BackgroundProperty, binding);
		Binding binding2 = new Binding("Slide.ForegroundBrush.SolidColorBrush");
		_preview.SetBinding(Control.ForegroundProperty, binding2);
		Binding binding3 = new Binding("Slide.Text");
		binding3.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		_preview.SetBinding(TextBox.TextProperty, binding3);
		Binding binding4 = new Binding("Slide.FontFamily");
		_preview.SetBinding(Control.FontFamilyProperty, binding4);
		Binding binding5 = new Binding("Slide.FontSize");
		_preview.SetBinding(Control.FontSizeProperty, binding5);
		Binding binding6 = new Binding("Slide.IsReadOnly");
		_preview.SetBinding(TextBoxBase.IsReadOnlyProperty, binding6);
	}

	private void _postSlides_SlideSelected(object sender, SlideSelectedEventArgs e)
	{
		if (e.Slide != null)
		{
			_preSlides.ClearSelection();
			UpdateViewModelSlide(e);
		}
	}

	private void _preSlides_SlideSelected(object sender, SlideSelectedEventArgs e)
	{
		if (e.Slide != null)
		{
			_postSlides.ClearSelection();
			UpdateViewModelSlide(e);
		}
	}

	private void UpdateViewModelSlide(SlideSelectedEventArgs e)
	{
		if (base.DataContext is VideoTextWindowViewModel videoTextWindowViewModel && videoTextWindowViewModel.Slide != e.Slide)
		{
			videoTextWindowViewModel.Slide = e.Slide;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
