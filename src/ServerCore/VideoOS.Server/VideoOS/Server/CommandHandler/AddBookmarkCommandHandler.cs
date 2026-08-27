using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CommandHandler;

public sealed class AddBookmarkCommandHandler : ICommandHandler
{
	private readonly IVmoClientFactory _clientFactory;

	public AddBookmarkCommandHandler(IVmoClientFactory clientFactory)
	{
		_clientFactory = clientFactory;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is AddBookmarkCommand addBookmarkCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			VmoClientsPool.VmoTimedClient connectedClient = _clientFactory.GetConnectedClient();
			try
			{
				Device[] objectsByIds = connectedClient.Repositories.GetObjectsByIds<Device>(addBookmarkCommand.DeviceIds, refreshCachedObjects: false);
				Device[] array = objectsByIds;
				foreach (Device device in array)
				{
					try
					{
						MarkedDataReference markedDataReference = connectedClient.ManagementServer.CreateMarkedDataReference(new Device[1] { device }, triggerMarkedDataReferenceRequestedEvent: false, MarkedDataDefinitions.MarkedDataTypes.Bookmark);
						MarkedData markedData = new MarkedData(Guid.NewGuid().ToString(), markedDataReference);
						markedData.MarkedDataType = MarkedDataDefinitions.MarkedDataTypes.Bookmark;
						markedData.Header = addBookmarkCommand.Header;
						markedData.Description = addBookmarkCommand.Description;
						markedData.TagTime = addBookmarkCommand.TimeTrigged;
						markedData.StartTime = addBookmarkCommand.TimeTrigged - addBookmarkCommand.TimeBefore;
						markedData.EndTime = addBookmarkCommand.TimeTrigged + addBookmarkCommand.TimeAfter;
						markedData.Create();
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						Server.Instance.Logger.LogWarning("Unable to set bookmark for device {DeviceId}, Error: {ExceptionMessage}", device.Id, ex2.Message);
					}
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				Server.Instance.Logger.LogWarning("Failed to execute Bookmark command (inner). Error: {ExceptionMessage}", ex4.Message);
			}
			finally
			{
				_clientFactory.ReleaseClient(connectedClient);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex6)
		{
			Server.Instance.Logger.LogWarning("Failed to execute Bookmark command (outer). Error: {ExceptionMessage}", ex6.Message);
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
