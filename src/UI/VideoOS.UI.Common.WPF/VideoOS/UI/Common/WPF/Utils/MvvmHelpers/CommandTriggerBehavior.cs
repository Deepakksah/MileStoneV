using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class CommandTriggerBehavior : Behavior<FrameworkElement>, ICommandSource
{
	public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof(CommandTriggerBehavior));

	public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(CommandTriggerBehavior));

	public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof(CommandTriggerBehavior));

	public static readonly DependencyProperty CommandTriggerProperty = DependencyProperty.Register("CommandTrigger", typeof(CommandTrigger), typeof(CommandTriggerBehavior), new FrameworkPropertyMetadata(null, CommandTriggerPropertyChanged));

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

	public IInputElement CommandTarget
	{
		get
		{
			return (IInputElement)((DependencyObject)(object)this).GetValue(CommandTargetProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(CommandTargetProperty, (object)value);
		}
	}

	public CommandTrigger CommandTrigger
	{
		get
		{
			return (CommandTrigger)((DependencyObject)(object)this).GetValue(CommandTriggerProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(CommandTriggerProperty, (object)value);
		}
	}

	private static void CommandTriggerPropertyChanged(DependencyObject instance, DependencyPropertyChangedEventArgs args)
	{
		CommandTriggerBehavior obj = instance as CommandTriggerBehavior;
		CommandTrigger newTrigger = args.NewValue as CommandTrigger;
		CommandTrigger oldTrigger = args.OldValue as CommandTrigger;
		obj.OnCommandTriggerChanged(oldTrigger, newTrigger);
	}

	private void OnCommandTriggerChanged(CommandTrigger oldTrigger, CommandTrigger newTrigger)
	{
		if (oldTrigger != null)
		{
			oldTrigger.CommandTriggered += trigger_CommandTriggered;
		}
		if (newTrigger != null)
		{
			newTrigger.CommandTriggered += trigger_CommandTriggered;
		}
	}

	private void trigger_CommandTriggered(object sender, EventArgs e)
	{
		ExecuteCommandCore();
	}

	private void ExecuteCommandCore()
	{
		if (Command == null)
		{
			return;
		}
		if (Command is RoutedCommand routedCommand)
		{
			IInputElement target = CommandTarget ?? base.AssociatedObject;
			if (routedCommand.CanExecute(CommandParameter, target))
			{
				routedCommand.Execute(CommandParameter, target);
			}
		}
		else if (Command.CanExecute(CommandParameter))
		{
			Command.Execute(CommandParameter);
		}
	}
}
