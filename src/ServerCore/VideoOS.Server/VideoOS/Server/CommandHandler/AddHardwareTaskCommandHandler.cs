using System;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.HardwareSetup;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.DataComponents;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public class AddHardwareTaskCommandHandler : ICommandHandler
{
	private static class ICPHardwareNotSupportedErrorCodes
	{
		public const int UnknownServerType = 1;

		public const int UnknownProductVersion = 2;

		public const int VersionNotSupported = 3;

		public const int ServerNotSupported = 4;
	}

	private static class ICPErrorCodes
	{
		public const int LogOnErrorNoAdminAccess = 100;

		public const int LogOnErrorNoAccess = 101;

		public const int NoRecordersError = 102;
	}

	private IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	private IConfigurationDataLoader DataLoader => serverEnvironment.ConfigurationDataLoader;

	private IRecorderCommManager RecorderCommManager => serverEnvironment.RecorderCommManager;

	private ILocalCommandProcessor LocalCommandProcessor => serverEnvironment.LocalCommandProcessor;

	public AddHardwareTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		this.serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is AddHardwareTaskCommand { CallContextInfo: var callContextInfo } addHardwareTaskCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (callContextInfo == null)
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ProcessCommand(addHardwareTaskCommand, responseSink, callContextInfo);
	}

	private void ProcessCommand(AddHardwareTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
		Guid guid = QueryDefaultStorageId(cmd.RecordingServerId);
		if (guid == Guid.Empty)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60274;
			taskError.ErrorText = VmoResources.VMO60274;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
			return;
		}
		num += 5;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
		HardwareSetupInformation hardwareSetupInformation;
		if (!string.IsNullOrEmpty(cmd.DefinitionXml))
		{
			try
			{
				hardwareSetupInformation = BuildHardwareSetupInfo(cmd);
			}
			catch (TaskErrorException ex)
			{
				TaskError taskError2 = new TaskError();
				taskError2.ErrorCode = ex.ErrorCode;
				taskError2.ErrorText = ex.Message;
				responseSink.AddResponse(new TaskFailedResponse(ex.IsFatal, taskError2.ErrorText, taskError2));
				return;
			}
			try
			{
				CheckRecorderOffline(cmd.RecordingServerId);
			}
			catch (TaskErrorException ex2)
			{
				TaskError taskError3 = new TaskError();
				taskError3.ErrorCode = ex2.ErrorCode;
				taskError3.ErrorText = ex2.Message;
				responseSink.AddResponse(new TaskFailedResponse(ex2.IsFatal, taskError3.ErrorText, taskError3));
				return;
			}
			try
			{
				CheckIfHardwareWithSameAddressExists(cmd.RecordingServerId, cmd.Address);
			}
			catch (TaskErrorException ex3)
			{
				TaskError taskError4 = new TaskError();
				taskError4.ErrorCode = ex3.ErrorCode;
				taskError4.ErrorText = ex3.Message;
				responseSink.AddResponse(new TaskFailedResponse(ex3.IsFatal, taskError4.ErrorText, taskError4));
				return;
			}
		}
		else
		{
			hardwareSetupInformation = AcquireHardwareSetupInfo(cmd, responseSink, callContextInfo, utcNow, ref num, 90);
			if (hardwareSetupInformation == null)
			{
				return;
			}
		}
		num = 90;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
		HardwareSetupSettings hardwareSetupSettings = HardwareSetupSettings.CreateFromHardwareSetupInfo(hardwareSetupInformation, cmd.Address, cmd.UserName, cmd.Password, cmd.CustomData, null);
		hardwareSetupSettings.Enabled = false;
		hardwareSetupSettings.Name = NamingScheme.FindHardwareName(HardwareNameTemplate.Default, hardwareSetupSettings);
		foreach (DeviceSetupSettings device in hardwareSetupSettings.Devices)
		{
			device.Enabled = false;
			device.Name = NamingScheme.FindDeviceName(DeviceNameTemplate.Default, hardwareSetupSettings, device);
		}
		Guid iD;
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(DataLoader)))
		{
			Recorder recorder = configurationManager.Recorders[cmd.RecordingServerId];
			responseSink.AddResponse(new TaskProgressResponse(utcNow, num++));
			HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode = (serverEnvironment.LicenseManager.IsFeatureLicensed("HardwareAcceleratedVMD") ? HardwareAccelerationMode.HardwareAccelerationModeType.Automatic : HardwareAccelerationMode.HardwareAccelerationModeType.Off);
			IPHardware iPHardware = recorder.AddNewHardware(hardwareSetupSettings, guid, hardwareAccelerationMode);
			iD = iPHardware.ID;
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			ConfigurationManagerDataTransferObject configurationManagerDto = new ConfigurationManagerDataTransferObject(changes);
			UpdateConfigurationManagerCommand command = new UpdateConfigurationManagerCommand(configurationManagerDto)
			{
				PersistChanges = true
			};
			IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
			ResponseBase responseBase = null;
			try
			{
				responseBase = responseCollector.GetNextResponse(TimeSpan.FromMinutes(5.0));
			}
			catch (TimeoutException)
			{
				responseBase = null;
			}
			bool flag = false;
			if (responseBase != null && responseBase is SuccessResponse)
			{
				flag = true;
			}
			if (!flag)
			{
				Logger.LogWarning("Failed to update configuration with added hardware. Response: {Response}", responseBase);
				TaskError taskError5 = new TaskError();
				taskError5.ErrorCode = 60293;
				taskError5.ErrorText = VmoResources.VMO60293;
				responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5));
				return;
			}
		}
		AddHardwareTaskResult addHardwareTaskResult = new AddHardwareTaskResult();
		addHardwareTaskResult.HardwareId = iD;
		responseSink.AddResponse(new TaskSuccessResponse(addHardwareTaskResult.ToXml()));
	}

	private void CheckIfHardwareWithSameAddressExists(Guid recorderId, Uri addressToCheck)
	{
		string[] array;
		try
		{
			array = ConfigurationDataManager.QueryHardwareAddressesByRecorder(recorderId);
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("AddHardwareTaskCommandHandler: Failed to look up existing hardware addresses for recorder. Recorder id: {RecorderId}; Exception: \n{Exception}", recorderId, ex);
			throw new TaskErrorException(VmoResources.VMO60294, 60294, isFatal: true, ex);
		}
		string[] array2 = array;
		foreach (string uriString in array2)
		{
			if (Uri.TryCreate(uriString, UriKind.Absolute, out var result) && (string.Equals(result.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) || string.Equals(result.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)) && string.Equals(addressToCheck.Host, result.Host, StringComparison.OrdinalIgnoreCase) && addressToCheck.Port == result.Port)
			{
				throw new TaskErrorException(VmoResources.VMO60276, 60276, isFatal: true);
			}
		}
	}

	private void CheckRecorderOffline(Guid recorderId)
	{
		if (RecorderCommManager.IsRecorderAvailable(recorderId))
		{
			throw new TaskErrorException(VmoResources.VMO60301, 60301, isFatal: true);
		}
	}

	private Guid QueryDefaultStorageId(Guid recorderId)
	{
		Guid result = Guid.Empty;
		try
		{
			result = ConfigurationDataManager.FindDefaultStorageIdByRecorder(recorderId);
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("AddHardwareTaskCommandHandler: Failed to look up default storage id for recorder. Recorder id: {RecorderId}; Exception: \n{Exception}", recorderId, ex);
		}
		return result;
	}

	private static HardwareSetupInformation BuildHardwareSetupInfo(AddHardwareTaskCommand cmd)
	{
		MainDefinition mainDefinition;
		try
		{
			mainDefinition = MainDefinition.LoadXml(cmd.DefinitionXml);
		}
		catch (XmlException inner)
		{
			throw new TaskErrorException(VmoResources.VMO60295, 60295, isFatal: true, inner);
		}
		Guid id = mainDefinition.Driver.Id;
		int number = mainDefinition.Driver.Number;
		string productId = mainDefinition.HardwareDefinition.ProductId;
		string definitionXml = DefinitionUtility.RemoveHardwareSpecificInfoFromHardwareDefinitionXml(cmd.DefinitionXml);
		DriverSettings driverSettings = new DriverSettings();
		DefinitionUtility.InitializeSettings(mainDefinition, mainDefinition.HardwareDefinition.Capabilities, driverSettings);
		if (mainDefinition.HardwareDefinition.PtzSettings != null)
		{
			DefinitionUtility.InitializeSettings(mainDefinition, mainDefinition.HardwareDefinition.PtzSettings.Capabilities, driverSettings);
		}
		HardwareSettingUtilities hardwareSettingUtilities = new HardwareSettingUtilities(driverSettings);
		hardwareSettingUtilities.MacAddress = "000000000000";
		hardwareSettingUtilities.SerialNumber = "000000000000";
		hardwareSettingUtilities.FirmwareVersion = "0.00";
		string settingsXml = driverSettings.SaveAsXml();
		HardwareSetupInformation hardwareSetupInformation = new HardwareSetupInformation(id, definitionXml, settingsXml, productId, number);
		foreach (DeviceDefinition deviceDefinition in mainDefinition.HardwareDefinition.DeviceDefinitions)
		{
			DriverSettings driverSettings2 = new DriverSettings();
			DefinitionUtility.InitializeSettings(mainDefinition, deviceDefinition.Capabilities, driverSettings2);
			if (deviceDefinition.PtzSettings != null)
			{
				DefinitionUtility.InitializeSettings(mainDefinition, deviceDefinition.PtzSettings.Capabilities, driverSettings2);
			}
			foreach (StreamDefinition stream in deviceDefinition.Streams)
			{
				DefinitionUtility.InitializeSettings(mainDefinition, stream.Capabilities, driverSettings2);
			}
			foreach (MetadataItemDefinition metadataItem in deviceDefinition.MetadataItems)
			{
				DefinitionUtility.InitializeSettings(mainDefinition, metadataItem.Capabilities, driverSettings2);
			}
			string settingsXml2 = driverSettings2.SaveAsXml();
			PtzPresetCollection ptzPresetCollection = new PtzPresetCollection();
			string defaultStreamXml4NewDevice = Device.GetDefaultStreamXml4NewDevice(deviceDefinition, settingsXml2);
			string deviceTypeString = RegisteredDeviceType.GetDeviceTypeString(deviceDefinition.Hwid.Type);
			DeviceSetupInformation item = new DeviceSetupInformation(deviceDefinition.Hwid.Text, deviceTypeString, settingsXml2, ptzPresetCollection.SaveAsXml(), defaultStreamXml4NewDevice);
			hardwareSetupInformation.Devices.Add(item);
		}
		return hardwareSetupInformation;
	}

	private HardwareSetupInformation AcquireHardwareSetupInfo(AddHardwareTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo, DateTime taskStartTime, ref int taskProgress, int maxTaskProgress)
	{
		int num = 0;
		HardwareSetupInformationCommand command = new HardwareSetupInformationCommand(cmd.RecordingServerId, cmd.Address, cmd.UserName, cmd.Password, cmd.CustomData, cmd.DriverNumber, string.Empty, aacSupported: false, (cmd.Address.Scheme == Uri.UriSchemeHttps) ? new int?(cmd.Address.Port) : ((int?)null));
		IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
		ResponseBase responseBase;
		while (true)
		{
			try
			{
				responseBase = responseCollector.GetNextResponse(TimeSpan.FromMinutes(2.0));
			}
			catch (TimeoutException)
			{
				responseBase = null;
			}
			Logger.LogInformation("Add hardware task - response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.GetType().Name);
			if (!(responseBase is HeartBeatResponse))
			{
				break;
			}
			taskProgress = Math.Min(maxTaskProgress, taskProgress + 5);
			if (taskProgress != num)
			{
				responseSink.AddResponse(new TaskProgressResponse(taskStartTime, taskProgress));
				num = taskProgress;
			}
		}
		if (responseBase is HardwareSetupInformationResponse hardwareSetupInformationResponse)
		{
			return hardwareSetupInformationResponse.HardwareSetupInformation;
		}
		if (responseBase == null)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60275;
			taskError.ErrorText = VmoResources.VMO60275;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
			return null;
		}
		if (responseBase is HardwareAlreadyAddedResponse)
		{
			TaskError taskError2 = new TaskError();
			taskError2.ErrorCode = 60276;
			taskError2.ErrorText = VmoResources.VMO60276;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
			return null;
		}
		if (responseBase is HardwareUnsupportedResponse hardwareUnsupportedResponse)
		{
			TaskError taskError3 = new TaskError();
			string text = string.Empty;
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(hardwareUnsupportedResponse.ProductName))
			{
				text = hardwareUnsupportedResponse.ProductName;
				text2 = (string.IsNullOrEmpty(hardwareUnsupportedResponse.ProductVersion) ? string.Empty : hardwareUnsupportedResponse.ProductVersion);
				taskError3.Arguments.Add(0, text);
				taskError3.Arguments.Add(1, text2);
			}
			Logger.LogInformation("Add hardware task - UnsupportedResponse error code: {ErrorCode}", hardwareUnsupportedResponse.ErrorCode);
			switch (hardwareUnsupportedResponse.ErrorCode)
			{
			case 1:
				if (!string.IsNullOrEmpty(text))
				{
					taskError3.ErrorCode = 60286;
					taskError3.ErrorText = string.Format(CultureInfo.CurrentCulture, VmoResources.VMO60286, text, text2);
				}
				else
				{
					taskError3.ErrorCode = 60285;
					taskError3.ErrorText = VmoResources.VMO60285;
				}
				break;
			case 2:
				if (!string.IsNullOrEmpty(text))
				{
					taskError3.ErrorCode = 60288;
					taskError3.ErrorText = string.Format(CultureInfo.CurrentCulture, VmoResources.VMO60288, text, text2);
				}
				else
				{
					taskError3.ErrorCode = 60287;
					taskError3.ErrorText = VmoResources.VMO60287;
				}
				break;
			case 3:
				taskError3.ErrorCode = 60289;
				if (!string.IsNullOrEmpty(text))
				{
					taskError3.ErrorCode = 60290;
					taskError3.ErrorText = string.Format(CultureInfo.CurrentCulture, VmoResources.VMO60290, text, text2);
				}
				else
				{
					taskError3.ErrorCode = 60289;
					taskError3.ErrorText = VmoResources.VMO60289;
				}
				break;
			default:
				if (!string.IsNullOrEmpty(text))
				{
					taskError3.ErrorCode = 60292;
					taskError3.ErrorText = string.Format(CultureInfo.CurrentCulture, VmoResources.VMO60292, text, text2);
				}
				else
				{
					taskError3.ErrorCode = 60291;
					taskError3.ErrorText = VmoResources.VMO60291;
				}
				break;
			}
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError3.ErrorText, taskError3));
			return null;
		}
		if (responseBase is HardwareSetupErrorResponse hardwareSetupErrorResponse)
		{
			TaskError taskError4 = new TaskError();
			Logger.LogInformation("Add hardware task - HardwareSetupErrorResponse error code: {ErrorCode}", hardwareSetupErrorResponse.ErrorCode);
			switch (hardwareSetupErrorResponse.ErrorCode)
			{
			case 100:
				taskError4.ErrorCode = 60282;
				taskError4.ErrorText = VmoResources.VMO60282;
				break;
			case 101:
				taskError4.ErrorCode = 60283;
				taskError4.ErrorText = VmoResources.VMO60283;
				break;
			case 102:
				taskError4.ErrorCode = 60296;
				taskError4.ErrorText = VmoResources.VMO60296;
				break;
			default:
				taskError4.ErrorCode = 60284;
				taskError4.SubErrorCode = hardwareSetupErrorResponse.ErrorCode;
				taskError4.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60284, hardwareSetupErrorResponse.ErrorMessageInternal);
				taskError4.Arguments.Add(0, hardwareSetupErrorResponse.ErrorMessageInternal);
				break;
			}
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError4.ErrorText, taskError4));
			return null;
		}
		if (responseBase is RecorderUnavailableResponse)
		{
			TaskError taskError5 = new TaskError();
			taskError5.ErrorCode = 60277;
			taskError5.ErrorText = VmoResources.VMO60277;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5));
			return null;
		}
		if (responseBase is RecorderNotSupportedResponse)
		{
			TaskError taskError6 = new TaskError();
			taskError6.ErrorCode = 60278;
			taskError6.ErrorText = VmoResources.VMO60278;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError6.ErrorText, taskError6));
			return null;
		}
		if (responseBase is TimeoutResponse)
		{
			TaskError taskError7 = new TaskError();
			taskError7.ErrorCode = 60279;
			taskError7.ErrorText = VmoResources.VMO60279;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError7.ErrorText, taskError7));
			return null;
		}
		if (responseBase is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to setup device during 'Add hardware'. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to setup device during 'Add hardware'. [Error: {Exception}]", exceptionResponse.Exception);
			}
			TaskError taskError8 = new TaskError();
			taskError8.ErrorCode = 60281;
			taskError8.ErrorText = VmoResources.VMO60281;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError8.ErrorText, taskError8));
			return null;
		}
		Logger.LogWarning("Failed to setup device during 'Add hardware'. Unexpected response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
		TaskError taskError9 = new TaskError();
		taskError9.ErrorCode = 60280;
		taskError9.ErrorText = VmoResources.VMO60280;
		responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError9.ErrorText, taskError9));
		return null;
	}
}
