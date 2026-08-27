using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using VideoOS.UI.Common.WPF.Annotations;

namespace VideoOS.UI.Common.WPF.ObservableCollections;

public class BulkObservableCollection<T> : ObservableCollection<T>
{
	public sealed class NotificationsAutoresetSwitch : IDisposable
	{
		[NotNull]
		private readonly BulkObservableCollection<T> _hostCollection;

		private readonly bool _supressNotificationsOrigState;

		public NotificationsAutoresetSwitch([NotNull] BulkObservableCollection<T> hostCollection, bool suppressNotifications)
		{
			if (hostCollection == null)
			{
				throw new ArgumentNullException("hostCollection");
			}
			_hostCollection = hostCollection;
			_supressNotificationsOrigState = _hostCollection.SuppressNotifications(suppressNotifications);
		}

		public void Dispose()
		{
			_hostCollection.SuppressNotifications(_supressNotificationsOrigState);
		}
	}

	private bool _suppressNotifications;

	[NotNull]
	private static Dispatcher Dispatcher => Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

	public BulkObservableCollection()
	{
	}

	public BulkObservableCollection(IEnumerable<T> collection)
		: base(collection)
	{
	}

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		if (_suppressNotifications)
		{
			return;
		}
		if (Dispatcher.CheckAccess())
		{
			base.OnCollectionChanged(e);
			return;
		}
		Dispatcher.InvokeAsync(delegate
		{
			base.OnCollectionChanged(e);
		});
	}

	public bool SuppressNotifications(bool newState)
	{
		bool suppressNotifications = _suppressNotifications;
		_suppressNotifications = newState;
		return suppressNotifications;
	}

	[DebuggerStepThrough]
	public void AddRange([NotNull] IEnumerable<T> range)
	{
		AddRange(range, NotifyCollectionChangedAction.Add);
	}

	[DebuggerStepThrough]
	public void AddRange([NotNull] IEnumerable<T> range, NotifyCollectionChangedAction notificationAction)
	{
		if (range == null)
		{
			throw new ArgumentNullException("range");
		}
		if (notificationAction != NotifyCollectionChangedAction.Add && notificationAction != NotifyCollectionChangedAction.Reset)
		{
			throw new InvalidOperationException("Only notification action of type Add or Reset is valid");
		}
		T[] array = range.ToArray();
		bool suppressNotifications = _suppressNotifications;
		_suppressNotifications = true;
		try
		{
			T[] array2 = array;
			foreach (T item in array2)
			{
				Add(item);
			}
		}
		finally
		{
			_suppressNotifications = suppressNotifications;
		}
		if (array.Any())
		{
			NotifyCollectionChangedEventArgs e = ((notificationAction != NotifyCollectionChangedAction.Add) ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, array));
			OnCollectionChanged(e);
		}
	}

	[DebuggerStepThrough]
	public void RemoveRange([NotNull] IEnumerable<T> range)
	{
		RemoveRange(range, NotifyCollectionChangedAction.Reset);
	}

	public void RemoveRange([NotNull] IEnumerable<T> range, NotifyCollectionChangedAction notificationAction)
	{
		if (range == null)
		{
			throw new ArgumentNullException("range");
		}
		if (notificationAction != NotifyCollectionChangedAction.Remove && notificationAction != NotifyCollectionChangedAction.Reset)
		{
			throw new InvalidOperationException("Only notification action of type Add or Reset is valid");
		}
		T[] array = range.ToArray();
		bool suppressNotifications = _suppressNotifications;
		_suppressNotifications = true;
		try
		{
			if (base.Items.Count < 100)
			{
				T[] array2 = array;
				foreach (T item in array2)
				{
					Remove(item);
				}
			}
			else
			{
				List<T> range2 = base.Items.Except(array).ToList();
				Clear();
				AddRange(range2);
			}
		}
		finally
		{
			_suppressNotifications = suppressNotifications;
		}
		if (array.Any())
		{
			NotifyCollectionChangedEventArgs e = ((notificationAction != NotifyCollectionChangedAction.Remove) ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, array));
			OnCollectionChanged(e);
		}
	}

	public NotificationsAutoresetSwitch SwitchNotification(bool suppressNotifications)
	{
		return new NotificationsAutoresetSwitch(this, suppressNotifications);
	}
}
