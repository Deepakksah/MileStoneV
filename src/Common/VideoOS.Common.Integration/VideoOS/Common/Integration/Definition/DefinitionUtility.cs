#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public static class DefinitionUtility
{
	public enum RoundingMethod
	{
		Nearest,
		LowerOrEqual,
		HigherOrEqual
	}

	public static IDriverResourcesLookup DriverResourcesLookup { get; set; }

	public static object DecodeTypedValue(datatype type, object value, IFormatProvider provider)
	{
		if (value == null)
		{
			return null;
		}
		return type switch
		{
			datatype.@bool => Convert.ToBoolean(value, provider), 
			datatype.@int => Convert.ToInt32(value, provider), 
			datatype.@double => Convert.ToDouble(value, provider), 
			datatype.@string => Convert.ToString(value, provider), 
			_ => throw new NotSupportedException(), 
		};
	}

	public static datatype DatatypeFromString(string datatypeString)
	{
		return datatypeString switch
		{
			"bool" => datatype.@bool, 
			"int" => datatype.@int, 
			"double" => datatype.@double, 
			"string" => datatype.@string, 
			"ptzcontrol" => datatype.ptzcontrol, 
			"ptzpresets" => datatype.ptzpresets, 
			_ => throw new ArgumentException("Unknown datatype string. Value: " + datatypeString, "datatypeString"), 
		};
	}

	public static object DecodeTypedValue(datatype type, string value)
	{
		if (value == null)
		{
			return null;
		}
		return type switch
		{
			datatype.@bool => Convert.ToBoolean(value, CultureInfo.InvariantCulture), 
			datatype.@string => value, 
			datatype.@int => Convert.ToInt32(value, CultureInfo.InvariantCulture), 
			datatype.@double => Convert.ToDouble(value, CultureInfo.InvariantCulture), 
			_ => throw new NotSupportedException(), 
		};
	}

	public static bool DecodeTypedBoolean(string value)
	{
		return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
	}

	public static int DecodeTypedInt32(string value)
	{
		return Convert.ToInt32(value, CultureInfo.InvariantCulture);
	}

	public static double DecodeTypedDouble(string value)
	{
		return Convert.ToDouble(value, CultureInfo.InvariantCulture);
	}

	public static string EncodeTypedValue(datatype type, object value)
	{
		switch (type)
		{
		case datatype.@bool:
			return Convert.ToString(value, CultureInfo.InvariantCulture).ToLowerInvariant();
		case datatype.@int:
		case datatype.@double:
		case datatype.@string:
			return Convert.ToString(value, CultureInfo.InvariantCulture);
		default:
			throw new NotSupportedException();
		}
	}

	public static string GetStaticSettingValue(definition definition, string hwid, string name)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		if (definition.properties == null)
		{
			return null;
		}
		return definition.FindSettingsByHwid(hwid)?.FindSettingByName(name)?.value;
	}

	public static string ResolveDisplayName(string displayNameReferenceId, CultureInfo culture)
	{
		string text = string.Empty;
		try
		{
			if (DriverResourcesLookup != null)
			{
				text = DriverResourcesLookup.GetDisplayNameByGuid(displayNameReferenceId, culture);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = DriverResourceHelper.GetDisplayNameByDisplayNameReferenceId(displayNameReferenceId, culture);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Failed to read display name! Error: {0}", ex2.Message));
		}
		return text;
	}

	public static string ResolveDisplayName(Guid displayNameReferenceId, CultureInfo culture)
	{
		return ResolveDisplayName(displayNameReferenceId.ToString().ToUpperInvariant(), culture);
	}

	public static string ResolveDisplayName(string displayNameReferenceId)
	{
		return ResolveDisplayName(displayNameReferenceId, Thread.CurrentThread.CurrentUICulture);
	}

	public static string ResolveDisplayName(Guid displayNameReferenceId)
	{
		return ResolveDisplayName(displayNameReferenceId.ToString().ToUpperInvariant(), CultureInfo.CurrentUICulture);
	}

	public static string ResolveDisplayName(displaynameType displayName)
	{
		if (displayName == null)
		{
			throw new ArgumentNullException("displayName");
		}
		string text = ResolveDisplayName(displayName.referenceid, CultureInfo.CurrentUICulture);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return displayName.Value;
	}

	public static string ResolveNameFromDisplayNameEnum(enumerationType enumType, string displayName)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		enumValueType[] value = enumType.value;
		foreach (enumValueType enumValueType2 in value)
		{
			if (string.Equals(ResolveDisplayName(enumValueType2.displayname), displayName, StringComparison.OrdinalIgnoreCase))
			{
				return enumValueType2.name;
			}
		}
		return displayName;
	}

	public static string ResolveDisplayNameEnum(enumerationType enumType, string name)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		enumValueType[] value = enumType.value;
		foreach (enumValueType enumValueType2 in value)
		{
			if (enumValueType2.name == name)
			{
				return ResolveDisplayName(enumValueType2.displayname);
			}
		}
		return name;
	}

	public static void InitializeSettings(MainDefinition definition, CapabilitiesCollection capabilities, DriverSettings settings)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		if (capabilities == null)
		{
			throw new ArgumentNullException("capabilities");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		if (capabilities.Count == 0)
		{
			return;
		}
		settingsType settingsType2 = null;
		if (definition.Base.properties != null)
		{
			settingsType2 = definition.Base.FindSettingsByHwid(capabilities[0].Hwid.Text);
		}
		foreach (CapabilityDefinition capability in capabilities)
		{
			if (settingsType2 != null)
			{
				settingType settingType2 = settingsType2.FindSettingByName(capability.Name);
				if (settingType2 != null)
				{
					settings.CreateOrUpdate(capability.Hwid.Text, capability.Name, settingType2.value, capability.TypeDefinition.ReferenceId.ToString());
				}
			}
		}
	}

	public static string RoundCapabilityValue(RoundingMethod method, CapabilityDefinition capabilityDefinition, double desiredValue)
	{
		if (capabilityDefinition == null)
		{
			throw new ArgumentNullException("capabilityDefinition");
		}
		TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
		if (activeTypeContent.IsEnumeration)
		{
			return RoundCapabilityValue(method, activeTypeContent.AsEnumeration, desiredValue);
		}
		if (activeTypeContent.IsData)
		{
			return RoundCapabilityValue(method, activeTypeContent.AsData, desiredValue);
		}
		throw new InvalidOperationException();
	}

	private static string RoundCapabilityValue(RoundingMethod method, EnumerationContentDefinition enumContent, double desiredValue)
	{
		double num = double.MaxValue;
		double num2 = double.MaxValue;
		double num3 = double.MinValue;
		enumValueType enumValueType2 = null;
		enumValueType enumValueType3 = null;
		enumValueType enumValueType4 = null;
		enumValueType enumValueType5 = null;
		enumValueType enumValueType6 = null;
		enumValueType[] activeEnumerationValues = enumContent.GetActiveEnumerationValues();
		foreach (enumValueType enumValueType7 in activeEnumerationValues)
		{
			try
			{
				if (string.Equals(enumValueType7.name, "min", StringComparison.OrdinalIgnoreCase))
				{
					enumValueType3 = enumValueType7;
					continue;
				}
				if (string.Equals(enumValueType7.name, "max", StringComparison.OrdinalIgnoreCase))
				{
					enumValueType4 = enumValueType7;
					continue;
				}
				double num4 = Convert.ToDouble(enumValueType7.name, CultureInfo.InvariantCulture);
				if (num4 < num2)
				{
					num2 = num4;
					enumValueType5 = enumValueType7;
				}
				if (num4 > num3)
				{
					num3 = num4;
					enumValueType6 = enumValueType7;
				}
				double num5 = Math.Abs(desiredValue - num4);
				bool flag = false;
				if (num5 < num)
				{
					switch (method)
					{
					case RoundingMethod.Nearest:
						flag = true;
						break;
					case RoundingMethod.LowerOrEqual:
						flag = num4 <= desiredValue;
						break;
					case RoundingMethod.HigherOrEqual:
						flag = num4 >= desiredValue;
						break;
					}
				}
				if (flag)
				{
					enumValueType2 = enumValueType7;
					num = num5;
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception)
			{
			}
		}
		switch (method)
		{
		case RoundingMethod.LowerOrEqual:
			if (enumValueType2 == null)
			{
				if (enumValueType3 != null)
				{
					enumValueType2 = enumValueType3;
				}
				else if (enumValueType5 != null)
				{
					enumValueType2 = enumValueType5;
				}
			}
			break;
		case RoundingMethod.HigherOrEqual:
			if (enumValueType2 == null)
			{
				if (enumValueType4 != null)
				{
					enumValueType2 = enumValueType4;
				}
				else if (enumValueType6 != null)
				{
					enumValueType2 = enumValueType6;
				}
			}
			break;
		}
		if (enumValueType2 == null)
		{
			return enumContent.Base.defaultvalue;
		}
		return enumValueType2.name;
	}

	private static string RoundCapabilityValue(RoundingMethod method, DataContentDefinition dataContent, double desiredValue)
	{
		datacontentType datacontentType2 = dataContent.Base;
		datatype dataType = dataContent.DataType;
		switch (dataContent.DataType)
		{
		case datatype.@int:
		{
			int num4 = Convert.ToInt32(desiredValue, CultureInfo.InvariantCulture);
			if (datacontentType2.resolution != null)
			{
				int num5 = (int)DecodeTypedValue(dataType, datacontentType2.resolution);
				if (num5 != 0)
				{
					int num6 = 0;
					if (datacontentType2.minvalue != null)
					{
						num6 = (int)DecodeTypedValue(dataType, datacontentType2.minvalue);
					}
					else if (datacontentType2.maxvalue != null)
					{
						num6 = (int)DecodeTypedValue(dataType, datacontentType2.maxvalue);
					}
					num4 = method switch
					{
						RoundingMethod.Nearest => Convert.ToInt32((double)num6 + (double)num5 * Math.Round((desiredValue - (double)num6) / (double)num5)), 
						RoundingMethod.LowerOrEqual => Convert.ToInt32((double)num6 + (double)num5 * Math.Floor((desiredValue - (double)num6) / (double)num5)), 
						RoundingMethod.HigherOrEqual => Convert.ToInt32((double)num6 + (double)num5 * Math.Ceiling((desiredValue - (double)num6) / (double)num5)), 
						_ => (int)DecodeTypedValue(dataType, datacontentType2.defaultvalue), 
					};
				}
			}
			if (datacontentType2.minvalue != null)
			{
				num4 = Math.Max((int)DecodeTypedValue(dataType, datacontentType2.minvalue), num4);
			}
			if (datacontentType2.maxvalue != null)
			{
				num4 = Math.Min((int)DecodeTypedValue(dataType, datacontentType2.maxvalue), num4);
			}
			return EncodeTypedValue(dataType, num4);
		}
		case datatype.@double:
		{
			double num = desiredValue;
			if (datacontentType2.resolution != null)
			{
				double num2 = (double)DecodeTypedValue(dataType, datacontentType2.resolution);
				if (num2 != 0.0)
				{
					double num3 = 0.0;
					if (datacontentType2.minvalue != null)
					{
						num3 = (double)DecodeTypedValue(dataType, datacontentType2.minvalue);
					}
					else if (datacontentType2.maxvalue != null)
					{
						num3 = (double)DecodeTypedValue(dataType, datacontentType2.maxvalue);
					}
					num = method switch
					{
						RoundingMethod.Nearest => num3 + num2 * Math.Round((desiredValue - num3) / num2), 
						RoundingMethod.LowerOrEqual => num3 + num2 * Math.Floor((desiredValue - num3) / num2), 
						RoundingMethod.HigherOrEqual => num3 + num2 * Math.Ceiling((desiredValue - num3) / num2), 
						_ => (double)DecodeTypedValue(dataType, datacontentType2.defaultvalue), 
					};
				}
			}
			if (datacontentType2.minvalue != null)
			{
				num = Math.Max((double)DecodeTypedValue(dataType, datacontentType2.minvalue), num);
			}
			if (datacontentType2.maxvalue != null)
			{
				num = Math.Min((double)DecodeTypedValue(dataType, datacontentType2.maxvalue), num);
			}
			return EncodeTypedValue(dataType, num);
		}
		default:
			throw new ArgumentException("Unknown data type in data content.", "dataContent");
		}
	}

	public static bool IsPtzEnabled(DeviceDefinition definition, DriverSettings deviceSettings)
	{
		return GetPtzSupportFromReferenceId(definition, deviceSettings, TypeReferenceIds.PtzEnabled);
	}

	private static bool GetPtzSupportFromReferenceId(DeviceDefinition definition, DriverSettings deviceSettings, Guid ptzCapabilityGuid)
	{
		if (!GetPtzSettingFromReferenceId(definition, deviceSettings, ptzCapabilityGuid, out var _, out var _, out var value))
		{
			return false;
		}
		try
		{
			return (bool)DecodeTypedValue(datatype.@bool, value);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return false;
		}
	}

	private static bool GetPtzSettingFromReferenceId(DeviceDefinition def, DriverSettings deviceSettings, Guid ptzCapabilityGuid, out string capabilityName, out string displayName, out string value)
	{
		value = null;
		capabilityName = null;
		displayName = null;
		if (def.PtzSettings == null)
		{
			return false;
		}
		CapabilityDefinition capabilityDefinition = def.PtzSettings.Capabilities.FindByTypeReferenceId(ptzCapabilityGuid);
		if (capabilityDefinition == null)
		{
			return false;
		}
		value = deviceSettings.Get(def.PtzSettings.Hwid.Text, capabilityDefinition.Name);
		if (value == null)
		{
			return false;
		}
		capabilityName = capabilityDefinition.Name;
		displayName = capabilityDefinition.TypeDefinition.DisplayNameResolved;
		return true;
	}

	public static DevicePresetConfiguration GetDevicePresetConfiguration(DeviceDefinition deviceDefinition, DriverSettings deviceSettings)
	{
		if (deviceDefinition == null)
		{
			throw new ArgumentNullException("deviceDefinition");
		}
		if (deviceSettings == null)
		{
			throw new ArgumentNullException("deviceSettings");
		}
		if (deviceDefinition.PtzSettings == null)
		{
			throw new NotSupportedException("Definition does not contain a Ptz settings section.");
		}
		CapabilityDefinition capabilityDefinition = deviceDefinition.PtzSettings.Capabilities.FindByTypeReferenceId(TypeReferenceIds.PtzPresetConfiguration);
		if (capabilityDefinition == null)
		{
			throw new NotSupportedException("Definition does not contain a preset configuration capability.");
		}
		return DevicePresetConfiguration.LoadXml(deviceSettings.Get(capabilityDefinition.Hwid.Text, capabilityDefinition.Name) ?? throw new SettingNotFoundException("No preset configuration setting was found for the device."));
	}

	public static string GetCapabilityDisplayValue(HardwareDefinition hardwareDefinition, DriverSettings settings, Guid capabilityTypeReferenceId)
	{
		if (hardwareDefinition == null)
		{
			throw new ArgumentNullException("hardwareDefinition");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		CapabilityDefinition capabilityDefinition = hardwareDefinition.Capabilities.FindByTypeReferenceId(capabilityTypeReferenceId);
		if (capabilityDefinition == null)
		{
			return null;
		}
		string text = settings.Get(capabilityDefinition.Hwid.Text, capabilityDefinition.Name);
		if (text == null)
		{
			return null;
		}
		TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
		if (activeTypeContent == null)
		{
			return null;
		}
		if (activeTypeContent.IsData)
		{
			return text;
		}
		if (activeTypeContent.IsEnumeration && activeTypeContent.AsEnumeration.Base.value != null)
		{
			enumValueType[] activeEnumerationValues = activeTypeContent.AsEnumeration.GetActiveEnumerationValues();
			foreach (enumValueType enumValueType2 in activeEnumerationValues)
			{
				if (string.Equals(enumValueType2.name, text, StringComparison.Ordinal))
				{
					return enumValueType2.displayname.Resolve();
				}
			}
		}
		return null;
	}

	public static string GetCapabilityValue(HardwareDefinition hardwareDefinition, DriverSettings settings, Guid capabilityTypeReferenceId)
	{
		if (hardwareDefinition == null)
		{
			throw new ArgumentNullException("hardwareDefinition");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		CapabilityDefinition capabilityDefinition = hardwareDefinition.Capabilities.FindByTypeReferenceId(capabilityTypeReferenceId);
		if (capabilityDefinition == null)
		{
			return null;
		}
		return settings.Get(capabilityDefinition.Hwid.Text, capabilityDefinition.Name);
	}

	public static void WriteProperties(XmlReader reader, XmlWriter writer, DriverSettings settings)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		string xml = reader.ReadOuterXml();
		DriverSettings driverSettings = new DriverSettings(xml);
		string[] sectionNames = settings.GetSectionNames();
		foreach (string section in sectionNames)
		{
			string[] sectionSettingNames = settings.GetSectionSettingNames(section);
			foreach (string name in sectionSettingNames)
			{
				string value = settings.Get(section, name);
				driverSettings.Update(section, name, value);
			}
		}
		xml = driverSettings.SaveAsXml();
		writer.WriteRaw(xml);
	}

	public static StreamDefinition GetDefaultStreamDefinition(DeviceDefinition deviceDefinition, DriverSettings settings)
	{
		if (deviceDefinition == null)
		{
			throw new ArgumentNullException("deviceDefinition");
		}
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		StreamDefinition streamDefinition = null;
		CapabilityDefinition capabilityDefinition = deviceDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.DefaultStream);
		if (capabilityDefinition != null)
		{
			int num = Convert.ToInt32(settings.Get(deviceDefinition.Hwid.Text, capabilityDefinition.Name), CultureInfo.InvariantCulture);
			foreach (StreamDefinition stream in deviceDefinition.Streams)
			{
				if (stream.Hwid.StreamIndex == num)
				{
					streamDefinition = stream;
					break;
				}
			}
		}
		if (streamDefinition == null && deviceDefinition.Streams.Count > 0)
		{
			streamDefinition = deviceDefinition.Streams[0];
		}
		return streamDefinition;
	}

	public static string InsertTypeAttributeInHardwareSettingXml(string settingXml, MainDefinition mainDefinition, bool refresh)
	{
		if (mainDefinition == null)
		{
			throw new ArgumentNullException("mainDefinition");
		}
		if (!refresh)
		{
			using StringReader textReader = new StringReader(settingXml);
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			bool flag = true;
			string xpath = "//setting";
			foreach (XPathNavigator item in xPathNavigator.Select(xpath))
			{
				if (item.SelectSingleNode("@typeid") == null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return settingXml;
			}
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(settingXml);
		foreach (XmlNode item2 in xmlDocument.SelectNodes("/properties/settings"))
		{
			Hwid hwid = Hwid.ParseHwid(item2.SelectSingleNode("@hwid").Value);
			CapabilitiesCollection capabilitiesByHwid = mainDefinition.GetCapabilitiesByHwid(hwid);
			foreach (XmlNode item3 in item2.SelectNodes("setting"))
			{
				if (item3.SelectSingleNode("@typeid") == null)
				{
					string settingName = item3.SelectSingleNode("name").InnerXml;
					CapabilityDefinition capabilityDefinition = capabilitiesByHwid.FirstOrDefault((CapabilityDefinition c) => string.Equals(c.Name, settingName, StringComparison.Ordinal));
					if (capabilityDefinition == null)
					{
						throw new InvalidSettingException(string.Format(CultureInfo.InvariantCulture, "Capability for setting not found: Hwid: {0}; Setting name: {1} not found in definition.", hwid.Text, settingName));
					}
					XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("typeid");
					xmlAttribute.InnerText = capabilityDefinition.TypeDefinition.ReferenceId.ToString();
					item3.Attributes.Append(xmlAttribute);
				}
			}
		}
		return xmlDocument.InnerXml;
	}

	public static string InsertTypeAttributeInHardwareSettingXml(string settingXml, string definitionXml, bool refresh)
	{
		if (!refresh)
		{
			XPathNavigator xPathNavigator = new XPathDocument(new StringReader(settingXml)).CreateNavigator();
			bool flag = true;
			string xpath = "//setting";
			foreach (XPathNavigator item in xPathNavigator.Select(xpath))
			{
				if (item.SelectSingleNode("@typeid") == null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return settingXml;
			}
		}
		XPathNavigator xPathNavigator2 = null;
		xPathNavigator2 = new XPathDocument(new StringReader(definitionXml)).CreateNavigator();
		Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
		string xpath2 = "//capabilities";
		foreach (XPathNavigator item2 in xPathNavigator2.Select(xpath2))
		{
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			string value = item2.SelectSingleNode("@hwid").Value;
			foreach (XPathNavigator item3 in item2.Select("capability"))
			{
				string value2 = item3.SelectSingleNode("name").Value;
				string value3 = item3.SelectSingleNode("@type").Value;
				dictionary2.Add(value2, value3);
			}
			dictionary.Add(value, dictionary2);
		}
		Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
		string xpath3 = "/definition/types/type";
		foreach (XPathNavigator item4 in xPathNavigator2.Select(xpath3))
		{
			string value4 = item4.SelectSingleNode("@name").Value;
			string value5 = item4.SelectSingleNode("@referenceid").Value.ToUpperInvariant();
			dictionary3.Add(value4, value5);
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(settingXml);
		foreach (XmlNode item5 in xmlDocument.SelectNodes("/properties/settings"))
		{
			string value6 = item5.SelectSingleNode("@hwid").Value;
			Dictionary<string, string> value7 = new Dictionary<string, string>();
			if (dictionary.TryGetValue(value6, out value7))
			{
				foreach (XmlNode item6 in item5.SelectNodes("setting"))
				{
					if (item6.SelectSingleNode("@typeid") == null)
					{
						string innerXml = item6.SelectSingleNode("name").InnerXml;
						if (!value7.TryGetValue(innerXml, out var value8))
						{
							throw new InvalidSettingException(string.Format(CultureInfo.InvariantCulture, "Typename for setting: {0} not found in definition.", innerXml));
						}
						if (!dictionary3.TryGetValue(value8, out var value9))
						{
							throw new InvalidSettingException(string.Format(CultureInfo.InvariantCulture, "Typereferenceid for typename: {0} not found in definition.", value8));
						}
						XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("typeid");
						xmlAttribute.InnerText = value9;
						item6.Attributes.Append(xmlAttribute);
					}
				}
				continue;
			}
			throw new InvalidSettingException(string.Format(CultureInfo.InvariantCulture, "Hwid: {0} not found in definition.", value6));
		}
		return xmlDocument.InnerXml;
	}

	public static string RemoveContentForDriverReference(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		string b = TypeReferenceIds.ProductId.ToString();
		XmlNode xmlNode = xmlDocument.SelectSingleNode("definition/types");
		foreach (XmlNode item in xmlNode.SelectNodes("type"))
		{
			XmlAttribute xmlAttribute = item.Attributes["referenceid"];
			if (xmlAttribute != null && !string.Equals(xmlAttribute.Value, b, StringComparison.OrdinalIgnoreCase))
			{
				xmlNode.RemoveChild(item);
			}
		}
		foreach (XmlNode item2 in xmlDocument.SelectNodes("//*/ptzsettings"))
		{
			item2.ParentNode.RemoveChild(item2);
		}
		foreach (XmlNode item3 in xmlDocument.SelectNodes("//*/device/supportedstreamtypes"))
		{
			item3.ParentNode.RemoveChild(item3);
		}
		foreach (XmlNode item4 in xmlDocument.SelectNodes("//*/device/streams"))
		{
			item4.ParentNode.RemoveChild(item4);
		}
		foreach (XmlNode item5 in xmlDocument.SelectNodes("//*/capabilities"))
		{
			foreach (XmlNode item6 in item5.SelectNodes("capability"))
			{
				XmlAttribute xmlAttribute2 = item6.Attributes["type"];
				if (xmlAttribute2 != null && !xmlAttribute2.Value.Equals("EventPreconfiguredType", StringComparison.OrdinalIgnoreCase))
				{
					item5.RemoveChild(item6);
				}
			}
		}
		return xmlDocument.OuterXml;
	}

	public static string CreateUniqueHardwareDefinitionKey(string xml)
	{
		return CreateC14NDigest(xml);
	}

	private static string CreateC14NDigest(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		string securityUrl = xmlDocument?.BaseURI;
		XmlResolver resolver = new XmlSecureResolver(new XmlUrlResolver(), securityUrl);
		XmlDsigExcC14NTransform xmlDsigExcC14NTransform = new XmlDsigExcC14NTransform();
		xmlDsigExcC14NTransform.Resolver = resolver;
		xmlDsigExcC14NTransform.LoadInput(xmlDocument);
		return Convert.ToBase64String(xmlDsigExcC14NTransform.GetDigestedOutput(SHA256.Create()));
	}

	public static string RemoveHardwareSpecificInfoFromHardwareDefinitionXml(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("definition/properties");
		if (xmlNode != null)
		{
			foreach (XmlNode item in xmlNode.SelectNodes("settings"))
			{
				XmlNode namedItem = item.Attributes.GetNamedItem("hwid");
				if (namedItem != null && Regex.IsMatch(namedItem.Value, "^(hardware|hardwareptz|device:\\d+\\.\\d+|deviceptz:\\d+\\.\\d+|stream:\\d+\\.\\d+\\.\\d+|event:\\d+\\..*)$"))
				{
					xmlNode.RemoveChild(item);
				}
			}
		}
		foreach (XmlNode item2 in xmlDocument.SelectNodes("//*/implementation"))
		{
			string text = string.Empty;
			XmlNode xmlNode3 = item2.SelectSingleNode("assemblyfilename");
			if (xmlNode3 != null)
			{
				string innerText = xmlNode3.InnerText;
				text = Path.GetFileName(innerText);
				if (!string.IsNullOrEmpty(innerText))
				{
					xmlNode3.InnerText = text;
				}
			}
			XmlNode xmlNode4 = item2.SelectSingleNode("tag");
			if (xmlNode4 == null)
			{
				continue;
			}
			string innerText2 = xmlNode4.InnerText;
			if (!string.IsNullOrEmpty(innerText2))
			{
				if (text.Contains("MIPDriverFramework"))
				{
					xmlNode4.InnerText = innerText2;
				}
				else
				{
					xmlNode4.InnerText = Path.GetFileName(innerText2);
				}
			}
		}
		return xmlDocument.InnerXml;
	}

	public static string GetDefaultOrFirstAudioEncodingFromDefinition(CapabilitiesCollection capabilities)
	{
		if (capabilities == null)
		{
			return string.Empty;
		}
		CapabilityDefinition capabilityDefinition = capabilities.FindByTypeReferenceId(TypeReferenceIds.AudioCodec);
		if (capabilityDefinition == null)
		{
			return string.Empty;
		}
		TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
		if (activeTypeContent == null)
		{
			return string.Empty;
		}
		if (!DoesStringContainAac(activeTypeContent.DefaultValue))
		{
			return activeTypeContent.DefaultValue;
		}
		if (activeTypeContent.IsEnumeration)
		{
			enumValueType[] activeEnumerationValues = activeTypeContent.AsEnumeration.GetActiveEnumerationValues();
			foreach (enumValueType enumValueType2 in activeEnumerationValues)
			{
				if (!IsValueTypeAac(enumValueType2))
				{
					return enumValueType2.name;
				}
			}
		}
		return string.Empty;
	}

	public static string RemoveIdsFromHardwareDefinitionXml(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		((XmlElement)xmlDocument.SelectSingleNode("definition/hardware")).RemoveAttribute("ID");
		foreach (XmlNode item in xmlDocument.SelectNodes("definition/hardware/devices/device"))
		{
			((XmlElement)item).RemoveAttribute("ID");
			foreach (XmlNode item2 in item.SelectNodes("streams/stream"))
			{
				((XmlElement)item2).RemoveAttribute("ID");
			}
		}
		return xmlDocument.InnerXml;
	}

	public static string InsertDriverNumberInHardwareDefinitionXml(string xml, int driverNumber)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("definition/driver");
		XmlNode refChild = xmlDocument.SelectSingleNode("definition/driver/name");
		XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "number", null);
		xmlNode2.InnerText = driverNumber.ToString(CultureInfo.InvariantCulture);
		xmlNode.InsertAfter(xmlNode2, refChild);
		return xmlDocument.InnerXml;
	}

	public static string InsertDevicePackVersion(string xml, string devicePackVersion)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("definition/driver/version");
		xmlNode.InnerText = "DevicePack: " + devicePackVersion + ", " + xmlNode.InnerText;
		return xmlDocument.InnerXml;
	}

	public static string ExpandRepeatCount(string definitionXml)
	{
		bool flag = false;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(definitionXml);
		XmlNode xmlNode = xmlDocument.DocumentElement["hardware"];
		if (xmlNode == null)
		{
			return definitionXml;
		}
		XmlNode xmlNode2 = xmlNode["devices"];
		if (xmlNode2 == null || xmlNode2.ChildNodes == null)
		{
			return definitionXml;
		}
		List<XmlNode> list = new List<XmlNode>();
		foreach (XmlNode childNode in xmlNode2.ChildNodes)
		{
			if (childNode.Attributes == null || childNode.Attributes["repeatcount"] == null)
			{
				continue;
			}
			flag = true;
			XmlAttribute xmlAttribute = childNode.Attributes["repeatcount"];
			int num = Convert.ToInt32(xmlAttribute.InnerText, CultureInfo.InvariantCulture);
			if (num > 0)
			{
				childNode.Attributes.Remove(xmlAttribute);
				for (int i = 1; i < num; i++)
				{
					XmlNode xmlNode4 = childNode.CloneNode(deep: true);
					DeviceHwid deviceHwid = new DeviceHwid(xmlNode4["capabilities"].Attributes["hwid"].InnerText);
					deviceHwid = new DeviceHwid(deviceHwid.Type, i);
					xmlNode4["capabilities"].Attributes["hwid"].InnerText = deviceHwid.Text;
					XmlNode xmlNode5 = xmlNode4["ptzsettings"];
					if (xmlNode5 != null)
					{
						XmlElement xmlElement = xmlNode5["capabilities"];
						DevicePtzHwid devicePtzHwid = new DevicePtzHwid(xmlElement.Attributes["hwid"].InnerText);
						devicePtzHwid = new DevicePtzHwid(deviceHwid.Type, i);
						xmlElement.Attributes["hwid"].InnerText = devicePtzHwid.Text;
					}
					XmlNode xmlNode6 = xmlNode4["streams"];
					if (xmlNode6 != null && xmlNode6.ChildNodes != null)
					{
						foreach (XmlNode childNode2 in xmlNode6.ChildNodes)
						{
							if (string.Equals(childNode2.Name, "stream", StringComparison.Ordinal))
							{
								StreamHwid streamHwid = new StreamHwid(childNode2["capabilities"].Attributes["hwid"].InnerText);
								streamHwid = new StreamHwid(deviceHwid.Type, deviceHwid.Index, streamHwid.StreamIndex);
								childNode2["capabilities"].Attributes["hwid"].InnerText = streamHwid.Text;
							}
						}
					}
					childNode.ParentNode.AppendChild(xmlNode4);
				}
			}
			else if (num == 0)
			{
				list.Add(childNode);
			}
		}
		foreach (XmlNode item in list)
		{
			xmlNode2.RemoveChild(item);
		}
		if (flag)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			XmlTextWriter w = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
			xmlDocument.Save(w);
			return stringBuilder.ToString();
		}
		return definitionXml;
	}

	public static string CheckMacAddressAndEnsureUppercase(string definitionXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(definitionXml);
		XmlNode namedItem = (xmlDocument.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "definition/types/type[translate(@referenceid, 'abcdef', 'ABCDEF')='{0}']", TypeReferenceIds.MacAddress.ToString().ToUpperInvariant())) ?? throw new InvalidDefinitionException("The definition xml does not contain a mac address type.")).Attributes.GetNamedItem("name");
		if (namedItem == null)
		{
			throw new InvalidDefinitionException("The definition xml is invalid. The mac address type does not have a name attribute.");
		}
		XmlNode xmlNode = xmlDocument.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "definition/hardware/capabilities[@hwid='hardware']/capability[@type='{0}']/name", namedItem.Value));
		if (xmlNode == null || string.IsNullOrEmpty(xmlNode.InnerText))
		{
			throw new InvalidDefinitionException("The definition xml is invalid. The mac address capability name node could not be found.");
		}
		XmlNode obj = xmlDocument.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "definition/properties/settings[@hwid='hardware']/setting[name='{0}']/value", xmlNode.InnerText)) ?? throw new InvalidDefinitionException("The definition xml is invalid. The mac address setting value node could not be found.");
		if (!Regex.IsMatch(obj.InnerText, "[0-9a-f]{2}([:-]?[0-9a-f]{2}){5}", RegexOptions.IgnoreCase))
		{
			throw new InvalidDefinitionException("The definition xml is invalid. The mac address setting is not a valid mac address.");
		}
		if (Regex.IsMatch(obj.InnerText, "[0]{2}([:-]?[0]{2}){5}", RegexOptions.IgnoreCase))
		{
			throw new InvalidDefinitionException("The definition xml is invalid. The mac address setting is not a valid mac address (all zeroes).");
		}
		obj.InnerText = obj.InnerText.ToUpperInvariant();
		return xmlDocument.OuterXml;
	}

	public static string EnsureAllSettingsInitialized(string definitionXml)
	{
		List<XmlNode> list = new List<XmlNode>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(definitionXml);
		foreach (XmlNode item in xmlDocument.SelectNodes("//capabilities/capability"))
		{
			string value = item.ParentNode.Attributes["hwid"].Value;
			string innerText = item["name"].InnerText;
			XmlNode xmlNode2 = EnsureSettingsNode(xmlDocument, value);
			string xpath = string.Format(CultureInfo.InvariantCulture, "setting[name='{0}']", innerText);
			if (xmlNode2.SelectSingleNode(xpath) == null)
			{
				try
				{
					EnsureSettingInitialized(xmlDocument, item);
				}
				catch (DependentSettingMissingException)
				{
					list.Add(item);
				}
			}
		}
		while (list.Count > 0)
		{
			int count = list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				XmlNode capabilityNode = list[i];
				try
				{
					EnsureSettingInitialized(xmlDocument, capabilityNode);
					list.RemoveAt(i);
				}
				catch (DependentSettingMissingException)
				{
				}
			}
			if (list.Count != count)
			{
				continue;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (XmlNode item2 in list)
			{
				try
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} ({1}); ", item2["name"].InnerText, item2.Attributes["type"].Value);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", item2.InnerXml);
				}
			}
			throw new SchemaViolationException(string.Format(CultureInfo.InvariantCulture, "Unable to initialize all settings. The definition may have types containing circular references in selects. Capabilities: [{0}], Xml: {1}", stringBuilder.ToString(), definitionXml));
		}
		return xmlDocument.OuterXml;
	}

	private static void EnsureSettingInitialized(XmlDocument doc, XmlNode capabilityNode)
	{
		string value = capabilityNode.ParentNode.Attributes["hwid"].Value;
		string value2 = capabilityNode.Attributes["type"].Value;
		string xpath = string.Format(CultureInfo.InvariantCulture, "/definition/types/type[@name='{0}']", value2);
		XmlNode xmlNode = doc.SelectSingleNode(xpath);
		if (xmlNode == null)
		{
			throw new SchemaViolationException(string.Format(CultureInfo.InvariantCulture, "The referenced type with the name '{0}' is missing in the definition xml.", value2));
		}
		XmlNode activeTypedefNode = GetActiveTypedefNode(doc, capabilityNode, xmlNode);
		if (activeTypedefNode == null)
		{
			throw new DependentSettingMissingException();
		}
		XmlNode activeTypeDefContentNode = GetActiveTypeDefContentNode(doc, capabilityNode, activeTypedefNode);
		if (activeTypeDefContentNode == null)
		{
			throw new DependentSettingMissingException();
		}
		if (activeTypeDefContentNode["defaultvalue"] == null)
		{
			return;
		}
		string innerText = capabilityNode["name"].InnerText;
		string text = activeTypeDefContentNode["defaultvalue"].InnerText;
		try
		{
			datatype datatype2 = DatatypeFromString(activeTypedefNode.Attributes["datatype"].Value);
			switch (datatype2)
			{
			case datatype.@bool:
			case datatype.@int:
			case datatype.@double:
			case datatype.@string:
			{
				object value3 = DecodeTypedValue(datatype2, text);
				text = EncodeTypedValue(datatype2, value3);
				break;
			}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		XmlNode xmlNode2 = EnsureSettingsNode(doc, value);
		XmlNode xmlNode3 = doc.CreateNode(XmlNodeType.Element, "setting", null);
		xmlNode2.AppendChild(xmlNode3);
		XmlNode xmlNode4 = doc.CreateNode(XmlNodeType.Element, "name", null);
		xmlNode4.InnerText = innerText;
		xmlNode3.AppendChild(xmlNode4);
		XmlNode xmlNode5 = doc.CreateNode(XmlNodeType.Element, "value", null);
		xmlNode5.InnerText = text;
		xmlNode3.AppendChild(xmlNode5);
	}

	private static XmlNode EnsurePropertiesNode(XmlDocument doc)
	{
		XmlNode xmlNode = doc.SelectSingleNode("/definition/properties");
		if (xmlNode == null)
		{
			xmlNode = doc.CreateNode(XmlNodeType.Element, "properties", null);
			string xpath = "/definition/driver";
			XmlNode xmlNode2 = doc.SelectSingleNode(xpath);
			if (xmlNode2 == null)
			{
				throw new SchemaViolationException("The 'definition/driver' node is missing in the definition xml.");
			}
			doc.DocumentElement.InsertAfter(xmlNode, xmlNode2);
		}
		return xmlNode;
	}

	private static XmlNode EnsureSettingsNode(XmlDocument doc, string hwid)
	{
		XmlNode xmlNode = EnsurePropertiesNode(doc);
		string xpath = string.Format(CultureInfo.InvariantCulture, "settings[@hwid='{0}']", hwid);
		XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
		if (xmlNode2 == null)
		{
			xmlNode2 = doc.CreateNode(XmlNodeType.Element, "settings", null);
			XmlAttribute xmlAttribute = doc.CreateAttribute("hwid");
			xmlAttribute.Value = hwid;
			xmlNode2.Attributes.Append(xmlAttribute);
			xmlNode.AppendChild(xmlNode2);
		}
		return xmlNode2;
	}

	private static XmlNode GetActiveTypeDefContentNode(XmlDocument doc, XmlNode capabilityNode, XmlNode typedefNode)
	{
		foreach (XmlNode item in typedefNode.SelectNodes("*[name()='data' or name()='enumeration']"))
		{
			XmlAttribute selectAttribute = item.Attributes.GetNamedItem("select") as XmlAttribute;
			if (SelectResult(doc, capabilityNode, selectAttribute))
			{
				return item;
			}
		}
		return null;
	}

	private static XmlNode GetActiveTypedefNode(XmlDocument doc, XmlNode capabilityNode, XmlNode typeNode)
	{
		foreach (XmlNode item in typeNode.SelectNodes("typedef"))
		{
			XmlAttribute selectAttribute = item.Attributes.GetNamedItem("select") as XmlAttribute;
			if (SelectResult(doc, capabilityNode, selectAttribute))
			{
				return item;
			}
		}
		return null;
	}

	private static bool SelectResult(XmlDocument doc, XmlNode capabilityNode, XmlAttribute selectAttribute)
	{
		string value = capabilityNode.ParentNode.Attributes["hwid"].Value;
		if (selectAttribute == null || selectAttribute.Value.Trim().Length == 0)
		{
			return true;
		}
		string text = selectAttribute.Value.Trim();
		while (true)
		{
			Match match = Regex.Match(text, "(\\{(?<innerExpression>[^}]*)})");
			if (!match.Success)
			{
				break;
			}
			string value2 = match.Groups["innerExpression"].Value;
			Match match2 = Regex.Match(value2, "^((?<hwid>[a-z0-9:\\.]+)\\|)?(?<capability>[a-z0-9]+)=(?<value>.*)$", RegexOptions.IgnoreCase);
			if (match2.Success)
			{
				string value3 = match2.Groups["capability"].Value;
				string value4 = match2.Groups["value"].Value;
				Group obj = match2.Groups["hwid"];
				string arg = ((!obj.Success) ? value : obj.Value);
				string newValue = "false";
				string xpath = string.Format(CultureInfo.InvariantCulture, "definition/properties/settings[@hwid='{0}']/setting[name='{1}']", arg, value3);
				XmlNode xmlNode = doc.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					string innerText = xmlNode["value"].InnerText;
					newValue = ((innerText != null && innerText == value4) ? "true" : "false");
				}
				text = text.Replace(match.Value, newValue);
			}
			else
			{
				string newValue2 = ((capabilityNode.SelectSingleNode(value2) != null) ? "true" : "false");
				text = text.Replace(match.Value, newValue2);
			}
		}
		try
		{
			lock (JScriptEvaluator.Instance)
			{
				return (bool)JScriptEvaluator.Instance.Evaluate(text);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return false;
		}
	}

	public static string GetPropertiesXml(DriverSettings driverSettings, string hardwareDefinitionXml)
	{
		if (driverSettings == null)
		{
			throw new ArgumentNullException("driverSettings");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(hardwareDefinitionXml);
		foreach (XmlNode item in (xmlDocument.SelectNodes("/definition/properties/settings[@hwid]") ?? throw new InvalidOperationException()).OfType<XmlNode>())
		{
			foreach (XmlNode item2 in item.SelectNodes("setting").OfType<XmlNode>())
			{
				driverSettings.CreateOrUpdate(item.Attributes["hwid"].Value, item2["name"].InnerText, item2["value"].InnerText, "");
			}
		}
		XmlDocument xmlDocument2 = new XmlDocument();
		xmlDocument2.LoadXml(driverSettings.SaveAsXml());
		return xmlDocument2.SelectSingleNode("properties").OuterXml;
	}

	public static string GetDefinitionXml(string hardwareDefinitionXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(hardwareDefinitionXml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/definition/properties");
		xmlNode?.ParentNode.RemoveChild(xmlNode);
		return xmlDocument.InnerXml;
	}

	public static void RemoveCapabilities(DriverSettings settings, MainDefinition definition)
	{
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		CapabilityDefinition capabilityDefinition;
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.InternalEventServerIP)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.InternalEventServerPort)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AlertEventServerIP)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AlertEventServerPort)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.SmtpEventServerIP)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.SmtpEventServerPort)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AttachmentSavePath)) != null)
		{
			settings.Remove(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name);
		}
	}

	public static DriverSettings PrepareEventSettings(MainDefinition definition, int port, string host, int alertPort)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		DriverSettings driverSettings = new DriverSettings();
		string value = IPAddress.Loopback.ToString();
		string value2 = "25";
		string value3 = "C:\\DummyEventData";
		CapabilityDefinition capabilityDefinition;
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.InternalEventServerIP)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, IPAddress.Loopback.ToString(), TypeReferenceIds.InternalEventServerIP.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.InternalEventServerPort)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, port.ToString(CultureInfo.InvariantCulture), TypeReferenceIds.InternalEventServerPort.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AlertEventServerIP)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, host, TypeReferenceIds.AlertEventServerIP.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AlertEventServerPort)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, alertPort.ToString(CultureInfo.InvariantCulture), TypeReferenceIds.AlertEventServerPort.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.SmtpEventServerIP)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, value, TypeReferenceIds.SmtpEventServerIP.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.SmtpEventServerPort)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, value2, TypeReferenceIds.SmtpEventServerPort.ToString());
		}
		if ((capabilityDefinition = definition.HardwareDefinition.Capabilities.FindByTypeReferenceId(TypeReferenceIds.AttachmentSavePath)) != null)
		{
			driverSettings.CreateOrUpdate(definition.HardwareDefinition.Hwid.Text, capabilityDefinition.Name, value3, TypeReferenceIds.AttachmentSavePath.ToString());
		}
		return driverSettings;
	}

	public static bool IsValueTypeAac(enumValueType valueType)
	{
		if (valueType == null)
		{
			return false;
		}
		return DoesStringContainAac(valueType.displayname.Resolve());
	}

	public static bool IsSettingAac(SettingEntry settingEntry)
	{
		if (settingEntry == null)
		{
			return false;
		}
		return DoesStringContainAac(settingEntry.Value);
	}

	public static bool IsEnumerationValueAac(EnumerationValue enumerationValue)
	{
		if (enumerationValue == null)
		{
			return false;
		}
		return DoesStringContainAac(enumerationValue.DisplayNameResolved);
	}

	private static bool DoesStringContainAac(string s)
	{
		return s.IndexOf("AAC", StringComparison.OrdinalIgnoreCase) >= 0;
	}
}
