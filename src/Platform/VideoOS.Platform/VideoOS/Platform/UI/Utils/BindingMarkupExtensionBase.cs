using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Utils;

public abstract class BindingMarkupExtensionBase<TState, TReturnType> : MarkupExtension where TState : BindingMarkupExtensionBase<TState, TReturnType>.StateBase, new()
{
	public class StateBase
	{
		public FrameworkElement TargetFrameworkElement { get; set; }

		public DependencyProperty TargetDependencyProperty { get; set; }

		public List<Action> RemovePropertyListenerActions { get; } = new List<Action>();

		public bool InitStateCalled { get; set; }
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
		{
			if (provideValueTarget.TargetObject is FrameworkElement frameworkElement)
			{
				TState state = new TState();
				state.TargetFrameworkElement = frameworkElement;
				state.TargetDependencyProperty = (DependencyProperty)provideValueTarget.TargetProperty;
				frameworkElement.Loaded += delegate
				{
					TargetFrameworkElementLoaded(state);
				};
				frameworkElement.Unloaded += delegate
				{
					TargetFrameworkElementUnloaded(state);
				};
				if (frameworkElement.IsLoaded)
				{
					TargetFrameworkElementLoaded(state);
					return ResolveValue(state);
				}
				return GetDefaultValue();
			}
			return this;
		}
		return default(TReturnType);
	}

	private void TargetFrameworkElementLoaded(TState state)
	{
		if (!state.InitStateCalled)
		{
			InitState(state);
			state.InitStateCalled = true;
		}
		AddPropertyListeners(state);
		UpdateValue(state);
	}

	private static void TargetFrameworkElementUnloaded(TState state)
	{
		RemovePropertyListeners(state);
	}

	private static void RemovePropertyListeners(TState state)
	{
		foreach (Action removePropertyListenerAction in state.RemovePropertyListenerActions)
		{
			removePropertyListenerAction();
		}
		state.RemovePropertyListenerActions.Clear();
	}

	public void AddPropertyListener(TState state, string propertyName, object propertyOwner, bool refreshPropertyListenersOnChange)
	{
		Type type = propertyOwner.GetType();
		DependencyPropertyDescriptor propertyDependencyPropertyDescriptor = DependencyPropertyDescriptor.FromName(propertyName, type, type);
		if (propertyDependencyPropertyDescriptor != null)
		{
			EventHandler propertyChangedEventHandler = delegate
			{
				PropertyChanged(state, refreshPropertyListenersOnChange);
			};
			propertyDependencyPropertyDescriptor.AddValueChanged(propertyOwner, propertyChangedEventHandler);
			state.RemovePropertyListenerActions.Add(delegate
			{
				propertyDependencyPropertyDescriptor.RemoveValueChanged(propertyOwner, propertyChangedEventHandler);
			});
			return;
		}
		INotifyPropertyChanged propertyOwnerNotifyPropertyChanged = propertyOwner as INotifyPropertyChanged;
		if (propertyOwnerNotifyPropertyChanged == null)
		{
			return;
		}
		PropertyChangedEventHandler propertyChangedEventHandler2 = delegate(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == propertyName)
			{
				PropertyChanged(state, refreshPropertyListenersOnChange);
			}
		};
		propertyOwnerNotifyPropertyChanged.PropertyChanged += propertyChangedEventHandler2;
		state.RemovePropertyListenerActions.Add(delegate
		{
			propertyOwnerNotifyPropertyChanged.PropertyChanged -= propertyChangedEventHandler2;
		});
	}

	private void PropertyChanged(TState state, bool refreshPropertyListeners)
	{
		UpdateValue(state);
		if (refreshPropertyListeners)
		{
			RemovePropertyListeners(state);
			AddPropertyListeners(state);
		}
	}

	private void UpdateValue(TState state)
	{
		TReturnType val = ResolveValue(state);
		state.TargetFrameworkElement.SetValue(state.TargetDependencyProperty, val);
	}

	public abstract void InitState(TState state);

	public abstract void AddPropertyListeners(TState state);

	public abstract TReturnType GetDefaultValue();

	public abstract TReturnType ResolveValue(TState state);
}
