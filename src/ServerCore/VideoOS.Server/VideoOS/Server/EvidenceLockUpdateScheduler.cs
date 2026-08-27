using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.CommandHandler;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class EvidenceLockUpdateScheduler : IDisposable
{
	public static class TestAccessor
	{
		public static void FullCheckTimerCallback(EvidenceLockUpdateScheduler instance)
		{
			instance.FullCheckTimerCallback(null);
		}

		public static void UnknownSizeCheckTimerCallback(EvidenceLockUpdateScheduler instance)
		{
			instance.UnknownSizeCheckTimerCallback(null);
		}
	}

	private bool started;

	private Timer fullCheckTimer;

	private Timer unknownSizeCheckTimer;

	private HashSet<Guid> activeUpdates = new HashSet<Guid>();

	private IServerEnvironment serverEnvironment;

	private static Guid FullCheckEvidenceLockId => Guid.Empty;

	private ILogger Logger => serverEnvironment.Server.Logger;

	private IServer Server => serverEnvironment.Server;

	private IDataComponentFactory DataComponentFactory => serverEnvironment.DataComponentFactory;

	private ILocalCommandProcessor LocalCommandProcessor => serverEnvironment.LocalCommandProcessor;

	public bool IsStarted => started;

	public TimeSpan TimeBetweenAllSizesUpdates { get; set; } = TimeSpan.FromHours(2.0);

	public TimeSpan TimeBetweenUknownSizeUpdates { get; set; } = TimeSpan.FromMinutes(3.0);

	public EvidenceLockUpdateScheduler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		this.serverEnvironment = serverEnvironment;
		fullCheckTimer = new Timer(FullCheckTimerCallback, null, -1, -1);
		unknownSizeCheckTimer = new Timer(UnknownSizeCheckTimerCallback, null, -1, -1);
	}

	public void Start()
	{
		if (started)
		{
			throw new InvalidOperationException("The scheduler is already started.");
		}
		fullCheckTimer.Change(TimeBetweenAllSizesUpdates, TimeBetweenAllSizesUpdates);
		unknownSizeCheckTimer.Change(TimeBetweenUknownSizeUpdates, TimeBetweenUknownSizeUpdates);
		started = true;
	}

	public void Stop()
	{
		if (!started)
		{
			throw new InvalidOperationException("The scheduler is not started.");
		}
		fullCheckTimer.Change(-1, -1);
		unknownSizeCheckTimer.Change(-1, -1);
		started = false;
	}

	public void CheckSpecificEvidenceLock(Guid evidenceLockId)
	{
		lock (activeUpdates)
		{
			if (!Server.IsClosing && started && !activeUpdates.Contains(evidenceLockId))
			{
				activeUpdates.Add(evidenceLockId);
				LocalCommandProcessor.ProcessCommandAsync(UpdateEvidenceLockSizesCommand.CreateSingleEvidenceLockSizesCommand(evidenceLockId), Server.CreateServerServiceCallContextUserInfo(), HandleResponse);
			}
		}
	}

	private void FullCheckTimerCallback(object state)
	{
		lock (activeUpdates)
		{
			if (!Server.IsClosing && started && !activeUpdates.Contains(FullCheckEvidenceLockId))
			{
				Logger.LogDebug("Start updating size for all evidence locks.");
				activeUpdates.Add(FullCheckEvidenceLockId);
				LocalCommandProcessor.ProcessCommandAsync(UpdateEvidenceLockSizesCommand.CreateAllEvidenceLockSizesCommand(), Server.CreateServerServiceCallContextUserInfo(), HandleResponse);
			}
		}
	}

	private void UnknownSizeCheckTimerCallback(object state)
	{
		if (Server.IsClosing || !started)
		{
			return;
		}
		Logger.LogDebug("Check for evidence locks having unknown size.");
		IEnumerable<Guid> first;
		using (IEvidenceLockComponent evidenceLockComponent = DataComponentFactory.CreateEvidenceLockComponent())
		{
			try
			{
				first = evidenceLockComponent.QueryEvidenceLockIdsWithoutSize();
			}
			catch (DatabaseException ex)
			{
				Logger.LogError("Error when reading evidence locks without size from the database.\r\nException: {Exception}", ex);
				return;
			}
		}
		lock (activeUpdates)
		{
			IEnumerable<Guid> enumerable = first.Except(activeUpdates);
			foreach (Guid item in enumerable)
			{
				if (Server.IsClosing || !started)
				{
					break;
				}
				Logger.LogDebug("Start updating size for evidence lock. Id: {EvidenceLockId}", item);
				CheckSpecificEvidenceLock(item);
			}
		}
	}

	private void HandleResponse(UpdateEvidenceLockSizesCommand command, ResponseBase response)
	{
		lock (activeUpdates)
		{
			activeUpdates.Remove(command.EvidenceLockId);
		}
	}

	public void Dispose()
	{
		fullCheckTimer.Dispose();
		unknownSizeCheckTimer.Dispose();
	}
}
