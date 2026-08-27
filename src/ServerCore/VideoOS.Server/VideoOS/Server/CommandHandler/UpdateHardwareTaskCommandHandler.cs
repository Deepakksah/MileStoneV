using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.Common.Integration.HardwareSetup;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public class UpdateHardwareTaskCommandHandler : ICommandHandler
{
	private static class ICPErrorCodes
	{
		public const int LogOnErrorNoAdminAccess = 100;

		public const int LogOnErrorNoAccess = 101;

		public const int NoRecordersError = 102;
	}

	private class UpdateHardwareLocals
	{
		public HardwareSetupSettings NewHardwareInfo { get; set; }

		public string NewHardwareName { get; set; }

		public string NewAuditHardwareName { get; set; }

		public HardwareSetupInformation NewHardwareSetupInformation { get; set; }

		public List<RHInheritedDevice> InheritedDevices { get; set; }

		public HashSet<Guid> RemovedDevices { get; set; }

		public HashSet<Guid> RemovedEvents { get; set; }

		public ReplaceHardwareResponse Response { get; set; }

		public DeviceSetupSettings GetNewDeviceSetupInfo(DeviceHwid hwid)
		{
			using (IEnumerator<DeviceSetupSettings> enumerator = NewHardwareInfo.Devices.Where((DeviceSetupSettings device) => device.DeviceHwid == hwid).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}
	}

	private IServerEnvironment _serverEnvironment;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	public UpdateHardwareTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateHardwareTaskCommand updateHardwareCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ProcessCommand(updateHardwareCommand, responseSink, callContextInfo);
	}

	private void ProcessCommand(UpdateHardwareTaskCommand updateHardwareCommand, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 10;
		if (!RecorderCommManager.IsRecorderNotDisconnected(updateHardwareCommand.RecordingServerId))
		{
			Logger.LogError("Update hardware task - Recorder not available. Recorder Id: {RecordingServerId}", updateHardwareCommand.RecordingServerId);
			responseSink.AddResponse(CreateTaskFailedResponse(67011, VmoResources.VMO67011, updateHardwareCommand.HardwareName));
			return;
		}
		if (RecorderCommManager.IsRecorderIncompatible(updateHardwareCommand.RecordingServerId))
		{
			Logger.LogError("Update hardware task - Recorder is running unsupported version. Recorder Id: {RecordingServerId}", updateHardwareCommand.RecordingServerId);
			responseSink.AddResponse(CreateTaskFailedResponse(67012, VmoResources.VMO67012, updateHardwareCommand.HardwareName));
			return;
		}
		IPHardware iPHardware = GetIPHardware(updateHardwareCommand.HardwareId);
		if (iPHardware == null)
		{
			Logger.LogError("Update hardware task - Hardware not found: {HardwareId}", updateHardwareCommand.HardwareId);
			responseSink.AddResponse(CreateTaskFailedResponse(67016, VmoResources.VMO67016, updateHardwareCommand.HardwareName));
			return;
		}
		if (updateHardwareCommand.SessionDataId == 0)
		{
			responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
			HardwareSetupInformation hardwareSetupInformation;
			if (!iPHardware.IsMipDriver && !iPHardware.IsInterconnectedProduct)
			{
				Logger.LogError("Update hardware task - Is only supported for MIP and Interconnect drivers");
				responseSink.AddResponse(CreateTaskFailedResponse(67001, VmoResources.VMO67001, updateHardwareCommand.HardwareName));
			}
			else if (GetNewHardwareSetupInformation(updateHardwareCommand, iPHardware, responseSink, callContextInfo, out hardwareSetupInformation))
			{
				UpdateHardwareLocals updateHardwareLocals = new UpdateHardwareLocals();
				updateHardwareLocals.NewHardwareSetupInformation = hardwareSetupInformation;
				LoadHardwareInformation(iPHardware, updateHardwareLocals);
				UpdateHardwareTaskResult updateHardwareTaskResult = BuildHardwareUpdateTaskResult(iPHardware, updateHardwareLocals);
				if (updateHardwareTaskResult == null)
				{
					Logger.LogError(VmoResources.VMO67017, updateHardwareCommand.HardwareId);
					responseSink.AddResponse(CreateTaskFailedResponse(67017, VmoResources.VMO67017, updateHardwareCommand.HardwareName));
				}
				else
				{
					updateHardwareTaskResult.SessionDataId = StoreHardwareInformation(updateHardwareCommand.HardwareId, hardwareSetupInformation);
					responseSink.AddResponse(new TaskSuccessResponse(updateHardwareTaskResult.ToXml()));
				}
			}
			return;
		}
		UpdateHardwareSessionData updateHardwareSessionData = UpdateHardwareSessionData.Query(updateHardwareCommand.SessionDataId);
		if (updateHardwareSessionData == null)
		{
			Logger.LogError("Update hardware task - Session timed out: {SessionDataId}", updateHardwareCommand.SessionDataId);
			responseSink.AddResponse(CreateTaskFailedResponse(67002, VmoResources.VMO67002, updateHardwareCommand.HardwareName));
			return;
		}
		if (updateHardwareSessionData.HardwareId != updateHardwareCommand.HardwareId)
		{
			Logger.LogError("Update hardware task - Invalid SessionDataId: {SessionDataId}", updateHardwareCommand.SessionDataId);
			responseSink.AddResponse(CreateTaskFailedResponse(67003, VmoResources.VMO67003, updateHardwareCommand.HardwareName));
			return;
		}
		UpdateHardwareLocals updateHardwareLocals2 = new UpdateHardwareLocals();
		updateHardwareLocals2.NewHardwareSetupInformation = updateHardwareSessionData.HardwareSetupInformation;
		LoadHardwareInformation(iPHardware, updateHardwareLocals2);
		UpdateHardwareTaskResult result = BuildHardwareUpdateTaskResult(iPHardware, updateHardwareLocals2);
		if (!ExecuteReplaceHardware(iPHardware.IDRecorder, iPHardware.Recorder.Name, iPHardware.ID, iPHardware.Name, updateHardwareLocals2, responseSink, callContextInfo))
		{
			updateHardwareSessionData.Dispose();
			return;
		}
		UpdateHardwareTaskResult updateHardwareTaskResult2 = BuildHardwareUpdateTaskResult2(updateHardwareLocals2, result);
		updateHardwareSessionData.Dispose();
		responseSink.AddResponse(new TaskSuccessResponse(updateHardwareTaskResult2.ToXml()));
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

	private bool ExecuteReplaceHardware(Guid recorderId, string recorderName, Guid hardwareId, string hardwareName, UpdateHardwareLocals locals, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		if (locals == null)
		{
			throw new ArgumentNullException("locals");
		}
		try
		{
			ReplaceHardwareCommand command = new ReplaceHardwareCommand(recorderId, recorderName, locals.NewHardwareInfo.Address, locals.NewHardwareInfo.UserName, locals.NewHardwareInfo.Password, locals.NewHardwareInfo.DriverNumber, locals.NewHardwareSetupInformation.ProductId, locals.NewAuditHardwareName, locals.NewHardwareSetupInformation.DefinitionXml, locals.NewHardwareSetupInformation.SettingsXml, locals.NewHardwareSetupInformation.DriverId, hardwareId, locals.InheritedDevices.ToArray(), locals.RemovedDevices.ToArray(), locals.RemovedEvents.ToArray());
			IResponseCollector responseCollector = _serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
			int num = 10;
			int num2 = 10;
			int val = 90;
			while (true)
			{
				ResponseBase responseBase;
				try
				{
					responseBase = responseCollector.GetNextResponse(TimeSpan.FromMinutes(2.0));
				}
				catch (TimeoutException)
				{
					responseBase = null;
				}
				DateTime utcNow = DateTime.UtcNow;
				if (responseBase is HeartBeatResponse)
				{
					num = Math.Min(val, num + 5);
					if (num != num2)
					{
						responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
						num2 = num;
					}
					continue;
				}
				if (responseBase == null)
				{
					TaskError taskError = new TaskError();
					taskError.ErrorCode = 67006;
					taskError.ErrorText = VmoResources.VMO67006;
					responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
					return false;
				}
				if (responseBase is ReplaceHardwareResponse response)
				{
					locals.Response = response;
					return true;
				}
				if (responseBase is ExceptionResponse exceptionResponse)
				{
					TaskError taskError2 = new TaskError();
					taskError2.ErrorCode = 67004;
					taskError2.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, locals.NewHardwareInfo.Address.DnsSafeHost, hardwareName, exceptionResponse.Exception.Message);
					responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
					return false;
				}
				if (responseBase is EvidenceLockFailedRemoveResponse evidenceLockFailedRemoveResponse)
				{
					string text = string.Empty;
					foreach (LockedDevices faultDevice in evidenceLockFailedRemoveResponse.FaultDevices)
					{
						text = string.Format(CultureInfo.CurrentCulture, "Locked Device: {0}{1}{2}", text, faultDevice.DeviceName, Environment.NewLine);
					}
					TaskError taskError3 = new TaskError();
					taskError3.ErrorCode = 67004;
					taskError3.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, locals.NewHardwareInfo.Address.DnsSafeHost, hardwareName, text);
					responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError3.ErrorText, taskError3));
					return false;
				}
				FailResponse failResponse = responseBase as FailResponse;
				if (failResponse != null)
				{
					break;
				}
			}
			TaskError taskError4 = new TaskError();
			taskError4.ErrorCode = 67005;
			taskError4.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67005, locals.NewHardwareInfo.Address.DnsSafeHost, hardwareName);
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError4.ErrorText, taskError4));
			return false;
		}
		catch (Exception ex2)
		{
			TaskError taskError5 = new TaskError();
			taskError5.ErrorCode = 67004;
			taskError5.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, locals.NewHardwareInfo.Address.DnsSafeHost, hardwareName, ex2.Message);
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5));
			return false;
		}
	}

	private IPHardware GetIPHardware(Guid id)
	{
		return _serverEnvironment.ServerConfigurationLookup.FindIPHardware(id);
	}

	private bool GetNewHardwareSetupInformation(UpdateHardwareTaskCommand updateHardwareCommand, IPHardware ipHardware, IResponseSink responseSink, ICallContextInfo callContextInfo, out HardwareSetupInformation hardwareSetupInformation)
	{
		hardwareSetupInformation = null;
		string text = null;
		try
		{
			text = (ipHardware.IsMipDriver ? ipHardware.CameraData : null);
		}
		catch (StrongTypingException)
		{
			text = null;
		}
		HardwareSetupInformationCommand command = new HardwareSetupInformationCommand(updateHardwareCommand.RecordingServerId, ipHardware.Uri, ipHardware.LoginID, ipHardware.Password, text, ipHardware.DriverNumber, ipHardware.SettingUtilities.MacAddress);
		IResponseCollector responseCollector = _serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
		int num = 10;
		int num2 = 10;
		int val = 90;
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
			DateTime utcNow = DateTime.UtcNow;
			if (!(responseBase is HeartBeatResponse))
			{
				break;
			}
			num = Math.Min(val, num + 5);
			if (num != num2)
			{
				responseSink.AddResponse(new TaskProgressResponse(utcNow, num));
				num2 = num;
			}
		}
		if (responseBase == null)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 67006;
			taskError.ErrorText = VmoResources.VMO67006;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
			return false;
		}
		if (responseBase is HardwareSetupInformationResponse hardwareSetupInformationResponse)
		{
			hardwareSetupInformation = hardwareSetupInformationResponse.HardwareSetupInformation;
			return true;
		}
		if (responseBase is HardwareSetupErrorResponse hardwareSetupErrorResponse)
		{
			TaskError taskError2 = new TaskError();
			Logger.LogInformation("Update hardware task - HardwareSetupErrorResponse error code: {ErrorCode}", hardwareSetupErrorResponse.ErrorCode);
			switch (hardwareSetupErrorResponse.ErrorCode)
			{
			case 100:
				taskError2.ErrorCode = 67007;
				taskError2.ErrorText = VmoResources.VMO67007;
				break;
			case 101:
				taskError2.ErrorCode = 67008;
				taskError2.ErrorText = VmoResources.VMO67008;
				break;
			case 102:
				taskError2.ErrorCode = 67009;
				taskError2.ErrorText = VmoResources.VMO67009;
				break;
			default:
				taskError2.ErrorCode = 67010;
				taskError2.SubErrorCode = hardwareSetupErrorResponse.ErrorCode;
				taskError2.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67010, hardwareSetupErrorResponse.ErrorMessageInternal);
				taskError2.Arguments.Add(0, hardwareSetupErrorResponse.ErrorMessageInternal);
				break;
			}
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
			return false;
		}
		if (responseBase is RecorderUnavailableResponse)
		{
			TaskError taskError3 = new TaskError();
			taskError3.ErrorCode = 67011;
			taskError3.ErrorText = VmoResources.VMO67011;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError3.ErrorText, taskError3));
			return false;
		}
		if (responseBase is RecorderNotSupportedResponse)
		{
			TaskError taskError4 = new TaskError();
			taskError4.ErrorCode = 67012;
			taskError4.ErrorText = VmoResources.VMO67012;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError4.ErrorText, taskError4));
			return false;
		}
		if (responseBase is TimeoutResponse)
		{
			TaskError taskError5 = new TaskError();
			taskError5.ErrorCode = 67013;
			taskError5.ErrorText = VmoResources.VMO67013;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5));
			return false;
		}
		if (responseBase is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to setup device during 'Update hardware'. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to setup device during 'Update hardware'. [Error: {Exception}]", exceptionResponse.Exception);
			}
			TaskError taskError6 = new TaskError();
			taskError6.ErrorCode = 67014;
			taskError6.ErrorText = VmoResources.VMO67014;
			responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError6.ErrorText, taskError6));
			return false;
		}
		Logger.LogWarning("Failed to setup device during 'Update hardware'. Unexpected response received. Response: {Response}", (responseBase == null) ? "<NULL>" : responseBase.ToString());
		TaskError taskError7 = new TaskError();
		taskError7.ErrorCode = 67015;
		taskError7.ErrorText = VmoResources.VMO67015;
		responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError7.ErrorText, taskError7));
		return false;
	}

	private static UpdateHardwareTaskResult BuildHardwareUpdateTaskResult(IPHardware oldIPHardware, UpdateHardwareLocals locals)
	{
		string capabilityDisplayValue = DefinitionUtility.GetCapabilityDisplayValue(oldIPHardware.Definition, oldIPHardware.Settings, TypeReferenceIds.MacAddress);
		DriverSettings settings = new DriverSettings(locals.NewHardwareSetupInformation.SettingsXml);
		List<string> list = new List<string>();
		list.Add(locals.NewHardwareSetupInformation.SettingsXml);
		list.AddRange(locals.NewHardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => d.SettingsXml));
		HardwareDefinition hardwareDefinition = new HardwareDefinition(MainDefinition.LoadXml(locals.NewHardwareSetupInformation.DefinitionXml, list));
		string capabilityDisplayValue2 = DefinitionUtility.GetCapabilityDisplayValue(hardwareDefinition, settings, TypeReferenceIds.MacAddress);
		if (!capabilityDisplayValue.Equals(capabilityDisplayValue2.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		UpdateHardwareTaskResult updateHardwareTaskResult = new UpdateHardwareTaskResult();
		updateHardwareTaskResult.HardwareId = oldIPHardware.ID;
		updateHardwareTaskResult.HardwareName = oldIPHardware.Name;
		updateHardwareTaskResult.RecordingServerId = oldIPHardware.Recorder.ID;
		List<UpdateHardwareTaskDetailResult> list2 = new List<UpdateHardwareTaskDetailResult>();
		Dictionary<string, DeviceSetupInformation> dictionary = new Dictionary<string, DeviceSetupInformation>();
		Dictionary<Guid, DeviceSetupInformation> dictionary2 = new Dictionary<Guid, DeviceSetupInformation>();
		Dictionary<Guid, DeviceSetupInformation> dictionary3 = new Dictionary<Guid, DeviceSetupInformation>();
		HashSet<Guid> hashSet = new HashSet<Guid>();
		List<RHInheritedDevice> list3 = new List<RHInheritedDevice>();
		Dictionary<Guid, Device> dictionary4 = new Dictionary<Guid, Device>();
		foreach (DeviceSetupInformation device3 in locals.NewHardwareSetupInformation.Devices)
		{
			DriverSettings driverSettings = new DriverSettings(device3.SettingsXml);
			string valueByTypeId = driverSettings.GetValueByTypeId(device3.DeviceHwid, TypeReferenceIds.DeviceId.ToString());
			dictionary.Add(valueByTypeId, device3);
		}
		foreach (Device device4 in oldIPHardware.Devices)
		{
			DriverSettings driverSettings2 = new DriverSettings(device4.SettingsAsXml);
			string valueByTypeId2 = driverSettings2.GetValueByTypeId(device4.Hwid.ToString(), TypeReferenceIds.DeviceId.ToString());
			Guid guid = new Guid(valueByTypeId2);
			if (dictionary.TryGetValue(valueByTypeId2, out var value))
			{
				dictionary.Remove(valueByTypeId2);
				DriverSettings newDriverSettings = new DriverSettings(value.SettingsXml);
				if (IsDeviceSettingsEqual(driverSettings2, newDriverSettings))
				{
					list2.Add(new UpdateHardwareTaskDetailResult(device4.ID, device4.Name, "UNCHANGED", device4.DeviceType));
					dictionary3.Add(device4.ID, value);
				}
				else
				{
					list2.Add(new UpdateHardwareTaskDetailResult(device4.ID, device4.Name, "UPDATED", device4.DeviceType));
					dictionary2.Add(device4.ID, value);
				}
			}
			else
			{
				list2.Add(new UpdateHardwareTaskDetailResult(device4.ID, device4.Name, "DELETED", device4.DeviceType));
				hashSet.Add(device4.ID);
			}
			dictionary4.Add(device4.ID, device4);
		}
		foreach (DeviceSetupInformation value2 in dictionary.Values)
		{
			DriverSettings driverSettings3 = new DriverSettings(value2.SettingsXml);
			string valueByTypeId3 = driverSettings3.GetValueByTypeId(value2.DeviceHwid, TypeReferenceIds.DeviceName.ToString());
			list2.Add(new UpdateHardwareTaskDetailResult(Guid.Empty, valueByTypeId3, "ADDED", value2.DeviceType));
		}
		foreach (KeyValuePair<Guid, DeviceSetupInformation> item in dictionary2)
		{
			Device device = dictionary4[item.Key];
			DriverSettings driverSettings4 = new DriverSettings(device.SettingsAsXml);
			string newAuditDeviceName = GetNewAuditDeviceName(device.Hwid, driverSettings4, locals);
			list3.Add(new RHInheritedDevice(item.Value.DeviceHwid, device.Name, newAuditDeviceName, item.Value.SettingsXml, item.Value.PresetsXml, item.Key, transferDevicePresets: true));
		}
		foreach (KeyValuePair<Guid, DeviceSetupInformation> item2 in dictionary3)
		{
			Device device2 = dictionary4[item2.Key];
			DriverSettings driverSettings5 = new DriverSettings(device2.SettingsAsXml);
			string newAuditDeviceName2 = GetNewAuditDeviceName(device2.Hwid, driverSettings5, locals);
			list3.Add(new RHInheritedDevice(item2.Value.DeviceHwid, device2.Name, newAuditDeviceName2, item2.Value.SettingsXml, item2.Value.PresetsXml, item2.Key, transferDevicePresets: true));
		}
		foreach (DeviceSetupInformation value3 in dictionary.Values)
		{
			DeviceHwid deviceHwid = new DeviceHwid(value3.DeviceHwid);
			DriverSettings driverSettings6 = new DriverSettings(value3.SettingsXml);
			string newDeviceName = NamingScheme.FindDeviceName(DeviceNameTemplate.Default, locals.NewHardwareInfo, locals.GetNewDeviceSetupInfo(deviceHwid));
			string newAuditDeviceName3 = GetNewAuditDeviceName(deviceHwid, driverSettings6, locals);
			list3.Add(new RHInheritedDevice(value3.DeviceHwid, newDeviceName, newAuditDeviceName3, value3.SettingsXml, value3.PresetsXml, Guid.Empty, transferDevicePresets: false));
		}
		locals.RemovedDevices = hashSet;
		locals.InheritedDevices = list3;
		locals.RemovedEvents = new HashSet<Guid>();
		updateHardwareTaskResult.HardwareEventsChanged = !IsHardwareEventEqual(oldIPHardware.Definition.Events, hardwareDefinition.Events);
		updateHardwareTaskResult.TaskDetails = list2.ToArray();
		return updateHardwareTaskResult;
	}

	private static UpdateHardwareTaskResult BuildHardwareUpdateTaskResult2(UpdateHardwareLocals locals, UpdateHardwareTaskResult result)
	{
		UpdateHardwareTaskDetailResult[] taskDetails = result.TaskDetails;
		foreach (UpdateHardwareTaskDetailResult updateHardwareTaskDetailResult in taskDetails)
		{
			if (!(updateHardwareTaskDetailResult.Change == "ADDED"))
			{
				continue;
			}
			for (int j = 0; j < locals.Response.AddedDevices.Length; j++)
			{
				if (locals.Response.AddedDevices[j].EndsWith(updateHardwareTaskDetailResult.DeviceName, StringComparison.Ordinal))
				{
					updateHardwareTaskDetailResult.DeviceId = new Guid(locals.Response.AddedDeviceIds[j]);
				}
			}
		}
		return result;
	}

	private static long StoreHardwareInformation(Guid hardwareId, HardwareSetupInformation hardwareSetupInformation)
	{
		UpdateHardwareSessionData updateHardwareSessionData = new UpdateHardwareSessionData();
		updateHardwareSessionData.HardwareSetupInformation = hardwareSetupInformation;
		updateHardwareSessionData.HardwareId = hardwareId;
		return updateHardwareSessionData.SessionDataId;
	}

	private static void LoadHardwareInformation(IPHardware ipHardware, UpdateHardwareLocals newLocals)
	{
		HardwareSetupInformation newHardwareSetupInformation = newLocals.NewHardwareSetupInformation;
		newLocals.NewHardwareInfo = new HardwareSetupSettings(newHardwareSetupInformation.DriverId, newHardwareSetupInformation.DriverNumber, newHardwareSetupInformation.ProductId, ipHardware.Uri, ipHardware.LoginID, ipHardware.Password, newHardwareSetupInformation.DefinitionXml, newHardwareSetupInformation.SettingsXml, null, null);
		newLocals.NewHardwareName = ipHardware.Name;
		newLocals.NewAuditHardwareName = string.Format(CultureInfo.InvariantCulture, "New hardware {0} with address {1}", ipHardware.Name, ipHardware.Uri.DnsSafeHost);
		List<string> list = new List<string>();
		list.Add(newHardwareSetupInformation.SettingsXml);
		list.AddRange(newHardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => d.SettingsXml));
		newLocals.NewHardwareInfo.Definition = MainDefinition.LoadXml(newHardwareSetupInformation.DefinitionXml, list);
		IOrderedEnumerable<DeviceSetupSettings> orderedEnumerable = newHardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => new DeviceSetupSettings(newLocals.NewHardwareInfo, new DeviceHwid(d.DeviceHwid), d.DeviceType, d.SettingsXml, d.PresetsXml, d.Streams)).OrderBy((DeviceSetupSettings d) => d.DeviceHwid, new DeviceHwidComparer());
		foreach (DeviceSetupSettings item in orderedEnumerable)
		{
			newLocals.NewHardwareInfo.Devices.Add(item);
		}
		newLocals.NewHardwareInfo.Name = NamingScheme.FindHardwareName(HardwareNameTemplate.Default, newLocals.NewHardwareInfo);
	}

	public static bool IsDeviceSettingsEqual(DriverSettings oldDriverSettings, DriverSettings newDriverSettings)
	{
		try
		{
			Dictionary<string, string> oldSectionNamesDictionary = new Dictionary<string, string>(oldDriverSettings.GetSectionNames().ToDictionary(MaskDeviceIndex, (string x) => x));
			Dictionary<string, string> newSectionNamesDictionary = new Dictionary<string, string>(newDriverSettings.GetSectionNames().ToDictionary(MaskDeviceIndex, (string x) => x));
			if (oldSectionNamesDictionary.Count != newSectionNamesDictionary.Count)
			{
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>(oldSectionNamesDictionary.Keys);
			hashSet.SymmetricExceptWith(newSectionNamesDictionary.Keys);
			if (hashSet.Count > 0)
			{
				return false;
			}
			foreach (string sectionName in oldSectionNamesDictionary.Keys)
			{
				Dictionary<string, string> dictionary = new List<string>(oldDriverSettings.GetSectionSettingNames(oldSectionNamesDictionary[sectionName])).ToDictionary((string x) => x, (string x) => oldDriverSettings.Get(oldSectionNamesDictionary[sectionName], x));
				Dictionary<string, string> dictionary2 = new List<string>(newDriverSettings.GetSectionSettingNames(newSectionNamesDictionary[sectionName])).ToDictionary((string x) => x, (string x) => newDriverSettings.Get(newSectionNamesDictionary[sectionName], x));
				if (dictionary.Count != dictionary2.Count)
				{
					return false;
				}
				HashSet<string> hashSet2 = new HashSet<string>(dictionary.Keys);
				hashSet2.SymmetricExceptWith(dictionary2.Keys);
				if (hashSet2.Count > 0)
				{
					return false;
				}
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					string text = dictionary2[item.Key];
					if (!text.Equals(item.Value, StringComparison.Ordinal))
					{
						return false;
					}
				}
			}
			return true;
		}
		catch (ArgumentException)
		{
			return false;
		}
	}

	private static string MaskDeviceIndex(string x)
	{
		StringBuilder stringBuilder = new StringBuilder(x);
		int num = x.IndexOf('.');
		stringBuilder[num + 1] = 'x';
		return stringBuilder.ToString();
	}

	private static bool IsHardwareEventEqual(EventDefinitionCollection oldEvents, EventDefinitionCollection newEvents)
	{
		if (oldEvents.Count != newEvents.Count)
		{
			return false;
		}
		HashSet<Guid> hashSet = new HashSet<Guid>(oldEvents.ToList().ConvertAll((EventDefinition x) => x.ReferenceId));
		hashSet.SymmetricExceptWith(newEvents.ToList().ConvertAll((EventDefinition x) => x.ReferenceId));
		if (hashSet.Count > 0)
		{
			return false;
		}
		foreach (EventDefinition oldEvent in oldEvents)
		{
			EventDefinition newEventDefinition = newEvents[oldEvent.ReferenceId];
			if (!IsEventDefinitionEqual(oldEvent, newEventDefinition))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsEventDefinitionEqual(EventDefinition oldEventDefinition, EventDefinition newEventDefinition)
	{
		if (!oldEventDefinition.Base.name.Equals(newEventDefinition.Base.name, StringComparison.Ordinal))
		{
			return false;
		}
		string text = string.Empty;
		if (oldEventDefinition.Base.displayname != null)
		{
			text = oldEventDefinition.Base.displayname.Value;
		}
		string value = string.Empty;
		if (newEventDefinition.Base.displayname != null)
		{
			value = newEventDefinition.Base.displayname.Value;
		}
		if (!text.Equals(value, StringComparison.Ordinal))
		{
			return false;
		}
		string text2 = string.Empty;
		Dictionary<string, CapabilityDefinition> dictionary = new Dictionary<string, CapabilityDefinition>();
		if (oldEventDefinition.Capabilities != null)
		{
			text2 = oldEventDefinition.Capabilities.Hwid.ToString();
			dictionary = oldEventDefinition.Capabilities.ToDictionary((CapabilityDefinition x) => x.Name, (CapabilityDefinition x) => x);
		}
		string value2 = string.Empty;
		Dictionary<string, CapabilityDefinition> dictionary2 = new Dictionary<string, CapabilityDefinition>();
		if (newEventDefinition.Capabilities != null)
		{
			value2 = newEventDefinition.Capabilities.Hwid.ToString();
			dictionary2 = newEventDefinition.Capabilities.ToDictionary((CapabilityDefinition x) => x.Name, (CapabilityDefinition x) => x);
		}
		if (!text2.Equals(value2, StringComparison.Ordinal))
		{
			return false;
		}
		if (dictionary.Count != dictionary2.Count)
		{
			return false;
		}
		HashSet<string> hashSet = new HashSet<string>(dictionary.Keys);
		hashSet.SymmetricExceptWith(dictionary2.Keys);
		if (hashSet.Count > 0)
		{
			return false;
		}
		foreach (CapabilityDefinition value3 in dictionary.Values)
		{
			CapabilityDefinition capabilityDefinition = dictionary2[value3.Name];
			if (!value3.Hwid.Text.Equals(capabilityDefinition.Hwid.Text, StringComparison.Ordinal) || value3.ResetFeedRequired != capabilityDefinition.ResetFeedRequired)
			{
				return false;
			}
			if (!IsTypeDefinitionEqual(value3.TypeDefinition, capabilityDefinition.TypeDefinition))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsTypeDefinitionEqual(TypeDefinition oldTypeDefinition, TypeDefinition newTypeDefinition)
	{
		if (oldTypeDefinition.Access != newTypeDefinition.Access || oldTypeDefinition.Visible != newTypeDefinition.Visible || !oldTypeDefinition.Name.Equals(newTypeDefinition.Name, StringComparison.Ordinal) || oldTypeDefinition.ReferenceId != newTypeDefinition.ReferenceId || !oldTypeDefinition.DisplayNameResolved.Equals(newTypeDefinition.DisplayNameResolved, StringComparison.Ordinal))
		{
			return false;
		}
		datatype? datatype2 = null;
		try
		{
			datatype2 = oldTypeDefinition.ActiveDataType;
		}
		catch (ActiveTypeDefinitionMissingException)
		{
		}
		datatype? datatype3 = null;
		try
		{
			datatype3 = newTypeDefinition.ActiveDataType;
		}
		catch (ActiveTypeDefinitionMissingException)
		{
		}
		if (datatype2.HasValue != datatype3.HasValue)
		{
			return false;
		}
		if (datatype2.HasValue && datatype3.HasValue && datatype2.Value != datatype3.Value)
		{
			return false;
		}
		typename? typename2 = null;
		try
		{
			typename2 = oldTypeDefinition.ActiveTypeName;
		}
		catch (ActiveTypeDefinitionMissingException)
		{
		}
		typename? typename3 = null;
		try
		{
			typename3 = newTypeDefinition.ActiveTypeName;
		}
		catch (ActiveTypeDefinitionMissingException)
		{
		}
		if (typename2.HasValue != typename3.HasValue)
		{
			return false;
		}
		if (typename2.HasValue && typename3.HasValue && typename2.Value != typename3.Value)
		{
			return false;
		}
		return true;
	}

	private static string GetNewAuditDeviceName(DeviceHwid deviceHwid, DriverSettings driverSettings, UpdateHardwareLocals locals)
	{
		string valueByTypeId = driverSettings.GetValueByTypeId(deviceHwid.ToString(), TypeReferenceIds.DeviceName.ToString());
		return string.Format(CultureInfo.CurrentCulture, "{0} on new hardware {1} with address {2}", valueByTypeId, locals.NewHardwareName, locals.NewHardwareInfo.Address.DnsSafeHost);
	}
}
