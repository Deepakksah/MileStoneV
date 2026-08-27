using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.Definition;

public class DevicePresetConfiguration
{
	private PresetType presetType;

	private Internal @internal;

	private Absolute absolute;

	public PresetType PresetType
	{
		[DebuggerStepThrough]
		get
		{
			return presetType;
		}
	}

	public Internal Internal
	{
		[DebuggerStepThrough]
		get
		{
			return @internal;
		}
	}

	public Absolute Absolute
	{
		[DebuggerStepThrough]
		get
		{
			return absolute;
		}
	}

	public bool HasInternalPresets
	{
		get
		{
			if (presetType != PresetType.Internal)
			{
				return presetType == PresetType.Both;
			}
			return true;
		}
	}

	public bool HasAbsolutePresets
	{
		get
		{
			if (presetType != PresetType.Absolute)
			{
				return presetType == PresetType.Both;
			}
			return true;
		}
	}

	private DevicePresetConfiguration()
	{
	}

	public static DevicePresetConfiguration LoadXml(string xml)
	{
		DevicePresetConfiguration devicePresetConfiguration = new DevicePresetConfiguration();
		using (StringReader textReader = new StringReader(xml))
		{
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			switch ((xPathNavigator.SelectSingleNode("/ptzpresets/presettype") ?? throw new XmlException("The 'presettype' was not specified.")).Value)
			{
			case "both":
				devicePresetConfiguration.presetType = PresetType.Both;
				break;
			case "internal":
				devicePresetConfiguration.presetType = PresetType.Internal;
				break;
			case "absolute":
				devicePresetConfiguration.presetType = PresetType.Absolute;
				break;
			default:
				throw new XmlException("The 'presettype' has an invalid value.");
			}
			if (devicePresetConfiguration.presetType == PresetType.Internal || devicePresetConfiguration.presetType == PresetType.Both)
			{
				XPathNavigator obj = xPathNavigator.SelectSingleNode("/ptzpresets/internal") ?? throw new XmlException("The 'internal' xml node is missing.");
				devicePresetConfiguration.@public = new Internal();
				XPathNavigator xPathNavigator2 = obj.SelectSingleNode("isnumbered");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'isnumbered' node is missing.");
				}
				devicePresetConfiguration.Internal.IsNumbered = xPathNavigator2.ValueAsBoolean;
				xPathNavigator2 = obj.SelectSingleNode("minnumber");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'minnumber' node is missing.");
				}
				devicePresetConfiguration.Internal.MinimumNumber = xPathNavigator2.ValueAsInt;
				xPathNavigator2 = obj.SelectSingleNode("maxnumber");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'maxnumber' node is missing.");
				}
				devicePresetConfiguration.Internal.MaximumNumber = xPathNavigator2.ValueAsInt;
				xPathNavigator2 = obj.SelectSingleNode("formatexpression");
				if (xPathNavigator2 != null)
				{
					devicePresetConfiguration.Internal.FormatExpression = xPathNavigator2.Value;
				}
				xPathNavigator2 = obj.SelectSingleNode("canloadfromdevice");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'canloadfromdevice' node is missing.");
				}
				devicePresetConfiguration.Internal.CanLoadFromDevice = xPathNavigator2.ValueAsBoolean;
				xPathNavigator2 = obj.SelectSingleNode("cansetpreset");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'cansetpreset' node is missing.");
				}
				devicePresetConfiguration.Internal.CanSetPreset = xPathNavigator2.ValueAsBoolean;
				xPathNavigator2 = obj.SelectSingleNode("speedsupport");
				if (xPathNavigator2 == null)
				{
					throw new XmlException("The 'speedsupport' node is missing.");
				}
				devicePresetConfiguration.Internal.SpeedSupport = xPathNavigator2.ValueAsBoolean;
			}
			if (devicePresetConfiguration.presetType == PresetType.Absolute || devicePresetConfiguration.presetType == PresetType.Both)
			{
				XPathNavigator obj2 = xPathNavigator.SelectSingleNode("/ptzpresets/absolute") ?? throw new XmlException("The 'absolute' xml node is missing.");
				devicePresetConfiguration.absolute = new Absolute();
				XPathNavigator xPathNavigator3 = obj2.SelectSingleNode("canqueryposition");
				if (xPathNavigator3 == null)
				{
					throw new XmlException("The 'canqueryposition' node is missing.");
				}
				devicePresetConfiguration.Absolute.CanQueryPosition = xPathNavigator3.ValueAsBoolean;
				xPathNavigator3 = obj2.SelectSingleNode("speedsupport");
				if (xPathNavigator3 == null)
				{
					throw new XmlException("The 'speedsupport' node is missing.");
				}
				devicePresetConfiguration.Absolute.SpeedSupport = xPathNavigator3.ValueAsBoolean;
			}
		}
		return devicePresetConfiguration;
	}

	public string SaveAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture)))
		{
			xmlTextWriter.WriteStartElement("ptzsettings");
			switch (presetType)
			{
			case PresetType.Internal:
				xmlTextWriter.WriteElementString("presettype", "internal");
				break;
			case PresetType.Absolute:
				xmlTextWriter.WriteElementString("presettype", "absolute");
				break;
			case PresetType.Both:
				xmlTextWriter.WriteElementString("presettype", "both");
				break;
			}
			if (@public != null)
			{
				xmlTextWriter.WriteStartElement("internal");
				xmlTextWriter.WriteElementString("isnumbered", @internal.IsNumbered ? "true" : "false");
				xmlTextWriter.WriteElementString("minnumber", Convert.ToString(@internal.MinimumNumber, CultureInfo.InvariantCulture));
				xmlTextWriter.WriteElementString("maxnumber", Convert.ToString(@internal.MaximumNumber, CultureInfo.InvariantCulture));
				xmlTextWriter.WriteElementString("canloadfromdevice", @internal.CanLoadFromDevice ? "true" : "false");
				xmlTextWriter.WriteElementString("cansetpreset", @internal.CanSetPreset ? "true" : "false");
				xmlTextWriter.WriteElementString("speedsupport", @internal.SpeedSupport ? "true" : "false");
				xmlTextWriter.WriteEndElement();
			}
			if (absolute != null)
			{
				xmlTextWriter.WriteStartElement("absolute");
				xmlTextWriter.WriteElementString("canqueryposition", absolute.CanQueryPosition ? "true" : "false");
				xmlTextWriter.WriteElementString("speedsupport", absolute.SpeedSupport ? "true" : "false");
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
		}
		return stringBuilder.ToString();
	}
}
