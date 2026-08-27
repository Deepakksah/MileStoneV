using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class GeminiIdentityService : PeriodicTimerServiceBase, IGeminiIdentityService
{
	private int _counter;

	protected override TimeSpan InitialTimeInterval => TimeSpan.FromSeconds(5.0);

	protected override TimeSpan PeriodicTimeInterval => TimeSpan.FromMinutes(10.0);

	protected override void OnTimer(object state)
	{
		SynchronizeAllRoles();
	}

	public void SynchronizeWindowsGroup(Identity identity)
	{
		if (identity == null || identity.Type != IdentityType.WindowsGroup)
		{
			throw new ArgumentException("Identity needs to of type windows group");
		}
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			SynchronizeIdentity(identity);
		}, TaskCreationOptions.LongRunning);
	}

	private void SynchronizeAllRoles()
	{
		int num = Interlocked.Increment(ref _counter);
		try
		{
			GeminiTrace.Info(TraceType.Security, "Triggering scheduled Active Directory synchronization for all roles (count {0})", num);
			if (num == 1)
			{
				SynchronizeIdentity(null);
			}
			GeminiTrace.Info(TraceType.Security, "Finished scheduled Active Directory synchronization for all roles (count {0})", num);
		}
		finally
		{
			Interlocked.Decrement(ref _counter);
		}
	}

	private static void SynchronizeIdentity(object identity)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		SynchronizeIdentityWorker(identity, requestContext);
	}

	private static void SynchronizeIdentityWorker(object identity, IGeminiRequestContext requestContext)
	{
		Thread currentThread = Thread.CurrentThread;
		ThreadPriority priority = currentThread.Priority;
		currentThread.Priority = ThreadPriority.BelowNormal;
		bool flag = false;
		try
		{
			if (!(identity is Identity identity2))
			{
				flag |= ClaimToRoleMapper.PeriodicCheckForChanges(requestContext, clearCacheOnChanges: false);
				flag |= SynchronizationManager.FullSynchronizaionWithIdentityStore(requestContext);
			}
			else
			{
				flag = SynchronizationManager.SynchronizeWindowsGroupWithIdentityStore(requestContext, identity2);
			}
		}
		catch (DatabaseException ex)
		{
			GeminiTrace.TraceException(ex);
		}
		catch (GenericIdpException ex2)
		{
			GeminiTrace.TraceException(ex2);
		}
		catch (ThreadAbortException)
		{
			GeminiTrace.Info(TraceType.Security, "SynchronizeIdentityWorker: Thread is aborted");
		}
		catch (Exception ex4)
		{
			GeminiTrace.TraceException(ex4);
			throw;
		}
		finally
		{
			currentThread.Priority = priority;
		}
		if (flag)
		{
			IdentityCache.ClearMemoryCache();
			requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
		}
	}
}
