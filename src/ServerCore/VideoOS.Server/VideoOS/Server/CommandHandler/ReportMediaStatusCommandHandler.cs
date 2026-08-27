using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class ReportMediaStatusCommandHandler : ICommandHandler
{
	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ReportMediaStatusCommand reportMediaStatusCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		MediaStatusEntry2[] mediaStatus = reportMediaStatusCommand.MediaStatus;
		foreach (MediaStatusEntry2 mediaStatusEntry in mediaStatus)
		{
			try
			{
				ConfigurationDataManager.DeleteBookmarksBeforeTime(mediaStatusEntry.DeviceId, mediaStatusEntry.MediaRetentionTime);
			}
			catch (DatabaseException ex)
			{
				Server.Instance.Logger.LogWarning("Failed to delete bookmarks before time. Device: {DeviceId}, Error: {ExceptionMessage}", mediaStatusEntry.DeviceId, ex.Message);
			}
			try
			{
				ConfigurationDataManager.DeleteRestrictedMediaBeforeTime(mediaStatusEntry.DeviceId, mediaStatusEntry.MediaRetentionTime);
			}
			catch (DatabaseException ex2)
			{
				Server.Instance.Logger.LogWarning("Failed to delete restricted media before time. Device: {DeviceId}, Error: {ExceptionMessage}", mediaStatusEntry.DeviceId, ex2.Message);
			}
			try
			{
				ConfigurationDataManager.DeleteCompletedTasksBeforeTime(mediaStatusEntry.DeviceId, mediaStatusEntry.MediaRetentionTime);
			}
			catch (DatabaseException ex3)
			{
				Server.Instance.Logger.LogWarning("Failed to delete completed tasks before time. Device: {DeviceId}, Error: {ExceptionMessage}", mediaStatusEntry.DeviceId, ex3.Message);
			}
			try
			{
				ConfigurationDataManager.DeletePrivacyMasksBeforeTime(mediaStatusEntry.DeviceId, mediaStatusEntry.MediaRetentionTime);
			}
			catch (DatabaseException ex4)
			{
				Server.Instance.Logger.LogWarning("Failed to delete historical masks before time. Device: {DeviceId}, Error: {ExceptionMessage}", mediaStatusEntry.DeviceId, ex4.Message);
			}
		}
		TrackStatusEntry[] trackStatus = reportMediaStatusCommand.TrackStatus;
		foreach (TrackStatusEntry trackStatusEntry in trackStatus)
		{
			try
			{
				ConfigurationDataManager.UpdateDeviceTrackOnlyUseMediaAfter(trackStatusEntry.TrackId, trackStatusEntry.MediaTime);
			}
			catch (DatabaseException ex5)
			{
				Server.Instance.Logger.LogWarning("Failed to update tracks OnlyUseMediaAfter time. Track: {TrackId}, Error: {ExceptionMessage}", trackStatusEntry.TrackId, ex5.Message);
			}
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
