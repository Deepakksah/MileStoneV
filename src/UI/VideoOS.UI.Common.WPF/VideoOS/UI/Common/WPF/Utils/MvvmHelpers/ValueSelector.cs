using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

[ContentProperty("ConditionEntries")]
public class ValueSelector : Behavior<DependencyObject>
{
	private ObservableCollection<ValueSelectorCondition> _conditionEntries = new ObservableCollection<ValueSelectorCondition>();

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ValueSelector), new FrameworkPropertyMetadata(null, ValuePropertyChanged));

	public ObservableCollection<ValueSelectorCondition> ConditionEntries => _conditionEntries;

	public DependencyProperty Property { get; set; }

	public object Value
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(ValueProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(ValueProperty, value);
		}
	}

	public ValueSelector()
	{
		_conditionEntries.CollectionChanged += _conditionEntries_CollectionChanged;
	}

	public virtual void OnValueChanged()
	{
		object obj = Value;
		if (Value != null && Value.GetType().IsEnum)
		{
			obj = Value.ToString();
		}
		foreach (ValueSelectorCondition conditionEntry in _conditionEntries)
		{
			bool flag = false;
			if ((obj == null || conditionEntry.Condition == null) ? (obj == conditionEntry) : (string.Compare(obj.ToString(), conditionEntry.Condition.ToString(), StringComparison.OrdinalIgnoreCase) == 0))
			{
				if (Property != null)
				{
					base.AssociatedObject.SetCurrentValue(Property, conditionEntry.Value);
				}
				break;
			}
		}
	}

	private void _conditionEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.NewItems == null)
		{
			return;
		}
		foreach (object newItem in e.NewItems)
		{
			if (newItem is ValueSelectorCondition valueSelectorCondition)
			{
				valueSelectorCondition.ParentInternal = (DependencyObject)(object)this;
			}
		}
	}

	private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as ValueSelector).OnValueChanged();
	}
}
