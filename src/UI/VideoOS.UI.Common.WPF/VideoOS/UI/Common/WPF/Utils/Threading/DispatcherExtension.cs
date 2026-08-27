using System;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.UI.Common.WPF.Utils.Threading;

public static class DispatcherExtension
{
	public static Dispatcher Create(string threadName)
	{
		return Create(threadName, ApartmentState.Unknown);
	}

	public static Dispatcher Create(string threadName, ApartmentState apartmentState)
	{
		ManualResetEventSlim slimEvent = new ManualResetEventSlim();
		Dispatcher dispatcher = null;
		Thread thread = new Thread((ThreadStart)delegate
		{
			dispatcher = Dispatcher.CurrentDispatcher;
			slimEvent.Set();
			Dispatcher.Run();
		});
		if (apartmentState != ApartmentState.Unknown)
		{
			thread.SetApartmentState(apartmentState);
		}
		thread.IsBackground = true;
		thread.Name = threadName;
		thread.Start();
		slimEvent.Wait();
		dispatcher.UnhandledException += delegate(object s, DispatcherUnhandledExceptionEventArgs e)
		{
			LogDispatcherException(threadName, e.Exception);
		};
		return dispatcher;
	}

	private static void LogApplicationThreadExit(string threadName)
	{
		ILogger iLogger = Telemetry.GetILogger("ClientLogger");
		if (iLogger != null)
		{
			LoggerExtensions.LogDebug(iLogger, "DispatcherExtension application thread exit (" + threadName + "). (DTS 29185 - only report this if it happens outside normal application shutdown)", Array.Empty<object>());
		}
	}

	private static void LogDispatcherShutdown(string threadName)
	{
		ILogger iLogger = Telemetry.GetILogger("ClientLogger");
		if (iLogger != null)
		{
			LoggerExtensions.LogDebug(iLogger, "DispatcherExtension dispatcher shut down (" + threadName + "). (DTS 29185 - only report this if it happens outside normal application shutdown)", Array.Empty<object>());
		}
	}

	private static void LogDispatcherException(string threadName, Exception exception)
	{
		ILogger iLogger = Telemetry.GetILogger("ClientLogger");
		if (iLogger != null)
		{
			LoggerExtensions.LogDebug(iLogger, "DispatcherExtension unhandled exception (" + threadName + "). Exception start. (DTS 29185)", Array.Empty<object>());
			LoggerExtensions.LogDebug(iLogger, exception.ToString(), Array.Empty<object>());
			LoggerExtensions.LogDebug(iLogger, "DispatcherExtension unhandled exception. Exceptions end.", Array.Empty<object>());
		}
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, Action action, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return action.DynamicInvoke(args);
		}
		return dispatcher.Invoke(action, args);
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, Action action, DispatcherPriority priority, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return action.DynamicInvoke(args);
		}
		return dispatcher.Invoke(action, priority, args);
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, DispatcherPriority priority, Action action, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return action.DynamicInvoke(args);
		}
		return dispatcher.Invoke(action, priority, args);
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, Delegate method, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return method.DynamicInvoke(args);
		}
		return dispatcher.Invoke(method, args);
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, Delegate method, DispatcherPriority priority, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return method.DynamicInvoke(args);
		}
		return dispatcher.Invoke(method, priority, args);
	}

	public static object InvokeOrCall(this Dispatcher dispatcher, DispatcherPriority priority, Delegate method, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			return method.DynamicInvoke(args);
		}
		return dispatcher.Invoke(method, priority, args);
	}

	public static void BeginInvokeOrCall(this Dispatcher dispatcher, Action action, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			action.DynamicInvoke(args);
		}
		else
		{
			dispatcher.BeginInvoke(action, args);
		}
	}

	public static void BeginInvokeOrCall(this Dispatcher dispatcher, Action action, DispatcherPriority priority, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			action.DynamicInvoke(args);
		}
		else
		{
			dispatcher.BeginInvoke(action, priority, args);
		}
	}

	public static void BeginInvokeOrCall(this Dispatcher dispatcher, Delegate method, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			method.DynamicInvoke(args);
		}
		else
		{
			dispatcher.Invoke(method, args);
		}
	}

	public static void BeginInvokeOrCall(this Dispatcher dispatcher, Delegate method, DispatcherPriority priority, params object[] args)
	{
		if (dispatcher.Thread == Thread.CurrentThread)
		{
			method.DynamicInvoke(args);
		}
		else
		{
			dispatcher.Invoke(method, priority, args);
		}
	}

	public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action)
	{
		return dispatcher.BeginInvoke(action);
	}

	public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
	{
		return dispatcher.BeginInvoke(action, priority);
	}

	public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, DispatcherPriority priority, Action action)
	{
		return dispatcher.BeginInvoke(action, priority);
	}

	public static DispatcherOperation BeginInvoke<T>(this Dispatcher dispatcher, Action<T> action, T arg)
	{
		return dispatcher.BeginInvoke(action, arg);
	}

	public static DispatcherOperation BeginInvoke<T>(this Dispatcher dispatcher, DispatcherPriority priority, Action<T> action, T arg)
	{
		return dispatcher.BeginInvoke(action, priority, arg);
	}

	public static DispatcherOperation BeginInvoke<T>(this Dispatcher dispatcher, Action<T> action, DispatcherPriority priority, T arg)
	{
		return dispatcher.BeginInvoke(action, priority, arg);
	}

	public static void Invoke(this Dispatcher dispatcher, Action action)
	{
		dispatcher.Invoke(action);
	}

	public static void Invoke(this Dispatcher dispatcher, DispatcherPriority priority, Action action)
	{
		dispatcher.Invoke(action, priority);
	}

	public static void Invoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
	{
		dispatcher.Invoke(action, priority);
	}
}
