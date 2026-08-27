using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.AlarmClient;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.SDK.Platform;

public class MessageManager
{
	public static Guid StartRecordingGuid = new Guid("85867627-B287-4439-9E55-A63701E1715B");

	public static Guid StopRecordingGuid = new Guid("77B1E70D-BA8D-4bb8-9EE8-43B09746D82A");

	public static Guid RequestPlayAudioGuid = new Guid("7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C");

	private AlarmClientManager _alarmClientManager = new AlarmClientManager();

	public MessageManager()
	{
		EnvironmentManager.Instance.RegisterReceiver(TriggerMessage, new MessageIdFilter(MessageId.Control.TriggerCommand));
		EnvironmentManager.Instance.RegisterReceiver(StartRecordingHandler, new MessageIdFilter(MessageId.Control.StartRecordingCommand));
		EnvironmentManager.Instance.RegisterReceiver(StopRecordingHandler, new MessageIdFilter(MessageId.Control.StopRecordingCommand));
		EnvironmentManager.Instance.RegisterReceiver(PTZMoveCommandHandler, new MessageIdFilter("Control.PTZMoveCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZMoveStartCommandHandler, new MessageIdFilter("Control.PTZMoveStartCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZMoveStopCommandHandler, new MessageIdFilter("Control.PTZMoveStopCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZRectangleCommandHandler, new MessageIdFilter("Control.PTZRectangleCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZLensCommandHandler, new MessageIdFilter("Control.LensCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZMoveAbsoluteCommandHandler, new MessageIdFilter("Control.PTZMoveAbsoluteCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZGetAbsoluteRequestHandler, new MessageIdFilter("Control.PTZGetAbsoluteRequest"));
		EnvironmentManager.Instance.RegisterReceiver(PTZAUXCommandHandler, new MessageIdFilter("Control.PTZAUXCommand"));
		EnvironmentManager.Instance.RegisterReceiver(BaseEventHandler, new MessageIdFilter("Server.NewEventCommand"));
		EnvironmentManager.Instance.RegisterReceiver(BaseAlarmHandler, new MessageIdFilter("Server.NewAlarmCommand"));
		EnvironmentManager.Instance.RegisterReceiver(PTZCenterCommandHandler, new MessageIdFilter("Control.PTZCenterCommand"));
		EnvironmentManager.Instance.RegisterReceiver(DriverCommandHandler, new MessageIdFilter("Control.DriverCommand"));
		EnvironmentManager.Instance.RegisterReceiver(OutputActivateHandler, new MessageIdFilter("Control.OutputActivate"));
		EnvironmentManager.Instance.RegisterReceiver(OutputDeactivateHandler, new MessageIdFilter("Control.OutputDeactivate"));
		EnvironmentManager.Instance.RegisterReceiver(ChangeThemeHandler, new MessageIdFilter("SmartClient.ChangeThemeCommand"));
	}

	private object TriggerMessage(Message message, FQID dest, FQID source)
	{
		if (dest != null)
		{
			if (dest.Kind == Kind.Preset)
			{
				ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
				if (systemInfo != null && systemInfo.Cameras.ContainsKey(dest.ParentId))
				{
					ICamera camera = systemInfo.Cameras[dest.ParentId];
					if (camera != null)
					{
						string objectIdString = dest.ObjectIdString;
						camera.MoveToPreset(objectIdString);
					}
				}
				new TrackEventData(MessageId.Control.TriggerCommand, deprecated: false, "Kind.Preset").Send();
			}
			else if (dest.Kind == Kind.Matrix)
			{
				if (message.RelatedFQID != null)
				{
					((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId)?.LoginServer.MatrixMonitorActivateCamera(dest.ObjectId, message.RelatedFQID.ObjectId);
				}
				else
				{
					EnvironmentManager.Instance.Log(error: true, "SendMessage(TriggerMessage)", "  Kind.Matrix: Related FQID does not contain a CameraId (is null)", null);
				}
				new TrackEventData(MessageId.Control.TriggerCommand, deprecated: false, "Kind.Matrix").Send();
			}
			else if (dest.Kind == Kind.Output)
			{
				ISystemInfo systemInfo2 = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
				if (systemInfo2 != null)
				{
					if (systemInfo2.OutputDevices.ContainsKey(dest.ObjectId))
					{
						systemInfo2.OutputDevices[dest.ObjectId].OutputTrigger(0);
					}
					else
					{
						EnvironmentManager.Instance.Log(error: true, "Trigger Output", "Output ID not found", null);
					}
				}
				new TrackEventData(MessageId.Control.TriggerCommand, deprecated: false, "Kind.Output").Send();
			}
			else if (dest.Kind == Kind.TriggerEvent)
			{
				ISystemInfo systemInfo3 = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
				if (systemInfo3 != null)
				{
					UserDefinedEvent userDefinedEvent = null;
					if (systemInfo3.Events.ContainsKey(dest.ObjectId))
					{
						userDefinedEvent = systemInfo3.Events[dest.ObjectId];
					}
					else
					{
						foreach (IRecorder recorder in systemInfo3.Recorders)
						{
							if (recorder.Events.ContainsKey(dest.ObjectId))
							{
								userDefinedEvent = recorder.Events[dest.ObjectId];
								break;
							}
						}
					}
					if (userDefinedEvent != null)
					{
						List<FQID> list = message.Data as List<FQID>;
						if (message.RelatedFQID != null || list != null)
						{
							List<Guid> list2 = new List<Guid>();
							if (message.RelatedFQID != null)
							{
								list2.Add(message.RelatedFQID.ObjectId);
							}
							if (list != null)
							{
								foreach (FQID item in list)
								{
									list2.Add(item.ObjectId);
								}
							}
							userDefinedEvent.Trigger(list2);
						}
						else
						{
							userDefinedEvent.Trigger();
						}
					}
					else
					{
						EnvironmentManager.Instance.Log(error: true, "Trigger event", "event ID not found", null);
					}
				}
				new TrackEventData(MessageId.Control.TriggerCommand, deprecated: false, "Kind.TriggerEvent").Send();
			}
		}
		else
		{
			new TrackEventData(MessageId.Control.TriggerCommand, deprecated: false, (dest == null) ? "Destination is null" : $"Destination contains invalid guid: {dest.Kind}").Send();
		}
		return null;
	}

	private object OutputActivateHandler(Message message, FQID dest, FQID source)
	{
		if (dest != null && dest.Kind == Kind.Output)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				OutputDevice outputDevice = systemInfo.OutputDevices[dest.ObjectId];
				if (outputDevice != null)
				{
					outputDevice.OutputActivate();
				}
				else
				{
					EnvironmentManager.Instance.Log(error: true, "OutputActivate", "Output ID not found", null);
				}
			}
		}
		new TrackEventData("Control.OutputActivate").Send();
		return null;
	}

	private object OutputDeactivateHandler(Message message, FQID dest, FQID source)
	{
		if (dest != null && dest.Kind == Kind.Output)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				OutputDevice outputDevice = systemInfo.OutputDevices[dest.ObjectId];
				if (outputDevice != null)
				{
					outputDevice.OutputDeactivate();
				}
				else
				{
					EnvironmentManager.Instance.Log(error: true, "OutputDeactivate", "Output ID not found", null);
				}
			}
		}
		new TrackEventData("Control.OutputDeactivate").Send();
		return null;
	}

	private object StartRecordingHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId);
			if (systemInfo != null)
			{
				KeyValue[] array = new KeyValue[1]
				{
					new KeyValue()
				};
				array[0].Key = "TargetDevices";
				array[0].Value = BuildEventMetaData(new FQID[1] { dest }, null);
				ILoginServer loginServer = systemInfo.LoginServer;
				Guid startRecordingGuid = StartRecordingGuid;
				IKeyValue[] metadata = array;
				loginServer.EventTriggerWithMetadata(startRecordingGuid, metadata);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("StartRecording Message", exception);
		}
		new TrackEventData(MessageId.Control.StartRecordingCommand).Send();
		return null;
	}

	private object StopRecordingHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId);
			if (systemInfo != null)
			{
				KeyValue[] array = new KeyValue[1]
				{
					new KeyValue()
				};
				array[0].Key = "TargetDevices";
				array[0].Value = BuildEventMetaData(new FQID[1] { dest }, null);
				ILoginServer loginServer = systemInfo.LoginServer;
				Guid stopRecordingGuid = StopRecordingGuid;
				IKeyValue[] metadata = array;
				loginServer.EventTriggerWithMetadata(stopRecordingGuid, metadata);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("StopRecording Message", exception);
		}
		new TrackEventData(MessageId.Control.StopRecordingCommand).Send();
		return null;
	}

	private object PTZMoveStartCommandHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data != null && message.Data is PTZMoveStartCommandData)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				ICamera camera = systemInfo.Cameras[dest.ObjectId];
				if (camera != null)
				{
					PTZMoveStartCommandData pTZMoveStartCommandData = (PTZMoveStartCommandData)message.Data;
					PTZArgs pTZArgs = new PTZArgs();
					pTZArgs.Pan = 0.0;
					if (pTZMoveStartCommandData.Pan < 0.0)
					{
						pTZArgs.Pan = -1.0;
					}
					else if (pTZMoveStartCommandData.Pan > 0.0)
					{
						pTZArgs.Pan = 1.0;
					}
					pTZArgs.PanSpeed = Math.Abs(pTZMoveStartCommandData.Pan * pTZMoveStartCommandData.Speed);
					pTZArgs.Tilt = 0.0;
					if (pTZMoveStartCommandData.Tilt < 0.0)
					{
						pTZArgs.Tilt = -1.0;
					}
					else if (pTZMoveStartCommandData.Tilt > 0.0)
					{
						pTZArgs.Tilt = 1.0;
					}
					pTZArgs.TiltSpeed = Math.Abs(pTZMoveStartCommandData.Tilt * pTZMoveStartCommandData.Speed);
					pTZArgs.Zoom = 0.0;
					if (pTZMoveStartCommandData.Zoom < 0.0)
					{
						pTZArgs.Zoom = -1.0;
					}
					else if (pTZMoveStartCommandData.Zoom > 0.0)
					{
						pTZArgs.Zoom = 1.0;
					}
					pTZArgs.ZoomSpeed = Math.Abs(pTZMoveStartCommandData.Zoom * pTZMoveStartCommandData.Speed);
					camera.PtzMoveStart(pTZArgs);
				}
			}
		}
		else if (message.Data != null && message.Data.GetType() == typeof(PTZMoveStartCommandData2))
		{
			ISystemInfo systemInfo2 = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo2 != null)
			{
				ICamera camera2 = systemInfo2.Cameras[dest.ObjectId];
				if (camera2 != null)
				{
					PTZMoveStartCommandData2 pTZMoveStartCommandData2 = (PTZMoveStartCommandData2)message.Data;
					PTZArgs pTZArgs2 = new PTZArgs();
					pTZArgs2.Pan = pTZMoveStartCommandData2.Pan;
					pTZArgs2.PanSpeed = pTZMoveStartCommandData2.PanSpeed;
					if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("PtzReverseTiltForContinuesMoves", out var value) && value.ToUpper() == "TRUE")
					{
						pTZArgs2.Tilt = pTZMoveStartCommandData2.Tilt * -1.0;
					}
					else
					{
						pTZArgs2.Tilt = pTZMoveStartCommandData2.Tilt;
					}
					pTZArgs2.TiltSpeed = pTZMoveStartCommandData2.TiltSpeed;
					pTZArgs2.Zoom = pTZMoveStartCommandData2.Zoom;
					pTZArgs2.ZoomSpeed = pTZMoveStartCommandData2.ZoomSpeed;
					camera2.PtzMoveStart(pTZArgs2);
				}
			}
		}
		new TrackEventData("Control.PTZMoveStartCommand").Send(useSpamFilter: true);
		return null;
	}

	private object PTZRectangleCommandHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data != null)
		{
			ICamera camera = GetCamera(dest);
			if (camera != null)
			{
				PTZRectangleCommandData pTZRectangleCommandData = (PTZRectangleCommandData)message.Data;
				camera.PtzRectangle(pTZRectangleCommandData.RefWidth, pTZRectangleCommandData.RefHeight, pTZRectangleCommandData.Left, pTZRectangleCommandData.Top, pTZRectangleCommandData.Bottom, pTZRectangleCommandData.Right);
			}
		}
		new TrackEventData("Control.PTZRectangleCommand").Send();
		return null;
	}

	private object PTZMoveStopCommandHandler(Message message, FQID dest, FQID source)
	{
		((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true)?.Cameras[dest.ObjectId]?.PtzMoveStop();
		new TrackEventData("Control.PTZMoveStopCommand").Send();
		return null;
	}

	private object PTZMoveAbsoluteCommandHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data != null && message.Data is PTZMoveAbsoluteCommandData)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				ICamera camera = systemInfo.Cameras[dest.ObjectId];
				if (camera != null)
				{
					PTZMoveAbsoluteCommandData pTZMoveAbsoluteCommandData = (PTZMoveAbsoluteCommandData)message.Data;
					PTZArgs pTZArgs = new PTZArgs();
					pTZArgs.Pan = pTZMoveAbsoluteCommandData.Pan;
					pTZArgs.Tilt = pTZMoveAbsoluteCommandData.Tilt;
					pTZArgs.Zoom = pTZMoveAbsoluteCommandData.Zoom;
					if (!double.IsNaN(pTZMoveAbsoluteCommandData.Pan))
					{
						pTZArgs.PanSpeed = pTZMoveAbsoluteCommandData.Speed;
					}
					if (!double.IsNaN(pTZMoveAbsoluteCommandData.Tilt))
					{
						pTZArgs.TiltSpeed = pTZMoveAbsoluteCommandData.Speed;
					}
					if (!double.IsNaN(pTZMoveAbsoluteCommandData.Zoom))
					{
						pTZArgs.ZoomSpeed = pTZMoveAbsoluteCommandData.Speed;
					}
					camera.PtzMoveAbsolute(pTZArgs);
				}
			}
		}
		new TrackEventData("Control.PTZMoveAbsoluteCommand").Send(useSpamFilter: true);
		return null;
	}

	private object PTZGetAbsoluteRequestHandler(Message message, FQID dest, FQID source)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
		PTZGetAbsoluteRequestData pTZGetAbsoluteRequestData = default(PTZGetAbsoluteRequestData);
		if (systemInfo != null)
		{
			ICamera camera = systemInfo.Cameras[dest.ObjectId];
			if (camera != null)
			{
				PTZArgs pTZArgs = camera.PtzGetAbsolutePosition();
				if (pTZArgs != null)
				{
					pTZGetAbsoluteRequestData.Pan = pTZArgs.Pan;
					pTZGetAbsoluteRequestData.Tilt = pTZArgs.Tilt;
					pTZGetAbsoluteRequestData.Zoom = pTZArgs.Zoom;
				}
			}
		}
		return pTZGetAbsoluteRequestData;
	}

	private object PTZCenterCommandHandler(Message message, FQID dest, FQID source)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
		if (systemInfo != null)
		{
			ICamera camera = systemInfo.Cameras[dest.ObjectId];
			if (camera != null)
			{
				PTZCenterCommandData pTZCenterCommandData = (PTZCenterCommandData)message.Data;
				camera.PtzCenter(pTZCenterCommandData.RefWidth, pTZCenterCommandData.RefHeight, pTZCenterCommandData.CenterX, pTZCenterCommandData.CenterY, pTZCenterCommandData.Zoom);
			}
		}
		new TrackEventData("Control.PTZCenterCommand").Send();
		return null;
	}

	private object PTZAUXCommandHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data is PTZAUXCommandData)
		{
			PTZAUXCommandData pTZAUXCommandData = (PTZAUXCommandData)message.Data;
			int auxNumber = pTZAUXCommandData.AuxNumber;
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				ICamera camera = systemInfo.Cameras[dest.ObjectId];
				if (camera != null)
				{
					if (pTZAUXCommandData.On)
					{
						camera.PtzAuxOn(auxNumber);
					}
					else
					{
						camera.PtzAuxOff(auxNumber);
					}
				}
			}
		}
		new TrackEventData("Control.PTZAUXCommand").Send();
		return null;
	}

	private object PTZLensCommandHandler(Message message, FQID dest, FQID source)
	{
		string text = message.Data as string;
		if (text != null)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				ICamera camera = systemInfo.Cameras[dest.ObjectId];
				if (camera != null)
				{
					IRecorder recorder = camera.Recorder;
					PTZArgs pTZArgs = new PTZArgs();
					Guid objectId = dest.ObjectId;
					switch (text)
					{
					case "BrightnessDown":
						pTZArgs.SetMovement("brightness", -1.0);
						pTZArgs.SetSpeed("brightness", 1.0);
						recorder.CameraPtzMoveRelative(dest.ObjectId, pTZArgs);
						break;
					case "BrightnessReset":
						pTZArgs.SetMovement("brightness", 0.0);
						pTZArgs.SetSpeed("brightness", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "BrightnessUp":
						pTZArgs.SetMovement("brightness", 1.0);
						pTZArgs.SetSpeed("brightness", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "FocusAuto":
						pTZArgs.SetMovement("focus", 0.0);
						pTZArgs.SetSpeed("focus", 0.0);
						pTZArgs.SetAuto("focus", value: true);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "FocusDown":
						pTZArgs.SetMovement("focus", 1.0);
						pTZArgs.SetSpeed("focus", 1.0);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "FocusDownStart":
						pTZArgs.SetMovement("focus", 1.0);
						pTZArgs.SetSpeed("focus", 1.0);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "FocusStop":
						pTZArgs.SetMovement("focus", 0.0);
						pTZArgs.SetSpeed("focus", 0.0);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "FocusUp":
						pTZArgs.SetMovement("focus", -1.0);
						pTZArgs.SetSpeed("focus", 1.0);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "FocusUpStart":
						pTZArgs.SetMovement("focus", -1.0);
						pTZArgs.SetSpeed("focus", 1.0);
						recorder.CameraPtzMoveStart(objectId, pTZArgs, throwOnError: false);
						break;
					case "GainDown":
						pTZArgs.SetMovement("gain", -1.0);
						pTZArgs.SetSpeed("gain", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "GainReset":
						pTZArgs.SetMovement("gain", 0.0);
						pTZArgs.SetSpeed("gain", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "GainUp":
						pTZArgs.SetMovement("gain", 1.0);
						pTZArgs.SetSpeed("gain", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "IrisDown":
						pTZArgs.SetMovement("iris", -1.0);
						pTZArgs.SetSpeed("iris", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "IrisReset":
						pTZArgs.SetMovement("iris", 0.0);
						pTZArgs.SetSpeed("iris", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "IrisUp":
						pTZArgs.SetMovement("iris", 1.0);
						pTZArgs.SetSpeed("iris", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "ShutterDown":
						pTZArgs.SetMovement("shutter", -1.0);
						pTZArgs.SetSpeed("shutter", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "ShutterReset":
						pTZArgs.SetMovement("shutter", 0.0);
						pTZArgs.SetSpeed("shutter", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					case "ShutterUp":
						pTZArgs.SetMovement("shutter", 1.0);
						pTZArgs.SetSpeed("shutter", 1.0);
						recorder.CameraPtzMoveRelative(objectId, pTZArgs);
						break;
					}
				}
			}
		}
		new TrackEventData("Control.LensCommand", deprecated: false, text).Send();
		return null;
	}

	private object BaseEventHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			if (message.Data is BaseEvent baseEvent)
			{
				ServerId serverId = EnvironmentManager.Instance.MasterSite.ServerId;
				if (baseEvent.EventHeader != null && baseEvent.EventHeader.Source != null && baseEvent.EventHeader.Source.FQID != null)
				{
					serverId = baseEvent.EventHeader.Source.FQID.ServerId;
				}
				IAlarmClient obj = _alarmClientManager.GetAlarmClient(serverId) ?? throw new Exception("EventServer end point not found");
				MoveImageToSnapshot(baseEvent);
				obj.AddEvent(baseEvent);
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("BaseEventHandler", exception);
		}
		new TrackEventData("Server.NewEventCommand").Send();
		return null;
	}

	private object BaseAlarmHandler(Message message, FQID dest, FQID source)
	{
		try
		{
			ServerId serverId = EnvironmentManager.Instance.MasterSite.ServerId;
			Alarm alarm = message.Data as Alarm;
			if (alarm != null && alarm.EventHeader != null && alarm.EventHeader.Source != null && alarm.EventHeader.Source.FQID != null)
			{
				serverId = alarm.EventHeader.Source.FQID.ServerId;
			}
			else if (message.Data is BaseEvent { EventHeader: not null } baseEvent && baseEvent.EventHeader.Source != null && baseEvent.EventHeader.Source.FQID != null)
			{
				serverId = baseEvent.EventHeader.Source.FQID.ServerId;
			}
			IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(serverId);
			if (alarmClient == null)
			{
				throw new Exception("EventServer end point not found");
			}
			if (alarm != null)
			{
				alarmClient.Add(alarm);
			}
			else
			{
				if (!(message.Data is BaseEvent))
				{
					EnvironmentManager instance = EnvironmentManager.Instance;
					object[] args = new Message[1] { message };
					instance.Log(error: false, "EventCommand handling", "Unknown class in the Data field", args);
					throw new Exception("Message.Data field not a known class");
				}
				alarmClient.Add(ConvertEvent(message.Data as BaseEvent, newID: true));
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("BaseAlarmHandler", exception);
		}
		new TrackEventData("Server.NewAlarmCommand").Send();
		return null;
	}

	private static Alarm ConvertEvent(BaseEvent baseData, bool newID)
	{
		Alarm alarm = new Alarm();
		alarm.EventHeader = new EventHeader();
		alarm.EventHeader.Message = baseData.EventHeader.Message;
		alarm.EventHeader.Priority = baseData.EventHeader.Priority;
		alarm.EventHeader.Type = baseData.EventHeader.Type;
		alarm.EventHeader.Name = baseData.EventHeader.Name;
		alarm.EventHeader.Class = baseData.EventHeader.Class;
		alarm.EventHeader.Timestamp = baseData.EventHeader.Timestamp;
		if (newID)
		{
			alarm.EventHeader.ID = Guid.NewGuid();
		}
		else
		{
			alarm.EventHeader.ID = baseData.EventHeader.ID;
		}
		alarm.EventHeader.CustomTag = baseData.EventHeader.CustomTag;
		if (baseData.EventHeader.Source != null)
		{
			alarm.EventHeader.Source = new VideoOS.Platform.Data.EventSource
			{
				FQID = baseData.EventHeader.Source.FQID,
				Name = baseData.EventHeader.Source.Name
			};
		}
		if (baseData is EventData)
		{
			EventData eventData = baseData as EventData;
			if (eventData.EventSequence != null)
			{
				alarm.StartTime = eventData.EventSequence.StartDateTime;
				alarm.EndTime = eventData.EventSequence.EndDateTime;
				if (eventData.EventSequence.SnapshotImages != null)
				{
					alarm.SnapshotList = new SnapshotList();
					Image[] snapshotImages = eventData.EventSequence.SnapshotImages;
					foreach (Image image in snapshotImages)
					{
						Snapshot snapshot = new Snapshot();
						if (alarm.EventHeader.Source != null)
						{
							snapshot.FQID = alarm.EventHeader.Source.FQID;
						}
						snapshot.HasOverlay = false;
						snapshot.Height = (uint)image.Height;
						snapshot.Width = (uint)image.Width;
						snapshot.Image = ImageToBytes(image);
						snapshot.Path = "";
						snapshot.SizeInBytes = (uint)snapshot.Image.Length;
						snapshot.TimeOffset = 0;
						alarm.SnapshotList.Add(snapshot);
					}
				}
			}
		}
		else if (baseData is AnalyticsEvent)
		{
			AnalyticsEvent analyticsEvent = baseData as AnalyticsEvent;
			alarm.Count = analyticsEvent.Count;
			alarm.Description = analyticsEvent.Description;
			alarm.EndTime = analyticsEvent.EndTime;
			alarm.Location = analyticsEvent.Location;
			alarm.ObjectList = analyticsEvent.ObjectList;
			alarm.ReferenceList = analyticsEvent.ReferenceList;
			alarm.RuleList = analyticsEvent.RuleList;
			alarm.SnapshotList = analyticsEvent.SnapshotList;
			alarm.StartTime = analyticsEvent.StartTime;
			alarm.Vendor = analyticsEvent.Vendor;
		}
		alarm.State = 1;
		alarm.AssignedTo = "";
		return alarm;
	}

	private static byte[] ImageToBytes(Image image)
	{
		if (image == null)
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Jpeg);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}

	private object DriverCommandHandler(Message message, FQID destination, FQID sender)
	{
		if (message.Data is DriverCommandData && destination != null)
		{
			ISystemInfo systemInfo = ((SDKConfiguration)destination.ServerId.UserContext.Configuration).FindSystemInfo(destination.ServerId, populate: true);
			if (systemInfo != null)
			{
				IRecorder recorder = systemInfo.Recorders[destination.ParentId];
				Task.Run(async delegate
				{
					DriverCommandData data = message.Data as DriverCommandData;
					string text = await recorder.AsyncDriverCommandWithResponse(destination.ObjectId, data.Command, data.Parameter);
					bool flag = text != null && !text.StartsWith("ERROR");
					EnvironmentManager.Instance.PostMessage(new Message("Control.DriverResponse", new DriverResponseData
					{
						Response = (flag ? text : string.Empty),
						SyncObject = data.SyncObject,
						Success = flag,
						ErrorText = (flag ? string.Empty : text)
					}));
				});
			}
		}
		new TrackEventData("Control.DriverCommand").Send();
		return null;
	}

	private object ChangeThemeHandler(Message message, FQID destination, FQID sender)
	{
		if (message.Data is ThemeType)
		{
			ClientControl.Instance.Theme.ThemeType = (ThemeType)message.Data;
			EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ThemeChangedIndication, null, ClientControl.Instance.Theme.ThemeType));
		}
		new TrackEventData("SmartClient.ChangeThemeCommand").Send();
		return null;
	}

	public void Logout(ServerId serverId)
	{
		_alarmClientManager.Clear(serverId);
	}

	public void LogoutAll()
	{
		_alarmClientManager.Clear();
	}

	private object PTZMoveCommandHandler(Message message, FQID dest, FQID source)
	{
		if (message.Data != null)
		{
			string text = (string)message.Data;
			ISystemInfo systemInfo = ((SDKConfiguration)dest.ServerId.UserContext.Configuration).FindSystemInfo(dest.ServerId, populate: true);
			if (systemInfo != null)
			{
				ICamera camera = systemInfo.Cameras[dest.ObjectId];
				if (camera != null)
				{
					if (text == "Home")
					{
						camera.PtzMoveHome();
					}
					else
					{
						PtzMoveRelativeTypes moveRelative = PtzMoveRelativeTypes.Home;
						switch (text)
						{
						case "Down":
							moveRelative = PtzMoveRelativeTypes.Down;
							break;
						case "DownLeft":
							moveRelative = PtzMoveRelativeTypes.DownLeft;
							break;
						case "DownRight":
							moveRelative = PtzMoveRelativeTypes.DownRight;
							break;
						case "Home":
							moveRelative = PtzMoveRelativeTypes.Home;
							break;
						case "Left":
							moveRelative = PtzMoveRelativeTypes.Left;
							break;
						case "Right":
							moveRelative = PtzMoveRelativeTypes.Right;
							break;
						case "Up":
							moveRelative = PtzMoveRelativeTypes.Up;
							break;
						case "UpLeft":
							moveRelative = PtzMoveRelativeTypes.UpLeft;
							break;
						case "UpRight":
							moveRelative = PtzMoveRelativeTypes.UpRight;
							break;
						case "ZoomIn":
							moveRelative = PtzMoveRelativeTypes.ZoomIn;
							break;
						case "ZoomOut":
							moveRelative = PtzMoveRelativeTypes.ZoomOut;
							break;
						}
						camera.PtzMoveRelative(moveRelative);
					}
				}
			}
		}
		new TrackEventData("Control.PTZMoveCommand").Send(useSpamFilter: true);
		return null;
	}

	private static string BuildEventMetaData(FQID[] cameraFQID, FQID[] groupFQID)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("rootUnused");
			xmlTextWriter.WriteStartElement("targetDevices");
			xmlTextWriter.WriteStartElement("deviceIds");
			if (cameraFQID != null && cameraFQID.Length != 0)
			{
				FQID[] array = cameraFQID;
				foreach (FQID fQID in array)
				{
					xmlTextWriter.WriteElementString("deviceId", fQID.ObjectId.ToString().ToUpper());
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("deviceGroupIds");
			if (groupFQID != null && groupFQID.Length != 0)
			{
				FQID[] array = groupFQID;
				foreach (FQID fQID2 in array)
				{
					xmlTextWriter.WriteElementString("deviceGroupId", fQID2.ObjectId.ToString());
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
			xmlTextWriter.Flush();
			memoryStream.Position = 0L;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(memoryStream);
			return xmlDocument.DocumentElement.InnerXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("BuildEventMetadata", exception);
			return "";
		}
	}

	private static void MoveImageToSnapshot(BaseEvent baseEvent)
	{
		if (baseEvent is EventData eventData && eventData.EventSequence.SnapshotImages != null)
		{
			List<byte[]> list = new List<byte[]>(eventData.EventSequence.SnapshotJPEGs ?? Array.Empty<byte[]>());
			Image[] snapshotImages = eventData.EventSequence.SnapshotImages;
			foreach (Image image in snapshotImages)
			{
				list.Add(ImageToBytes(image));
			}
			eventData.EventSequence.SnapshotJPEGs = list.ToArray();
			eventData.EventSequence.SnapshotImages = Array.Empty<Image>();
		}
	}

	private static ICamera GetCamera(FQID destination)
	{
		return ((SDKConfiguration)destination.ServerId.UserContext.Configuration).FindSystemInfo(destination.ServerId, populate: true)?.Cameras[destination.ObjectId];
	}
}
