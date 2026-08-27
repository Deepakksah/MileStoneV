using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public class CollectionHelper<TItem> : CollectionHelperBase
{
	private readonly Action<CollectionChangedInfo<TItem>> _collectionChangedCallback;

	private CollectionHelper(FrameworkElement host, DependencyProperty dependencyProperty, Action<CollectionChangedInfo<TItem>> collectionChangedCallback, Collection<TItem> defaultCollection, bool aggregateCallbackInvocations)
		: base(host, dependencyProperty, aggregateCallbackInvocations)
	{
		if (!typeof(ICollection<TItem>).IsAssignableFrom(dependencyProperty.PropertyType))
		{
			throw new ArgumentException(FormattableString.Invariant($"Must be of type {typeof(ICollection<TItem>).Name}"), "dependencyProperty");
		}
		if (defaultCollection != null)
		{
			Host.SetCurrentValue(DependencyProperty, defaultCollection);
		}
		_collectionChangedCallback = collectionChangedCallback;
	}

	public static void Initialize(FrameworkElement host, DependencyProperty dependencyProperty, Action<CollectionChangedInfo<TItem>> collectionChangedAction, Collection<TItem> defaultCollection = null, bool aggregateCallbackInvocations = false)
	{
		new CollectionHelper<TItem>(host, dependencyProperty, collectionChangedAction, defaultCollection, aggregateCallbackInvocations);
	}

	public override ICollectionChangedInfo CreateCollectionChangedInfo(CollectionChange change, IEnumerable removedItems, IEnumerable addedItems)
	{
		return new CollectionChangedInfo<TItem>(change, removedItems?.OfType<TItem>(), addedItems?.OfType<TItem>());
	}

	protected override void InvokeCollectionChangedCallback(ICollectionChangedInfo info)
	{
		_collectionChangedCallback?.Invoke((CollectionChangedInfo<TItem>)info);
	}
}
public class CollectionHelper : CollectionHelperBase
{
	private readonly Action<CollectionChangedInfo> _collectionChangedCallback;

	private CollectionHelper(FrameworkElement host, DependencyProperty dependencyProperty, Action<CollectionChangedInfo> collectionChangedCallback, ICollection defaultCollection, bool aggregateCallbackInvocations)
		: base(host, dependencyProperty, aggregateCallbackInvocations)
	{
		if (!typeof(ICollection).IsAssignableFrom(dependencyProperty.PropertyType))
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Must be of type {0}", "ICollection")), "dependencyProperty");
		}
		if (defaultCollection != null)
		{
			Host.SetCurrentValue(DependencyProperty, defaultCollection);
		}
		_collectionChangedCallback = collectionChangedCallback;
	}

	public static void Initialize(FrameworkElement host, DependencyProperty dependencyProperty, Action<CollectionChangedInfo> collectionChangedAction, ICollection defaultCollection = null, bool aggregateCallbackInvocations = false)
	{
		new CollectionHelper(host, dependencyProperty, collectionChangedAction, defaultCollection, aggregateCallbackInvocations);
	}

	public override ICollectionChangedInfo CreateCollectionChangedInfo(CollectionChange change, IEnumerable removedItems, IEnumerable addedItems)
	{
		return new CollectionChangedInfo(change, removedItems, addedItems);
	}

	protected override void InvokeCollectionChangedCallback(ICollectionChangedInfo info)
	{
		_collectionChangedCallback?.Invoke((CollectionChangedInfo)info);
	}
}
