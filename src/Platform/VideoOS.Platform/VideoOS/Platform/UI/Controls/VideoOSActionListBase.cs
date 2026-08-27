using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Controls.ActionLists.Internal;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSActionListBase : Control
{
	public const double DefaultSpacing = 2.0;

	public static readonly DependencyProperty ActionsProperty;

	public static readonly DependencyProperty SpacingProperty;

	public static readonly DependencyPropertyKey IsOpenPropertyKey;

	private ActionsPresenter _actionsPresenter;

	public static readonly DependencyProperty AreActionsFocusableProperty;

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return (ICollection<VideoOSActionBase>)GetValue(ActionsProperty);
		}
		set
		{
			SetValue(ActionsProperty, value);
		}
	}

	public double Spacing
	{
		get
		{
			return (double)GetValue(SpacingProperty);
		}
		set
		{
			SetValue(SpacingProperty, value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenPropertyKey.DependencyProperty);
		}
		protected set
		{
			SetValue(IsOpenPropertyKey, value);
		}
	}

	public bool AreActionsFocusable
	{
		get
		{
			return (bool)GetValue(AreActionsFocusableProperty);
		}
		set
		{
			SetValue(AreActionsFocusableProperty, value);
		}
	}

	static VideoOSActionListBase()
	{
		ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(VideoOSActionListBase), new PropertyMetadata(null));
		SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(VideoOSActionListBase), new PropertyMetadata(2.0));
		IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(bool), typeof(VideoOSActionListBase), new FrameworkPropertyMetadata(false));
		AreActionsFocusableProperty = DependencyProperty.Register("AreActionsFocusable", typeof(bool), typeof(VideoOSActionListBase), new PropertyMetadata(true));
		Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(VideoOSActionListBase), new FrameworkPropertyMetadata(HorizontalAlignment.Right));
	}

	public VideoOSActionListBase()
	{
		base.SizeChanged += VideoOSActionListBase_SizeChanged;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_actionsPresenter = this.GetTemplateChild<ActionsPresenter>("PART_ActionsPresenter");
		_actionsPresenter.IsOpenChanged += ActionsPresenter_IsOpenChanged;
	}

	private void ActionsPresenter_IsOpenChanged(object sender, EventArgs e)
	{
		IsOpen = ((ActionsPresenter)sender).IsOpen;
	}

	private void VideoOSActionListBase_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		_actionsPresenter?.UpdateChildPositions(base.ActualWidth);
	}

	public FrameworkElement GetActionFrameworkElementUnderMouseCursor()
	{
		return _actionsPresenter?.GetActionFrameworkElementUnderMouseCursor();
	}

	public FrameworkElement GetFrameworkElementForAction(VideoOSActionBase action)
	{
		return _actionsPresenter?.GetFrameworkElementForAction(action);
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSActionListBaseAutomationPeer(this);
	}
}
