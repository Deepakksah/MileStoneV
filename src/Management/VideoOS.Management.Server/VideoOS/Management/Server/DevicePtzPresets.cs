using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Log;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public class DevicePtzPresets
{
	private class UpdatePresetsXmlResult
	{
		public string PresetsXml { get; set; }

		public DateTime LastModified { get; set; }

		public UpdatePresetsXmlResult(string presetsXml, DateTime lastModified)
		{
			PresetsXml = presetsXml;
			LastModified = lastModified;
		}
	}

	public string Id { get; set; }

	public string PresetsXml { get; set; }

	public string SettingsXml { get; set; }

	public DateTime LastModified { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public static PtzPresetCollection QueryPtzPresets(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		DeviceType deviceType;
		try
		{
			deviceType = requestContext.DeviceTypeLookup.QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		switch (deviceType)
		{
		case DeviceType.Unknown:
			throw new DeviceNotFoundException();
		default:
			throw new DevicePtzPresetsNotSupportedException();
		case DeviceType.Camera:
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId);
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			DevicePtzPresets obj = deviceComponent.QueryPtzPresets(deviceId) ?? throw new DeviceNotFoundException();
			DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(obj.SettingsXml);
			if (!deviceSettingUtilities.IsPresetEnabled || (!deviceSettingUtilities.PresetConfiguration.HasAbsolutePresets && !deviceSettingUtilities.PresetConfiguration.HasInternalPresets))
			{
				throw new DevicePtzPresetsNotSupportedException();
			}
			return PtzPresetCollection.FromXml(obj.PresetsXml);
		}
		}
	}

	public static bool CanLoadPresetsFromDevice(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		DeviceType deviceType;
		try
		{
			deviceType = requestContext.DeviceTypeLookup.QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		switch (deviceType)
		{
		case DeviceType.Unknown:
			throw new DeviceNotFoundException();
		default:
			return false;
		case DeviceType.Camera:
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId);
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities((deviceComponent.QueryPtzPresets(deviceId) ?? throw new DeviceNotFoundException()).SettingsXml);
			if (deviceSettingUtilities.IsPresetEnabled && deviceSettingUtilities.PresetConfiguration.HasInternalPresets)
			{
				return true;
			}
			return false;
		}
		}
	}

	public static PtzPresetCollection AddPtzPreset(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, double pan, double tilt, double zoom, bool? isDefault, bool locked)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		Func<UpdatePresetsXmlResult> updatePresetsFunc = () => new UpdatePresetsXmlResult(ValidatePtzPresetAndAdd(requestContext, deviceId, presetId, name, description, pan, tilt, zoom, isDefault, locked, out var lastModified), lastModified);
		PtzPresetCollection result = UpdatePresetsXmlWithRetry(requestContext, deviceId, updatePresetsFunc, presetId, createPreset: true);
		Integration.FireConfigurationChanged(requestContext, "PtzPreset", presetId, "Camera", deviceId, EventAction.Added, name);
		return result;
	}

	public static PtzPresetCollection AddPtzPreset(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, bool? isDefault, bool locked)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		double pan = 0.0;
		double tilt = 0.0;
		double zoom = 0.0;
		ValidatePtzPresetAndAdd(requestContext, deviceId, presetId, name, description, pan, tilt, zoom, isDefault, locked, out var lastModified);
		GetAbsolutePtzPositionResponse getAbsolutePtzPositionResponse = null;
		using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture)))
		{
			getAbsolutePtzPositionResponse = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetAbsolutePtzPosition(deviceId.ToString()), null, delegate(Exception ex, Action ac)
			{
				GeminiTrace.Error(TraceType.Configuration, "Getting absolute ptz position via server service failed: UpdatePresetPosition('{0}')", deviceId.ToString());
				return UpdatePresetPositionErrorHandler(ex);
			});
		}
		if (getAbsolutePtzPositionResponse != null)
		{
			pan = getAbsolutePtzPositionResponse.Pan;
			tilt = getAbsolutePtzPositionResponse.Tilt;
			zoom = getAbsolutePtzPositionResponse.Zoom;
		}
		Func<UpdatePresetsXmlResult> updatePresetsFunc = () => new UpdatePresetsXmlResult(ValidatePtzPresetAndAdd(requestContext, deviceId, presetId, name, description, pan, tilt, zoom, isDefault, locked, out lastModified), lastModified);
		PtzPresetCollection result = UpdatePresetsXmlWithRetry(requestContext, deviceId, updatePresetsFunc, presetId, createPreset: true);
		Integration.FireConfigurationChanged(requestContext, "PtzPreset", presetId, "Camera", deviceId, EventAction.Added, name);
		return result;
	}

	private static string ValidatePtzPresetAndAdd(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, double pan, double tilt, double zoom, bool? isDefault, bool locked, out DateTime lastModified)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref presetId, "presetId", allowNullOrEmpty: false);
		Validation.CheckName(name, "name", allowNull: false);
		Validation.CheckDescription(ref description, "description", allowNull: true);
		Validation.ValidateDouble(pan, "pan", -1.0, 1.0, allowNull: false);
		Validation.ValidateDouble(tilt, "tilt", -1.0, 1.0, allowNull: false);
		Validation.ValidateDouble(zoom, "zoom", 0.0, 1.0, allowNull: false);
		if (description == null)
		{
			description = string.Empty;
		}
		ValidatePtzPresetCore(requestContext, deviceId, out lastModified, out var presets);
		if (presets.Any((PtzPreset p) => p.IsDevicePreset))
		{
			throw new DevicePtzPresetNotUpdatableException();
		}
		if (locked)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceId);
		}
		PtzPreset ptzPreset = presets.FirstOrDefault((PtzPreset p) => p.Id == new Guid(presetId));
		if (ptzPreset != null)
		{
			throw new DevicePtzPresetAlreadyExistsException();
		}
		if (presets.Any((PtzPreset p) => p.Id != new Guid(presetId) && string.Equals(name, p.Name, StringComparison.OrdinalIgnoreCase)))
		{
			throw new DevicePtzPresetNotUniqueException();
		}
		PtzArgs ptzArgs = new PtzArgs
		{
			Pan = pan,
			Tilt = tilt,
			Zoom = zoom
		};
		ptzPreset = PtzPreset.CreateAbsolutePreset(new Guid(presetId), name, description, ptzArgs, defaultPreset: false, locked);
		if (isDefault.HasValue)
		{
			if (isDefault.Value)
			{
				foreach (PtzPreset item in presets)
				{
					item.IsDefault = false;
				}
			}
			ptzPreset.IsDefault = isDefault.Value;
		}
		presets.Add(ptzPreset);
		return presets.SaveAsXml();
	}

	public static PtzPresetCollection RemovePtzPreset(IGeminiRequestContext requestContext, string deviceId, string presetId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		ValidatePtzPresetForDelete(requestContext, deviceId, presetId);
		Guid sessionId = Guid.NewGuid();
		string xml = string.Empty;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			xml = deviceComponent.DeletePtzPreset(deviceId, presetId, sessionId);
		}
		requestContext.GetIntegrationService().TryNotifyServerAsync();
		using (ISqlLogProcessor sqlLogProcessor = requestContext.CreateLogManager())
		{
			sqlLogProcessor.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "PtzPreset", presetId, "Camera", deviceId, EventAction.Deleted, string.Empty);
		return PtzPresetCollection.FromXml(xml);
	}

	private static void ValidatePtzPresetForDelete(IGeminiRequestContext requestContext, string deviceId, string presetId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref presetId, "presetId", allowNullOrEmpty: false);
		ValidatePtzPresetCore(requestContext, deviceId, out var _, out var presets);
		if ((presets.FirstOrDefault((PtzPreset p) => p.Id == new Guid(presetId)) ?? throw new DevicePtzPresetNotFoundException()).Locked)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceId);
		}
	}

	public static PtzPresetCollection UpdatePtzPreset(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, double? pan, double? tilt, double? zoom, bool? isDefault, bool? locked)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		Func<UpdatePresetsXmlResult> updatePresetsFunc = () => new UpdatePresetsXmlResult(ValidatePtzPresetAndUpdate(requestContext, deviceId, presetId, name, description, pan, tilt, zoom, isDefault, out var lastModified, locked), lastModified);
		PtzPresetCollection result = UpdatePresetsXmlWithRetry(requestContext, deviceId, updatePresetsFunc, presetId, createPreset: false);
		Integration.FireConfigurationChanged(requestContext, "PtzPreset", presetId, "Camera", deviceId, EventAction.Updated, name);
		return result;
	}

	public static void UpdatePtzPresetOrder(IGeminiRequestContext requestContext, string deviceId, string[] presetIds)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectIdArray(presetIds, "presetId", allowNullOrEmptyIds: false);
		ValidatePtzPresetCore(requestContext, deviceId, out var _, out var _);
		Guid sessionId = Guid.NewGuid();
		Device device;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			deviceComponent.UpdatePtzPresetsOrder(deviceId, presetIds, sessionId);
			device = deviceComponent.QueryDeviceById(deviceId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.ItemType, deviceId, EventAction.Updated, string.Empty);
	}

	public static void UpdatePresetPosition(IGeminiRequestContext requestContext, string presetId, string deviceId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		ValidatePtzPresetCore(requestContext, deviceId, out var _, out var presets);
		PtzPreset obj = presets.FirstOrDefault((PtzPreset p) => p.Id == new Guid(presetId)) ?? throw new DevicePtzPresetNotFoundException();
		if (obj.Locked)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceId);
		}
		if (obj.IsDevicePreset)
		{
			throw new DevicePtzPresetNotUpdatableException();
		}
		GetAbsolutePtzPositionResponse getAbsolutePtzPositionResponse = null;
		using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture)))
		{
			getAbsolutePtzPositionResponse = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetAbsolutePtzPosition(deviceId.ToString()), null, delegate(Exception ex, Action ac)
			{
				GeminiTrace.Error(TraceType.Configuration, "Getting absolute ptz position via server service failed: UpdatePresetPosition('{0}')", deviceId.ToString());
				return UpdatePresetPositionErrorHandler(ex);
			});
		}
		if (getAbsolutePtzPositionResponse == null)
		{
			throw new RecordingServerCommunicationException();
		}
		UpdatePtzPreset(requestContext, deviceId.ToString(), presetId.ToString(), null, null, getAbsolutePtzPositionResponse.Pan, getAbsolutePtzPositionResponse.Tilt, getAbsolutePtzPositionResponse.Zoom, null, null);
	}

	private static bool UpdatePresetPositionErrorHandler(Exception ex)
	{
		if (ex is FaultException)
		{
			if (ex.GetType() == typeof(FaultException<ErrorResponseFault>))
			{
				FaultException<ErrorResponseFault> ex2 = ex as FaultException<ErrorResponseFault>;
				if (ex2.Detail.ErrorNumber == 0)
				{
					throw new RecordingServerNotFoundException();
				}
				if (ex2.Detail.ErrorNumber == 1)
				{
					throw new DeviceNotFoundException();
				}
				if (ex2.Detail.ErrorNumber == 2 || ex2.Detail.ErrorNumber == 3 || ex2.Detail.ErrorNumber == 5)
				{
					throw new DevicePtzPresetsNotSupportedException();
				}
				if (ex2.Detail.ErrorNumber == 4)
				{
					throw new DatabaseConfigUnknownErrorException(ex2.Detail.Message);
				}
			}
			if (ex.GetType() == typeof(FaultException<RecordingServerUnavailableFault>))
			{
				throw new RecordingServerUnavailableException();
			}
			if (ex.GetType() == typeof(FaultException<RecordingServerCommunicationTimeoutFault>))
			{
				throw new RecordingServerTimeoutException();
			}
			GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
			GeminiTrace.TraceException(ex);
			throw new RecordingServerCommunicationException();
		}
		if (ex.GetType() == typeof(CommunicationException) || ex.GetType() == typeof(EndpointNotFoundException))
		{
			throw new ManagementServerServiceUnavailableException(ex.Message, ex);
		}
		GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
		GeminiTrace.TraceException(ex);
		throw new RecordingServerCommunicationException();
	}

	public static void ValidatePtzPresetUpdate(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, double? pan, double? tilt, double? zoom, bool? isDefault, bool? locked)
	{
		ValidatePtzPresetAndUpdate(requestContext, deviceId, presetId, name, description, pan, tilt, zoom, isDefault, out var _, locked);
	}

	private static string ValidatePtzPresetAndUpdate(IGeminiRequestContext requestContext, string deviceId, string presetId, string name, string description, double? pan, double? tilt, double? zoom, bool? isDefault, out DateTime lastModified, bool? locked)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref presetId, "presetId", allowNullOrEmpty: false);
		Validation.CheckName(name, "name", allowNull: true);
		Validation.CheckDescription(ref description, "description", allowNull: true);
		Validation.ValidateDouble(pan, "pan", -1.0, 1.0, allowNull: true);
		Validation.ValidateDouble(tilt, "tilt", -1.0, 1.0, allowNull: true);
		Validation.ValidateDouble(zoom, "zoom", 0.0, 1.0, allowNull: true);
		ValidatePtzPresetCore(requestContext, deviceId, out lastModified, out var presets);
		PtzPreset ptzPreset = presets.FirstOrDefault((PtzPreset p) => p.Id == new Guid(presetId));
		if (ptzPreset == null)
		{
			throw new DevicePtzPresetNotFoundException();
		}
		if (ptzPreset.Locked || (locked.HasValue && ptzPreset.Locked != locked.Value))
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceId);
		}
		if (ptzPreset.IsDevicePreset && (pan.HasValue || tilt.HasValue || zoom.HasValue))
		{
			throw new DevicePtzPresetNotUpdatableException();
		}
		if (name != null)
		{
			if (presets.Any((PtzPreset p) => p.Id != new Guid(presetId) && string.Equals(name, p.Name, StringComparison.OrdinalIgnoreCase)))
			{
				throw new DevicePtzPresetNotUniqueException();
			}
			if (presets.Any((PtzPreset p) => p.Id != new Guid(presetId) && string.Equals(name, p.Key, StringComparison.OrdinalIgnoreCase)))
			{
				throw VmoExceptionFactory.VMO60320_DevicePtzPresetNameKeyConflictException(name);
			}
			ptzPreset.Name = name;
		}
		if (description != null)
		{
			ptzPreset.Description = description;
		}
		if (pan.HasValue)
		{
			ptzPreset.PtzArgs.Pan = pan.Value;
		}
		if (tilt.HasValue)
		{
			ptzPreset.PtzArgs.Tilt = tilt.Value;
		}
		if (zoom.HasValue)
		{
			ptzPreset.PtzArgs.Zoom = zoom.Value;
		}
		if (isDefault.HasValue)
		{
			if (isDefault.Value)
			{
				foreach (PtzPreset item in presets)
				{
					item.IsDefault = false;
				}
			}
			ptzPreset.IsDefault = isDefault.Value;
		}
		if (locked.HasValue)
		{
			ptzPreset.Locked = locked.Value;
		}
		return presets.SaveAsXml();
	}

	private static void ValidatePtzPresetCore(IGeminiRequestContext requestContext, string deviceId, out DateTime lastModified, out PtzPresetCollection presets)
	{
		DeviceType deviceType;
		try
		{
			deviceType = requestContext.DeviceTypeLookup.QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		switch (deviceType)
		{
		case DeviceType.Unknown:
			throw new DeviceNotFoundException();
		default:
			throw new DevicePtzPresetsNotSupportedException();
		case DeviceType.Camera:
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", deviceId);
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			DevicePtzPresets devicePtzPresets = deviceComponent.QueryPtzPresets(deviceId);
			if (devicePtzPresets == null)
			{
				throw new DeviceNotFoundException();
			}
			lastModified = devicePtzPresets.LastModified;
			DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(devicePtzPresets.SettingsXml);
			if (!deviceSettingUtilities.IsPresetEnabled || deviceSettingUtilities.PresetConfiguration == null || (!deviceSettingUtilities.PresetConfiguration.HasAbsolutePresets && !deviceSettingUtilities.PresetConfiguration.HasInternalPresets))
			{
				throw new DevicePtzPresetsNotSupportedException();
			}
			presets = PtzPresetCollection.FromXml(devicePtzPresets.PresetsXml);
			break;
		}
		}
	}

	private static PtzPresetCollection UpdatePresetsXmlWithRetry(IGeminiRequestContext requestContext, string deviceId, Func<UpdatePresetsXmlResult> updatePresetsFunc, string presetId, bool createPreset)
	{
		Guid sessionId = Guid.NewGuid();
		int num = 5;
		bool flag;
		UpdatePresetsXmlResult updatePresetsXmlResult;
		do
		{
			flag = false;
			using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
			updatePresetsXmlResult = updatePresetsFunc();
			try
			{
				deviceComponent.UpdatePtzPresets(deviceId, updatePresetsXmlResult.LastModified, updatePresetsXmlResult.PresetsXml, sessionId, presetId, createPreset);
			}
			catch (DeviceNotRecentVersionException)
			{
				flag = true;
				num--;
				if (num == 0)
				{
					throw;
				}
				Thread.Sleep(50);
			}
		}
		while (flag);
		requestContext.GetIntegrationService().TryNotifyServerAsync();
		using (ISqlLogProcessor sqlLogProcessor = requestContext.CreateLogManager())
		{
			sqlLogProcessor.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return PtzPresetCollection.FromXml(updatePresetsXmlResult.PresetsXml);
	}
}
