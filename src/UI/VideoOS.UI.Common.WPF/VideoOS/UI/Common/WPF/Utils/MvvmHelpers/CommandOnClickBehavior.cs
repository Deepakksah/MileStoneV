using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class CommandOnClickBehavior : Behavior<FrameworkElement>
{
	public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof(CommandOnClickBehavior));

	public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(CommandOnClickBehavior));

	private ClickMode _clickMode;

	public ICommand Command
	{
		get
		{
			return (ICommand)((DependencyObject)(object)this).GetValue(CommandProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(CommandProperty, (object)value);
		}
	}

	public object CommandParameter
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(CommandParameterProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(CommandParameterProperty, value);
		}
	}

	public ClickMode ClickMode
	{
		get
		{
			return _clickMode;
		}
		set
		{
			_clickMode = value;
			if (ClickMode != ClickMode.Release)
			{
				throw new NotImplementedException();
			}
		}
	}

	public ModifierKeys ModifierKeys { get; set; }

	public CommandOnClickBehavior()
	{
		_clickMode = ClickMode.Release;
		ModifierKeys = ModifierKeys.None;
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
	}

	private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (Keyboard.Modifiers == ModifierKeys && Command != null && Command.CanExecute(CommandParameter))
		{
			Command.Execute(CommandParameter);
		}
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
	}
}
