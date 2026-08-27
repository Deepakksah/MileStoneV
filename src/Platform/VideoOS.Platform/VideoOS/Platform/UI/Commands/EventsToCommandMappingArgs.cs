using System;
using System.Windows;

namespace VideoOS.Platform.UI.Commands;

public class EventsToCommandMappingArgs : EventArgs
{
	public string EventName { get; }

	public DependencyObject Sender { get; }

	public object OriginalEventArgs { get; }

	public object CommandParameter { get; }

	public object CommandParameter2 { get; }

	public EventsToCommandMappingArgs(string eventName, DependencyObject sender, object originalEventArgs, object commandParameter, object commandParameter2)
	{
		EventName = eventName;
		Sender = sender;
		OriginalEventArgs = originalEventArgs;
		CommandParameter = commandParameter;
		CommandParameter2 = commandParameter2;
	}
}
