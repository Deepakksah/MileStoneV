using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using NSpring.Logging;

namespace VideoOS.Diagnostics.Logging.Configuration;

public sealed class Appender
{
	private readonly Level _defaultLogLevel = Level.Warning;

	private readonly string _defaultEventLogName = "VideoOS";

	private AppenderTypes appenderType;

	private Level logLevel;

	private PropertyCollection properties;

	public AppenderTypes AppenderType => appenderType;

	public Level LogLevel => logLevel;

	public PropertyCollection Properties => properties;

	public Appender(XmlNode appenderNode, Logger parent)
	{
		initAppenderType(appenderNode);
		initLogLevel(appenderNode);
		initPropertiesCollection(appenderNode, parent);
	}

	private void initPropertiesCollection(XmlNode appenderNode, Logger parent)
	{
		properties = new PropertyCollection();
		switch (appenderType)
		{
		case AppenderTypes.File:
		{
			XmlNode namedItem2 = appenderNode.Attributes.GetNamedItem("filename");
			string text = ProcessPath(parent, namedItem2);
			if (text.Length == 0)
			{
				text = Path.Combine(Path.GetTempPath(), parent.Name.Replace("\\", "").Replace("/", ""));
			}
			properties.Add("FileName", text);
			if (namedItem2 != null)
			{
				properties.Add("UnprocessedFileName", namedItem2.Value);
			}
			XmlNode namedItem3 = appenderNode.Attributes.GetNamedItem("archive");
			string value = ProcessPath(parent, namedItem3);
			properties.Add("Archive", value);
			if (namedItem3 != null)
			{
				properties.Add("UnprocessedArchive", namedItem3.Value);
			}
			XmlNode namedItem4 = appenderNode.Attributes.GetNamedItem("maxsize");
			int num = 100;
			try
			{
				if (namedItem4 != null)
				{
					num = Convert.ToInt32(namedItem4.Value, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			properties.Add("MaxSize", num);
			break;
		}
		case AppenderTypes.EventLog:
		{
			XmlNode namedItem = appenderNode.Attributes.GetNamedItem("logname");
			if (namedItem != null && namedItem.Value.Length > 0)
			{
				properties.Add("LogName", namedItem.Value);
			}
			else
			{
				properties.Add("LogName", _defaultEventLogName);
			}
			break;
		}
		case AppenderTypes.Console:
			break;
		}
	}

	private static string ProcessPath(Logger parent, XmlNode pathAttributeNode)
	{
		if (pathAttributeNode == null || pathAttributeNode.Value.Length == 0)
		{
			return string.Empty;
		}
		pathAttributeNode.Value = pathAttributeNode.Value.Replace("/", "\\");
		try
		{
			string text = Environment.ExpandEnvironmentVariables(pathAttributeNode.Value);
			if (text != pathAttributeNode.Value && Path.IsPathRooted(text))
			{
				return text;
			}
		}
		catch (ArgumentNullException)
		{
		}
		if (Path.IsPathRooted(pathAttributeNode.Value))
		{
			return pathAttributeNode.Value;
		}
		if (pathAttributeNode.Value.StartsWith("~", StringComparison.Ordinal))
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text2 = pathAttributeNode.Value.Substring(1);
			if (text2.StartsWith("\\", StringComparison.Ordinal))
			{
				text2 = text2.Substring(1);
			}
			if (text2.Length == 0)
			{
				text2 = parent.Name.Replace("\\", "").Replace("/", "");
			}
			return Path.Combine(baseDirectory, text2);
		}
		return Path.Combine(Path.GetTempPath(), pathAttributeNode.Value);
	}

	private void initLogLevel(XmlNode appenderNode)
	{
		XmlNode namedItem = appenderNode.Attributes.GetNamedItem("level");
		if (namedItem == null)
		{
			logLevel = _defaultLogLevel;
			return;
		}
		switch (namedItem.Value.ToLower(CultureInfo.InvariantCulture))
		{
		case "debug":
			logLevel = Level.Debug;
			break;
		case "info":
			logLevel = Level.Info;
			break;
		case "warning":
			logLevel = Level.Warning;
			break;
		case "error":
			logLevel = Level.Error;
			break;
		default:
			logLevel = _defaultLogLevel;
			break;
		}
	}

	private void initAppenderType(XmlNode appenderNode)
	{
		switch ((appenderNode.Attributes.GetNamedItem("type") ?? throw new ConfigurationErrorsException("An 'Appender' was defined without a 'type' attribute in the configuration file. An 'Appender' must have a type defined.")).Value.ToLower(CultureInfo.InvariantCulture))
		{
		case "file":
			appenderType = AppenderTypes.File;
			break;
		case "console":
			appenderType = AppenderTypes.Console;
			break;
		case "eventlog":
			appenderType = AppenderTypes.EventLog;
			break;
		default:
			throw new ConfigurationErrorsException("The type attribute for an appender must be set to 'File', 'Console' or 'EventLog'.");
		}
	}
}
