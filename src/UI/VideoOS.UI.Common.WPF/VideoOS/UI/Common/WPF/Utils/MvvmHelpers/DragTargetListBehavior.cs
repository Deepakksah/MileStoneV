using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using VideoOS.UI.Common.WPF.Tween;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public sealed class DragTargetListBehavior : Behavior<ItemsControl>
{
	private const string BringIntoViewEventName = "BringIntoView";

	private const double AutoScrollMargin = 50.0;

	private ScrollContentPresenter _scrollPresenter;

	private bool _isAnimationStarted;

	private bool _isScrollingUpward;

	private BidirectionalTween _tween;

	public string AcceptedDataFormat { get; set; }

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.AllowDrop = true;
		base.AssociatedObject.DragEnter += associatedObject_DragEnter;
		base.AssociatedObject.DragLeave += associatedObject_DragLeave;
		base.AssociatedObject.DragOver += associatedObject_DragOver;
		base.AssociatedObject.Drop += associatedObject_Drop;
		base.AssociatedObject.Loaded += AssociatedObject_Loaded;
	}

	private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
	{
		_scrollPresenter = base.AssociatedObject.GetDescendants<ScrollContentPresenter>().FirstOrDefault();
		_ = _scrollPresenter;
	}

	private void associatedObject_Drop(object sender, DragEventArgs e)
	{
		ResetVisualStates();
		if (!e.Data.GetDataPresent(AcceptedDataFormat))
		{
			return;
		}
		object data = e.Data.GetData(AcceptedDataFormat);
		int num = base.AssociatedObject.Items.IndexOf(data);
		int num2 = GetDropIndex(e);
		if (num >= 0 && num < num2)
		{
			num2--;
		}
		if (base.AssociatedObject.ItemsSource is IObservableCollectionEx observableCollectionEx)
		{
			if (num >= 0)
			{
				observableCollectionEx.Move(num, num2);
			}
			else
			{
				observableCollectionEx.Insert(num2, data);
			}
		}
		else if (base.AssociatedObject.ItemsSource is IList list)
		{
			if (num >= 0)
			{
				list.RemoveAt(num);
				list.Insert(num2, data);
			}
			else
			{
				list.Insert(num2, data);
			}
		}
	}

	private void associatedObject_DragLeave(object sender, DragEventArgs e)
	{
		ResetVisualStates();
	}

	private void associatedObject_DragOver(object sender, DragEventArgs e)
	{
		HandleDrag(e);
	}

	private void associatedObject_DragEnter(object sender, DragEventArgs e)
	{
		HandleDrag(e);
	}

	private void ResetVisualStates()
	{
		for (int i = 0; i < base.AssociatedObject.Items.Count; i++)
		{
			if (base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) is ListBoxItem control)
			{
				VisualStateManager.GoToState(control, "NoDrop", useTransitions: true);
				VisualStateManager.GoToState(control, "NoDrag", useTransitions: true);
			}
		}
	}

	private void HandleDrag(DragEventArgs e)
	{
		if (e.Data.GetDataPresent(AcceptedDataFormat))
		{
			object data = e.Data.GetData(AcceptedDataFormat);
			int num = base.AssociatedObject.Items.IndexOf(data);
			if (num >= 0)
			{
				if (base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(num) is Control control)
				{
					VisualStateManager.GoToState(control, "Dragged", useTransitions: true);
				}
				e.Effects &= DragDropEffects.Move;
			}
			else
			{
				e.Effects &= DragDropEffects.Copy;
			}
			int dropIndex = GetDropIndex(e);
			if (dropIndex != num && dropIndex != num + 1)
			{
				for (int i = 0; i < base.AssociatedObject.Items.Count; i++)
				{
					if (!(base.AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) is ListBoxItem control2))
					{
						continue;
					}
					if (i < dropIndex)
					{
						if (i < dropIndex - 1)
						{
							VisualStateManager.GoToState(control2, "DroppingAfter", useTransitions: true);
						}
						else if (num != i)
						{
							VisualStateManager.GoToState(control2, "DroppingImmediatelyAfter", useTransitions: true);
						}
					}
					else if (i > dropIndex)
					{
						VisualStateManager.GoToState(control2, "DroppingBefore", useTransitions: true);
					}
					else if (num != i)
					{
						VisualStateManager.GoToState(control2, "DroppingImmediatelyBefore", useTransitions: true);
					}
				}
			}
			else
			{
				ResetVisualStates();
			}
			if (_scrollPresenter != null)
			{
				double y = e.GetPosition(base.AssociatedObject).Y;
				if (y < 50.0)
				{
					StartScrolling(up: true);
				}
				else if (y > base.AssociatedObject.ActualHeight - 50.0)
				{
					StartScrolling(up: false);
				}
				else
				{
					StopScrolling();
				}
			}
			e.Handled = true;
		}
		else
		{
			e.Effects = DragDropEffects.None;
		}
	}

	private void StartScrolling(bool up)
	{
		if (_isAnimationStarted || _scrollPresenter == null)
		{
			return;
		}
		if (_tween == null)
		{
			_tween = new BidirectionalTween
			{
				CurrentValueFunc = () => _scrollPresenter.VerticalOffset,
				StartValueFunc = () => _scrollPresenter.VerticalOffset,
				EndValueFunc = () => (!_isScrollingUpward) ? (_scrollPresenter.ExtentHeight - _scrollPresenter.ViewportHeight) : 0.0,
				ApplyValueAction = delegate(double value)
				{
					_scrollPresenter.SetVerticalOffset(value);
				},
				Duration = () => TimeSpan.FromMilliseconds(Math.Abs(_scrollPresenter.VerticalOffset - (_isScrollingUpward ? 0.0 : (_scrollPresenter.ExtentHeight - _scrollPresenter.ViewportHeight))) * 5.0)
			};
		}
		_isScrollingUpward = up;
		_tween.StartForward();
		_isAnimationStarted = true;
	}

	private void StopScrolling()
	{
		if (_isAnimationStarted)
		{
			_tween.Stop();
			_isAnimationStarted = false;
		}
	}

	private int GetDropIndex(DragEventArgs e)
	{
		ListBoxItem dragOverContainer = null;
		VisualTreeHelper.HitTest(base.AssociatedObject, delegate(DependencyObject visual)
		{
			if (visual is ListBoxItem listBoxItem)
			{
				dragOverContainer = listBoxItem;
				return HitTestFilterBehavior.Stop;
			}
			return HitTestFilterBehavior.Continue;
		}, (HitTestResult _) => HitTestResultBehavior.Stop, new PointHitTestParameters(e.GetPosition(base.AssociatedObject)));
		int num = base.AssociatedObject.Items.Count;
		if (dragOverContainer != null)
		{
			num = base.AssociatedObject.ItemContainerGenerator.IndexFromContainer(dragOverContainer);
			if (e.GetPosition(dragOverContainer).Y > dragOverContainer.ActualHeight / 2.0)
			{
				num++;
			}
		}
		return num;
	}
}
