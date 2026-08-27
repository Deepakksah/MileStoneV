using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Interfaces.Scheduler;
using VideoOS.Vms.Utilities.Scheduler;

namespace VideoOS.Server.CommandHandler;

public sealed class ScheduledCommand : ISchedulable
{
	private CommandBase command;

	private Scheduler scheduler;

	public bool Repeat { get; private set; }

	public uint Interval { get; private set; }

	public IResponseSink ResponseSink { get; set; }

	public bool ExecuteAsync { get; set; }

	public ScheduledCommand(CommandBase command)
	{
		this.command = command;
		scheduler = null;
		Repeat = false;
		Interval = 0u;
		ResponseSink = new NullResponseSink();
		ExecuteAsync = false;
	}

	public ScheduledCommand(CommandBase command, Scheduler scheduler, uint interval)
	{
		this.command = command;
		this.scheduler = scheduler;
		Repeat = true;
		Interval = interval;
		ResponseSink = new NullResponseSink();
		ExecuteAsync = false;
	}

	public ScheduledCommand(CommandBase command, Scheduler scheduler, TimeSpan interval)
		: this(command, scheduler, (uint)interval.TotalMilliseconds)
	{
	}

	public void ProcessTask(Guid taskId)
	{
		if (Repeat)
		{
			scheduler.ScheduleTask(this, Interval);
		}
		if (ExecuteAsync)
		{
			CommandProcessor.ProcessAsync(command, Server.Instance.CreateServerServiceCallContextUserInfo(), ResponseSink);
		}
		else
		{
			CommandProcessor.ProcessSync(command, Server.Instance.CreateServerServiceCallContextUserInfo(), ResponseSink);
		}
	}
}
