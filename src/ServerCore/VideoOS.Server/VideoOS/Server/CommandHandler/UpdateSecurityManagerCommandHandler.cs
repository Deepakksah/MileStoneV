using System;
using System.Data;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Security;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateSecurityManagerCommandHandler : ICommandHandler
{
	private Server _server;

	public UpdateSecurityManagerCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateSecurityManagerCommand updateSecurityManagerCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo state2))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		if (updateSecurityManagerCommand.SecurityManagerDto.DataSet == null)
		{
			responseSink.AddResponse(new SuccessResponse());
			return;
		}
		try
		{
			SecurityManagerDataTransferObject securityManagerDto;
			if (updateSecurityManagerCommand.PersistChanges)
			{
				SecurityDataSet changes = updateSecurityManagerCommand.SecurityManagerDto.DataSet.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified);
				WriteChangesToDatabase(updateSecurityManagerCommand.SecurityManagerDto);
				securityManagerDto = new SecurityManagerDataTransferObject(changes);
			}
			else
			{
				securityManagerDto = updateSecurityManagerCommand.SecurityManagerDto;
			}
			AnalyzeUpdateSecurityManagerCommand command2 = new AnalyzeUpdateSecurityManagerCommand(securityManagerDto, updateSecurityManagerCommand.PersistChanges);
			LocalCommandProcessor.Instance.ProcessCommandAsync(command2, state2);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception exception)
		{
			responseSink.AddResponse(new ExceptionResponse(exception));
			return;
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	private void WriteChangesToDatabase(SecurityManagerDataTransferObject securityManagerDto)
	{
		try
		{
			_server.UpdateSecurityConfiguration(securityManagerDto);
		}
		catch (DBConcurrencyException)
		{
			throw new DatabaseConcurrencyException(Resources.IServerSqlConcurrencyError);
		}
		catch (DatabaseException ex2)
		{
			switch (ex2.Number)
			{
			case 547:
				throw new DatabaseConcurrencyException(Resources.IServerSqlConcurrencyError, ex2);
			case 2601:
			case 2627:
				throw new ServerServiceException(Resources.IServerSqlUniqueIndexOrConstraintViolation, ex2);
			default:
				throw new ServerServiceException(Resources.IServerSqlError, ex2);
			}
		}
		catch (ServerServiceException)
		{
			throw;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new ServerServiceException(Resources.IServerInternalError, innerException);
		}
	}
}
