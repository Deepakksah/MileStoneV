using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Admin;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Resources;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public class ConfigurationAsync
{
	private readonly UserContext _userContext;

	public static Guid TransactPluginId = new Guid("71E86D1A-30EE-4D58-8AD6-2CF7245DDA9F");

	private object _lockObject = new object();

	public ConfigurationAsync(UserContext userContext)
	{
		_userContext = userContext;
	}

	public void RequestAsyncItems(AsyncItemsHandler handler, Control control, object callerreference)
	{
		ThreadPool.UnsafeQueueUserWorkItem(RespondAsyncItems, new ThreadPoolContainer(handler, control, callerreference));
	}

	private void RespondAsyncItems(object threadPoolContainer)
	{
		ThreadPoolContainer threadPoolContainer2 = (ThreadPoolContainer)threadPoolContainer;
		List<Item> list = new List<Item>();
		list = GetItems(Guid.Empty, ItemHierarchy.UserDefined);
		if (threadPoolContainer2.Control != null)
		{
			threadPoolContainer2.Control.BeginInvoke(threadPoolContainer2.AsyncItemsHandler, list, threadPoolContainer2.CallerReference);
		}
		else
		{
			threadPoolContainer2.AsyncItemsHandler(list, threadPoolContainer2.CallerReference);
		}
	}

	public void RequestAsyncLookupItem(FQID fqid, AsyncItemHandler handler, Control control, object callerreference)
	{
		ThreadPool.UnsafeQueueUserWorkItem(RespondAsyncLookupItem, new AsyncLokupItemContainer(handler, control, callerreference, fqid));
	}

	private void RespondAsyncLookupItem(object threadPoolContainer)
	{
		AsyncLokupItemContainer asyncLokupItemContainer = (AsyncLokupItemContainer)threadPoolContainer;
		Item item = LookupItem(asyncLokupItemContainer.FQID);
		if (asyncLokupItemContainer.Control != null)
		{
			asyncLokupItemContainer.Control.BeginInvoke(asyncLokupItemContainer.AsyncItemHandler, item, asyncLokupItemContainer.CallerReference);
		}
		else
		{
			asyncLokupItemContainer.AsyncItemHandler(item, asyncLokupItemContainer.CallerReference);
		}
	}

	private void EnsureAllConfigurationLoaded()
	{
		lock (_lockObject)
		{
			Exception lastException = null;
			int count = ((SDKConfiguration)_userContext.Configuration).SystemInfoList.Count;
			bool flag = ParallelHandler.PopulateUserContext(_userContext, out lastException);
			if (count != ((SDKConfiguration)_userContext.Configuration).SystemInfoList.Count)
			{
				ParallelHandler.PopulateUserContext(_userContext, out lastException);
			}
			if (flag && EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Standalone)
			{
				PluginManager.PluginInitCheckForStandalone();
			}
		}
	}

	private static bool HandleAsBuiltInKind(Guid kind)
	{
		if (!(kind == Kind.TransactSource))
		{
			return VideoOS.Platform.UI.Util.IsBuiltInKind(kind);
		}
		return true;
	}

	public List<Item> GetItems(Guid kind, ItemHierarchy itemHierarchy)
	{
		EnsureAllConfigurationLoaded();
		ItemNode itemNode = null;
		if (kind != Guid.Empty)
		{
			itemNode = PluginManager.GetItemNode(kind);
			while (itemNode != null && itemNode.ParentKind != Guid.Empty)
			{
				itemNode = PluginManager.GetItemNode(itemNode.ParentKind);
			}
		}
		List<ISystemInfo> list = new List<ISystemInfo>(((SDKConfiguration)_userContext.Configuration).SystemInfoList.Values);
		List<Item> list2 = new List<Item>();
		if (kind == Guid.Empty || HandleAsBuiltInKind(kind))
		{
			try
			{
				foreach (ISystemInfo item2 in list)
				{
					if (!item2.ServerId.ServerType.StartsWith("XPCO"))
					{
						list2.Add(new ServerFolderByTypeItem(item2, kind));
					}
					if (item2.ServerId.ServerType == "XPCO")
					{
						if (itemHierarchy != ItemHierarchy.UserDefined)
						{
							list2.Add(new ServerFolderByTypeItem(item2, kind));
						}
						if (itemHierarchy != ItemHierarchy.SystemDefined)
						{
							list2.Add(new ServerFolderByGroupItem(item2, kind));
						}
					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager instance = EnvironmentManager.Instance;
				string message = ex.Message;
				object[] args = new Exception[1] { ex };
				instance.Log(error: true, "Configuration.GetItems()", message, args);
			}
		}
		if ((kind == Guid.Empty || !HandleAsBuiltInKind(kind)) && EnvironmentManager.Instance.EnvironmentType != EnvironmentType.Administration)
		{
			foreach (PluginDefinition pluginDefinition in PluginManager.GetPluginDefinitions())
			{
				try
				{
					if (pluginDefinition.ItemNodes == null)
					{
						continue;
					}
					foreach (ItemNode itemNode2 in pluginDefinition.ItemNodes)
					{
						if ((kind == Guid.Empty || (itemNode != null && itemNode.ItemKind == itemNode2.ItemKind)) && itemNode2.Selectable)
						{
							Item item = itemNode2.ItemManager.GetItem(new FQID(_userContext.Configuration.ServerFQID.ServerId, Guid.Empty, itemNode2.ItemKind, FolderType.SystemDefined, itemNode2.ItemKind));
							if (item != null)
							{
								list2.Add(item);
							}
						}
					}
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler("ConfigurationAsync::BuildTopLevelItems LoadPlugin Loop", exception);
				}
			}
		}
		return list2;
	}

	public Item LookupItem(FQID fqid)
	{
		if (fqid == null)
		{
			return null;
		}
		EnsureAllConfigurationLoaded();
		ISystemInfo systemInfo = ((SDKConfiguration)_userContext.Configuration).FindSystemInfo(fqid.ServerId, populate: true);
		try
		{
			if (systemInfo != null && systemInfo.Populated)
			{
				Item item = LookupWithinSystemInfo(systemInfo, fqid);
				if (item != null)
				{
					return item;
				}
				if (fqid.Kind == Kind.TransactSource)
				{
					return Configuration.Instance.GetItemConfiguration(TransactPluginId, fqid.Kind, fqid.ObjectId);
				}
				foreach (PluginDefinition item3 in PlatformManager.PlatformContentTypes())
				{
					if (item3.ItemNodes == null)
					{
						continue;
					}
					foreach (ItemNode itemNode in item3.ItemNodes)
					{
						if (fqid.Kind == itemNode.ItemKind)
						{
							return itemNode.ItemManager.GetItem(fqid);
						}
						Item item2 = null;
						if (itemNode.ItemNodes != null)
						{
							item2 = SearchChildItemNodes(item3, itemNode.ItemNodes, fqid);
						}
						if (item2 != null)
						{
							return item2;
						}
					}
				}
				return null;
			}
			return LookupItem(fqid.ObjectId, fqid.Kind);
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "Configuration.GetItem()", message, args);
		}
		return null;
	}

	private static Item LookupWithinSystemInfo(ISystemInfo systemInfo, FQID fqid)
	{
		if (fqid.Kind == Kind.Server)
		{
			if (fqid.ObjectId != Guid.Empty && fqid.ObjectId != fqid.ServerId.Id)
			{
				return null;
			}
			if (fqid.ServerId.ServerType == "XPCORS")
			{
				if (systemInfo.Recorders.ContainsKey(fqid.ServerId.Id))
				{
					IRecorder recorder = systemInfo.Recorders[fqid.ServerId.Id];
					if (recorder != null)
					{
						return new RecorderFolderByTypeItem(systemInfo, recorder, Guid.Empty);
					}
				}
				return null;
			}
			if (fqid.FolderType == FolderType.SystemDefined || fqid.FolderType == FolderType.No)
			{
				return new ServerFolderByTypeItem(systemInfo, Guid.Empty);
			}
			if (systemInfo.Id == fqid.ServerId.Id)
			{
				return new ServerFolderByGroupItem(systemInfo, Guid.Empty);
			}
			return null;
		}
		if (fqid.FolderType == FolderType.SystemDefined && fqid.Kind != Kind.Server)
		{
			if (systemInfo.Recorders.ContainsKey(fqid.ServerId.Id))
			{
				IRecorder recorder2 = systemInfo.Recorders[fqid.ServerId.Id];
				if (fqid.Kind == Kind.Camera)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllCameras, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.Output)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllOutputs, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.InputEvent)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllInputs, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.Microphone)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllMicrophones, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.Metadata)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllMetadataDevices, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.Speaker)
				{
					return new AllRSFolderItem(recorder2.ServerId, string.Format(Strings.AllSpeakers, recorder2.Name), fqid.Kind, recorder2);
				}
				if (fqid.Kind == Kind.Hardware)
				{
					return new HardwareFolderItem(recorder2.ServerId, string.Format(Strings.AllHardware, recorder2.Name), recorder2);
				}
			}
			if (fqid.Kind == Kind.TriggerEvent)
			{
				return new AllFolderItem(systemInfo, string.Format(Strings.AllEvents, systemInfo.Name), fqid.Kind);
			}
			if (fqid.Kind == Kind.Matrix)
			{
				return new AllFolderItem(systemInfo, string.Format(Strings.AllMatrix, systemInfo.Name), fqid.Kind);
			}
			if (fqid.Kind == Kind.TransactSource)
			{
				return new AllFolderItem(systemInfo, string.Format(Strings.AllTransactionSources, systemInfo.Name), fqid.Kind);
			}
		}
		if (fqid.Kind != Kind.Server && fqid.ServerId.Id == systemInfo.Id)
		{
			if (fqid.Kind == Kind.Camera && fqid.ObjectId == Kind.Camera)
			{
				return ServerFolderByGroupItem.BuildCameraGroup(systemInfo);
			}
			if (fqid.Kind == Kind.InputEvent && fqid.ObjectId == Kind.InputEvent)
			{
				return ServerFolderByGroupItem.BuildInputGroup(systemInfo);
			}
			if (fqid.Kind == Kind.Output && fqid.ObjectId == Kind.Output)
			{
				return ServerFolderByGroupItem.BuildOutputGroup(systemInfo);
			}
			if (fqid.Kind == Kind.Microphone && fqid.ObjectId == Kind.Microphone)
			{
				return ServerFolderByGroupItem.BuildMicrophoneGroup(systemInfo);
			}
			if (fqid.Kind == Kind.Speaker && fqid.ObjectId == Kind.Speaker)
			{
				return ServerFolderByGroupItem.BuildSpeakerGroup(systemInfo);
			}
			if (fqid.Kind == Kind.Metadata && fqid.ObjectId == Kind.Metadata)
			{
				return ServerFolderByGroupItem.BuildMetadataDeviceGroup(systemInfo);
			}
			if (fqid.Kind == Kind.TriggerEvent && fqid.ObjectId == Kind.TriggerEvent)
			{
				return ServerFolderByGroupItem.BuildEventGroup(systemInfo);
			}
			if (systemInfo.ConfigurationInfo != null)
			{
				Item item = null;
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.Camera)) && systemInfo.ConfigurationInfo.CameraGroups != null)
				{
					item = FindCameraGroup(systemInfo, systemInfo.ConfigurationInfo.CameraGroups, Kind.Camera, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.InputEvent)) && systemInfo.ConfigurationInfo.InputGroups != null)
				{
					item = FindInputGroup(systemInfo, systemInfo.ConfigurationInfo.InputGroups, Kind.InputEvent, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.Output)) && systemInfo.ConfigurationInfo.OutputGroups != null)
				{
					item = FindOutputGroup(systemInfo, systemInfo.ConfigurationInfo.OutputGroups, Kind.Output, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.TriggerEvent)) && systemInfo.ConfigurationInfo.EventTypeGroups != null)
				{
					item = FindEventGroup(systemInfo, systemInfo.ConfigurationInfo.EventTypeGroups, Kind.TriggerEvent, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.Microphone)) && systemInfo.ConfigurationInfo.MicrophoneGroups != null)
				{
					item = FindMicrophoneGroup(systemInfo, systemInfo.ConfigurationInfo.MicrophoneGroups, Kind.Microphone, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.Speaker)) && systemInfo.ConfigurationInfo.SpeakerGroups != null)
				{
					item = FindSpeakerGroup(systemInfo, systemInfo.ConfigurationInfo.SpeakerGroups, Kind.Speaker, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
				if ((fqid.Kind == Kind.Folder || (fqid.FolderType != FolderType.No && fqid.Kind == Kind.Metadata)) && systemInfo.ConfigurationInfo.MetadataDeviceGroups != null)
				{
					item = FindMetadataDeviceGroup(systemInfo, systemInfo.ConfigurationInfo.MetadataDeviceGroups, Kind.Metadata, fqid.ObjectId);
				}
				if (item != null)
				{
					return item;
				}
			}
		}
		if (fqid.ObjectId == Guid.Empty && fqid.FolderType != FolderType.No)
		{
			return null;
		}
		if (fqid.Kind == Kind.Camera && systemInfo.Cameras.ContainsKey(fqid.ObjectId))
		{
			ICamera camera = systemInfo.Cameras[fqid.ObjectId];
			return new CameraItem(camera.Recorder.ServerId, camera);
		}
		if (fqid.Kind == Kind.Preset)
		{
			if (fqid.ParentId != Guid.Empty && systemInfo.Cameras.ContainsKey(fqid.ParentId))
			{
				ICamera camera2 = systemInfo.Cameras[fqid.ParentId];
				foreach (Preset preset in camera2.Presets)
				{
					if (preset.Name.Equals(fqid.ObjectIdString))
					{
						return new PresetItem(camera2.Recorder.ServerId, camera2.Guid, preset.Guid, preset.Name);
					}
				}
			}
			else
			{
				foreach (ICamera camera3 in systemInfo.Cameras)
				{
					foreach (Preset preset2 in camera3.Presets)
					{
						if (preset2.Guid == fqid.ObjectId)
						{
							return new PresetItem(camera3.Recorder.ServerId, camera3.Guid, preset2.Guid, preset2.Name);
						}
					}
				}
			}
		}
		if (fqid.Kind == Kind.Microphone && systemInfo.Microphones.ContainsKey(fqid.ObjectId))
		{
			IMicrophone microphone = systemInfo.Microphones[fqid.ObjectId];
			return new DeviceItem(microphone.Recorder.ServerId, microphone.Recorder.Guid, microphone.Guid, microphone.Name, Kind.Microphone, null, null, null, microphone);
		}
		if (fqid.Kind == Kind.Metadata && systemInfo.MetadataDevices.TryGetValue(fqid.ObjectId, out var value))
		{
			return new DeviceItem(value.Recorder.ServerId, value.Recorder.Guid, value.Guid, value.Name, Kind.Metadata, null, null, BuildMetadataProperties(value), value);
		}
		if (fqid.Kind == Kind.TransactSource)
		{
			return Configuration.Instance.GetItemConfiguration(TransactPluginId, fqid.Kind, fqid.ObjectId);
		}
		if (fqid.Kind == Kind.InputEvent)
		{
			if (systemInfo.Events.ContainsKey(fqid.ObjectId))
			{
				UserDefinedEvent userDefinedEvent = systemInfo.Events[fqid.ObjectId];
				ServerId serverId = systemInfo.ServerId;
				IRecorder recorder3 = userDefinedEvent.Recorder;
				if (recorder3 != null && userDefinedEvent.EventType != "Generic")
				{
					serverId = recorder3.ServerId;
				}
				DeviceItem deviceItem = new DeviceItem(serverId, serverId.Id, fqid.ObjectId, userDefinedEvent.Name, Kind.InputEvent, null, null);
				deviceItem.Properties["EventType"] = userDefinedEvent.EventType;
				deviceItem.Properties["SubType"] = userDefinedEvent.EventType;
				return deviceItem;
			}
			if (systemInfo.Events.InputEvents.TryGetValue(fqid.ObjectId, out var value2))
			{
				IRecorder recorder4 = systemInfo.Recorders[value2.RecorderGuid];
				if (recorder4 != null)
				{
					ServerId serverId2 = recorder4.ServerId;
					DeviceItem deviceItem2 = new DeviceItem(serverId2, serverId2.Id, fqid.ObjectId, value2.Name, Kind.InputEvent, null, null);
					deviceItem2.Properties["EventType"] = "Input";
					deviceItem2.Properties["SubType"] = "Input";
					return deviceItem2;
				}
			}
			if (systemInfo.ConfigurationInfo != null)
			{
				ISystemEventTypeInfo[] systemEventTypes = systemInfo.ConfigurationInfo.SystemEventTypes;
				foreach (ISystemEventTypeInfo systemEventTypeInfo in systemEventTypes)
				{
					if (systemEventTypeInfo.EventSource == EventSource.External && systemEventTypeInfo.EventTypeId == fqid.ObjectId)
					{
						ServerId serverId3 = systemInfo.ServerId;
						DeviceItem deviceItem3 = new DeviceItem(serverId3, serverId3.Id, fqid.ObjectId, systemEventTypeInfo.Name, Kind.InputEvent, null, null);
						deviceItem3.Properties["EventType"] = "Generic";
						deviceItem3.Properties["SubType"] = "Generic";
						return deviceItem3;
					}
				}
			}
		}
		if (fqid.Kind == Kind.Output && systemInfo.OutputDevices.ContainsKey(fqid.ObjectId))
		{
			OutputDevice outputDevice = systemInfo.OutputDevices[fqid.ObjectId];
			return new DeviceItem(outputDevice.Recorder.ServerId, outputDevice.Recorder.Guid, outputDevice.Guid, outputDevice.Name, Kind.Output, null, null);
		}
		if (fqid.Kind == Kind.Speaker && systemInfo.Speakers.ContainsKey(fqid.ObjectId))
		{
			Speaker speaker = systemInfo.Speakers[fqid.ObjectId];
			return new DeviceItem(speaker.Recorder.ServerId, speaker.Recorder.Guid, speaker.Guid, speaker.Name, Kind.Speaker, null, null, null, speaker);
		}
		if (fqid.Kind == Kind.TriggerEvent)
		{
			if (systemInfo.Events.ContainsKey(fqid.ObjectId))
			{
				UserDefinedEvent userDefinedEvent2 = systemInfo.Events[fqid.ObjectId];
				ServerId serverId4 = systemInfo.ServerId;
				IRecorder recorder5 = null;
				recorder5 = userDefinedEvent2.Recorder;
				if (recorder5 != null)
				{
					serverId4 = recorder5.ServerId;
				}
				DeviceItem deviceItem4 = new DeviceItem(serverId4, serverId4.Id, fqid.ObjectId, userDefinedEvent2.Name, Kind.TriggerEvent, null, null);
				deviceItem4.Properties["EventType"] = userDefinedEvent2.EventType;
				deviceItem4.Properties["SubType"] = userDefinedEvent2.EventType;
				return deviceItem4;
			}
			foreach (IRecorder recorder6 in systemInfo.Recorders)
			{
				if (recorder6.Events.ContainsKey(fqid.ObjectId))
				{
					UserDefinedEvent userDefinedEvent3 = recorder6.Events[fqid.ObjectId];
					ServerId serverId5 = recorder6.ServerId;
					DeviceItem deviceItem5 = new DeviceItem(serverId5, serverId5.Id, fqid.ObjectId, userDefinedEvent3.Name, Kind.TriggerEvent, null, null);
					deviceItem5.Properties["EventType"] = userDefinedEvent3.EventType;
					deviceItem5.Properties["SubType"] = userDefinedEvent3.EventType;
					return deviceItem5;
				}
			}
		}
		if (fqid.Kind == Kind.Matrix && systemInfo.MatrixMonitors.ContainsKey(fqid.ObjectId))
		{
			MatrixMonitor matrixMonitor = systemInfo.MatrixMonitors[fqid.ObjectId];
			ServerId serverId6 = systemInfo.ServerId;
			return new DeviceItem(serverId6, serverId6.Id, fqid.ObjectId, matrixMonitor.Name, Kind.Matrix, null, null);
		}
		if (fqid.Kind == Kind.Hardware)
		{
			foreach (IRecorder recorder7 in systemInfo.Recorders)
			{
				if (recorder7.Hardware != null && recorder7.Hardware.TryGetValue(fqid.ObjectId, out var value3))
				{
					return new HardwareItem(new RecorderFolderByTypeItem(systemInfo, recorder7, Guid.Empty), recorder7, value3);
				}
			}
		}
		return null;
	}

	public Item LookupItem(Guid objectId, Guid kind)
	{
		if (objectId == Guid.Empty)
		{
			return null;
		}
		EnsureAllConfigurationLoaded();
		if (kind == Kind.Server)
		{
			foreach (ISystemInfo item2 in new List<ISystemInfo>(((SDKConfiguration)_userContext.Configuration).SystemInfoList.Values))
			{
				if (item2.Populate())
				{
					bool flag = kind == Kind.TriggerEvent || kind == Kind.Matrix;
					if (flag && item2.Recorders.ContainsKey(objectId) && item2.Recorders[objectId] != null)
					{
						return new RecorderFolderByTypeItem(item2, item2.Recorders[objectId], Guid.Empty);
					}
					if (item2.Id == objectId)
					{
						return new ServerFolderByTypeItem(item2, Guid.Empty);
					}
					if (!flag && item2.Recorders.ContainsKey(objectId) && item2.Recorders[objectId] != null)
					{
						return new RecorderFolderByTypeItem(item2, item2.Recorders[objectId], Guid.Empty);
					}
				}
			}
			return null;
		}
		foreach (ISystemInfo item3 in new List<ISystemInfo>(((SDKConfiguration)_userContext.Configuration).SystemInfoList.Values))
		{
			if (item3.Populate())
			{
				Item item = LookupItem(item3.ServerId, objectId, kind);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	public Item LookupItem(ServerId serverId, Guid objectId, Guid kind)
	{
		if (objectId == Guid.Empty)
		{
			return null;
		}
		EnsureAllConfigurationLoaded();
		ISystemInfo systemInfo = ((SDKConfiguration)_userContext.Configuration).FindSystemInfo(serverId, populate: true);
		try
		{
			if (systemInfo != null && systemInfo.Populated)
			{
				FQID fQID = ((objectId == kind) ? new FQID(serverId, serverId.Id, objectId, FolderType.SystemDefined, kind) : ((!(kind == Kind.Preset)) ? new FQID(serverId, serverId.Id, objectId, FolderType.No, kind) : new FQID(serverId, Guid.Empty, objectId, FolderType.No, kind)));
				Item item = LookupWithinSystemInfo(systemInfo, fQID);
				if (item != null)
				{
					return item;
				}
				fQID.FolderType = FolderType.UserDefined;
				item = LookupWithinSystemInfo(systemInfo, fQID);
				if (item != null)
				{
					return item;
				}
			}
			if (kind == Kind.TransactSource)
			{
				return Configuration.Instance.GetItemConfiguration(TransactPluginId, kind, objectId);
			}
			FQID fqid = new FQID(serverId, serverId.Id, objectId, FolderType.No, kind);
			foreach (PluginDefinition item3 in PlatformManager.PlatformContentTypes())
			{
				if (item3.ItemNodes == null)
				{
					continue;
				}
				foreach (ItemNode itemNode in item3.ItemNodes)
				{
					if (kind == itemNode.ItemKind)
					{
						return itemNode.ItemManager.GetItem(fqid);
					}
					Item item2 = null;
					if (itemNode.ItemNodes != null)
					{
						item2 = SearchChildItemNodes(item3, itemNode.ItemNodes, fqid);
					}
					if (item2 != null)
					{
						return item2;
					}
				}
			}
			return null;
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "Configuration.GetItem", message, args);
			EnvironmentManager.Instance.Log(error: true, "ConfigurationAsync.LookupItem", ex.ToString());
		}
		return null;
	}

	private static Item FindCameraGroup(ISystemInfo systemInfo, ICameraGroupInfo[] cameraGroups, Guid parentId, Guid groupId)
	{
		foreach (ICameraGroupInfo cameraGroupInfo in cameraGroups)
		{
			if (cameraGroupInfo.GroupId == groupId)
			{
				return new CameraFolderItem(systemInfo, parentId, cameraGroupInfo, cameraGroupInfo.Name);
			}
			if (cameraGroupInfo.CameraGroups != null && cameraGroupInfo.CameraGroups.Length != 0)
			{
				Item item = FindCameraGroup(systemInfo, cameraGroupInfo.CameraGroups, cameraGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindInputGroup(ISystemInfo systemInfo, IInputGroupInfo[] groups, Guid parentId, Guid groupId)
	{
		foreach (IInputGroupInfo inputGroupInfo in groups)
		{
			if (inputGroupInfo.GroupId == groupId)
			{
				return new InputFolderItem(systemInfo, parentId, inputGroupInfo, inputGroupInfo.Name);
			}
			if (inputGroupInfo.InputGroups != null && inputGroupInfo.InputGroups.Length != 0)
			{
				Item item = FindInputGroup(systemInfo, inputGroupInfo.InputGroups, inputGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindOutputGroup(ISystemInfo systemInfo, IOutputGroupInfo[] groups, Guid parentId, Guid groupId)
	{
		foreach (IOutputGroupInfo outputGroupInfo in groups)
		{
			if (outputGroupInfo.GroupId == groupId)
			{
				return new OutputFolderItem(systemInfo, parentId, outputGroupInfo, outputGroupInfo.Name);
			}
			if (outputGroupInfo.OutputGroups != null && outputGroupInfo.OutputGroups.Length != 0)
			{
				Item item = FindOutputGroup(systemInfo, outputGroupInfo.OutputGroups, outputGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindMicrophoneGroup(ISystemInfo systemInfo, IMicrophoneGroupInfo[] groups, Guid parentId, Guid groupId)
	{
		foreach (IMicrophoneGroupInfo microphoneGroupInfo in groups)
		{
			if (microphoneGroupInfo.GroupId == groupId)
			{
				return new MicrophoneFolderItem(systemInfo, parentId, microphoneGroupInfo, microphoneGroupInfo.Name);
			}
			if (microphoneGroupInfo.MicrophoneGroups != null && microphoneGroupInfo.MicrophoneGroups.Length != 0)
			{
				Item item = FindMicrophoneGroup(systemInfo, microphoneGroupInfo.MicrophoneGroups, microphoneGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindSpeakerGroup(ISystemInfo systemInfo, ISpeakerGroupInfo[] groups, Guid parentId, Guid groupId)
	{
		foreach (ISpeakerGroupInfo speakerGroupInfo in groups)
		{
			if (speakerGroupInfo.GroupId == groupId)
			{
				return new SpeakerFolderItem(systemInfo, parentId, speakerGroupInfo, speakerGroupInfo.Name);
			}
			if (speakerGroupInfo.SpeakerGroups != null && speakerGroupInfo.SpeakerGroups.Length != 0)
			{
				Item item = FindSpeakerGroup(systemInfo, speakerGroupInfo.SpeakerGroups, speakerGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindMetadataDeviceGroup(ISystemInfo systemInfo, IEnumerable<IMetadataDeviceGroupInfo> groups, Guid parentId, Guid groupId)
	{
		if (groups == null)
		{
			return null;
		}
		foreach (IMetadataDeviceGroupInfo group in groups)
		{
			if (group.GroupId == groupId)
			{
				return new MetadataDeviceFolderItem(systemInfo, parentId, group, group.Name);
			}
			if (group.MetadataDeviceGroups != null && group.MetadataDeviceGroups.Length != 0)
			{
				Item item = FindMetadataDeviceGroup(systemInfo, group.MetadataDeviceGroups, group.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	private static Item FindEventGroup(ISystemInfo systemInfo, IEventTypeGroupInfo[] groups, Guid parentId, Guid groupId)
	{
		foreach (IEventTypeGroupInfo eventTypeGroupInfo in groups)
		{
			if (eventTypeGroupInfo.GroupId == groupId)
			{
				return new EventTypeFolderItem(systemInfo, parentId, eventTypeGroupInfo, eventTypeGroupInfo.Name);
			}
			if (eventTypeGroupInfo.EventTypeGroups != null && eventTypeGroupInfo.EventTypeGroups.Length != 0)
			{
				Item item = FindEventGroup(systemInfo, eventTypeGroupInfo.EventTypeGroups, eventTypeGroupInfo.GroupId, groupId);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}

	public List<Item> GetAllOfKind(Guid kind, ItemHierarchy itemHierarchy)
	{
		return GetItems(kind, itemHierarchy);
	}

	private static Item SearchChildItemNodes(PluginDefinition plugin, List<ItemNode> itemNodes, FQID fqid)
	{
		foreach (ItemNode itemNode in itemNodes)
		{
			if (fqid.Kind == itemNode.ItemKind)
			{
				return itemNode.ItemManager.GetItem(fqid);
			}
			Item item = null;
			if (itemNode.ItemNodes != null)
			{
				item = SearchChildItemNodes(plugin, itemNode.ItemNodes, fqid);
			}
			if (item != null)
			{
				return item;
			}
		}
		return null;
	}

	public XmlNode GetConfiguration(ServerId serverId, Guid platformDefinitionId, out int dataVersion, bool privateUser = false)
	{
		try
		{
			dataVersion = 0;
			ISystemInfo systemInfo = GetSystemInfo(serverId);
			if (systemInfo == null)
			{
				return null;
			}
			ICustomSetting customSetting = (privateUser ? systemInfo.LoginServer.GetCustomSettingDataUser(platformDefinitionId) : systemInfo.LoginServer.GetCustomSettingDataGlobal(platformDefinitionId));
			if (customSetting == null)
			{
				EnvironmentManager.Instance.Log(error: true, "GetConfiguration", string.Format(CultureInfo.InvariantCulture, "CustomSetting not found for the given platformDefinitionId: {0}", platformDefinitionId));
				return null;
			}
			dataVersion = customSetting.CustomSettingData.DataVersion;
			return customSetting.CustomSettingData.CustomSettingDataXml;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "GetConfiguration", ex.Message, null);
			throw;
		}
	}

	public void SaveCustomSetting(ServerId serverId, Guid id, int version, XmlNode xml, bool global)
	{
		try
		{
			GetSystemInfo(serverId)?.LoginServer.SetCustomSettingGlobal(id, xml, version, global);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "SaveCustomSetting", ex.Message, null);
			throw;
		}
	}

	private ISystemInfo GetSystemInfo(ServerId serverId)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)_userContext.Configuration).FindSystemInfo(serverId);
		if (systemInfo == null)
		{
			EnvironmentManager.Instance.Log(error: true, "GetSystemInfo", string.Format(CultureInfo.InvariantCulture, "SystemInfo not found for the given serverId: {0}", serverId.Id));
			return null;
		}
		if (systemInfo.LoginServer == null)
		{
			EnvironmentManager.Instance.Log(error: true, "GetSystemInfo", "Please login first!");
			return null;
		}
		if (serverId.Id != systemInfo.Id)
		{
			EnvironmentManager.Instance.Log(error: true, "GetSystemInfo", "Unable to get item configuration from sub-site, as the site has not been added specifically.");
			return null;
		}
		return systemInfo;
	}

	public static List<Item> BuildPresets(ServerId recorderServerId, ICamera camera, ConfigItem cameraItem)
	{
		if (camera.Presets != null)
		{
			List<Item> list = new List<Item>();
			{
				foreach (Preset preset in camera.Presets)
				{
					list.Add(new PresetItem(cameraItem, preset.Guid, preset.Name));
				}
				return list;
			}
		}
		return null;
	}

	public static List<Item> BuildRelated(ICamera camera)
	{
		List<Item> list = new List<Item>();
		if (camera != null)
		{
			try
			{
				IMicrophone microphone = camera.Microphone;
				if (microphone != null)
				{
					list.Add(new DeviceItem(microphone.Recorder.ServerId, microphone.Recorder.Guid, microphone.Guid, microphone.Name, Kind.Microphone, null, null, null, microphone));
				}
				Speaker speaker = camera.Speaker;
				if (speaker != null)
				{
					list.Add(new DeviceItem(speaker.Recorder.ServerId, speaker.Recorder.Guid, speaker.Guid, speaker.Name, Kind.Speaker, null, null, null, speaker));
				}
				IEnumerable<DeviceItem> collection = camera.RelatedMetadataDevices.Select((IMetadataDevice metadataDevice) => new DeviceItem(metadataDevice.Recorder.ServerId, metadataDevice.Recorder.Guid, metadataDevice.Guid, metadataDevice.Name, Kind.Metadata, null, null, BuildMetadataProperties(metadataDevice), metadataDevice));
				list.AddRange(collection);
				OutputDeviceCollection outputs = camera.Outputs;
				if (outputs != null)
				{
					foreach (OutputDevice item in outputs)
					{
						list.Add(new DeviceItem(item.Recorder.ServerId, item.Recorder.Guid, item.Guid, item.Name, Kind.Output, null, null));
					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager instance = EnvironmentManager.Instance;
				string message = ex.Message;
				object[] args = new Exception[1] { ex };
				instance.Log(error: true, "ConfigurationAsync.BuildRelated", message, args);
			}
		}
		return list;
	}

	public static void AddProperties(IDevice device, IDeviceSecurity deviceSecurity, Item item)
	{
		if (deviceSecurity != null)
		{
			item.Authorization.Add("Live", deviceSecurity.Live ? "Yes" : "No");
			item.Authorization.Add("Playback", deviceSecurity.Browse ? "Yes" : "No");
			item.Authorization.Add("StartRecording", deviceSecurity.StartRecording ? "Yes" : "No");
			item.Authorization.Add("BookmarkAdd", deviceSecurity.BookmarkAdd ? "Yes" : "No");
			item.Authorization.Add("BookmarkDelete", deviceSecurity.BookmarkDelete ? "Yes" : "No");
			item.Authorization.Add("BookmarkEdit", deviceSecurity.BookmarkEdit ? "Yes" : "No");
			item.Authorization.Add("BookmarkView", deviceSecurity.BookmarkView ? "Yes" : "No");
			item.Authorization.Add("RetentionCreate", deviceSecurity.RetentionCreate ? "Yes" : "No");
			item.Authorization.Add("RetentionRemove", deviceSecurity.RetentionRemove ? "Yes" : "No");
			item.Authorization.Add("RetentionView", deviceSecurity.RetentionView ? "Yes" : "No");
			item.Authorization.Add("ExportDatabase", deviceSecurity.ExportDatabase ? "Yes" : "No");
			item.Authorization.Add("GetSequences", deviceSecurity.GetSequences ? "Yes" : "No");
			item.Authorization.Add("RetrieveEdgeRecordings", deviceSecurity.RetrieveEdgeRecordings ? "Yes" : "No");
		}
		if (device != null && device.Disabled)
		{
			item.Properties.Add("Enabled", "No");
		}
	}

	public static void AddCameraProperties(ICamera camera, Item item)
	{
		if (camera.PtzSettings.PtzEnabled)
		{
			item.Properties.Add("PTZ", "Yes");
			item.Properties.Add("PTZCenter", camera.PtzSettings.IsCenterOnPositionInViewSupported ? "Yes" : "No");
			item.Properties.Add("PTZRectangle", camera.PtzSettings.IsPtzCenterAndZoomToRectangleSupported ? "Yes" : "No");
			item.Properties.Add("PTZHome", camera.PtzSettings.IsPtzHomeSupported ? "Yes" : "No");
			item.Properties.Add("PTZDiagonal", camera.PtzSettings.IsPtzDiagonalSupported ? "Yes" : "No");
		}
		else
		{
			item.Properties.Add("PTZ", "No");
		}
		foreach (Capability capability in camera.PtzSettings.CapabilityList)
		{
			if (capability.Absolute)
			{
				item.Properties.Add(capability.Name, "Absolute");
			}
			else if (capability.Relative)
			{
				item.Properties.Add(capability.Name, "Relative");
			}
		}
		item.Properties.Add("Ipix", camera.IpixEnabled ? "Yes" : "No");
		if (camera.IpixEnabled)
		{
			item.Properties.Add("IpixData", camera.GetIpixData());
		}
		item.Properties.Add("PanoramicLens", camera.PanoramicLens.PanoramicLensEnabled ? "Yes" : "No");
		if (camera.PanoramicLens.PanoramicLensEnabled)
		{
			item.Properties.Add("PanoramicLensData", camera.GetPanoramicLensData());
		}
		item.Properties.Add("ShortCut", camera.Shortcut ?? "");
		item.Properties.Add("EdgeSupported", camera.EdgeSupported ? "Yes" : "No");
		item.Properties.Add("Channel", camera.Channel.ToString() ?? "");
		item.Authorization.Add("PTZ", camera.CameraAccess.PtzSecurity.ManualControl ? "Yes" : "No");
		item.Authorization.Add("Preset", camera.CameraAccess.PtzSecurity.PresetControl ? "Yes" : "No");
		item.Authorization.Add("Output", camera.CameraAccess.Outputs ? "Yes" : "No");
		item.Authorization.Add("Event", camera.CameraAccess.Events ? "Yes" : "No");
		item.Authorization.Add("ExportAvi", camera.CameraAccess.ExportAvi ? "Yes" : "No");
		item.Authorization.Add("ExportJpeg", camera.CameraAccess.ExportJpeg ? "Yes" : "No");
		item.Authorization.Add("SmartSearch", camera.CameraAccess.SmartSearch ? "Yes" : "No");
	}

	public static void AddSpeakerProperties(ISpeaker speaker, Item item)
	{
		if (speaker != null && speaker.SpeakerAccess != null)
		{
			item.Authorization.Add("Speak", speaker.SpeakerAccess.Speak ? "Yes" : "No");
		}
	}

	public static Dictionary<string, string> BuildMetadataProperties(IMetadataDevice metadataDevice)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Channel", metadataDevice.Channel.ToString() ?? "");
		if (metadataDevice.Disabled)
		{
			dictionary.Add("Enabled", "No");
		}
		return dictionary;
	}

	public Tuple<DateTime, string> GetLiveJPEG(FQID deviceFqid, int width, int height)
	{
		if (deviceFqid == null)
		{
			throw new ArgumentNullException("deviceFqid");
		}
		ISystemInfo systemInfo = ((SDKConfiguration)_userContext.Configuration).FindSystemInfo(deviceFqid.ServerId, populate: true);
		if (systemInfo != null && deviceFqid.Kind == Kind.Camera && systemInfo.Recorders.ContainsKey(deviceFqid.ServerId.Id))
		{
			return systemInfo.Recorders[deviceFqid.ServerId.Id].GetLiveJpeg(deviceFqid.ObjectId, width, height);
		}
		throw new PathNotFoundMIPException("Server or camera id not found");
	}
}
