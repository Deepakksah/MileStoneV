using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSCalendar : System.Windows.Controls.Calendar
{
	public static class VideoOSCalendarInternals
	{
		public static IMultiValueConverter CalendarTodayDayItemButtonBorderBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = GetResourceKey(isMouseOver, isPressed, isEnabled, "VideoOSResourceKeysInternalVideoOSCalendarTodayItemOutlineIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarTodayItemOutlineHoveredBrush", "VideoOSResourceKeysInternalVideoOSCalendarTodayItemOutlinePressedBrush", "VideoOSResourceKeysInternalVideoOSCalendarTodayItemOutlineDisabledBrush");
			return self.FindResource(resourceKey);
		}, "CalendarTodayDayItemButtonBorderBrushConverter");

		public static IMultiValueConverter CalendarDayItemButtonBackgroundBrushConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isHighlighted, bool isEnabled, bool isSelected)
		{
			string resourceKey = (isPressed ? "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundPressedBrush" : ((!(isSelected || isHighlighted)) ? GetResourceKey(isMouseOver, isPressed: false, isEnabled, "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundHoveredBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundPressedBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundDisabledBrush") : GetResourceKey(isMouseOver, isPressed: false, isEnabled, "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundHoveredBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundPressedBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundDisabledBrush")));
			return self.FindResource(resourceKey);
		}, "CalendarDayItemButtonBackgroundBrushConverter");

		public static IMultiValueConverter CalendarMonthYearItemButtonBackgroundBrushConverter { get; } = new LambdaConverter7<DependencyObject, bool, bool, bool, DateTime?, SelectedDatesCollection, CalendarMode>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, DateTime? calendarButtonDataContext, SelectedDatesCollection selectedDates, CalendarMode calendarMode)
		{
			bool flag = false;
			if (calendarButtonDataContext.HasValue)
			{
				switch (calendarMode)
				{
				case CalendarMode.Year:
					flag = selectedDates.Any((DateTime d) => d.Month == calendarButtonDataContext.Value.Month && d.Year == calendarButtonDataContext.Value.Year);
					break;
				case CalendarMode.Decade:
					flag = selectedDates.Any((DateTime d) => d.Year == calendarButtonDataContext.Value.Year);
					break;
				}
			}
			object resourceKey = ((!flag) ? GetResourceKey(isMouseOver, isPressed, isEnabled, "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundHoveredBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundPressedBrush", "VideoOSResourceKeysInternalVideoOSCalendarDeselectedItemBackgroundDisabledBrush") : GetResourceKey(isMouseOver, isPressed, isEnabled, "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundHoveredBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundIdleBrush", "VideoOSResourceKeysInternalVideoOSCalendarSelectedItemBackgroundDisabledBrush"));
			return self.FindResource(resourceKey);
		}, "CalendarMonthYearItemButtonBackgroundBrushConverter");

		public static IMultiValueConverter HeaderButtonContentConverter { get; } = new LambdaConverter4<DateTime, XmlLanguage, VideoOSCalendar, string>(delegate(DateTime displayDate, XmlLanguage xmlLanguage, VideoOSCalendar calendar, string format)
		{
			CultureInfo cultureInfo = (calendar.IsDefaultValueApplied(FrameworkElement.LanguageProperty) ? CultureInfo.CurrentCulture : xmlLanguage.GetSpecificCulture());
			return displayDate.ToString(format, cultureInfo);
		}, "HeaderButtonContentConverter");

		private static string GetResourceKey(bool isMouseOver, bool isPressed, bool isEnabled, string idleKey, string hoverKey, string pressedKey, string disabledKey)
		{
			if (!isEnabled)
			{
				return disabledKey;
			}
			if (isPressed)
			{
				return pressedKey;
			}
			if (isMouseOver)
			{
				return hoverKey;
			}
			return idleKey;
		}
	}

	public static readonly DependencyProperty IsBorderVisibleProperty;

	public static readonly DependencyProperty ToggleDayMonthViewCommandProperty;

	public static readonly DependencyProperty SelectTodayCommandProperty;

	public static readonly DependencyProperty CalendarButtonKeyDownCommandProperty;

	private VideoOSButtonBase _partYearHeaderButton;

	private VideoOSButtonBase _partMonthHeaderButton;

	private VideoOSButtonBase _partTodayButton;

	private static readonly MethodInfo OnSelectedMonthChangedMethodInfo;

	public bool IsBorderVisible
	{
		get
		{
			return (bool)GetValue(IsBorderVisibleProperty);
		}
		set
		{
			SetValue(IsBorderVisibleProperty, value);
		}
	}

	public ICommand ToggleDayMonthViewCommand
	{
		get
		{
			return (ICommand)GetValue(ToggleDayMonthViewCommandProperty);
		}
		private set
		{
			SetValue(ToggleDayMonthViewCommandProperty, value);
		}
	}

	public ICommand SelectTodayCommand
	{
		get
		{
			return (ICommand)GetValue(SelectTodayCommandProperty);
		}
		private set
		{
			SetValue(SelectTodayCommandProperty, value);
		}
	}

	public ICommand CalendarButtonKeyDownCommand
	{
		get
		{
			return (ICommand)GetValue(CalendarButtonKeyDownCommandProperty);
		}
		set
		{
			SetValue(CalendarButtonKeyDownCommandProperty, value);
		}
	}

	static VideoOSCalendar()
	{
		IsBorderVisibleProperty = DependencyProperty.Register("IsBorderVisible", typeof(bool), typeof(VideoOSCalendar), new PropertyMetadata(true));
		ToggleDayMonthViewCommandProperty = DependencyProperty.Register("ToggleDayMonthViewCommand", typeof(ICommand), typeof(VideoOSCalendar), new PropertyMetadata((object)null));
		SelectTodayCommandProperty = DependencyProperty.Register("SelectTodayCommand", typeof(ICommand), typeof(VideoOSCalendar), new PropertyMetadata((object)null));
		CalendarButtonKeyDownCommandProperty = DependencyProperty.Register("CalendarButtonKeyDownCommand", typeof(ICommand), typeof(VideoOSCalendar), new PropertyMetadata((object)null));
		OnSelectedMonthChangedMethodInfo = typeof(System.Windows.Controls.Calendar).GetMethod("OnSelectedMonthChanged", BindingFlags.Instance | BindingFlags.NonPublic);
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSCalendar), new FrameworkPropertyMetadata(typeof(VideoOSCalendar)));
		ThemeManager.InitializeThemingSystem();
	}

	public VideoOSCalendar()
	{
		ToggleDayMonthViewCommand = new DelegateCommand(ToggleDayMonthView);
		SelectTodayCommand = new DelegateCommand(delegate
		{
			SelectToday();
		});
		CalendarButtonKeyDownCommand = new DelegateCommand(CalendarButtonKeyDown);
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		this.GetTemplateChild<CalendarItem>("PART_CalendarItem").Loaded += CalendarItem_OnLoaded;
	}

	private void CalendarItem_OnLoaded(object sender, RoutedEventArgs e)
	{
		CalendarItem frameworkElement = (CalendarItem)sender;
		_partYearHeaderButton = frameworkElement.GetDescendant<VideoOSButtonBase>("PART_YearHeaderButton");
		_partMonthHeaderButton = frameworkElement.GetDescendant<VideoOSButtonBase>("PART_MonthHeaderButton");
		_partTodayButton = frameworkElement.GetDescendant<VideoOSButtonBase>("PART_TodayButton");
		Grid descendant = frameworkElement.GetDescendant<Grid>("PART_YearView");
		for (int i = 0; i < descendant.Children.Count; i++)
		{
			UIElement element = descendant.Children[i];
			Grid.SetRow(element, i / 3);
			Grid.SetColumn(element, i % 3);
		}
	}

	private void ToggleDayMonthView(object parameter)
	{
		base.DisplayMode = (CalendarMode)parameter;
	}

	private void SelectToday()
	{
		DateTime today = DateTime.Today;
		if (base.SelectionMode != CalendarSelectionMode.None)
		{
			base.SelectedDate = today;
		}
		base.DisplayDate = today.AddDays(-today.Day + 1);
		base.DisplayMode = CalendarMode.Month;
	}

	private void CalendarButtonKeyDown(object obj)
	{
		EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)obj;
		if (!(eventsToCommandMappingArgs.Sender is CalendarButton calendarButton) || !(eventsToCommandMappingArgs.OriginalEventArgs is KeyEventArgs e))
		{
			return;
		}
		int num = 0;
		switch (e.Key)
		{
		case Key.Down:
			num = 3;
			break;
		case Key.Up:
			num = -3;
			break;
		}
		if (num != 0)
		{
			switch (base.DisplayMode)
			{
			case CalendarMode.Year:
			{
				DateTime dateTime2 = ((DateTime)calendarButton.DataContext).AddMonths(num);
				OnSelectedMonthChangedMethodInfo.Invoke(this, new object[1] { dateTime2 });
				e.Handled = true;
				break;
			}
			case CalendarMode.Decade:
			{
				DateTime dateTime = ((DateTime)calendarButton.DataContext).AddYears(num);
				OnSelectedMonthChangedMethodInfo.Invoke(this, new object[1] { dateTime });
				e.Handled = true;
				break;
			}
			}
		}
	}

	protected override void OnDisplayModeChanged(CalendarModeChangedEventArgs e)
	{
		base.OnDisplayModeChanged(e);
		if (e.OldMode == CalendarMode.Decade)
		{
			base.DisplayMode = CalendarMode.Month;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		List<FrameworkElement> list = new List<FrameworkElement>();
		if (_partMonthHeaderButton != null)
		{
			list.Add(_partMonthHeaderButton);
		}
		if (_partYearHeaderButton != null)
		{
			list.Add(_partYearHeaderButton);
		}
		if (_partTodayButton != null)
		{
			list.Add(_partTodayButton);
		}
		return new VideoOSCalendarAutomationPeer(this, list);
	}
}
