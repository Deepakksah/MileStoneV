using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.UI.Common.WPF.Utils.Validation;

public class ValidatingObservableCollection<T> : ObservableCollection<T>
{
	public event EventHandler<ValidateCollectionChangedEventArgs<T>> ValidationRequested = delegate
	{
	};

	public ValidatingObservableCollection()
	{
	}

	public ValidatingObservableCollection(IEnumerable<T> collection)
		: base(collection)
	{
	}

	protected override void InsertItem(int index, T item)
	{
		ValidateCollectionChangedEventArgs<T> e = new ValidateCollectionChangedEventArgs<T>(ValidateCollectionChangedAction.Insert, index, item);
		this.ValidationRequested(this, e);
		if (!e.Cancel)
		{
			base.InsertItem(index, item);
		}
	}

	protected override void RemoveItem(int index)
	{
		ValidateCollectionChangedEventArgs<T> e = new ValidateCollectionChangedEventArgs<T>(ValidateCollectionChangedAction.Remove, index, base[index]);
		this.ValidationRequested(this, e);
		if (!e.Cancel)
		{
			base.RemoveItem(index);
		}
	}

	protected override void SetItem(int index, T item)
	{
		ValidateCollectionChangedEventArgs<T> e = new ValidateCollectionChangedEventArgs<T>(ValidateCollectionChangedAction.Replace, index, base[index], item);
		this.ValidationRequested(this, e);
		if (!e.Cancel)
		{
			base.SetItem(index, item);
		}
	}

	protected override void ClearItems()
	{
		ValidateCollectionChangedEventArgs<T> e = new ValidateCollectionChangedEventArgs<T>(ValidateCollectionChangedAction.Reset);
		this.ValidationRequested(this, e);
		if (!e.Cancel)
		{
			base.ClearItems();
		}
	}
}
