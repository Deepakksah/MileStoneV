using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Platform.Log;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Common.Logging;

public class LogClientManager
{
	private List<LogMessageDictionary> _allMessages = new List<LogMessageDictionary>();

	private ServerId _serverId;

	private string _culture;

	private Dictionary<Guid, bool> _uploadedDictionaries = new Dictionary<Guid, bool>();

	private const string userFields = "<userfields><userfield uname=\"EventType\" type=\"string\" /><userfield uname=\"ResourceId\" type=\"string\" /><userfield uname=\"ResourceName\" type=\"string\" /><userfield uname=\"ResourceLocation\" type=\"string\" /><userfield uname=\"ServiceName\" type=\"string\" /><userfield uname=\"SourceType\" type=\"enum\" /><userfield uname=\"AccessGranted\" type=\"enum\" /><userfield uname=\"ResourceType\" type=\"enum\" /><userfield uname=\"Category\" type=\"enum\" /></userfields>";

	private const string grantEnum = "<userEnum uname=\"AccessGranted\" fieldname=\"AccessGranted\" fieldtype=\"user\"><enum uname=\"Granted\" value=\"Granted\" /><enum uname=\"Denied\" value=\"Denied\" /><enum uname=\"Default\" value=\"Default\" /></userEnum>";

	public ILogClientHandler LogClient { get; private set; }

	public LogClientManager(ServerId serverId, string culture, LoginSettings loginSettings)
	{
		_serverId = serverId ?? throw new ArgumentNullException("serverId");
		_culture = culture ?? throw new ArgumentNullException("culture");
		LoginSettings loginSettings2 = loginSettings ?? LoginSettingsCache.GetLoginSettings(_serverId.Id);
		if (loginSettings2 == null)
		{
			EnvironmentManager.Instance.Log(error: true, "LogClientManager", "Unable to identify log server from item.ServerId - logentry NOT stored");
			return;
		}
		LogClient = LogClientHandlerFactory.Create(_culture, serverId);
		if (loginSettings2.IsOAuthIdentity)
		{
			LogClient.SetUserId(loginSettings2.FullyQualifiedUserName);
		}
		else if (loginSettings2.NetworkCredential != null)
		{
			if (loginSettings2.IsBasicUser)
			{
				LogClient.SetUserId(loginSettings2.NetworkCredential.UserName);
			}
			else if (loginSettings2.NetworkCredential.UserName != "")
			{
				LogClient.SetUserId((string.IsNullOrWhiteSpace(loginSettings2.NetworkCredential.Domain) ? string.Empty : (loginSettings2.NetworkCredential.Domain + "\\")) + loginSettings2.NetworkCredential.UserName);
			}
		}
	}

	public void Close()
	{
		_allMessages.Clear();
		LogClient.Dispose();
		LogClient = null;
	}

	public void AddLogDictionary(LogMessageDictionary lmd)
	{
		_allMessages.Add(lmd);
	}

	public void AddRangeLogDictionary(List<LogMessageDictionary> lmdList)
	{
		_allMessages.AddRange(lmdList);
	}

	public LogMessageDictionary CheckForUpload(string application, string component, string preferedCulture)
	{
		List<LogMessageDictionary> list = new List<LogMessageDictionary>();
		LogMessageDictionary logMessageDictionary = null;
		foreach (LogMessageDictionary allMessage in _allMessages)
		{
			if (allMessage.Application == application && allMessage.Component == component)
			{
				if (!_uploadedDictionaries.ContainsKey(allMessage.InstanceId))
				{
					list.Add(allMessage);
				}
				if (logMessageDictionary == null || (preferedCulture != null && allMessage.Culture == preferedCulture))
				{
					logMessageDictionary = allMessage;
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (LogMessageDictionary item in list)
			{
				string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				string s = GenerateXml(item);
				File.WriteAllBytes(text, Encoding.UTF8.GetBytes(s));
				if (!LogClient.UploadCultureFile(text, forceupload: false, asynchronous: true))
				{
					EnvironmentManager.Instance.Log(error: true, "LogClientManager", "LogClient Upload failed");
				}
				_uploadedDictionaries[item.InstanceId] = true;
			}
		}
		return logMessageDictionary;
	}

	private static string GenerateXml(LogMessageDictionary lmd)
	{
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Indent = true;
		xmlWriterSettings.Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
		xmlWriterSettings.CloseOutput = false;
		xmlWriterSettings.CheckCharacters = true;
		xmlWriterSettings.IndentChars = " ";
		using MemoryStream memoryStream = new MemoryStream();
		using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
		xmlWriter.WriteStartElement("LogMessages");
		xmlWriter.WriteElementString("version", lmd.Version);
		xmlWriter.WriteElementString("culture", lmd.Culture);
		xmlWriter.WriteRaw("<userfields><userfield uname=\"EventType\" type=\"string\" /><userfield uname=\"ResourceId\" type=\"string\" /><userfield uname=\"ResourceName\" type=\"string\" /><userfield uname=\"ResourceLocation\" type=\"string\" /><userfield uname=\"ServiceName\" type=\"string\" /><userfield uname=\"SourceType\" type=\"enum\" /><userfield uname=\"AccessGranted\" type=\"enum\" /><userfield uname=\"ResourceType\" type=\"enum\" /><userfield uname=\"Category\" type=\"enum\" /></userfields>");
		xmlWriter.WriteStartElement("userEnums");
		xmlWriter.WriteRaw("<userEnum uname=\"AccessGranted\" fieldname=\"AccessGranted\" fieldtype=\"user\"><enum uname=\"Granted\" value=\"Granted\" /><enum uname=\"Denied\" value=\"Denied\" /><enum uname=\"Default\" value=\"Default\" /></userEnum>");
		xmlWriter.WriteStartElement("userEnum");
		xmlWriter.WriteAttributeString("uname", "SourceType");
		xmlWriter.WriteAttributeString("fieldname", "SourceType");
		xmlWriter.WriteAttributeString("fieldtype", "user");
		xmlWriter.WriteStartElement("enum");
		xmlWriter.WriteAttributeString("uname", lmd.Application);
		xmlWriter.WriteAttributeString("value", lmd.ApplicationName);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("userEnum");
		xmlWriter.WriteAttributeString("uname", "ResourceType");
		xmlWriter.WriteAttributeString("fieldname", "ResourceType");
		xmlWriter.WriteAttributeString("fieldtype", "user");
		xmlWriter.WriteStartElement("enum");
		xmlWriter.WriteAttributeString("uname", "Application");
		xmlWriter.WriteAttributeString("value", "Application");
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("enum");
		xmlWriter.WriteAttributeString("uname", lmd.Application);
		xmlWriter.WriteAttributeString("value", lmd.ApplicationName);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("userEnum");
		xmlWriter.WriteAttributeString("uname", "Category");
		xmlWriter.WriteAttributeString("fieldname", "Category");
		xmlWriter.WriteAttributeString("fieldtype", "user");
		xmlWriter.WriteStartElement("enum");
		xmlWriter.WriteAttributeString("uname", lmd.Component);
		xmlWriter.WriteAttributeString("value", lmd.Component);
		xmlWriter.WriteEndElement();
		foreach (var (value, value2) in findAllCategories(lmd))
		{
			xmlWriter.WriteStartElement("enum");
			xmlWriter.WriteAttributeString("uname", value);
			xmlWriter.WriteAttributeString("value", value2);
			xmlWriter.WriteEndElement();
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("application");
		xmlWriter.WriteAttributeString("uname", lmd.Application);
		xmlWriter.WriteStartElement("component");
		xmlWriter.WriteAttributeString("uname", lmd.Component);
		xmlWriter.WriteStartElement("entries");
		foreach (LogMessage value4 in lmd.LogMessages.Values)
		{
			xmlWriter.WriteStartElement("group");
			xmlWriter.WriteAttributeString("name", value4.Group);
			xmlWriter.WriteAttributeString("category", value4.Category);
			xmlWriter.WriteAttributeString("field1", "ResourceId");
			xmlWriter.WriteAttributeString("value1", "@objId");
			xmlWriter.WriteAttributeString("field2", "ResourceName");
			xmlWriter.WriteAttributeString("value2", "@objName");
			xmlWriter.WriteAttributeString("field3", "SourceType");
			xmlWriter.WriteAttributeString("value3", ConvertToSourceType(value4.RelatedObjectKind));
			xmlWriter.WriteStartElement("entry");
			string value3 = ConvertToObjectType(value4.RelatedObjectKind);
			xmlWriter.WriteAttributeString("uname", value4.Id);
			xmlWriter.WriteAttributeString("object_type", value3);
			xmlWriter.WriteAttributeString("status", value4.Status);
			xmlWriter.WriteAttributeString("severity", value4.Severity);
			xmlWriter.WriteAttributeString("level", "10");
			xmlWriter.WriteString(value4.Message);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		xmlWriter.Close();
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}

	private static List<(string id, string value)> findAllCategories(LogMessageDictionary lmd)
	{
		List<(string, string)> list = new List<(string, string)>();
		foreach (LogMessage value in lmd.LogMessages.Values)
		{
			if (value.Group == "Audit")
			{
				list.Add((value.Category, value.CategoryName));
			}
		}
		return list;
	}

	private static string ConvertToObjectType(Guid kind)
	{
		if (!(Kind.DefaultTypeToNameTable[kind] is string result))
		{
			return "";
		}
		return result;
	}

	private static string ConvertToSourceType(Guid kind)
	{
		if (!(Kind.KindToLogMessageSourceType[kind] is string result))
		{
			return "Unknown";
		}
		return result;
	}
}
