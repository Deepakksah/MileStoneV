using System;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class ChangeIdpAdminTaskCommandHandler : ICommandHandler
{
	private IIdpAdminClient _idpAdministrationClient;

	public ChangeIdpAdminTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_idpAdministrationClient = serverEnvironment.IdpAdminClient;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ChangeIdpAdminTaskCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ResponseBase response = ProcessCommand(cmd, responseSink);
		responseSink.AddResponse(response);
	}

	private ResponseBase ProcessCommand(ChangeIdpAdminTaskCommand cmd, IResponseSink responseSink)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 0;
		bool flag = true;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		do
		{
			try
			{
				if (cmd.TaskType == TaskType.AddIdpAdmin)
				{
					try
					{
						_idpAdministrationClient.AddAdministrator(cmd.UserId);
					}
					catch (DuplicateAdminIdentityException)
					{
					}
				}
				else
				{
					if (cmd.TaskType != TaskType.DeleteIdpAdmin)
					{
						TaskError taskError = new TaskError();
						taskError.ErrorCode = 60254;
						taskError.ErrorText = VmoResources.VMO60254;
						return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
					}
					try
					{
						_idpAdministrationClient.DeleteAdministrator(cmd.UserId);
					}
					catch (NotFoundException)
					{
					}
				}
				flag = false;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				if (num < 60)
				{
					num++;
				}
				flag = true;
				Thread.Sleep(1000 * num);
			}
		}
		while (flag);
		return new TaskSuccessResponse();
	}
}
