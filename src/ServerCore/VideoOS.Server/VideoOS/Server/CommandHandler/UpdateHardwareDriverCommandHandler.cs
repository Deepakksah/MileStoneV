using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.Configuration;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateHardwareDriverCommandHandler : ICommandHandler
{
	[Serializable]
	public class DatabaseUpdateException : Exception
	{
		public DatabaseUpdateException()
		{
		}

		public DatabaseUpdateException(string message)
			: base(message)
		{
		}

		public DatabaseUpdateException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DatabaseUpdateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	private IServerEnvironment _serverEnvironment;

	public IServer Server => _serverEnvironment.Server;

	public ILogger Logger => _serverEnvironment.Server.Logger;

	public IConfigurationDataSetComponent ConfigurationDataSetComponent => _serverEnvironment.ConfigurationDataSetComponent;

	public IConfigurationDataLoader ConfigurationDataLoader => _serverEnvironment.ConfigurationDataLoader;

	public IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	public ILicenseManager LicenseManager => _serverEnvironment.LicenseManager;

	public UpdateHardwareDriverCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateHardwareDriverCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		ResponseBase response = ProcessCommand(cmd, callContextInfo);
		responseSink.AddResponse(response);
	}

	private ResponseBase ProcessCommand(UpdateHardwareDriverCommand cmd, ICallContextInfo callContextInfo)
	{
		Logger.LogInformation("UpdateHardwareDriverCommand: Start processing command. Token: {Token}", cmd.Token);
		if (!ValidateCommandContent(cmd, out var errorResponse))
		{
			return errorResponse;
		}
		ConfigurationDataSet dataSet = cmd.SystemConfigurationDto.DataSet;
		ConfigurationDataSet.HardwareRow hardwareRow = dataSet.Hardware.Rows[0] as ConfigurationDataSet.HardwareRow;
		ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow = dataSet.HardwareDefinition.Rows[0] as ConfigurationDataSet.HardwareDefinitionRow;
		Logger.LogDebug("UpdateHardwareDriverCommand: Processing hardware. Hardware id: {HardwareId}", hardwareRow.IDHardware);
		EnsureTypeAttributeInSettingXml(hardwareRow, dataSet.Devices, hardwareDefinitionRow.Definition);
		string definition = hardwareDefinitionRow.Definition;
		try
		{
			using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_serverEnvironment.ConfigurationDataLoader));
			EventTypes eventTypes = configurationManager.EventTypes;
			hardwareDefinitionRow.Definition = DynamicEventUtility.CheckDynamicEventIds(_serverEnvironment, eventTypes, definition);
		}
		catch (Exception ex) when (ex is DatabaseException || ex is NullReferenceException)
		{
			string text = dataSet.ToDiffGramXml();
			Logger.LogError("UpdateHardwareDriverCommand: Failed to update dynamicEvents. Token: {Token}; Exception:\r\n{Exception}\r\nDiffGram:\r\n{DiffGram}", cmd.Token, ex, text);
			return new ErrorResponse(4, "Failed to update dynamicEvents.");
		}
		try
		{
			ConfigurationDataSet changes = dataSet.Copy() as ConfigurationDataSet;
			WriteConfigurationDataSetChangesToDatabaseForced(changes, callContextInfo);
			if (CheckIsMacAddressChanged(hardwareRow))
			{
				Guid recorderId = new Guid(hardwareRow.IDRecorder);
				UpdateRecorderLicense(recorderId, callContextInfo);
			}
		}
		catch (DatabaseUpdateException ex2)
		{
			string text2 = dataSet.ToDiffGramXml();
			Logger.LogError("UpdateHardwareDriverCommand: Failed to process command. Token: {Token}; Exception:\r\n{Exception}\r\nDiffGram:\r\n{DiffGram}", cmd.Token, ex2, text2);
			return new ErrorResponse(4, "Failed to update the database.");
		}
		SendUpdateSystemConfigurationCommandsToHotStandbyRecordersAsync(dataSet, callContextInfo);
		Logger.LogInformation("UpdateHardwareDriverCommand: Processed successfully. Token: {Token}", cmd.Token);
		return new SuccessResponse();
	}

	private void WriteConfigurationDataSetChangesToDatabase(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			ConfigurationDataSetComponent.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
		}
		catch (DatabaseException inner)
		{
			throw new DatabaseUpdateException("Writing to the database failed.", inner);
		}
		catch (DBConcurrencyException inner2)
		{
			throw new DatabaseUpdateException("Writing to the database failed (concurrency error).", inner2);
		}
		finally
		{
			ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private void WriteConfigurationDataSetChangesToDatabaseForced(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			ConfigurationDataSetComponent.WriteConfigurationDataSetChangesToDatabaseForced(changes, callContextInfo);
		}
		catch (DatabaseException inner)
		{
			throw new DatabaseUpdateException("Writing to the database failed.", inner);
		}
		catch (DBConcurrencyException inner2)
		{
			throw new DatabaseUpdateException("Writing to the database failed (concurrency error).", inner2);
		}
		catch (NotSupportedException)
		{
			throw;
		}
		finally
		{
			ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private bool ValidateCommandContent(UpdateHardwareDriverCommand cmd, out ErrorResponse errorResponse)
	{
		ConfigurationDataSet dataSet = cmd.SystemConfigurationDto.DataSet;
		if (dataSet.Hardware.Rows.Count != 1)
		{
			Logger.LogError("UpdateHardwareDriverCommand: The contained data set must contain exactly one hardware row. Token: {Token}; Number of rows: {NumberOfRows}", cmd.Token, dataSet.Hardware.Rows.Count);
			errorResponse = new ErrorResponse(0, "Wrong number of hardware rows in data set. Exactly one row was expected.");
			return false;
		}
		if (dataSet.HardwareDefinition.Rows.Count != 1)
		{
			Logger.LogError("UpdateHardwareDriverCommand: The contained data set must contain exactly one hardware definition row. Token: {Token}; Number of rows: {NumberOfRows}", cmd.Token, dataSet.HardwareDefinition.Rows.Count);
			errorResponse = new ErrorResponse(1, "Wrong number of hardware definition rows in data set. Exactly one row was expected.");
			return false;
		}
		ConfigurationDataSet.HardwareRow hardwareRow = dataSet.Hardware.Rows[0] as ConfigurationDataSet.HardwareRow;
		ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow = dataSet.HardwareDefinition.Rows[0] as ConfigurationDataSet.HardwareDefinitionRow;
		if (!string.Equals(hardwareRow.C18NDigest, hardwareDefinitionRow.C18NDigest, StringComparison.Ordinal))
		{
			Logger.LogError("UpdateHardwareDriverCommand: Definition for hardware not included in data set. Token: {Token}", cmd.Token);
			errorResponse = new ErrorResponse(2, "Definition for hardware not included in data set.");
			return false;
		}
		foreach (ConfigurationDataSet.DevicesRow row in dataSet.Devices.Rows)
		{
			if (!string.Equals(row.IDHardware, hardwareRow.IDHardware, StringComparison.OrdinalIgnoreCase))
			{
				Logger.LogError("UpdateHardwareDriverCommand: Device row that doesn't relate to the hardware was included in the data set. Token: {Token}, Expected hardware id: {HardwareIdExpected}; Actual hardware id: {HardwareIdActual}", cmd.Token, hardwareRow.IDHardware, row.IDHardware);
				errorResponse = new ErrorResponse(3, "Device row that doesn't relate to the hardware was included in the data set.");
				return false;
			}
		}
		errorResponse = null;
		return true;
	}

	private static bool CheckIsMacAddressChanged(ConfigurationDataSet.HardwareRow hardwareRow)
	{
		HardwareSettingUtilities hardwareSettingUtilities = new HardwareSettingUtilities(hardwareRow.Settings);
		HardwareSettingUtilities hardwareSettingUtilities2 = new HardwareSettingUtilities(hardwareRow.GetSettings(DataRowVersion.Original));
		return !string.Equals(hardwareSettingUtilities.MacAddress, hardwareSettingUtilities2.MacAddress, StringComparison.Ordinal);
	}

	private byte[] RequestRecorderLicense(Guid recorderId)
	{
		lock (LicenseManager)
		{
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			requiredService.ClearAllCaches();
			return LicenseManager.GetRecorderLicense(recorderId);
		}
	}

	private void UpdateRecorderLicense(Guid recorderId, ICallContextInfo callContextInfo)
	{
		byte[] array = RequestRecorderLicense(recorderId);
		ConfigurationDataSet dataSet;
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(ConfigurationDataLoader)))
		{
			License license = null;
			try
			{
				license = configurationManager.Licenses[recorderId];
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager.Licenses.Add(recorderId, array);
			}
			else
			{
				license.DataAsBytes = array;
				license.LastModified = DateTime.UtcNow;
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			dataSet = changes.Copy() as ConfigurationDataSet;
			ICallContextInfo callContextInfo2 = Server.CreateServerServiceCallContextUserInfo();
			WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo2);
		}
		Guid[] array2 = RecorderCommManager.QueryRecorderServiceIds(recorderId);
		Guid[] array3 = array2;
		foreach (Guid serviceId in array3)
		{
			try
			{
				RecorderCommManager.UpdateSystemConfigurationAsync(serviceId, new SystemConfigurationDto(dataSet), callContextInfo);
			}
			catch (RecorderNotFoundException)
			{
			}
			catch (NotSupportedByLegacyRecorderException)
			{
			}
		}
	}

	private static void EnsureTypeAttributeInSettingXml(ConfigurationDataSet.HardwareRow hardwareRow, IEnumerable<ConfigurationDataSet.DevicesRow> deviceRows, string hardwareDefinitionXml)
	{
		if (hardwareRow.RowState == DataRowState.Modified)
		{
			hardwareRow.Settings = DefinitionUtility.InsertTypeAttributeInHardwareSettingXml(hardwareRow.Settings, hardwareDefinitionXml, refresh: true);
		}
		foreach (ConfigurationDataSet.DevicesRow deviceRow in deviceRows)
		{
			if (deviceRow.RowState == DataRowState.Modified || deviceRow.RowState == DataRowState.Added)
			{
				deviceRow.Settings = DefinitionUtility.InsertTypeAttributeInHardwareSettingXml(deviceRow.Settings, hardwareDefinitionXml, refresh: true);
			}
		}
	}

	private void SendUpdateSystemConfigurationCommandsToHotStandbyRecordersAsync(ConfigurationDataSet diffGram, ICallContextInfo callContextInfo)
	{
		ConfigurationDataSet.HardwareRow hardwareRow = diffGram.Hardware.Rows[0] as ConfigurationDataSet.HardwareRow;
		Guid recorderId = new Guid(hardwareRow.IDRecorder);
		Guid[] array = RecorderCommManager.QueryHotstandbyServiceIds(recorderId);
		Guid[] array2 = array;
		foreach (Guid guid in array2)
		{
			Logger.LogDebug("UpdateHardwareDriverCommand: Sending update hardware info to hot standby server. Hardware: {HardwareId}; Recorder service id: {ServiceId}", hardwareRow.IDHardware, guid);
			try
			{
				RecorderCommManager.UpdateSystemConfigurationAsync(guid, new SystemConfigurationDto(diffGram), callContextInfo);
			}
			catch (NotSupportedByLegacyRecorderException)
			{
			}
			catch (RecorderNotFoundException)
			{
			}
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
