using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Camera : IConfigurationItem
{
	public ClientSettingsFolder ClientSettingsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ClientSettingsFolder")
				{
					return new ClientSettingsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CustomPropertiesFolder CustomPropertiesFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CustomPropertiesFolder")
				{
					return new CustomPropertiesFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public DeviceDriverSettingsFolder DeviceDriverSettingsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "DeviceDriverSettingsFolder")
				{
					return new DeviceDriverSettingsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public FisheyeLensFolder FisheyeLensFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "FisheyeLensFolder")
				{
					return new FisheyeLensFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwareDeviceEventFolder HardwareDeviceEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareDeviceEventFolder")
				{
					return new HardwareDeviceEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MotionDetectionFolder MotionDetectionFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MotionDetectionFolder")
				{
					return new MotionDetectionFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public PatrollingProfileFolder PatrollingProfileFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "PatrollingProfileFolder")
				{
					return new PatrollingProfileFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public PrivacyProtectionFolder PrivacyProtectionFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "PrivacyProtectionFolder")
				{
					return new PrivacyProtectionFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public PtzPresetFolder PtzPresetFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "PtzPresetFolder")
				{
					return new PtzPresetFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public StreamFolder StreamFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "StreamFolder")
				{
					return new StreamFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public int Channel
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Channel");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string GisPoint
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisPoint");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisPoint", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ShortName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ShortName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ShortName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Icon
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Icon");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Icon", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageDirection
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageDirection");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageDirection", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageDepth
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageDepth");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageDepth", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageFieldOfView
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageFieldOfView");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageFieldOfView", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double RecordingFramerate
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecordingFramerate");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecordingFramerate", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool RecordKeyframesOnly
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecordKeyframesOnly");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecordKeyframesOnly", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool RecordOnRelatedDevices
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecordOnRelatedDevices");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecordOnRelatedDevices", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PtzEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PtzEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public bool RecordingEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RecordingEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RecordingEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PrebufferEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PrebufferEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrebufferEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PrebufferInMemory
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PrebufferInMemory");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrebufferInMemory", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int PrebufferSeconds
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PrebufferSeconds");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrebufferSeconds", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool EdgeStorageEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EdgeStorageEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EdgeStorageEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool EdgeStoragePlaybackEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EdgeStoragePlaybackEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EdgeStoragePlaybackEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ManualRecordingTimeoutEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ManualRecordingTimeoutEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManualRecordingTimeoutEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int ManualRecordingTimeoutMinutes
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ManualRecordingTimeoutMinutes");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManualRecordingTimeoutMinutes", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string RecordingStorage => base.Wrapper.GetPropertyValue("RecordingStorage");

	public Dictionary<string, string> FailoverSettingValues => base.Wrapper.GetEnumValues("FailoverSetting");

	public string FailoverSetting
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FailoverSetting");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FailoverSetting", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ICollection<StreamUsageChildItem> StreamUsages => StreamFolder.Streams?.FirstOrDefault()?.StreamUsageChildItems;

	public ICollection<StreamChildItem> StreamDefinitions => DeviceDriverSettingsFolder.DeviceDriverSettings?.FirstOrDefault()?.StreamChildItems;

	public Camera(ServerId serverId, ConfigurationItem camera)
		: base(serverId, camera)
	{
	}

	public Camera(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Camera(FQID fqid)
		: base(fqid)
	{
	}

	public ClassCollectionServerTask GetOtherRecordingServersWithMedia()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("GetOtherRecordingServersWithMedia").ExecuteMethod("GetOtherRecordingServersWithMedia"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'GetOtherRecordingServersWithMedia':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask GetOtherRecordingStoragesWithMedia()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("GetOtherRecordingStoragesWithMedia").ExecuteMethod("GetOtherRecordingStoragesWithMedia"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'GetOtherRecordingStoragesWithMedia':" + ex.Message);
		}
		return null;
	}

	public ServerTask RetrieveEdgeStorage(DateTime startTime, DateTime endTime, string identifier, Collection<string> relatedTasks)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RetrieveEdgeStorage");
			configurationApiItemWrapper.SetProperty("startTime", TimeConvert.ToGmtString(startTime, includeMilliSeconds: true));
			configurationApiItemWrapper.SetProperty("endTime", TimeConvert.ToGmtString(endTime, includeMilliSeconds: true));
			configurationApiItemWrapper.SetProperty("Identifier", identifier);
			configurationApiItemWrapper.SetProperty("RelatedTasks", relatedTasks);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RetrieveEdgeStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'RetrieveEdgeStorage':" + ex.Message);
		}
		return null;
	}

	public RetrieveEdgeStorageServerTask RetrieveEdgeStorage()
	{
		try
		{
			return new RetrieveEdgeStorageServerTask(base.Wrapper.ExecuteMethod("RetrieveEdgeStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'RetrieveEdgeStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask TruncateStorage()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("TruncateStorage").ExecuteMethod("TruncateStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'TruncateStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask TruncateStorageOnOtherRecordingServer(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("TruncateStorageOnOtherRecordingServer");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("TruncateStorageOnOtherRecordingServer"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'TruncateStorageOnOtherRecordingServer':" + ex.Message);
		}
		return null;
	}

	public TruncateStorageOnOtherRecordingServerServerTask TruncateStorageOnOtherRecordingServer()
	{
		try
		{
			return new TruncateStorageOnOtherRecordingServerServerTask(base.Wrapper.ExecuteMethod("TruncateStorageOnOtherRecordingServer"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'TruncateStorageOnOtherRecordingServer':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangeDeviceRecordingStorage(string itemSelection, bool moveData)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeDeviceRecordingStorage");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			configurationApiItemWrapper.SetProperty("moveData", moveData.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeDeviceRecordingStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'ChangeDeviceRecordingStorage':" + ex.Message);
		}
		return null;
	}

	public ChangeDeviceRecordingStorageServerTask ChangeDeviceRecordingStorage()
	{
		try
		{
			return new ChangeDeviceRecordingStorageServerTask(base.Wrapper.ExecuteMethod("ChangeDeviceRecordingStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'ChangeDeviceRecordingStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangeSecurityPermissions(string userPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeSecurityPermissions");
			configurationApiItemWrapper.SetProperty("UserPath", userPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ChangeSecurityPermissionsServerTask ChangeSecurityPermissions()
	{
		try
		{
			return new ChangeSecurityPermissionsServerTask(base.Wrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ServerTask GetPtzAbsolutePosition()
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("GetPtzAbsolutePosition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'GetPtzAbsolutePosition':" + ex.Message);
		}
		return null;
	}

	public ServerTask JpegGetLive(int width, int height)
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("JpegGetLive");
			configurationApiItemWrapper.SetProperty("width", width.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("height", height.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("JpegGetLive"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'JpegGetLive':" + ex.Message);
		}
		return null;
	}

	public ServerTask SendDriverCommand(string command, string parameter, int timeOut)
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SendDriverCommand");
			configurationApiItemWrapper.SetProperty("command", command);
			configurationApiItemWrapper.SetProperty("parameter", parameter);
			configurationApiItemWrapper.SetProperty("timeOut", timeOut.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SendDriverCommand"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'SendDriverCommand':" + ex.Message);
		}
		return null;
	}

	public ServerTask SetPtzAbsolutePosition(double pan, double tilt, double zoom, double speed, bool allowRepeats)
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(base.Wrapper.ServerId, base.Wrapper.Path);
			configurationApiItemWrapper.ClearProperties();
			configurationApiItemWrapper.ConfigurationItem.EnableProperty = null;
			configurationApiItemWrapper.ConfigurationItem.ItemType = "InvokeInfo";
			configurationApiItemWrapper.SetProperty("pan", pan.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("tilt", tilt.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("zoom", zoom.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("speed", speed.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("allowRepeats", allowRepeats.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SetPtzAbsolutePosition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'SetPtzAbsolutePosition':" + ex.Message);
		}
		return null;
	}

	public ServerTask RetrieveEdgeStorage(DateTime startTime, DateTime endTime)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RetrieveEdgeStorage");
			configurationApiItemWrapper.SetProperty("startTime", TimeConvert.ToGmtString(startTime, includeMilliSeconds: true));
			configurationApiItemWrapper.SetProperty("endTime", TimeConvert.ToGmtString(endTime, includeMilliSeconds: true));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RetrieveEdgeStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Camera", "Unable to call method 'RetrieveEdgeStorage':" + ex.Message);
		}
		return null;
	}
}
