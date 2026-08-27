using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;
using VideoOS.Server.Tasks;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.CommandHandler;

public class GetDevicePresetsTaskCommandHandler : ICommandHandler
{
	private static class ErrorCodes
	{
		public const int NullResponse = 1;
	}

	private IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	private IConfigurationDataLoader DataLoader => serverEnvironment.ConfigurationDataLoader;

	private ILocalCommandProcessor LocalCommandProcessor => serverEnvironment.LocalCommandProcessor;

	public GetDevicePresetsTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		serverEnvironment.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server\\CommandHandler\\GetDevicePresetsTaskCommandHandler.cs", 45);
		this.serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is GetDevicePresetsTaskCommand cmd))
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
		ProcessCommand(cmd, responseSink, callContextInfo);
	}

	private void ProcessCommand(GetDevicePresetsTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		ResponseBase responseBase = null;
		responseBase = ((cmd.SessionDataId != 0L) ? UpdateDevicePresets(cmd, callContextInfo) : CallRecordingServer(cmd, callContextInfo));
		if (responseBase is SynchronizeDevicePresetsResponse synchronizeDevicePresetsResponse && !string.IsNullOrEmpty(synchronizeDevicePresetsResponse.PresetsXml))
		{
			GetDevicePresetsTaskResult getDevicePresetsTaskResult = new GetDevicePresetsTaskResult
			{
				DeviceId = cmd.DeviceId.ToString(),
				PresetXml = synchronizeDevicePresetsResponse.PresetsXml,
				AffectedProfileIds = ""
			};
			getDevicePresetsTaskResult.SessionDataId = StoreInformation(cmd.DeviceId.ToString(), synchronizeDevicePresetsResponse.PresetsXml);
			responseSink.AddResponse(new TaskSuccessResponse(getDevicePresetsTaskResult.ToXml()));
		}
		else if (responseBase is TaskSuccessResponse)
		{
			responseSink.AddResponse(responseBase);
		}
		else
		{
			responseSink.AddResponse(ProcessErrors(responseBase));
		}
	}

	private ResponseBase ProcessErrors(ResponseBase response)
	{
		if (response is RecorderUnavailableResponse)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60143;
			taskError.ErrorText = VmoResources.VMO60143;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is RecorderNotSupportedResponse)
		{
			TaskError taskError2 = new TaskError();
			taskError2.ErrorCode = 60311;
			taskError2.ErrorText = string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60311, "GetDevicePresets");
			return new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2);
		}
		if (response is TimeoutResponse)
		{
			TaskError taskError3 = new TaskError();
			taskError3.ErrorCode = 60144;
			taskError3.ErrorText = VmoResources.VMO60144;
			return new TaskFailedResponse(isFatal: true, taskError3.ErrorText, taskError3);
		}
		if (response is TaskFailedResponse)
		{
			return response;
		}
		if (response is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to GetDevicePresets. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to GetDevicePresets. [Error: {Exception}]", exceptionResponse.Exception);
			}
			TaskError taskError4 = new TaskError();
			taskError4.ErrorCode = 67014;
			taskError4.ErrorText = VmoResources.VMO67014;
			return new TaskFailedResponse(isFatal: true, taskError4.ErrorText, taskError4);
		}
		TaskError taskError5 = new TaskError();
		taskError5.ErrorCode = 60000;
		taskError5.ErrorText = string.Format(CultureInfo.InvariantCulture, "GetDevicePresetsCommandHandler: Unknown Response type: {0}", response.GetType());
		return new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5);
	}

	private ResponseBase CallRecordingServer(GetDevicePresetsTaskCommand cmd, ICallContextInfo callContextInfo)
	{
		SynchronizeDevicePresetsCommand command = new SynchronizeDevicePresetsCommand(cmd.RecordingServerId, cmd.DeviceId);
		IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
		try
		{
			return responseCollector.GetNextResponse(TimeSpan.FromMinutes(5.0));
		}
		catch (TimeoutException)
		{
			return null;
		}
	}

	private ResponseBase UpdateDevicePresets(GetDevicePresetsTaskCommand cmd, ICallContextInfo callContextInfo)
	{
		GetDevicePresetsSessionData getDevicePresetsSessionData = GetDevicePresetsSessionData.Query(cmd.SessionDataId);
		if (getDevicePresetsSessionData == null)
		{
			return new TaskFailedResponse(isFatal: true, "Unable to lookup SessionData");
		}
		PtzPresetCollection ptzPresetCollection = new PtzPresetCollection();
		ptzPresetCollection.LoadXml(getDevicePresetsSessionData.PresetXml);
		try
		{
			using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(DataLoader));
			Device device = configurationManager.FindDeviceByDeviceId(cmd.DeviceId);
			PatrollingProfiles patrollingProfiles = configurationManager.PatrollingProfiles;
			string affectedProfileIds;
			string presetXml = ProcessChanges(device, patrollingProfiles, ptzPresetCollection, out affectedProfileIds);
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			if (changes != null)
			{
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
					Logger.LogWarning("Failed to update PTZ Presets from the camera. Response: {Response}", responseBase);
					TaskError taskError = new TaskError();
					taskError.ErrorCode = 60197;
					taskError.ErrorText = VmoResources.VMO60197;
					return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
				}
			}
			GetDevicePresetsTaskResult getDevicePresetsTaskResult = new GetDevicePresetsTaskResult
			{
				DeviceId = cmd.DeviceId.ToString(),
				PresetXml = presetXml,
				AffectedProfileIds = affectedProfileIds
			};
			return new TaskSuccessResponse(getDevicePresetsTaskResult.ToXml());
		}
		catch (Exception ex2)
		{
			return new TaskFailedResponse(isFatal: true, ex2.Message, ex2);
		}
	}

	private static string ProcessChanges(Device device, PatrollingProfiles profiles, PtzPresetCollection newPresets, out string affectedProfileIds)
	{
		List<Guid> list = new List<Guid>();
		for (int num = device.Presets.Count - 1; num >= 0; num--)
		{
			PtzPreset ptzPreset = device.Presets.ElementAt(num);
			string text = ptzPreset.Id.ToString();
			if (!newPresets.ContainsKey(text))
			{
				foreach (PatrollingProfile profile in profiles)
				{
					if (!(profile.IDDevice == device.ID))
					{
						continue;
					}
					PatrollingSchema patrollingSchema = new PatrollingSchema(profile.ID);
					patrollingSchema.LoadXml(profile.DefinitionAsXml);
					if (patrollingSchema.ContainsPreset(text))
					{
						if (!list.Contains(profile.ID))
						{
							list.Add(profile.ID);
						}
						patrollingSchema.RemovePreset(text);
						profile.DefinitionAsXml = patrollingSchema.ToXml();
					}
				}
				device.Presets.Remove(ptzPreset.Key);
			}
		}
		affectedProfileIds = string.Empty;
		foreach (Guid item in list)
		{
			affectedProfileIds = affectedProfileIds + item.ToString() + ",";
		}
		affectedProfileIds = affectedProfileIds.TrimEnd(',');
		foreach (PtzPreset newPreset in newPresets)
		{
			if (!device.Presets.ContainsKey(newPreset.Id.ToString()))
			{
				int num2 = 0;
				string newName = newPreset.Name;
				while (device.Presets.Any((PtzPreset p) => string.Equals(newName, p.Name, StringComparison.OrdinalIgnoreCase)) || device.Presets.Any((PtzPreset p) => string.Equals(newName, p.Key, StringComparison.OrdinalIgnoreCase)))
				{
					newName = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", newPreset.Name, ++num2);
				}
				if (!string.Equals(newName, newPreset.Name, StringComparison.OrdinalIgnoreCase))
				{
					newPreset.Name = newName;
				}
				device.Presets.Add(newPreset);
			}
		}
		return device.PresetsAsXml;
	}

	private static long StoreInformation(string deviceId, string presetXml)
	{
		GetDevicePresetsSessionData getDevicePresetsSessionData = new GetDevicePresetsSessionData();
		getDevicePresetsSessionData.DeviceId = deviceId;
		getDevicePresetsSessionData.PresetXml = presetXml;
		return getDevicePresetsSessionData.SessionDataId;
	}
}
