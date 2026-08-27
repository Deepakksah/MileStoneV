using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls;

public class DialogActionsCanvas : Canvas
{
	public static class DialogActionsCanvasInternals
	{
		public const string PrimaryButtonLargeTemplateKey = "DialogActionsCanvasPrimaryButtonLargeTemplateKey";

		public const string PrimaryButtonMediumTemplateKey = "DialogActionsCanvasPrimaryButtonMediumTemplateKey";

		public const string SecondaryButtonLargeTemplateKey = "DialogActionsCanvasSecondaryButtonLargeTemplateKey";

		public const string SecondaryButtonMediumTemplateKey = "DialogActionsCanvasSecondaryButtonMediumTemplateKey";

		public const string TertiaryButtonLargeTemplateKey = "DialogActionsCanvasTertiaryButtonLargeTemplateKey";

		public const string TertiaryButtonMediumTemplateKey = "DialogActionsCanvasTertiaryButtonMediumTemplateKey";

		public const string TransparentButtonLargeTemplateKey = "DialogActionsCanvasTransparentButtonLargeTemplateKey";

		public const string TransparentButtonMediumTemplateKey = "DialogActionsCanvasTransparentButtonMediumTemplateKey";

		public const string TextButtonTemplateKey = "DialogActionsCanvasTextButtonTemplateKey";

		public const string CheckBoxTemplateKey = "DialogActionsCanvasCheckBoxTemplateKey";
	}

	private class ActionDetails
	{
		private static readonly Size MaxSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

		public double DesiredWidth { get; private set; }

		public double DesiredHeight { get; private set; }

		public double MinimumWidth { get; private set; }

		public bool CanResize => Action.CanResize;

		public VideoOSDialogActionBase Action { get; }

		public FrameworkElement FrameworkElement { get; }

		public ActionDetails(VideoOSDialogActionBase action, FrameworkElement frameworkElement)
		{
			Action = action;
			FrameworkElement = frameworkElement;
		}

		public void UpdateDesiredAndMinimumSizes()
		{
			FrameworkElement.ClearValue(FrameworkElement.MaxWidthProperty);
			FrameworkElement.ClearValue(FrameworkElement.MinWidthProperty);
			FrameworkElement.Measure(MaxSize);
			DesiredWidth = FrameworkElement.DesiredSize.Width + Action.DesiredWidthPadding;
			DesiredHeight = FrameworkElement.DesiredSize.Height;
			MinimumWidth = GetActionMinWidth();
		}

		public void SetExactWidth(double width)
		{
			FrameworkElement frameworkElement = FrameworkElement;
			double maxWidth = (FrameworkElement.MinWidth = width);
			frameworkElement.MaxWidth = maxWidth;
		}

		public void ApplyMinWidth()
		{
			FrameworkElement.MinWidth = MinimumWidth;
		}

		public void PositionAction(double x, double y)
		{
			Canvas.SetLeft(FrameworkElement, x);
			Canvas.SetTop(FrameworkElement, y);
		}

		private double GetActionMinWidth()
		{
			if ((Action is VideoOSDialogActionButtonPrimary || Action is VideoOSDialogActionButtonSecondary || Action is VideoOSDialogActionButtonTertiary || Action is VideoOSDialogActionButtonTransparent) && !CanResize)
			{
				return FrameworkElement.DesiredSize.Width;
			}
			if (Action is VideoOSDialogActionCheckBox)
			{
				return 50.0;
			}
			if (!(Action is VideoOSDialogActionButtonText))
			{
				return Math.Max(0.0, DesiredHeight + 10.0);
			}
			return Math.Max(0.0, DesiredHeight - 10.0);
		}
	}

	public static readonly DependencyProperty DialogActionsProperty = DependencyProperty.Register("DialogActions", typeof(ICollection<VideoOSDialogActionBase>), typeof(DialogActionsCanvas), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));

	public static readonly DependencyProperty DialogActionsSizeProperty = DependencyProperty.Register("DialogActionsSize", typeof(VideoOSDialog.ActionsSize), typeof(DialogActionsCanvas), new PropertyMetadata(VideoOSDialog.ActionsSize.Large, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((DialogActionsCanvas)o).DialogActionsSizeChanged();
	}));

	public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.Register("SizeToContent", typeof(SizeToContent), typeof(DialogActionsCanvas), new PropertyMetadata(SizeToContent.Manual));

	public static readonly DependencyProperty ActionCanvasDesiredWidthProperty = DependencyProperty.Register("ActionCanvasDesiredWidth", typeof(double?), typeof(DialogActionsCanvas), new PropertyMetadata(null));

	private readonly Dictionary<VideoOSDialogActionBase, ActionDetails> _actionDetails = new Dictionary<VideoOSDialogActionBase, ActionDetails>();

	private bool _actionEventsSubscribed;

	public ICollection<VideoOSDialogActionBase> DialogActions
	{
		get
		{
			return (ICollection<VideoOSDialogActionBase>)GetValue(DialogActionsProperty);
		}
		set
		{
			SetValue(DialogActionsProperty, value);
		}
	}

	public VideoOSDialog.ActionsSize DialogActionsSize
	{
		get
		{
			return (VideoOSDialog.ActionsSize)GetValue(DialogActionsSizeProperty);
		}
		set
		{
			SetValue(DialogActionsSizeProperty, value);
		}
	}

	public SizeToContent SizeToContent
	{
		get
		{
			return (SizeToContent)GetValue(SizeToContentProperty);
		}
		set
		{
			SetValue(SizeToContentProperty, value);
		}
	}

	public double? ActionCanvasDesiredWidth
	{
		get
		{
			return (double?)GetValue(ActionCanvasDesiredWidthProperty);
		}
		set
		{
			SetValue(ActionCanvasDesiredWidthProperty, value);
		}
	}

	private bool SizeToContentHorizontally
	{
		get
		{
			if (SizeToContent != SizeToContent.Width)
			{
				return SizeToContent == SizeToContent.WidthAndHeight;
			}
			return true;
		}
	}

	public DialogActionsCanvas()
	{
		base.Loaded += OnLoaded;
		base.Unloaded += OnUnloaded;
		CollectionHelper<VideoOSDialogActionBase>.Initialize(this, DialogActionsProperty, DialogActionsChanged);
	}

	protected override void OnVisualParentChanged(DependencyObject oldParent)
	{
		if (oldParent is FrameworkElement frameworkElement)
		{
			frameworkElement.SizeChanged -= OnParentSizeChanged;
		}
		if (base.Parent is FrameworkElement frameworkElement2)
		{
			frameworkElement2.SizeChanged += OnParentSizeChanged;
		}
		base.OnVisualParentChanged(oldParent);
	}

	private void OnParentSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (GetValue(FrameworkElement.MinWidthProperty) != null && base.IsLoaded)
		{
			ClearValue(FrameworkElement.MinWidthProperty);
		}
		UpdateActionPositions();
	}

	private FrameworkElement CreateFrameworkElement(VideoOSDialogActionBase action)
	{
		DataTemplate dataTemplate = null;
		if (action is VideoOSDialogActionButtonPrimary)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource((DialogActionsSize == VideoOSDialog.ActionsSize.Large) ? "DialogActionsCanvasPrimaryButtonLargeTemplateKey" : "DialogActionsCanvasPrimaryButtonMediumTemplateKey");
		}
		else if (action is VideoOSDialogActionButtonSecondary)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource((DialogActionsSize == VideoOSDialog.ActionsSize.Large) ? "DialogActionsCanvasSecondaryButtonLargeTemplateKey" : "DialogActionsCanvasSecondaryButtonMediumTemplateKey");
		}
		else if (action is VideoOSDialogActionButtonTertiary)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource((DialogActionsSize == VideoOSDialog.ActionsSize.Large) ? "DialogActionsCanvasTertiaryButtonLargeTemplateKey" : "DialogActionsCanvasTertiaryButtonMediumTemplateKey");
		}
		if (action is VideoOSDialogActionButtonTransparent)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource((DialogActionsSize == VideoOSDialog.ActionsSize.Large) ? "DialogActionsCanvasTransparentButtonLargeTemplateKey" : "DialogActionsCanvasTransparentButtonMediumTemplateKey");
		}
		else if (action is VideoOSDialogActionButtonText)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource("DialogActionsCanvasTextButtonTemplateKey");
		}
		else if (action is VideoOSDialogActionCheckBox)
		{
			dataTemplate = (DataTemplate)ThemeManager.Instance.FindStaticResource("DialogActionsCanvasCheckBoxTemplateKey");
		}
		if (dataTemplate == null)
		{
			throw new ArgumentException(FormattableString.Invariant($"No matching data template found for action type: {action.GetType().Name}"));
		}
		FrameworkElement obj = (FrameworkElement)dataTemplate.LoadContent();
		obj.DataContext = action;
		AutomationProperties.SetAutomationId(obj, action.AutomationId);
		return obj;
	}

	private void DialogActionsSizeChanged()
	{
		UnsubscribeActionEvents();
		_actionDetails.Clear();
		base.Children.Clear();
		DialogActionsChanged(null);
	}

	private void DialogActionsChanged(CollectionChangedInfo<VideoOSDialogActionBase> info)
	{
		UnsubscribeActionEvents();
		if (DialogActions != null)
		{
			_actionDetails.Keys.Except(DialogActions).ToList().ForEach(delegate(VideoOSDialogActionBase a)
			{
				ActionDetails actionDetails2 = _actionDetails[a];
				_actionDetails.Remove(a);
				base.Children.Remove(actionDetails2.FrameworkElement);
			});
			foreach (VideoOSDialogActionBase dialogAction in DialogActions)
			{
				if (!_actionDetails.ContainsKey(dialogAction))
				{
					ActionDetails actionDetails = new ActionDetails(dialogAction, CreateFrameworkElement(dialogAction));
					_actionDetails.Add(dialogAction, actionDetails);
					base.Children.Add(actionDetails.FrameworkElement);
				}
			}
		}
		else
		{
			_actionDetails.Clear();
			base.Children.Clear();
		}
		SubscribeActionEvents();
		UpdateActionPositions();
	}

	private void SubscribeActionEvents()
	{
		if (_actionEventsSubscribed)
		{
			return;
		}
		foreach (ActionDetails value in _actionDetails.Values)
		{
			value.Action.PropertyChanged += ActionOnPropertyChanged;
			value.FrameworkElement.SizeChanged += ActionOnSizeChanged;
		}
		_actionEventsSubscribed = true;
	}

	private void UnsubscribeActionEvents()
	{
		if (!_actionEventsSubscribed)
		{
			return;
		}
		foreach (ActionDetails value in _actionDetails.Values)
		{
			value.Action.PropertyChanged -= ActionOnPropertyChanged;
			value.FrameworkElement.SizeChanged -= ActionOnSizeChanged;
		}
		_actionEventsSubscribed = false;
	}

	private void ActionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Placement" || e.PropertyName == "IsVisible")
		{
			UpdateActionPositions();
		}
	}

	private void ActionOnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		UpdateActionPositions();
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		SubscribeActionEvents();
		UpdateActionPositions();
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		UnsubscribeActionEvents();
	}

	private void UpdateActionPositions()
	{
		List<ActionDetails> list = new List<ActionDetails>();
		List<ActionDetails> list2 = new List<ActionDetails>();
		double num = 0.0;
		foreach (ActionDetails value in _actionDetails.Values)
		{
			value.UpdateDesiredAndMinimumSizes();
			num = Math.Max(num, value.FrameworkElement.DesiredSize.Height);
			if (value.Action.Placement == VideoOSDialogActionBase.Placements.Left)
			{
				list.Add(value);
			}
			if (value.Action.Placement == VideoOSDialogActionBase.Placements.Right)
			{
				list2.Add(value);
			}
		}
		base.Height = num;
		SqueezeUniformly(list, list2);
	}

	private void SqueezeUniformly(List<ActionDetails> leftActionDetails, List<ActionDetails> rightActionDetails)
	{
		double num = Math.Max(0.0, leftActionDetails.Sum((ActionDetails b) => b.DesiredWidth) + (double)(leftActionDetails.Count((ActionDetails d) => d.Action.IsVisible) - 1) * 16.0);
		double num2 = Math.Max(0.0, rightActionDetails.Sum((ActionDetails b) => b.DesiredWidth) + (double)(rightActionDetails.Count((ActionDetails d) => d.Action.IsVisible) - 1) * 16.0);
		double num3 = ((num > 0.0 && num2 > 0.0) ? 40.0 : 0.0);
		ActionCanvasDesiredWidth = num + num2 + num3;
		if (base.IsLoaded && SizeToContentHorizontally)
		{
			base.MinWidth = ActionCanvasDesiredWidth.Value + 1.0;
		}
		double num4 = Math.Max(0.0 - base.ActualWidth + num + num2 + num3, 0.0);
		List<ActionDetails> list = (from b in leftActionDetails.Concat(rightActionDetails)
			where b.CanResize
			select b).ToList();
		double widthToTrimOffEachAction = RespectMinimumWidths(list, num4 / (double)list.Count);
		double num5 = 0.0;
		if (leftActionDetails.Count > 0)
		{
			num5 = PositionActions(leftActionDetails, widthToTrimOffEachAction, num5, 16.0);
		}
		if (rightActionDetails.Count > 0)
		{
			num5 = ((num4 > 0.0 && num > 0.0) ? (num5 - 16.0 + num3) : (base.ActualWidth - num2 + num4));
			PositionActions(rightActionDetails, widthToTrimOffEachAction, num5, 16.0);
		}
	}

	private double PositionActions(List<ActionDetails> actionDetails, double widthToTrimOffEachAction, double initialX, double actionSpacing)
	{
		double num = initialX;
		foreach (ActionDetails item in actionDetails.Where((ActionDetails d) => d.Action.IsVisible))
		{
			double y = (base.Height - item.DesiredHeight) / 2.0;
			double num2 = item.DesiredWidth;
			if (widthToTrimOffEachAction > 0.0 && item.CanResize && num2 > item.MinimumWidth)
			{
				num2 = Math.Max(item.MinimumWidth, num2 - widthToTrimOffEachAction);
			}
			if (num2 < item.DesiredWidth)
			{
				item.SetExactWidth(num2);
			}
			else
			{
				item.ApplyMinWidth();
			}
			item.PositionAction(num, y);
			num += num2 + actionSpacing;
		}
		return num;
	}

	private static double RespectMinimumWidths(List<ActionDetails> actionsThatCanResize, double widthToTrimOffEachAction)
	{
		foreach (ActionDetails item in actionsThatCanResize)
		{
			double num = item.DesiredWidth - widthToTrimOffEachAction;
			if (num < item.MinimumWidth)
			{
				actionsThatCanResize.Remove(item);
				if (actionsThatCanResize.Any())
				{
					widthToTrimOffEachAction += (item.MinimumWidth - num) / (double)actionsThatCanResize.Count;
					return RespectMinimumWidths(actionsThatCanResize, widthToTrimOffEachAction);
				}
				return widthToTrimOffEachAction;
			}
		}
		return widthToTrimOffEachAction;
	}
}
