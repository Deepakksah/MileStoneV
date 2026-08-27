using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class DeleteIdpUserTaskCommandHandler : ICommandHandler
{
	private IIdpAdminClient _idpAdministrationClient;

	private IServerEnvironment _serverEnvironment;

	private IServer Server => _serverEnvironment.Server;

	public DeleteIdpUserTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
		_idpAdministrationClient = serverEnvironment.IdpAdminClient;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is DeleteIdpUserTaskCommand cmd))
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

	private ResponseBase ProcessCommand(DeleteIdpUserTaskCommand cmd, IResponseSink responseSink)
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
				if (cmd.TaskType == TaskType.DeleteIdpUser)
				{
					_idpAdministrationClient.DeleteUser(cmd.UserId);
					flag = false;
					continue;
				}
				TaskError taskError = new TaskError();
				taskError.ErrorCode = 60254;
				taskError.ErrorText = VmoResources.VMO60254;
				return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (UserNotFoundException)
			{
				Server.Logger.LogInformation("DeleteIdpUserTaskCommandHandler, User not found in IDP. UserId: {UserId}", cmd.UserId);
				break;
			}
			catch (Exception ex3)
			{
				if (num == 0)
				{
					Server.Logger.LogWarning("DeleteIdpUserTaskCommandHandler, Failed to delete user in IDP. Retrying... UserId: {UserId}; Error: {Exception}", cmd.UserId, ex3);
				}
				if (num < 60)
				{
					num++;
				}
				flag = true;
				Thread.Sleep(1000 * num);
			}
		}
		while (flag);
		Server.Logger.LogInformation("DeleteIdpUserTaskCommandHandler, User deleted in IDP. UserId: {UserId}", cmd.UserId);
		return new TaskSuccessResponse();
	}
}
