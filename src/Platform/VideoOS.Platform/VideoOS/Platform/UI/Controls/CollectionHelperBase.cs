using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace VideoOS.Platform.UI.Controls;

public abstract class CollectionHelperBase : ICollectionHelper
{
	private static readonly DependencyProperty CollectionHelperProperty = DependencyProperty.RegisterAttached("CollectionHelper", typeof(ICollectionHelper), typeof(CollectionHelperBase), new PropertyMetadata(null));

	protected readonly FrameworkElement Host;

	protected readonly DependencyProperty DependencyProperty;

	private readonly bool _aggregateCallbackInvocations;

	private bool _pendingCollectionChangedNotification;

	protected static void SetCollectionHelper(DependencyObject element, ICollectionHelper value)
	{
		element.SetValue(CollectionHelperProperty, value);
	}

	protected static ICollectionHelper GetCollectionHelper(DependencyObject element)
	{
		return (ICollectionHelper)element.GetValue(CollectionHelperProperty);
	}

	public CollectionHelperBase(FrameworkElement host, DependencyProperty dependencyProperty, bool aggregateCallbackInvocations)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		if (host.IsLoaded)
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Initialize(...) should be called before the {0} is loaded", "host")), "host");
		}
		if (dependencyProperty == null)
		{
			throw new ArgumentNullException("dependencyProperty");
		}
		PropertyChangedCallback propertyChangedCallback = dependencyProperty.GetMetadata(dependencyProperty.OwnerType).PropertyChangedCallback;
		if (propertyChangedCallback == null || propertyChangedCallback.Method != typeof(CollectionHelperBase).GetMethod("CollectionChangedCallback"))
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Property changed callback not registered to invoke {0}.{1}", GetType().Name, "CollectionChangedCallback")), "dependencyProperty");
		}
		Host = host;
		DependencyProperty = dependencyProperty;
		_aggregateCallbackInvocations = aggregateCallbackInvocations;
		Host.Loaded += HostOnLoaded;
		Host.Unloaded += HostOnUnloaded;
		SetCollectionHelper(host, this);
	}

	public abstract ICollectionChangedInfo CreateCollectionChangedInfo(CollectionChange change, IEnumerable removedItems, IEnumerable addedItems);

	protected abstract void InvokeCollectionChangedCallback(ICollectionChangedInfo info);

	private void HostOnLoaded(object sender, RoutedEventArgs e)
	{
		ICollection collection = (ICollection)Host.GetValue(DependencyProperty);
		if (collection != null)
		{
			if (collection is INotifyCollectionChanged notifyCollectionChanged)
			{
				notifyCollectionChanged.CollectionChanged += ((ICollectionHelper)this).HandleCollectionChanged;
			}
			if (collection.Count > 0)
			{
				((ICollectionHelper)this).RaiseCollectionChanged(CreateCollectionChangedInfo(CollectionChange.HostLoaded, null, collection));
			}
		}
	}

	private void HostOnUnloaded(object sender, RoutedEventArgs e)
	{
		ICollection collection = (ICollection)Host.GetValue(DependencyProperty);
		if (collection != null)
		{
			if (collection is INotifyCollectionChanged notifyCollectionChanged)
			{
				notifyCollectionChanged.CollectionChanged -= ((ICollectionHelper)this).HandleCollectionChanged;
			}
			if (collection.Count > 0)
			{
				((ICollectionHelper)this).RaiseCollectionChanged(CreateCollectionChangedInfo(CollectionChange.HostUnloaded, collection, null));
			}
		}
	}

	public void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		((ICollectionHelper)this).RaiseCollectionChanged(e.Action switch
		{
			NotifyCollectionChangedAction.Add => CreateCollectionChangedInfo(CollectionChange.Add, e.OldItems, e.NewItems), 
			NotifyCollectionChangedAction.Remove => CreateCollectionChangedInfo(CollectionChange.Remove, e.OldItems, e.NewItems), 
			NotifyCollectionChangedAction.Replace => CreateCollectionChangedInfo(CollectionChange.Replace, e.OldItems, e.NewItems), 
			NotifyCollectionChangedAction.Move => CreateCollectionChangedInfo(CollectionChange.Move, e.OldItems, e.NewItems), 
			NotifyCollectionChangedAction.Reset => CreateCollectionChangedInfo(CollectionChange.NewOrResetCollection, e.OldItems, e.NewItems), 
			_ => throw new ArgumentOutOfRangeException("Action", e.Action, null), 
		});
	}

	public void RaiseCollectionChanged(ICollectionChangedInfo info)
	{
		if (_aggregateCallbackInvocations)
		{
			if (!_pendingCollectionChangedNotification)
			{
				_pendingCollectionChangedNotification = true;
				Host.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
				{
					_pendingCollectionChangedNotification = false;
					InvokeCollectionChangedCallback(info);
				});
			}
		}
		else
		{
			InvokeCollectionChangedCallback(info);
		}
	}

	public static void CollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		FrameworkElement frameworkElement = (FrameworkElement)d;
		if (frameworkElement == null || !frameworkElement.IsInitialized)
		{
			return;
		}
		ICollectionHelper collectionHelper = GetCollectionHelper(frameworkElement);
		if (collectionHelper != null)
		{
			if (e.OldValue is INotifyCollectionChanged notifyCollectionChanged)
			{
				notifyCollectionChanged.CollectionChanged -= collectionHelper.HandleCollectionChanged;
			}
			if (e.NewValue is INotifyCollectionChanged notifyCollectionChanged2)
			{
				notifyCollectionChanged2.CollectionChanged += collectionHelper.HandleCollectionChanged;
			}
			collectionHelper.RaiseCollectionChanged(collectionHelper.CreateCollectionChangedInfo(CollectionChange.NewOrResetCollection, (IEnumerable)e.OldValue, (IEnumerable)e.NewValue));
		}
	}
}
