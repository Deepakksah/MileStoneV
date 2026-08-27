using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class SDKDeviceControl : ISDKDeviceControl
{
	public void SetAux(UserContext userContext, Guid cameraGuid, int auxNo, bool onOrOff)
	{
		CallRecorderWithExceptionHandling(userContext, cameraGuid, delegate(IRecorder recorder)
		{
			ICamera camera = recorder.Cameras[cameraGuid];
			if (onOrOff)
			{
				recorder.CameraPtzAuxOn(camera, auxNo, throwOnError: true);
			}
			else
			{
				recorder.CameraPtzAuxOff(camera, auxNo, throwOnError: true);
			}
		});
	}

	public void StartRecording(UserContext userContext, List<Guid> anyDeviceIds)
	{
		if (anyDeviceIds == null || anyDeviceIds.Count == 0)
		{
			throw new ArgumentException("anyDeviceIds cannot be null or empty.", "anyDeviceIds");
		}
		ISystemInfo obj = LookupSystemInfoFromAnyDevice(userContext, anyDeviceIds[0]) ?? throw new PathNotFoundMIPException($"Device with id {anyDeviceIds[0]} not found or is not a recording device");
		KeyValue[] array = new KeyValue[1]
		{
			new KeyValue()
		};
		array[0].Key = "TargetDevices";
		array[0].Value = BuildEventMetaData(anyDeviceIds, null);
		ILoginServer loginServer = obj.LoginServer;
		Guid startRecordingGuid = MessageManager.StartRecordingGuid;
		IKeyValue[] metadata = array;
		loginServer.EventTriggerWithMetadata(startRecordingGuid, metadata);
	}

	public void StopRecording(UserContext userContext, List<Guid> anyDeviceIds)
	{
		if (anyDeviceIds == null || anyDeviceIds.Count == 0)
		{
			throw new ArgumentException("anyDeviceIds cannot be null or empty.", "anyDeviceIds");
		}
		ISystemInfo obj = LookupSystemInfoFromAnyDevice(userContext, anyDeviceIds[0]) ?? throw new PathNotFoundMIPException($"Device with id {anyDeviceIds[0]} not found or is not a recording device");
		KeyValue[] array = new KeyValue[1]
		{
			new KeyValue()
		};
		array[0].Key = "TargetDevices";
		array[0].Value = BuildEventMetaData(anyDeviceIds, null);
		ILoginServer loginServer = obj.LoginServer;
		Guid stopRecordingGuid = MessageManager.StopRecordingGuid;
		IKeyValue[] metadata = array;
		loginServer.EventTriggerWithMetadata(stopRecordingGuid, metadata);
	}

	public void ReservePTZ(UserContext userContext, Guid cameraGuid, bool reserve)
	{
		CallRecorderWithExceptionHandling(userContext, cameraGuid, delegate(IRecorder recorder)
		{
			recorder.ReservePTZ(cameraGuid, reserve, throwOnError: true);
		});
	}

	public void ExtendPTZ(UserContext userContext, Guid cameraGuid)
	{
		CallRecorderWithExceptionHandling(userContext, cameraGuid, delegate(IRecorder recorder)
		{
			recorder.ExtendPTZ(cameraGuid, throwOnError: true);
		});
	}

	public void PTZMoveStart(UserContext userContext, Guid cameraGuid, PTZArgs ptzArgs)
	{
		CallRecorderWithExceptionHandling(userContext, cameraGuid, delegate(IRecorder recorder)
		{
			recorder.CameraPtzMoveStart(cameraGuid, ptzArgs, throwOnError: true);
		});
	}

	public PTZArgs PTZGetAbsolutePosition(UserContext userContext, Guid cameraGuid)
	{
		return CallRecorderWithExceptionHandlingResult(userContext, cameraGuid, (IRecorder recorder) => recorder.PtzGetAbsolutePosition(recorder.Cameras[cameraGuid], throwOnError: true));
	}

	public void PTZSetAbsolutePosition(UserContext userContext, Guid cameraId, PTZArgs ptzArgs)
	{
		CallRecorderWithExceptionHandling(userContext, cameraId, delegate(IRecorder recorder)
		{
			recorder.CameraPtzMoveActionAbsolute(recorder.Cameras[cameraId], ptzArgs, throwOnError: true);
		});
	}

	public PtzSessionInfo GetPTZSessionInfo(UserContext userContext, Guid cameraGuid)
	{
		return CallRecorderWithExceptionHandlingResult(userContext, cameraGuid, (IRecorder recorder) => recorder.GetPTZSessionInfo(cameraGuid, throwOnError: true));
	}

	public void GotoPreset(UserContext userContext, Guid cameraId, string presetId)
	{
		CallRecorderWithExceptionHandling(userContext, cameraId, delegate(IRecorder recorder)
		{
			recorder.MoveToPreset(cameraId.ToString(), presetId, throwOnError: true);
		});
	}

	public void PTZActivatePatrollingSchema(UserContext userContext, Guid cameraId, Guid patrollingSchemaId)
	{
		CallRecorderWithExceptionHandling(userContext, cameraId, delegate(IRecorder recorder)
		{
			recorder.PTZActivatePatrollingSchema(cameraId, patrollingSchemaId, throwOnError: true);
		});
	}

	public void PTZDeactivatePatrollingSchema(UserContext userContext, Guid cameraId, Guid patrollingSchemaId)
	{
		CallRecorderWithExceptionHandling(userContext, cameraId, delegate(IRecorder recorder)
		{
			recorder.PTZDeactivatePatrollingSchema(cameraId, patrollingSchemaId, throwOnError: true);
		});
	}

	public string DriverCommandWithResponse(UserContext userContext, Guid anyDeviceId, string command, string parameter)
	{
		CheckParams(userContext, anyDeviceId);
		if (string.IsNullOrEmpty(command))
		{
			throw new ArgumentException("Command cannot be null or empty.", "command");
		}
		return (LookupRecorderFromAnyDevice(userContext, anyDeviceId) ?? throw new PathNotFoundMIPException()).DriverCommandWithResponse(anyDeviceId, command, parameter, throwOnError: true);
	}

	public void OutputActivate(UserContext userContext, Guid outputId)
	{
		CheckParams(userContext, outputId);
		(LookupRecorderFromOutput(userContext, outputId) ?? throw new PathNotFoundMIPException()).OutputActivate(outputId.ToString(), throwOnError: true);
	}

	public void OutputDeactivate(UserContext userContext, Guid outputId)
	{
		CheckParams(userContext, outputId);
		(LookupRecorderFromOutput(userContext, outputId) ?? throw new PathNotFoundMIPException()).OutputDeactive(outputId.ToString(), throwOnError: true);
	}

	public IOState OutputGetState(UserContext userContext, Guid outputId)
	{
		CheckParams(userContext, outputId);
		return (LookupRecorderFromOutput(userContext, outputId) ?? throw new PathNotFoundMIPException()).OutputGetState(outputId.ToString(), throwOnError: true);
	}

	public void OutputTrigger(UserContext userContext, Guid outputId)
	{
		CheckParams(userContext, outputId);
		(LookupRecorderFromOutput(userContext, outputId) ?? throw new PathNotFoundMIPException()).OuputTrigger(outputId.ToString(), 0, throwOnError: true);
	}

	public Tuple<DateTime, string> GetVideoFrame(UserContext userContext, Guid cameraId, DateTime atTimeStamp)
	{
		CheckParams(userContext, cameraId);
		return (LookupRecorderFromAnyDevice(userContext, cameraId) ?? throw new PathNotFoundMIPException()).GetJpeg(cameraId, atTimeStamp);
	}

	private static void CallRecorderWithExceptionHandling(UserContext userContext, Guid cameraId, Action<IRecorder> action)
	{
		CheckParams(userContext, cameraId);
		IRecorder recorder = LookupRecorderFromCamera(userContext, cameraId);
		if (recorder == null)
		{
			throw new PathNotFoundMIPException();
		}
		try
		{
			action(recorder);
		}
		catch (Exception ex)
		{
			throw new MIPException("An error occurred while calling recorder: " + ex.Message);
		}
	}

	private static TResult CallRecorderWithExceptionHandlingResult<TResult>(UserContext userContext, Guid cameraId, Func<IRecorder, TResult> action)
	{
		CheckParams(userContext, cameraId);
		IRecorder recorder = LookupRecorderFromCamera(userContext, cameraId);
		if (recorder == null)
		{
			throw new PathNotFoundMIPException();
		}
		try
		{
			return action(recorder);
		}
		catch (Exception ex)
		{
			throw new MIPException("An error occurred while calling recorder: " + ex.Message);
		}
	}

	private static void CheckParams(UserContext userContext, Guid cameraGuid)
	{
		if (userContext == null)
		{
			throw new ArgumentNullException("userContext");
		}
		if (cameraGuid == Guid.Empty)
		{
			throw new ArgumentException("Camera GUID cannot be empty.", "cameraGuid");
		}
	}

	private static IRecorder LookupRecorderFromCamera(UserContext userContext, Guid cameraGuid)
	{
		foreach (ISystemInfo item in new List<ISystemInfo>(((SDKConfiguration)userContext.Configuration).SystemInfoList.Values))
		{
			if (item.Populate() && item.Cameras.ContainsKey(cameraGuid))
			{
				return item.Cameras[cameraGuid].Recorder;
			}
		}
		return null;
	}

	private static IRecorder LookupRecorderFromOutput(UserContext userContext, Guid outputGuid)
	{
		foreach (ISystemInfo item in new List<ISystemInfo>(((SDKConfiguration)userContext.Configuration).SystemInfoList.Values))
		{
			if (item.Populate() && item.OutputDevices.ContainsKey(outputGuid))
			{
				return item.OutputDevices[outputGuid].Recorder;
			}
		}
		return null;
	}

	private static ISystemInfo LookupSystemInfoFromAnyDevice(UserContext userContext, Guid id)
	{
		foreach (ISystemInfo item in new List<ISystemInfo>(((SDKConfiguration)userContext.Configuration).SystemInfoList.Values))
		{
			if (item.Populate() && (item.Cameras.ContainsKey(id) || item.Microphones.ContainsKey(id) || item.Speakers.ContainsKey(id) || item.OutputDevices.ContainsKey(id) || item.MetadataDevices.ContainsKey(id) || item.Events.ContainsKey(id)))
			{
				return item;
			}
		}
		return null;
	}

	private static IRecorder LookupRecorderFromAnyDevice(UserContext userContext, Guid id)
	{
		foreach (ISystemInfo item in new List<ISystemInfo>(((SDKConfiguration)userContext.Configuration).SystemInfoList.Values))
		{
			if (!item.Populate())
			{
				continue;
			}
			if (item.Cameras.ContainsKey(id))
			{
				ICamera camera = item.Cameras[id];
				if (camera != null)
				{
					return camera.Recorder;
				}
			}
			if (item.Microphones.ContainsKey(id))
			{
				IMicrophone microphone = item.Microphones[id];
				if (microphone != null)
				{
					return microphone.Recorder;
				}
			}
			if (item.Speakers.ContainsKey(id))
			{
				Speaker speaker = item.Speakers[id];
				if (speaker != null)
				{
					return speaker.Recorder;
				}
			}
			if (item.OutputDevices.ContainsKey(id))
			{
				OutputDevice outputDevice = item.OutputDevices[id];
				if (outputDevice != null)
				{
					return outputDevice.Recorder;
				}
			}
			if (item.MetadataDevices.TryGetValue(id, out var value))
			{
				return value.Recorder;
			}
			UserDefinedEvent userDefinedEvent = item.Events[id];
			if (userDefinedEvent != null)
			{
				return userDefinedEvent.Recorder;
			}
		}
		return null;
	}

	private static string BuildEventMetaData(List<Guid> deviceIds, List<Guid> groupIds)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("rootUnused");
			xmlTextWriter.WriteStartElement("targetDevices");
			xmlTextWriter.WriteStartElement("deviceIds");
			if (deviceIds != null && deviceIds.Count > 0)
			{
				foreach (Guid deviceId in deviceIds)
				{
					xmlTextWriter.WriteElementString("deviceId", deviceId.ToString().ToUpper(CultureInfo.InvariantCulture));
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("deviceGroupIds");
			if (groupIds != null && groupIds.Count > 0)
			{
				foreach (Guid groupId in groupIds)
				{
					xmlTextWriter.WriteElementString("deviceGroupId", groupId.ToString());
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			XmlDocument xmlDocument = new XmlDocument();
			using (XmlReader reader = XmlReader.Create(memoryStream, new XmlReaderSettings
			{
				DtdProcessing = DtdProcessing.Prohibit
			}))
			{
				xmlDocument.Load(reader);
			}
			return xmlDocument.DocumentElement.InnerXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("BuildEventMetadata", exception);
			return "";
		}
	}
}
