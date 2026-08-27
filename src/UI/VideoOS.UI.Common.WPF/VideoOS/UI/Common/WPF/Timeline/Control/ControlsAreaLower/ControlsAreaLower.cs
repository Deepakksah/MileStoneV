using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.Control.Data;
using VideoOS.UI.Common.WPF.Timeline.Events;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower;

public partial class ControlsAreaLower : UserControl, IComponentConnector
{
	private TimeAndSpanController _timeAndSpanController = new TimeAndSpanController();

	private AppearanceAndBehaviourController _appearanceAndBehaviourController = new AppearanceAndBehaviourController();

	public static double ControlsAreaLowerHeight => 38.0;

	public event EventHandler<EventArgs> SetupClicked;

	public ControlsAreaLower()
	{
		InitializeComponent();
	}

	public void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		_timeAndSpanController = timeAndSpanController;
		_appearanceAndBehaviourController = appearanceAndBehaviourController;
		_timeAndSpanController.TargetTimelineSpanChanged += _timeAndSpanController_TargetTimelineSpanChanged;
		_appearanceAndBehaviourController.PropertyChanged += _appearanceAndBehaviourController_PropertyChanged;
	}

	public void Close()
	{
		_timeAndSpanController.TargetTimelineSpanChanged -= _timeAndSpanController_TargetTimelineSpanChanged;
		_appearanceAndBehaviourController.PropertyChanged -= _appearanceAndBehaviourController_PropertyChanged;
	}

	private void UserControl_Loaded(object sender, RoutedEventArgs e)
	{
		_spanControl.SetTimelineSpans(_appearanceAndBehaviourController.TimelineSpans, LookupTimelineSpan(_timeAndSpanController.TimelineSpan));
		ApplyAppearanceAndBehaviourControllerProperty(null);
	}

	private void _timeAndSpanController_TargetTimelineSpanChanged(object sender, TimeSpanEventArgs e)
	{
		_spanControl.SelectedSpan = LookupTimelineSpan(e.TimeSpan);
	}

	private void _appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		ApplyAppearanceAndBehaviourControllerProperty(e.PropertyName);
	}

	private void _spanControl_SelectedSpanChanged(object sender, EventArgs e)
	{
		if (_timeAndSpanController.TargetTimelineSpan != _spanControl.SelectedSpan.TimeSpan)
		{
			_timeAndSpanController.SetSpan(_spanControl.SelectedSpan.TimeSpan, animate: true);
		}
	}

	private void _setupControl_SetupButtonClicked(object sender, EventArgs e)
	{
		if (this.SetupClicked != null)
		{
			this.SetupClicked(this, new EventArgs());
		}
	}

	private void ApplyAppearanceAndBehaviourControllerProperty(string propertyName)
	{
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowSpanControl.ToString())
		{
			_spanControl.Visibility = ((!_appearanceAndBehaviourController.ShowSpanControl) ? Visibility.Collapsed : Visibility.Visible);
		}
		if (string.IsNullOrEmpty(propertyName) || propertyName == AppearanceAndBehaviourController.PropertyNames.ShowSetupControl.ToString())
		{
			_setupControl.Visibility = ((!_appearanceAndBehaviourController.ShowSetupControl) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private TimelineSpan LookupTimelineSpan(TimeSpan timeSpan)
	{
		foreach (TimelineSpan timelineSpan in _appearanceAndBehaviourController.TimelineSpans)
		{
			if (timelineSpan.TimeSpan == timeSpan)
			{
				return timelineSpan;
			}
		}
		return _appearanceAndBehaviourController.TimelineSpans[0];
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
