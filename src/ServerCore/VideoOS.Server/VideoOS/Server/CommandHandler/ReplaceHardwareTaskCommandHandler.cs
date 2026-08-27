using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Extensions;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.Properties;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public class ReplaceHardwareTaskCommandHandler : ICommandHandler
{
	private class Progress : IProgress<int>
	{
		public const int TaskStart = 5;

		public const int HardwareSetupInformationCommandStart = 10;

		public const int HardwareSetupInformationCommandMax = 49;

		public const int ReplaceHardwareCommandStart = 50;

		public const int ReplaceHardwareCommandMax = 99;

		private readonly DateTime _startTime;

		private readonly IResponseSink _responseSink;

		public Progress(IResponseSink responseSink)
		{
			_startTime = DateTime.UtcNow;
			_responseSink = responseSink ?? throw new ArgumentNullException("responseSink");
		}

		public void Report(int value)
		{
			_responseSink.AddResponse(new TaskProgressResponse(_startTime, value));
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	private IServerConfigurationLookup ServerConfigurationLookup => _serverEnvironment.ServerConfigurationLookup;

	private ILocalCommandProcessor LocalCommandProcessor => _serverEnvironment.LocalCommandProcessor;

	public ReplaceHardwareTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		_serverEnvironment = serverEnvironment ?? throw new ArgumentNullException("serverEnvironment");
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ReplaceHardwareTaskCommand replaceHardwareTaskCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo serverCallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ICallContextInfo userCallContextInfo = new BasicCallContextInfo(replaceHardwareTaskCommand.ActingUserSid, replaceHardwareTaskCommand.ActingUserDomainAccountName, replaceHardwareTaskCommand.ActingUserLocationIPAddress);
		Progress progress = new Progress(responseSink);
		TaskResponseBase response = ProcessCommand(replaceHardwareTaskCommand, progress, serverCallContextInfo, userCallContextInfo);
		responseSink.AddResponse(response);
	}

	private TaskResponseBase ProcessCommand(ReplaceHardwareTaskCommand cmd, IProgress<int> progress, ICallContextInfo serverCallContextInfo, ICallContextInfo userCallContextInfo)
	{
		progress.Report(5);
		if (!RecorderCommManager.IsRecorderNotDisconnected(cmd.RecordingServerId))
		{
			Logger.LogError("Replace hardware task - Recorder not available. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(60513, VmoResources.VMO60513, cmd.HardwareName);
		}
		if (RecorderCommManager.IsRecorderIncompatible(cmd.RecordingServerId))
		{
			Logger.LogError("Replace hardware task - Recorder is running unsupported version. Recorder Id: {ServerId}", cmd.RecordingServerId);
			return CreateTaskFailedResponse(60514, VmoResources.VMO60514, cmd.HardwareName);
		}
		IPHardware iPHardware = ServerConfigurationLookup.FindIPHardware(cmd.HardwareId);
		if (iPHardware == null)
		{
			Logger.LogError("Replace hardware task - Hardware not found: {HardwareId}", cmd.HardwareId);
			return CreateTaskFailedResponse(60517, VmoResources.VMO60517, cmd.HardwareName);
		}
		Uri address;
		HardwareSetupInformation hardwareSetupInformation;
		(bool, TaskResponseBase) newHardwareSetupInformation = GetNewHardwareSetupInformation(cmd, iPHardware.SettingUtilities.MacAddress, progress, serverCallContextInfo, out address, out hardwareSetupInformation);
		if (!newHardwareSetupInformation.Item1)
		{
			return newHardwareSetupInformation.Item2;
		}
		ReplaceHardwareLocals locals = new ReplaceHardwareLocals(cmd, address, hardwareSetupInformation, iPHardware);
		IEnumerable<ReplaceHardwareDevice> source = iPHardware.Devices.Select((Device d) => new ReplaceHardwareDevice(d));
		IEnumerable<ReplaceHardwareDevice> source2 = hardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => new ReplaceHardwareDevice(d, Guid.Empty));
		DeviceMapper deviceMapper = new DeviceMapper(locals, source.ToList(), source2.ToList());
		switch (deviceMapper.ReplaceHardware(cmd.AllowDeletingDisabledDevices))
		{
		case ReplaceHardwareResult.Error_TooManyDevices_Delete:
			Logger.LogError("Existing hardware has more devices than the new hardware. Cannot succeed without deleting devices.");
			return CreateTaskFailedResponse(60518, VmoResources.VMO60518, cmd.HardwareName);
		case ReplaceHardwareResult.Error_TooManyDevices_Disable:
			Logger.LogError("Existing hardware has more devices than the new hardware. Cannot succeed without disabling some devices.");
			return CreateTaskFailedResponse(60519, VmoResources.VMO60519, cmd.HardwareName);
		default:
			return ExecuteReplaceHardware(iPHardware, locals, progress, userCallContextInfo);
		}
	}

	private (bool Success, TaskResponseBase Response) GetNewHardwareSetupInformation(ReplaceHardwareTaskCommand replaceHardwareTaskCommand, string macAddress, IProgress<int> progress, ICallContextInfo callContextInfo, out Uri address, out HardwareSetupInformation hardwareSetupInformation)
	{
		UriBuilder uriBuilder = new UriBuilder(Uri.UriSchemeHttp, replaceHardwareTaskCommand.HardwareAddress, replaceHardwareTaskCommand.Port);
		address = uriBuilder.Uri;
		try
		{
			ResponseBase response = SendHardwareSetupInformationCommandAndWaitforFinalResponse(address);
			return HandleFinalResponse(response, out hardwareSetupInformation);
		}
		finally
		{
			progress.Report(49);
		}
		(bool Success, TaskResponseBase Response) HandleFinalResponse(ResponseBase responseBase, out HardwareSetupInformation hardwareSetupInfo)
		{
			hardwareSetupInfo = null;
			if (responseBase == null)
			{
				return (Success: false, Response: CreateTaskFailedResponse(60512, VmoResources.VMO60512));
			}
			if (responseBase is HardwareSetupInformationResponse hardwareSetupInformationResponse)
			{
				hardwareSetupInfo = hardwareSetupInformationResponse.HardwareSetupInformation;
				return (Success: true, Response: new TaskSuccessResponse());
			}
			if (responseBase is HardwareSetupErrorResponse hardwareSetupErrorResponse)
			{
				Logger.LogInformation("Replace hardware task - HardwareSetupErrorResponse error code: {ErrorCode}", hardwareSetupErrorResponse.ErrorCode);
				return (Success: false, Response: CreateTaskFailedResponse(60520, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60520, hardwareSetupErrorResponse.ErrorMessageInternal), hardwareSetupErrorResponse.ErrorMessageInternal));
			}
			if (responseBase is RecorderUnavailableResponse)
			{
				return (Success: false, Response: CreateTaskFailedResponse(60513, VmoResources.VMO60513));
			}
			if (responseBase is RecorderNotSupportedResponse)
			{
				return (Success: false, Response: CreateTaskFailedResponse(60514, VmoResources.VMO60514));
			}
			if (responseBase is TimeoutResponse)
			{
				return (Success: false, Response: CreateTaskFailedResponse(60515, VmoResources.VMO60515));
			}
			if (responseBase is ExceptionResponse exceptionResponse)
			{
				if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
				{
					Logger.LogWarning("Failed to setup device during 'Replace hardware'. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
				}
				else
				{
					Logger.LogWarning("Failed to setup device during 'Replace hardware'. [Error: {Exception}]", exceptionResponse.Exception);
				}
				return (Success: false, Response: CreateTaskFailedResponse(60515, VmoResources.VMO60515));
			}
			Logger.LogWarning("Failed to setup device during 'Replace hardware'. Unexpected response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
			return (Success: false, Response: CreateTaskFailedResponse(60516, VmoResources.VMO60516));
		}
		ResponseBase SendHardwareSetupInformationCommandAndWaitforFinalResponse(Uri uri)
		{
			HardwareSetupInformationCommand command = new HardwareSetupInformationCommand(replaceHardwareTaskCommand.RecordingServerId, uri, replaceHardwareTaskCommand.UserName, replaceHardwareTaskCommand.Password, replaceHardwareTaskCommand.CustomDriverData, replaceHardwareTaskCommand.DriverNumber, macAddress, aacSupported: false, replaceHardwareTaskCommand.UseHttps ? new int?(replaceHardwareTaskCommand.HttpsPort) : ((int?)null));
			IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
			int num = 10;
			progress.Report(num);
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
				if (!(responseBase is HeartBeatResponse))
				{
					break;
				}
				num = Math.Min(48, num + 5);
				progress.Report(num);
			}
			return responseBase;
		}
	}

	private TaskResponseBase ExecuteReplaceHardware(IPHardware oldHardware, ReplaceHardwareLocals locals, IProgress<int> progress, ICallContextInfo callContextInfo)
	{
		try
		{
			PreLoadDataThatMayChangeDuringReplace();
			ResponseBase response = SendReplaceHardwareCommandAndWaitForFinalResponse();
			return HandleFinalResponse(response);
		}
		catch (Exception ex)
		{
			return CreateTaskFailedResponse(60510, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, ex.Message), locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, ex.Message);
		}
		finally
		{
			progress.Report(99);
		}
		TaskResponseBase HandleFinalResponse(ResponseBase responseBase)
		{
			if (responseBase == null)
			{
				return CreateTaskFailedResponse(60512, VmoResources.VMO60512);
			}
			if (responseBase is ReplaceHardwareResponse replaceHardwareResponse)
			{
				IPHardware newHardware = ServerConfigurationLookup.FindIPHardware(oldHardware.ID);
				ReplaceHardwareTaskResult replaceHardwareTaskResult = new ReplaceHardwareTaskResult();
				replaceHardwareTaskResult.Hardware = new ReplaceHardwareTaskDevice(oldHardware.ID, "Hardware", oldHardware.Name);
				replaceHardwareTaskResult.Hardware.SettingsChanged = !UpdateHardwareTaskCommandHandler.IsDeviceSettingsEqual(oldHardware.Settings, newHardware.Settings);
				replaceHardwareTaskResult.AddedDevices.AddRange(from d in newHardware.Devices
					where !oldHardware.Devices.Any((Device o) => o.ID == d.ID)
					select new ReplaceHardwareTaskDevice(d.ID, DeviceTypesExtensions.FromBuiltInDeviceType(d.DeviceType).ToString(), d.Name));
				replaceHardwareTaskResult.DeletedDevices.AddRange(from d in oldHardware.Devices
					where !newHardware.Devices.Any((Device n) => n.ID == d.ID)
					select new ReplaceHardwareTaskDevice(d.ID, DeviceTypesExtensions.FromBuiltInDeviceType(d.DeviceType).ToString(), d.Name));
				replaceHardwareTaskResult.MappedDevices.AddRange(from d in newHardware.Devices
					where oldHardware.Devices.Any((Device o) => o.ID == d.ID)
					select new ReplaceHardwareTaskDevice(d.ID, DeviceTypesExtensions.FromBuiltInDeviceType(d.DeviceType).ToString(), d.Name));
				StringComparer ordinal = StringComparer.Ordinal;
				foreach (ReplaceHardwareTaskDevice mappedDevice in replaceHardwareTaskResult.MappedDevices)
				{
					Device newDevice = newHardware.Devices.FirstOrDefault((Device d) => d.ID == mappedDevice.Id);
					if (newDevice != null)
					{
						Device oldDevice = oldHardware.Devices.FirstOrDefault((Device d) => d.ID == mappedDevice.Id);
						if (oldDevice != null)
						{
							mappedDevice.SettingsChanged = !UpdateHardwareTaskCommandHandler.IsDeviceSettingsEqual(oldDevice.Settings, newDevice.Settings);
							RHSettingsDevice rHSettingsDevice = replaceHardwareResponse.SettingsDevices.FirstOrDefault((RHSettingsDevice d) => d.DeviceId == oldDevice.ID);
							if (rHSettingsDevice != null)
							{
								mappedDevice.DeletedEvents = new ReplaceHardwareTaskEventCollection();
								RHEventValue[] deletedEvents = rHSettingsDevice.DeletedEvents;
								foreach (RHEventValue rHEventValue in deletedEvents)
								{
									EventHwid eventHwid = new EventHwid(rHEventValue.EventHWId);
									VideoOS.Common.Integration.Definition.EventDefinition eventDefinition = oldHardware.Definition.MainDefinition.Events.FirstOrDefault((VideoOS.Common.Integration.Definition.EventDefinition e) => e.Hwid == eventHwid);
									if (eventDefinition != null)
									{
										mappedDevice.DeletedEvents.Add(new ReplaceHardwareTaskEvent(eventDefinition.ReferenceId, eventDefinition.DisplayNameResolved));
									}
								}
								if (!string.IsNullOrEmpty(oldDevice.Streams) && !string.IsNullOrEmpty(newDevice.Streams))
								{
									StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
									streamsConfiguration.LoadXml(oldDevice.Streams);
									StreamsConfiguration newStreams = new StreamsConfiguration();
									newStreams.LoadXml(newDevice.Streams);
									IEnumerable<StreamInfo> source = streamsConfiguration.Where((StreamInfo o) => !newStreams.Any((StreamInfo n) => n.StreamReferenceId == o.StreamReferenceId));
									mappedDevice.DeletedStreams = new ReplaceHardwareTaskStreamCollection();
									mappedDevice.DeletedStreams.AddRange(source.Select((StreamInfo s) => new ReplaceHardwareTaskStream(s.StreamReferenceId, s.Name)));
								}
								if (oldDevice.Presets.Any() || newDevice.Presets.Any())
								{
									IEnumerable<PtzPreset> source2 = oldDevice.Presets.Where((PtzPreset o) => !newDevice.Presets.Any((PtzPreset n) => n.Id == o.Id));
									mappedDevice.DeletedPtzPresets = new ReplaceHardwareTaskPtzPresetCollection();
									mappedDevice.DeletedPtzPresets.AddRange(source2.Select((PtzPreset p) => new ReplaceHardwareTaskPtzPreset(p.Id, p.Name)));
								}
								if (RegisteredDeviceType.IsCamera(oldDevice.DeviceType))
								{
									mappedDevice.DeletedPatrollingProfiles = new ReplaceHardwareTaskPatrollingProfileCollection();
									string[] deletedPatrollingProfiles = rHSettingsDevice.DeletedPatrollingProfiles;
									foreach (string name in deletedPatrollingProfiles)
									{
										mappedDevice.DeletedPatrollingProfiles.Add(new ReplaceHardwareTaskPatrollingProfile(name));
									}
								}
							}
						}
					}
				}
				return new TaskSuccessResponse(replaceHardwareTaskResult.ToXml());
			}
			if (responseBase is ExceptionResponse exceptionResponse)
			{
				return CreateTaskFailedResponse(60510, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, exceptionResponse.Exception.Message), locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, exceptionResponse.Exception.Message);
			}
			if (responseBase is EvidenceLockFailedRemoveResponse evidenceLockFailedRemoveResponse)
			{
				string text = string.Empty;
				foreach (LockedDevices faultDevice in evidenceLockFailedRemoveResponse.FaultDevices)
				{
					text = string.Format(CultureInfo.CurrentCulture, "Locked Device: {0}{1}{2}", text, faultDevice.DeviceName, Environment.NewLine);
				}
				return CreateTaskFailedResponse(60510, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, text), locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name, text);
			}
			if (responseBase is FailResponse)
			{
				return CreateTaskFailedResponse(60511, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60511, locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name), locals.NewHardwareInfo.Address.DnsSafeHost, oldHardware.Name);
			}
			Logger.LogWarning("Replace hardware returned an unexpected response. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
			return CreateTaskFailedResponse(60521, VmoResources.VMO60521);
		}
		bool PreLoadDataThatMayChangeDuringReplace()
		{
			return oldHardware.Definition != null;
		}
		ResponseBase SendReplaceHardwareCommandAndWaitForFinalResponse()
		{
			ReplaceHardwareCommand command = new ReplaceHardwareCommand(oldHardware.IDRecorder, oldHardware.Recorder.Name, locals.NewHardwareInfo.Address, locals.NewHardwareInfo.UserName, locals.NewHardwareInfo.Password, locals.NewHardwareInfo.DriverNumber, locals.NewHardwareSetupInformation.ProductId, locals.NewAuditHardwareName, locals.NewHardwareSetupInformation.DefinitionXml, locals.NewHardwareSetupInformation.SettingsXml, locals.NewHardwareSetupInformation.DriverId, oldHardware.ID, locals.InheritedDevices.ToArray(), locals.RemovedDevices.ToArray(), Array.Empty<Guid>());
			IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
			int num = 50;
			progress.Report(num);
			ResponseBase nextResponse;
			while (true)
			{
				try
				{
					nextResponse = responseCollector.GetNextResponse(TimeSpan.FromMinutes(2.0));
				}
				catch (TimeoutException)
				{
					return null;
				}
				if (!(nextResponse is HeartBeatResponse))
				{
					break;
				}
				num = Math.Min(98, num + 5);
				progress.Report(num);
			}
			return nextResponse;
		}
	}

	private static TaskFailedResponse CreateTaskFailedResponse(int errorCode, string errorText, params string[] arguments)
	{
		TaskError taskError = new TaskError
		{
			ErrorCode = errorCode,
			ErrorText = errorText
		};
		for (int i = 0; i < arguments.Length; i++)
		{
			taskError.Arguments.Add(i, arguments[i]);
		}
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}
}
