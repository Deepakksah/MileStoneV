using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Behaviors;

public abstract class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
{
	private bool _isSetup;

	private bool _isHookedUp;

	private WeakReference _weakTarget;

	protected virtual void OnSetup()
	{
	}

	protected virtual void OnCleanup()
	{
	}

	protected virtual void Attached()
	{
	}

	protected virtual void Detached()
	{
	}

	protected override void OnChanged()
	{
		T associatedObject = base.AssociatedObject;
		if (associatedObject != null)
		{
			HookupBehavior(associatedObject);
		}
		else
		{
			UnHookupBehavior();
		}
	}

	private void OnTarget_Loaded(object sender, RoutedEventArgs e)
	{
		SetupBehavior();
		Attached();
	}

	private void OnTarget_Unloaded(object sender, RoutedEventArgs e)
	{
		CleanupBehavior();
		Detached();
	}

	private void HookupBehavior(T target)
	{
		if (!_isHookedUp)
		{
			_weakTarget = new WeakReference(target);
			_isHookedUp = true;
			target.Unloaded += OnTarget_Unloaded;
			target.Loaded += OnTarget_Loaded;
			SetupBehavior();
		}
	}

	private void UnHookupBehavior()
	{
		if (_isHookedUp)
		{
			_isHookedUp = false;
			FrameworkElement associatedObject = GetAssociatedObject();
			if (associatedObject != null)
			{
				associatedObject.Unloaded -= OnTarget_Unloaded;
				associatedObject.Loaded -= OnTarget_Loaded;
			}
			CleanupBehavior();
		}
	}

	private void SetupBehavior()
	{
		if (!_isSetup)
		{
			_isSetup = true;
			OnSetup();
		}
	}

	private void CleanupBehavior()
	{
		if (_isSetup)
		{
			_isSetup = false;
			OnCleanup();
		}
	}

	protected FrameworkElement GetAssociatedObject()
	{
		return (T)_weakTarget.Target;
	}
}
