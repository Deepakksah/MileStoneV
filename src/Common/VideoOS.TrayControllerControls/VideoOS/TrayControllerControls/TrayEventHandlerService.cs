using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace VideoOS.TrayControllerControls;

public class TrayEventHandlerService
{
	private EventWaitHandle _waitHandle;

	private string _eventHandlerName;

	private IQuit _quitter;

	public TrayEventHandlerService(string eventHandlerName)
	{
		_eventHandlerName = eventHandlerName;
		_waitHandle = GetOrCreateEventWaitHandler(_eventHandlerName, create: false);
	}

	public bool TrySignalEvent()
	{
		if (_waitHandle != null)
		{
			_waitHandle.Set();
			return true;
		}
		return false;
	}

	public bool IsAlreadyRunning(Assembly mainAssembly)
	{
		if (_waitHandle != null && ProcessHelper.IsProcessRunningByCurrentUser(mainAssembly))
		{
			return true;
		}
		return false;
	}

	public void EnsureCreateEventCreated()
	{
		if (_waitHandle == null)
		{
			_waitHandle = GetOrCreateEventWaitHandler(_eventHandlerName, create: true);
		}
	}

	public void StartSignalMonitoring(IQuit quitter)
	{
		_quitter = quitter;
		Thread thread = new Thread(WaitForExit);
		thread.IsBackground = true;
		thread.Start(this);
	}

	private static void WaitForExit(object data)
	{
		TrayEventHandlerService obj = data as TrayEventHandlerService;
		obj._waitHandle.WaitOne();
		obj._quitter.QuitSignalled();
	}

	public static EventWaitHandle GetOrCreateEventWaitHandler(string eventHandlerName, bool create)
	{
		EventWaitHandle result = null;
		if (!create)
		{
			try
			{
				EventWaitHandle.TryOpenExisting(eventHandlerName, EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, out result);
			}
			catch
			{
			}
			return result;
		}
		bool createdNew = false;
		EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
		EventWaitHandleAccessRule rule = new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, AccessControlType.Allow);
		eventWaitHandleSecurity.AddAccessRule(rule);
		return new EventWaitHandle(initialState: false, EventResetMode.ManualReset, eventHandlerName, out createdNew, eventWaitHandleSecurity);
	}
}
