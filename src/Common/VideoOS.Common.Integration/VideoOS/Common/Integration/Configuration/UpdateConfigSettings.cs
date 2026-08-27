using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Configuration;

public class UpdateConfigSettings
{
	public class ConfigSettingValue
	{
		public string SectionName { get; private set; }

		public string SettingName { get; private set; }

		public string SettingValue { get; private set; }

		public bool Visible { get; private set; }

		public Guid TypeReferenceId { get; private set; }

		public ConfigSettingValue(string sectionName, string settingName, string settingValue, bool visible, Guid typeReferenceId)
		{
			SectionName = sectionName;
			SettingName = settingName;
			SettingValue = settingValue;
			Visible = visible;
			TypeReferenceId = typeReferenceId;
		}
	}

	private ILogger _logger;

	public UpdateConfigSettings(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_logger = logger;
	}

	public void UpdateEventSettings(DriverSettings previousEventSettings, DriverSettings newEventSettings, EventDefinitionCollection eventDefinitions)
	{
		UpdateEventSettings(previousEventSettings, newEventSettings, eventDefinitions, out var _, out var _);
	}

	public void UpdateEventSettings(DriverSettings previousEventSettings, DriverSettings newEventSettings, EventDefinitionCollection eventDefinitions, out List<string> inheritedEvents, out List<string> deletedEvents)
	{
		if (previousEventSettings == null)
		{
			throw new ArgumentNullException("previousEventSettings");
		}
		if (newEventSettings == null)
		{
			throw new ArgumentNullException("newEventSettings");
		}
		if (eventDefinitions == null)
		{
			throw new ArgumentNullException("eventDefinitions");
		}
		inheritedEvents = new List<string>();
		deletedEvents = new List<string>();
		string[] sectionNames = previousEventSettings.GetSectionNames();
		foreach (string text in sectionNames)
		{
			bool flag = false;
			EventHwid eventHwid = Hwid.ParseHwid(text) as EventHwid;
			if (!(eventHwid != null))
			{
				continue;
			}
			EventDefinition eventDefinition = eventDefinitions.Find(eventHwid);
			if (eventDefinition == null)
			{
				LoggerExtensions.LogWarning(_logger, "Could not find event definition for configured event in the new driver definition while upgrading driver. Event Hwid: {EventHwId}", new object[1] { eventHwid.Text });
				deletedEvents.Add(text);
				continue;
			}
			List<string> list = new List<string>();
			list.AddRange(previousEventSettings.GetSectionSettingNames(text));
			int count;
			do
			{
				string[] array = list.ToArray();
				count = list.Count;
				list.Clear();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!eventDefinition.Capabilities.TryGetValue(text2, out var capabilityDefinition))
					{
						continue;
					}
					TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
					if (activeTypeContent == null)
					{
						list.Add(text2);
						continue;
					}
					string value = previousEventSettings.Get(text, text2);
					if (capabilityDefinition.ValidateValue(value, Thread.CurrentThread.CurrentCulture))
					{
						newEventSettings.CreateOrUpdate(text, text2, value, capabilityDefinition.TypeDefinition.ReferenceId.ToString());
						flag = true;
						continue;
					}
					string defaultValue = activeTypeContent.DefaultValue;
					if (capabilityDefinition.ValidateValue(defaultValue, Thread.CurrentThread.CurrentCulture))
					{
						newEventSettings.CreateOrUpdate(text, text2, defaultValue, capabilityDefinition.TypeDefinition.ReferenceId.ToString());
						flag = true;
					}
				}
			}
			while (list.Count > 0 && count != list.Count);
			if (flag)
			{
				inheritedEvents.Add(text);
			}
			if (count != list.Count)
			{
				continue;
			}
			foreach (string item in list)
			{
				LoggerExtensions.LogWarning(_logger, "Could not set event setting '{SettingName}' for configured event in the new driver definition while upgrading driver. Event Hwid: {HwId}", new object[2] { item, eventHwid.Text });
			}
		}
	}

	public void UpdateSettings(DriverSettings previousSettings, DriverSettings newSettings, CapabilitiesCollection capabilities, bool transferSettingIfPossible)
	{
		UpdateSettings(previousSettings, newSettings, capabilities, transferSettingIfPossible, useCurrentReadOnlyValues: false, out var _, out var _);
	}

	public void UpdateSettings(DriverSettings previousSettings, DriverSettings newSettings, CapabilitiesCollection capabilities, bool transferSettingIfPossible, out List<ConfigSettingValue> inheritedSettings, out List<ConfigSettingValue> defaultSettings)
	{
		UpdateSettings(previousSettings, newSettings, capabilities, transferSettingIfPossible, useCurrentReadOnlyValues: false, out inheritedSettings, out defaultSettings);
	}

	public void UpdateSettings(DriverSettings previousSettings, DriverSettings newSettings, CapabilitiesCollection capabilities, bool transferSettingIfPossible, bool useCurrentReadOnlyValues)
	{
		UpdateSettings(previousSettings, newSettings, capabilities, transferSettingIfPossible, useCurrentReadOnlyValues, out var _, out var _);
	}

	public void UpdateSettings(DriverSettings previousSettings, DriverSettings newSettings, CapabilitiesCollection capabilities, bool transferSettingIfPossible, bool useCurrentReadOnlyValues, out List<ConfigSettingValue> inheritedSettings, out List<ConfigSettingValue> defaultSettings)
	{
		if (previousSettings == null)
		{
			throw new ArgumentNullException("previousSettings");
		}
		if (newSettings == null)
		{
			throw new ArgumentNullException("newSettings");
		}
		if (capabilities == null)
		{
			throw new ArgumentNullException("capabilities");
		}
		inheritedSettings = new List<ConfigSettingValue>();
		defaultSettings = new List<ConfigSettingValue>();
		string text = capabilities.Hwid.Text;
		string[] sectionSettingNames = newSettings.GetSectionSettingNames(text);
		foreach (string text2 in sectionSettingNames)
		{
			if (!capabilities.Contains(text2))
			{
				continue;
			}
			CapabilityDefinition capabilityDefinition = capabilities[text2];
			if (capabilityDefinition == null || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.ProductId || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.MacAddress || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.SerialNumber || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.FirmwareVersion || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.HttpsEnabled || capabilityDefinition.TypeDefinition.ReferenceId == TypeReferenceIds.HttpsPort)
			{
				continue;
			}
			string text3 = newSettings.Get(text, text2);
			if (!useCurrentReadOnlyValues && capabilityDefinition.TypeDefinition.Access == access.read)
			{
				LoggerExtensions.LogInformation(_logger, "Driver update: Ignore read-only setting during update of settings. Section: {SectionName}; Name: {SettingName}", new object[2] { text, text2 });
				if (text3 != null)
				{
					defaultSettings.Add(new ConfigSettingValue(text, text2, text3, capabilityDefinition.TypeDefinition.Visible, capabilityDefinition.TypeDefinition.ReferenceId));
				}
				continue;
			}
			string text4 = previousSettings.Get(text, text2);
			if (text4 == null)
			{
				LoggerExtensions.LogInformation(_logger, "Driver update: Ignore read-only setting during update of settings. Section: {SectionName}; Name: {SettingName}", new object[2] { text, text2 });
				if (text3 != null)
				{
					defaultSettings.Add(new ConfigSettingValue(text, text2, text3, capabilityDefinition.TypeDefinition.Visible, capabilityDefinition.TypeDefinition.ReferenceId));
				}
				continue;
			}
			bool flag = false;
			try
			{
				flag = capabilityDefinition.ValidateValue(text4, CultureInfo.InvariantCulture);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				LoggerExtensions.LogWarning(_logger, "Failed to validate value for setting while updating driver settings. Section: {SectionName}; Name: {SettingName}\nException:{Exception}", new object[3] { text, text2, ex2 });
			}
			if (!transferSettingIfPossible)
			{
				flag = false;
			}
			if (flag)
			{
				newSettings.CreateOrUpdate(text, text2, text4, capabilityDefinition.TypeDefinition.ReferenceId.ToString());
				inheritedSettings.Add(new ConfigSettingValue(text, text2, text4, capabilityDefinition.TypeDefinition.Visible, capabilityDefinition.TypeDefinition.ReferenceId));
			}
			else if (text3 != null)
			{
				defaultSettings.Add(new ConfigSettingValue(text, text2, text3, capabilityDefinition.TypeDefinition.Visible, capabilityDefinition.TypeDefinition.ReferenceId));
			}
		}
	}
}
