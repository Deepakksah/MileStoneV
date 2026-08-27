using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.Mobile.UI.Behaviors;

public class MultiSelectionBehavior : Behavior<Selector>
{
	public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiSelectionBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsPropertyChanged));

	public IList SelectedItems
	{
		get
		{
			return (IList)((DependencyObject)(object)this).GetValue(SelectedItemsProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(SelectedItemsProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		base.AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
	}

	protected override void OnDetaching()
	{
		base.AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
	}

	private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		AddNewItems(SelectedItems, e.AddedItems);
		RemoveOldItems(SelectedItems, e.RemovedItems);
	}

	private static void OnSelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			throw new ArgumentNullException("e.NewValue", "The bound collection must not be null.");
		}
		if (((e.NewValue as IList) ?? throw new ArgumentNullException("e.NewValue", "The bound collection must not implement IList.")).Count > 0)
		{
			throw new ArgumentException("Bound collection must be empty when bound.", "e.NewValue");
		}
	}

	private static void AddNewItems(IList collectionToUpdate, IList newItems)
	{
		if (collectionToUpdate == null || newItems == null || newItems.Count <= 0)
		{
			return;
		}
		foreach (object newItem in newItems)
		{
			if (!collectionToUpdate.Contains(newItem))
			{
				collectionToUpdate.Add(newItem);
			}
		}
	}

	private static void RemoveOldItems(IList collectionToUpdate, IList oldItems)
	{
		if (collectionToUpdate == null || oldItems == null || oldItems.Count <= 0)
		{
			return;
		}
		foreach (object oldItem in oldItems)
		{
			if (collectionToUpdate.Contains(oldItem))
			{
				collectionToUpdate.Remove(oldItem);
			}
		}
	}
}
