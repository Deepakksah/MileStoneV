using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.Control.Controllers;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimelineLabels;

public partial class TimelineLabelsLayer : LayerBase, IComponentConnector
{
	private TimelineLabelsLayerModel Model { get; set; }

	public TimelineLabelsLayer()
	{
		InitializeComponent();
		base.FlowDirection = RtlUtility.SelectedFlowDirection;
		Model = new TimelineLabelsLayerModel();
		base.DataContext = Model;
		Popup.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
	}

	public override void Init(TimeAndSpanController timeAndSpanController, AppearanceAndBehaviourController appearanceAndBehaviourController, ITimelineSequenceCache timelineCache)
	{
		base.Init(timeAndSpanController, appearanceAndBehaviourController, timelineCache);
		base.AppearanceAndBehaviourController.PropertyChanged += appearanceAndBehaviourController_PropertyChanged;
	}

	public override void Close()
	{
		base.Close();
		base.AppearanceAndBehaviourController.PropertyChanged -= appearanceAndBehaviourController_PropertyChanged;
	}

	private static void TimelineTimelinesBackgroundBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		(sender as TimelineLabelsLayer).UpdateLegend();
	}

	private void appearanceAndBehaviourController_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowUpperTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineTopLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineBottomLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLowerTimelineCustomDataLine.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.UpperTimelineLabel.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.LowerTimelineLabel.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.ShowLegend.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSources.ToString())
		{
			UpdateControls();
		}
		if (e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineAreaDisplayMode.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.TimelineLineBackgroundBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder1Brush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceFillExtraZOrder2Brush.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.SequenceSourceVisualAppearanceMarkerImage.ToString() || e.PropertyName == AppearanceAndBehaviourController.PropertyNames.NoDataDisplayText.ToString())
		{
			UpdateLegend();
		}
	}

	private void LayerBase_Loaded(object sender, RoutedEventArgs e)
	{
		UpdateControls();
	}

	private void LegendButton_OnClick(object sender, RoutedEventArgs e)
	{
		Popup.IsOpen = true;
	}

	private void UpdateControls()
	{
		Model.TimelineLabelBackgroundMargin = new Thickness(0.0, base.AppearanceAndBehaviourController.TimelineAreaRulerSectionHeight, 0.0, 0.0);
		bool flag = false;
		bool flag2 = false;
		if (base.AppearanceAndBehaviourController.ShowUpperTimelineTopLine || base.AppearanceAndBehaviourController.ShowUpperTimelineBottomLine || base.AppearanceAndBehaviourController.ShowUpperTimelineCustomDataLine)
		{
			flag = true;
		}
		if (base.AppearanceAndBehaviourController.ShowLowerTimelineTopLine || base.AppearanceAndBehaviourController.ShowLowerTimelineBottomLine || base.AppearanceAndBehaviourController.ShowLowerTimelineCustomDataLine)
		{
			flag2 = true;
		}
		if (flag)
		{
			if (!string.IsNullOrEmpty(base.AppearanceAndBehaviourController.UpperTimelineLabel))
			{
				Model.UpperTimelineLabelVisibility = Visibility.Visible;
				Model.UpperTimelineLabelText = base.AppearanceAndBehaviourController.UpperTimelineLabel;
			}
			else
			{
				Model.UpperTimelineLabelVisibility = Visibility.Hidden;
			}
		}
		else
		{
			Model.UpperTimelineLabelVisibility = Visibility.Hidden;
		}
		if (flag2)
		{
			if (!string.IsNullOrEmpty(base.AppearanceAndBehaviourController.LowerTimelineLabel))
			{
				Model.LowerTimelineLabelVisibility = Visibility.Visible;
				Model.LowerTimelineLabelText = base.AppearanceAndBehaviourController.LowerTimelineLabel;
			}
			else
			{
				Model.LowerTimelineLabelVisibility = Visibility.Hidden;
			}
		}
		else
		{
			Model.LowerTimelineLabelVisibility = Visibility.Hidden;
		}
		int num = ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? 35 : 27);
		int num2 = ((!flag) ? num : ((base.AppearanceAndBehaviourController.TimelineAreaDisplayMode == TimelineAreaDisplayMode.Full) ? 55 : 41));
		Model.UpperTimelineLabelMargin = new Thickness(0.0, num, 0.0, 0.0);
		Model.LowerTimelineLabelMargin = new Thickness(0.0, num2, 0.0, 0.0);
		UpdateLegend();
	}

	private void UpdateLegend()
	{
		IEnumerable<LegendEntry> second = (from ss in base.AppearanceAndBehaviourController.UpperSequenceSources.Concat(base.AppearanceAndBehaviourController.LowerSequenceSources).OfType<ISequenceSource>()
			select GetLegendEntry(ss)).Distinct();
		Model.Legend = new LegendEntry[1]
		{
			new LegendEntry(base.AppearanceAndBehaviourController.NoDataDisplayText, base.AppearanceAndBehaviourController.TimelineLineBackgroundBrush, null)
		}.Concat(second);
		Model.LegendVisibility = ((!base.AppearanceAndBehaviourController.ShowLegend || !Model.Legend.Any()) ? Visibility.Collapsed : Visibility.Visible);
		int num = base.AppearanceAndBehaviourController.TimelineAreaRulerSectionHeight + base.AppearanceAndBehaviourController.TimelineAreaTracksSectionHeight / 2 - 8;
		Model.LegendMargin = new Thickness(0.0, num, 4.0, 0.0);
	}

	private LegendEntry GetLegendEntry(ISequenceSource sequenceSource)
	{
		Brush brush = null;
		ImageSource imageSource = null;
		switch (sequenceSource.VisualAppearance)
		{
		case VisualAppearance.FillTopLineHighZOrder:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineHighZOrderBrush;
			break;
		case VisualAppearance.FillTopLineLowZOrder:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillTopLineLowZOrderBrush;
			break;
		case VisualAppearance.FillBottomLineHighZOrder:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineHighZOrderBrush;
			break;
		case VisualAppearance.FillBottomLineLowZOrder:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillBottomLineLowZOrderBrush;
			break;
		case VisualAppearance.FillExtraZOrder1:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder1Brush;
			break;
		case VisualAppearance.FillExtraZOrder2:
			brush = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceFillExtraZOrder2Brush;
			break;
		case VisualAppearance.FillCustomDataLine:
			brush = sequenceSource.CustomDataBrush;
			break;
		case VisualAppearance.Marker:
			imageSource = base.AppearanceAndBehaviourController.SequenceSourceVisualAppearanceMarkerImage;
			break;
		case VisualAppearance.CustomDataMarker:
			brush = sequenceSource.CustomDataBrush;
			break;
		}
		return new LegendEntry(sequenceSource.DisplayName, brush, imageSource);
	}

	private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
	{
		return new CustomPopupPlacement[1]
		{
			new CustomPopupPlacement(new Point(offset.X + targetSize.Width - popupSize.Width, offset.Y + targetSize.Height - popupSize.Height), PopupPrimaryAxis.None)
		};
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
