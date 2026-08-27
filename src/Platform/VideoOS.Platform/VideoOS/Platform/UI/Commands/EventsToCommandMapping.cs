using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace VideoOS.Platform.UI.Commands;

public static class EventsToCommandMapping
{
	private class EventToCommandProxyManager : IDisposable
	{
		private DependencyObject _element;

		private readonly List<EventToCommandProxy> _proxies = new List<EventToCommandProxy>();

		private bool _disposed;

		public EventToCommandProxyManager(DependencyObject element)
		{
			_element = element;
		}

		public void Update(string eventNamesString)
		{
			if (_disposed)
			{
				return;
			}
			_proxies.ForEach(delegate(EventToCommandProxy p)
			{
				p.Dispose();
			});
			_proxies.Clear();
			if (!string.IsNullOrWhiteSpace(eventNamesString))
			{
				string[] array = eventNamesString.Split(',');
				foreach (string text in array)
				{
					_proxies.Add(new EventToCommandProxy(_element, text.Trim()));
				}
			}
		}

		public void InvokeLoaded(RoutedEventArgs loadedEventArgs)
		{
			_proxies.FirstOrDefault((EventToCommandProxy p) => p.EventName == "Loaded")?.Execute(loadedEventArgs);
		}

		public void InvokeUnloaded(RoutedEventArgs unloadedEventArgs)
		{
			_proxies.FirstOrDefault((EventToCommandProxy p) => p.EventName == "Unloaded")?.Execute(unloadedEventArgs);
		}

		public void InvokeInitialized(EventArgs loadedEventArgs)
		{
			_proxies.FirstOrDefault((EventToCommandProxy p) => p.EventName == "Initialized")?.Execute(loadedEventArgs);
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_proxies.ForEach(delegate(EventToCommandProxy p)
				{
					p.Dispose();
				});
				_proxies.Clear();
				_element = null;
			}
		}
	}

	private class EventToCommandProxy : IDisposable
	{
		private DependencyObject _element;

		private readonly EventInfo _eventInfo;

		private readonly Delegate _eventHandler;

		private bool _disposed;

		private readonly RoutedEventHandler _attachedEventHandler;

		private readonly MethodInfo _removeAttachedEventHandlerInfo;

		public string EventName { get; }

		public EventToCommandProxy(DependencyObject element, string eventName)
		{
			_element = element;
			EventName = eventName;
			_eventInfo = _element.GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);
			MethodInfo attachedEventAddHandlerInfo;
			if (_eventInfo != null)
			{
				Type eventArgsType = GetEventArgsType(_eventInfo);
				MethodInfo method = GetType().GetMethod("OnEvent", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(eventArgsType);
				_eventHandler = Delegate.CreateDelegate(_eventInfo.EventHandlerType, this, method);
				_eventInfo.AddEventHandler(_element, _eventHandler);
			}
			else if (TryResolveAttachedEventMethodInfos(eventName, out attachedEventAddHandlerInfo, out _removeAttachedEventHandlerInfo))
			{
				_attachedEventHandler = OnAttachedEvent;
				attachedEventAddHandlerInfo.Invoke(null, new object[2] { _element, _attachedEventHandler });
			}
		}

		private bool TryResolveAttachedEventMethodInfos(string eventName, out MethodInfo attachedEventAddHandlerInfo, out MethodInfo attachedEventRemoveHandlerInfo)
		{
			attachedEventAddHandlerInfo = null;
			attachedEventRemoveHandlerInfo = null;
			int num = eventName.LastIndexOf('.');
			if (num <= 0)
			{
				return false;
			}
			string name = eventName.Substring(0, num);
			string text = eventName.Substring(num + 1);
			Type type = GetType().Assembly.GetType(name);
			if (type == null)
			{
				return false;
			}
			string name2 = "Add" + text + "Handler";
			string name3 = "Remove" + text + "Handler";
			attachedEventAddHandlerInfo = type.GetMethod(name2, BindingFlags.Static | BindingFlags.NonPublic) ?? type.GetMethod(name2, BindingFlags.Static | BindingFlags.Public);
			attachedEventRemoveHandlerInfo = type.GetMethod(name3, BindingFlags.Static | BindingFlags.NonPublic) ?? type.GetMethod(name2, BindingFlags.Static | BindingFlags.Public);
			if (attachedEventAddHandlerInfo != null)
			{
				return attachedEventRemoveHandlerInfo != null;
			}
			return false;
		}

		private void OnEvent<EventArgsType>(object sender, EventArgsType eventArgs)
		{
			ExecuteCommands(sender, eventArgs);
		}

		private void OnAttachedEvent(object sender, RoutedEventArgs eventArgs)
		{
			ExecuteCommands(sender, eventArgs);
		}

		private void ExecuteCommands(object sender, object originalEventArgs)
		{
			if (!_disposed)
			{
				object commandParameter = GetCommandParameter(_element);
				object commandParameter2 = GetCommandParameter2(_element);
				EventsToCommandMappingArgs parameter = new EventsToCommandMappingArgs(EventName, sender as DependencyObject, originalEventArgs, commandParameter, commandParameter2);
				GetCommand(_element)?.Execute(parameter);
			}
		}

		private static Type GetEventArgsType(EventInfo eventType)
		{
			ParameterInfo[] obj = eventType.EventHandlerType.GetMethod("Invoke")?.GetParameters();
			if (obj == null)
			{
				return null;
			}
			return obj[1].ParameterType;
		}

		public void Execute(object originalEventArgs)
		{
			ExecuteCommands(_element, originalEventArgs);
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				if ((object)_eventHandler != null)
				{
					_eventInfo.RemoveEventHandler(_element, _eventHandler);
				}
				if (_attachedEventHandler != null)
				{
					_removeAttachedEventHandlerInfo.Invoke(null, new object[2] { _element, _attachedEventHandler });
				}
				_element = null;
			}
		}
	}

	private static readonly Dictionary<DependencyObject, EventToCommandProxyManager> EventProxyManagers = new Dictionary<DependencyObject, EventToCommandProxyManager>();

	public static readonly DependencyProperty EventsProperty = DependencyProperty.RegisterAttached("Events", typeof(string), typeof(EventsToCommandMapping), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, EventsChangedCallback));

	public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventsToCommandMapping), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(EventsToCommandMapping), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty CommandParameter2Property = DependencyProperty.RegisterAttached("CommandParameter2", typeof(object), typeof(EventsToCommandMapping), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

	private static void EventsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		bool isLoaded;
		if (!(d is FrameworkElement frameworkElement))
		{
			if (!(d is FrameworkContentElement frameworkContentElement))
			{
				throw new ArgumentOutOfRangeException("d", d, FormattableString.Invariant(FormattableStringFactory.Create("Must be either a {0} or a {1}.", "FrameworkElement", "FrameworkContentElement")));
			}
			HookupEventHandlers(frameworkContentElement);
			isLoaded = frameworkContentElement.IsLoaded;
		}
		else
		{
			HookupEventHandlers(frameworkElement);
			isLoaded = frameworkElement.IsLoaded;
		}
		if (isLoaded)
		{
			if (!EventProxyManagers.TryGetValue(d, out var value))
			{
				value = new EventToCommandProxyManager(d);
				EventProxyManagers.Add(d, value);
			}
			value.Update(e.NewValue as string);
		}
	}

	private static void HookupEventHandlers(FrameworkElement element)
	{
		element.Loaded -= ElementOnLoaded;
		element.Unloaded -= ElementOnUnloaded;
		element.Initialized -= ElementOnInitialized;
		element.Loaded += ElementOnLoaded;
		element.Unloaded += ElementOnUnloaded;
		element.Initialized += ElementOnInitialized;
	}

	private static void HookupEventHandlers(FrameworkContentElement element)
	{
		element.Loaded -= ElementOnLoaded;
		element.Unloaded -= ElementOnUnloaded;
		element.Initialized -= ElementOnInitialized;
		element.Loaded += ElementOnLoaded;
		element.Unloaded += ElementOnUnloaded;
		element.Initialized += ElementOnInitialized;
	}

	private static void ElementOnLoaded(object sender, RoutedEventArgs e)
	{
		DependencyObject dependencyObject = (DependencyObject)sender;
		if (!EventProxyManagers.ContainsKey(dependencyObject))
		{
			EventToCommandProxyManager eventToCommandProxyManager = new EventToCommandProxyManager(dependencyObject);
			EventProxyManagers.Add(dependencyObject, eventToCommandProxyManager);
			eventToCommandProxyManager.Update(GetEvents(dependencyObject));
			eventToCommandProxyManager.InvokeLoaded(e);
		}
	}

	private static void ElementOnUnloaded(object sender, RoutedEventArgs e)
	{
		DependencyObject key = (DependencyObject)sender;
		if (EventProxyManagers.TryGetValue(key, out var value))
		{
			value.InvokeUnloaded(e);
			value.Dispose();
			EventProxyManagers.Remove(key);
		}
	}

	private static void ElementOnInitialized(object sender, EventArgs e)
	{
		DependencyObject dependencyObject = (DependencyObject)sender;
		if (!EventProxyManagers.ContainsKey(dependencyObject))
		{
			EventToCommandProxyManager eventToCommandProxyManager = new EventToCommandProxyManager(dependencyObject);
			EventProxyManagers.Add(dependencyObject, eventToCommandProxyManager);
			eventToCommandProxyManager.Update(GetEvents(dependencyObject));
			eventToCommandProxyManager.InvokeInitialized(e);
		}
	}

	public static void SetEvents(DependencyObject element, string value)
	{
		element.SetValue(EventsProperty, value);
	}

	public static string GetEvents(DependencyObject element)
	{
		return (string)element.GetValue(EventsProperty);
	}

	public static void SetCommand(DependencyObject element, ICommand value)
	{
		element.SetValue(CommandProperty, value);
	}

	public static ICommand GetCommand(DependencyObject element)
	{
		return (ICommand)element.GetValue(CommandProperty);
	}

	public static void SetCommandParameter(DependencyObject element, object value)
	{
		element.SetValue(CommandParameterProperty, value);
	}

	public static object GetCommandParameter(DependencyObject element)
	{
		return element.GetValue(CommandParameterProperty);
	}

	public static void SetCommandParameter2(DependencyObject element, object value)
	{
		element.SetValue(CommandParameter2Property, value);
	}

	public static object GetCommandParameter2(DependencyObject element)
	{
		return element.GetValue(CommandParameter2Property);
	}
}
