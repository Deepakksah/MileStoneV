using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public sealed class DriverSettingsBusiness
{
	private static readonly HashSet<Guid> _internalSettings = new HashSet<Guid>
	{
		TypeReferenceIds.InternalEventServerIP,
		TypeReferenceIds.InternalEventServerPort,
		TypeReferenceIds.AlertEventServerIP,
		TypeReferenceIds.AlertEventServerPort,
		TypeReferenceIds.FtpEventServerIP,
		TypeReferenceIds.FtpEventServerPort,
		TypeReferenceIds.SmtpEventServerIP,
		TypeReferenceIds.SmtpEventServerPort,
		TypeReferenceIds.AttachmentSavePath
	};

	public static HashSet<Guid> InternalHardwareSettings => _internalSettings;

	public static void QueryHardwareSettings(IGeminiRequestContext requestContext, string hardwareId, out MainDefinition mainDefinition, out DriverSettings hardwareSettings)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", hardwareId);
		mainDefinition = null;
		hardwareSettings = null;
		using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		HardwareDriverSettingsInfo hardwareDriverSettingsInfo = driverSettingsComponent.QueryHardwareDriverSettingsInfo(hardwareId);
		string definitionXml = hardwareDefinitionComponent.QueryDefinition(hardwareDriverSettingsInfo.DefinitionDigest);
		string[] settingsXml = driverSettingsComponent.QueryAllSettingsByHardware(hardwareDriverSettingsInfo.HardwareId);
		MainDefinition mainDefinition2 = MainDefinition.LoadXml(definitionXml, settingsXml);
		DriverSettings driverSettings = new DriverSettings(hardwareDriverSettingsInfo.SettingsXml);
		hardwareSettings = driverSettings;
		mainDefinition = mainDefinition2;
	}

	public static void UpdateHardwareSettings(IGeminiRequestContext requestContext, string hardwareId, DriverSettings hardwareSettings)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareId);
		Hardware hardware = Hardware.QueryHardwareById(requestContext, hardwareId);
		RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, hardware.RecordingServerId);
		int num = 5;
		bool flag;
		do
		{
			flag = false;
			DateTime lastModified;
			string settingsXml = ValidateHardwareSettings(requestContext, hardwareId, hardwareSettings, out lastModified);
			using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
			try
			{
				driverSettingsComponent.UpdateHardwareSettings(hardwareId, lastModified, settingsXml);
			}
			catch (UpdateHardwareDriverSettingsNotRecentVersionException)
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
		Integration.TryNotifyServerAsync();
		Integration.FireConfigurationChanged(requestContext, "HardwareDriverSettings", hardwareId, EventAction.Updated, string.Empty);
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditHardwareSettingsChanged(new Guid(hardwareId), hardware.Name, recordingServer.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static string ValidateHardwareSettings(IGeminiRequestContext requestContext, string hardwareId, DriverSettings hardwareSettings)
	{
		DateTime lastModified;
		return ValidateHardwareSettings(requestContext, hardwareId, hardwareSettings, out lastModified);
	}

	private static string ValidateHardwareSettings(IGeminiRequestContext requestContext, string hardwareId, DriverSettings hardwareSettings, out DateTime lastModified)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareId);
		using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		HardwareDriverSettingsInfo hardwareDriverSettingsInfo = driverSettingsComponent.QueryHardwareDriverSettingsInfo(hardwareId);
		lastModified = hardwareDriverSettingsInfo.LastModified;
		string definitionXml = hardwareDefinitionComponent.QueryDefinition(hardwareDriverSettingsInfo.DefinitionDigest);
		string mergedSettingsXml = MainDefinition.MergeSettings(driverSettingsComponent.QueryAllSettingsByHardware(hardwareDriverSettingsInfo.HardwareId));
		return PrepareAndValidateSettings(requestContext, hardwareSettings, hardwareDriverSettingsInfo.SettingsXml, definitionXml, mergedSettingsXml);
	}

	public static void QueryDeviceSettings(IGeminiRequestContext requestContext, string deviceId, out MainDefinition mainDefinition, out DeviceHwid hwid, out DriverSettings deviceSettings)
	{
		Guid namespaceId = DeviceTypeLookup.Instance.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_READ", deviceId);
		using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		DeviceDriverSettingsInfo deviceDriverSettingsInfo = driverSettingsComponent.QueryDeviceDriverSettingsInfo(deviceId);
		DeviceHwid deviceHwid = new DeviceHwid(deviceDriverSettingsInfo.DeviceType.ToDeviceTypeEnum(), deviceDriverSettingsInfo.DeviceIndex);
		string definitionXml = hardwareDefinitionComponent.QueryDefinition(deviceDriverSettingsInfo.DefinitionDigest);
		string[] settingsXml = driverSettingsComponent.QueryAllSettingsByHardware(deviceDriverSettingsInfo.HardwareId);
		MainDefinition mainDefinition2 = MainDefinition.LoadXml(definitionXml, settingsXml);
		DriverSettings driverSettings = new DriverSettings(deviceDriverSettingsInfo.SettingsXml);
		deviceSettings = driverSettings;
		hwid = deviceHwid;
		mainDefinition = mainDefinition2;
	}

	public static void UpdateDeviceSettings(IGeminiRequestContext requestContext, string deviceId, DriverSettings deviceSettings)
	{
		Guid namespaceId = DeviceTypeLookup.Instance.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
		Guid sessionId = Guid.NewGuid();
		int num = 5;
		bool flag;
		do
		{
			flag = false;
			DateTime lastModified;
			string settingsXml = ValidateDeviceSettings(requestContext, deviceId, deviceSettings, out lastModified);
			using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
			try
			{
				driverSettingsComponent.UpdateDeviceSettings(deviceId, lastModified, settingsXml, sessionId);
			}
			catch (UpdateDeviceDriverSettingsNotRecentVersionException)
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
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "DeviceDriverSettings", deviceId, EventAction.Updated, string.Empty);
	}

	public static string ValidateDeviceSettings(IGeminiRequestContext requestContext, string deviceId, DriverSettings deviceSettings)
	{
		DateTime lastModified;
		return ValidateDeviceSettings(requestContext, deviceId, deviceSettings, out lastModified);
	}

	private static string ValidateDeviceSettings(IGeminiRequestContext requestContext, string deviceId, DriverSettings deviceSettings, out DateTime lastModified)
	{
		Guid namespaceId = DeviceTypeLookup.Instance.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
		using IDriverSettingsComponent driverSettingsComponent = requestContext.CreateDriverSettingsComponent();
		using IHardwareDefinitionComponent hardwareDefinitionComponent = requestContext.CreateHardwareDefinitionComponent();
		DeviceDriverSettingsInfo deviceDriverSettingsInfo = driverSettingsComponent.QueryDeviceDriverSettingsInfo(deviceId);
		string definitionXml = hardwareDefinitionComponent.QueryDefinition(deviceDriverSettingsInfo.DefinitionDigest);
		string mergedSettingsXml = MainDefinition.MergeSettings(driverSettingsComponent.QueryAllSettingsByHardware(deviceDriverSettingsInfo.HardwareId));
		lastModified = deviceDriverSettingsInfo.LastModified;
		return PrepareAndValidateSettings(requestContext, deviceSettings, deviceDriverSettingsInfo.SettingsXml, definitionXml, mergedSettingsXml);
	}

	public static string PrepareAndValidateSettings(IGeminiRequestContext requestContext, DriverSettings updateSettings, string currentSettingsXml, string definitionXml, string mergedSettingsXml)
	{
		DriverSettings driverSettings = new DriverSettings(currentSettingsXml);
		DriverSettings driverSettings2 = new DriverSettings(mergedSettingsXml);
		MainDefinition mainDefinition = MainDefinition.LoadXml(definitionXml, mergedSettingsXml);
		CheckAacLicenseAndUsage(requestContext, updateSettings);
		string[] sectionNames = updateSettings.GetSectionNames();
		foreach (string text in sectionNames)
		{
			Hwid hwid = Hwid.ParseHwid(text);
			CapabilitiesCollection capabilitiesByHwid = mainDefinition.GetCapabilitiesByHwid(hwid);
			string[] sectionSettingNames = updateSettings.GetSectionSettingNames(text);
			foreach (string text2 in sectionSettingNames)
			{
				string a = driverSettings.Get(text, text2);
				string text3 = updateSettings.Get(text, text2);
				if (!capabilitiesByHwid.TryGetValue(text2, out var capabilityDefinition))
				{
					throw new UpdateDriverSettingsInvalidSettingNameException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60193, text2), text, text2, Guid.Empty, new MessageParameter(0, "Name", text2));
				}
				if (!string.Equals(a, text3, StringComparison.Ordinal))
				{
					if (capabilityDefinition.TypeDefinition.IsReadOnly && !SettingsUtil.ShouldOverrideReadOnlyFlag(mainDefinition, capabilityDefinition.TypeDefinition.ReferenceId))
					{
						throw new UpdateDriverSettingsSettingReadOnlyException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60194, text2), text, capabilityDefinition.Name, capabilityDefinition.TypeDefinition.ReferenceId, new MessageParameter(0, "Name", text2));
					}
					TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
					if (activeTypeContent == null)
					{
						throw new InvalidDefinitionException("Unable to find type definition for the setting.");
					}
					if (activeTypeContent.IsData && !activeTypeContent.ResolveClrTypeSupported())
					{
						throw new UpdateDriverSettingsNotSupportedException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60195, text2), text, capabilityDefinition.Name, capabilityDefinition.TypeDefinition.ReferenceId, new MessageParameter(0, "Name", text2));
					}
					driverSettings.Update(text, text2, text3);
					driverSettings2.Update(text, text2, text3);
				}
			}
		}
		string text4 = driverSettings2.SaveAsXml();
		mainDefinition = MainDefinition.LoadXml(definitionXml, text4);
		sectionNames = driverSettings.GetSectionNames();
		foreach (string text5 in sectionNames)
		{
			Hwid hwid2 = Hwid.ParseHwid(text5);
			CapabilitiesCollection capabilitiesByHwid2 = mainDefinition.GetCapabilitiesByHwid(hwid2);
			string[] sectionSettingNames = driverSettings.GetSectionSettingNames(text5);
			foreach (string text6 in sectionSettingNames)
			{
				string value = driverSettings.Get(text5, text6);
				if (!capabilitiesByHwid2.TryGetValue(text6, out var capabilityDefinition2))
				{
					throw new InvalidDefinitionException(string.Format(CultureInfo.InvariantCulture, "Unable to find capability for the setting. Setting: {0} | {1}", text5, text6));
				}
				if (capabilityDefinition2.TypeDefinition.IsReadOnly || _internalSettings.Contains(capabilityDefinition2.TypeDefinition.ReferenceId))
				{
					continue;
				}
				TypeContent activeTypeContent2 = capabilityDefinition2.TypeDefinition.GetActiveTypeContent();
				if (activeTypeContent2 == null)
				{
					throw new InvalidDefinitionException("Unable to find type definition for the setting.");
				}
				if (!activeTypeContent2.IsData || activeTypeContent2.ResolveClrTypeSupported())
				{
					try
					{
						ValidateSettingsValue(value, capabilityDefinition2);
					}
					catch (ArgumentOutOfRangeException)
					{
						throw new UpdateDriverSettingsValueOutOfRangeException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60196, text6), hwid2.Text, text6, capabilityDefinition2.TypeDefinition.ReferenceId, new MessageParameter(0, "Name", text6));
					}
				}
			}
		}
		return driverSettings.SaveAsXml();
	}

	private static void CheckAacLicenseAndUsage(IGeminiRequestContext requestContext, DriverSettings settings)
	{
		if (requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "AACAudioSupport"))
		{
			return;
		}
		IEnumerable<SettingEntry> settingsByTypeReferenceId = settings.GetSettingsByTypeReferenceId(TypeReferenceIds.AudioCodec.ToString());
		if (settingsByTypeReferenceId.Count() != 0)
		{
			SettingEntry settingEntry = settingsByTypeReferenceId.FirstOrDefault(DefinitionUtility.IsSettingAac);
			if (settingEntry != null)
			{
				string section = settingEntry.Section;
				string name = settingEntry.Name;
				throw new UpdateDriverSettingsValueNotLicensedException(typeReferenceId: new Guid(settingEntry.TypeId), message: string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60253, name), hwid: section, capabilityName: name, parameters: new MessageParameter[1]
				{
					new MessageParameter(0, "Name", name)
				});
			}
		}
	}

	public static void ValidateSettingsValue(string value, CapabilityDefinition capability)
	{
		TypeContent activeTypeContent = capability.TypeDefinition.GetActiveTypeContent();
		if (activeTypeContent == null)
		{
			throw new InvalidDefinitionException("Unable to find type definition for the setting.");
		}
		if (activeTypeContent.IsData)
		{
			Type type = activeTypeContent.ResolveClrType();
			DataContentDefinition asData = activeTypeContent.AsData;
			if (type == typeof(bool))
			{
				ValidateBooleanValue(value);
			}
			else if (type == typeof(int))
			{
				ValidateInt32Value(value, asData);
			}
			else if (type == typeof(double))
			{
				ValidateDoubleValue(value, asData);
			}
			else if (!(type == typeof(string)))
			{
				throw new NotSupportedException("Type not supported. " + type.Name);
			}
		}
		else if (activeTypeContent.IsEnumeration)
		{
			EnumerationContentDefinition asEnumeration = activeTypeContent.AsEnumeration;
			ValidateEnumerationValue(value, asEnumeration);
		}
	}

	private static void ValidateBooleanValue(string value)
	{
		if (!string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase) && !string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentOutOfRangeException("value", "The value is not a valid boolean value");
		}
	}

	private static void ValidateInt32Value(string value, DataContentDefinition dataContentDefinition)
	{
		int num = int.MinValue;
		int num2 = int.MaxValue;
		int num3 = 1;
		if (dataContentDefinition.Base.minvalue != null)
		{
			num = DefinitionUtility.DecodeTypedInt32(dataContentDefinition.Base.minvalue);
		}
		if (dataContentDefinition.Base.maxvalue != null)
		{
			num2 = DefinitionUtility.DecodeTypedInt32(dataContentDefinition.Base.maxvalue);
		}
		if (dataContentDefinition.Base.resolution != null)
		{
			num3 = DefinitionUtility.DecodeTypedInt32(dataContentDefinition.Base.resolution);
		}
		if (!int.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result))
		{
			throw new ArgumentOutOfRangeException("value", "The value is not a valid integer value");
		}
		if (result < num || result > num2 || (result - num) % num3 != 0)
		{
			throw new ArgumentOutOfRangeException("value", "The value is not within the specified range.");
		}
	}

	private static void ValidateDoubleValue(string value, DataContentDefinition dataContentDefinition)
	{
		double num = double.MinValue;
		double num2 = double.MaxValue;
		double num3 = double.NaN;
		if (dataContentDefinition.Base.minvalue != null)
		{
			num = DefinitionUtility.DecodeTypedDouble(dataContentDefinition.Base.minvalue);
		}
		if (dataContentDefinition.Base.maxvalue != null)
		{
			num2 = DefinitionUtility.DecodeTypedDouble(dataContentDefinition.Base.maxvalue);
		}
		if (dataContentDefinition.Base.resolution != null)
		{
			num3 = DefinitionUtility.DecodeTypedDouble(dataContentDefinition.Base.resolution);
		}
		if (!double.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result))
		{
			throw new ArgumentOutOfRangeException("value", "The value is not a valid double value");
		}
		if (result < num || result > num2 || (!double.IsNaN(num3) && num3 != 0.0 && (result - num) % num3 != 0.0))
		{
			throw new ArgumentOutOfRangeException("value", "The value is not within the specified range.");
		}
	}

	private static void ValidateEnumerationValue(string value, EnumerationContentDefinition enumerationContentDefinition)
	{
		if (enumerationContentDefinition.Values.FirstOrDefault((EnumerationValue v) => string.Equals(v.Name, value, StringComparison.Ordinal)) == null && enumerationContentDefinition.Values.FirstOrDefault((EnumerationValue v) => string.Equals(v.DisplayNameResolved, value, StringComparison.Ordinal)) == null)
		{
			throw new ArgumentOutOfRangeException("value", "The value is not within the specified range.");
		}
	}
}
