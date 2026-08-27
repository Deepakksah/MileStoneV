using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Common.Integration;

public class SectionSettings
{
	private object _settingsChangedEventLock = new object();

	private bool _updating;

	private List<SettingChangedEventData> _events = new List<SettingChangedEventData>();

	private Dictionary<string, SettingsCollection> _data = new Dictionary<string, SettingsCollection>();

	private string _sectionAttributeText = "section";

	private string _settingAttributeText = "setting";

	public static string XmlRootName => "properties";

	protected virtual bool Updating
	{
		[DebuggerStepThrough]
		get
		{
			return _updating;
		}
		[DebuggerStepThrough]
		set
		{
			_updating = value;
		}
	}

	protected virtual List<SettingChangedEventData> Events
	{
		[DebuggerStepThrough]
		get
		{
			return _events;
		}
		[DebuggerStepThrough]
		set
		{
			_events = value;
		}
	}

	protected virtual Dictionary<string, SettingsCollection> Data
	{
		[DebuggerStepThrough]
		get
		{
			return _data;
		}
		[DebuggerStepThrough]
		set
		{
			_data = value;
		}
	}

	public string SectionAttributeText
	{
		[DebuggerStepThrough]
		get
		{
			return _sectionAttributeText;
		}
		[DebuggerStepThrough]
		set
		{
			_sectionAttributeText = value;
		}
	}

	public string SettingAttributeText
	{
		[DebuggerStepThrough]
		get
		{
			return _settingAttributeText;
		}
		[DebuggerStepThrough]
		set
		{
			_settingAttributeText = value;
		}
	}

	private event EventHandler<SettingsEventArgs> _settingsChangedEvent;

	public event EventHandler<SettingsEventArgs> SettingsChanged
	{
		add
		{
			lock (_settingsChangedEventLock)
			{
				_settingsChangedEvent += value;
			}
		}
		remove
		{
			lock (_settingsChangedEventLock)
			{
				_settingsChangedEvent -= value;
			}
		}
	}

	protected virtual void OnSettingsChanged(SettingsEventArgs args)
	{
		EventHandler<SettingsEventArgs> eventHandler;
		lock (_settingsChangedEventLock)
		{
			eventHandler = this._settingsChangedEvent;
		}
		eventHandler?.Invoke(this, args);
	}

	public void Clear()
	{
		lock (_data)
		{
			string[] array = new string[_data.Keys.Count];
			_data.Keys.CopyTo(array, 0);
			string[] array2 = array;
			foreach (string section in array2)
			{
				RemoveSection(section);
			}
			_data.Clear();
			_updating = false;
			_events.Clear();
		}
	}

	protected void Set(string section, SettingEntry settingEntry, bool allowCreate, bool silent = false)
	{
		SettingsEventArgs e = InternalSet(section, settingEntry, allowCreate);
		if (e != null && !silent)
		{
			OnSettingsChanged(e);
		}
	}

	private SettingsEventArgs InternalSet(string section, SettingEntry settingEntry, bool allowCreate)
	{
		SettingsEventArgs result = null;
		lock (_data)
		{
			SettingEntry settingEntry2 = null;
			string valueBefore = null;
			bool flag = false;
			if (!_data.TryGetValue(section, out var value))
			{
				if (!allowCreate)
				{
					return null;
				}
				value = new SettingsCollection();
				_data[section] = value;
			}
			flag = value.TryGetValue(settingEntry.Name, out settingEntry2);
			if (!flag && !allowCreate)
			{
				return null;
			}
			if (flag)
			{
				valueBefore = settingEntry2.Value;
				settingEntry2.Value = settingEntry.Value;
			}
			else
			{
				value.Add(settingEntry);
			}
			SettingChangedEventData settingChangedEventData = ((!flag) ? new SettingChangedEventData(SettingChangedAction.Added, section, settingEntry, null, settingEntry.Value) : new SettingChangedEventData(SettingChangedAction.Modified, section, settingEntry, valueBefore, settingEntry.Value));
			if (_updating)
			{
				_events.Add(settingChangedEventData);
			}
			else
			{
				result = new SettingsEventArgs(settingChangedEventData);
			}
		}
		return result;
	}

	protected string Get(string section, string name)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value) && value.TryGetValue(name, out var settingEntry))
			{
				return settingEntry.Value;
			}
		}
		return null;
	}

	protected string GetTypeId(string section, string name)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value) && value.TryGetValue(name, out var settingEntry))
			{
				return settingEntry.TypeId;
			}
		}
		return null;
	}

	protected string GetValueByTypeId(string section, string typeId)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value))
			{
				foreach (SettingEntry item in value)
				{
					if (string.Equals(item.TypeId, typeId, StringComparison.OrdinalIgnoreCase))
					{
						return item.Value;
					}
				}
			}
		}
		return null;
	}

	protected SettingEntry GetSettingByTypeId(string section, string typeId)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value))
			{
				foreach (SettingEntry item in value)
				{
					if (string.Equals(item.TypeId, typeId, StringComparison.OrdinalIgnoreCase))
					{
						return item;
					}
				}
			}
		}
		return null;
	}

	protected string GetNameByTypeId(string section, string typeId)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value))
			{
				foreach (SettingEntry item in value)
				{
					if (string.Equals(item.TypeId, typeId, StringComparison.OrdinalIgnoreCase))
					{
						return item.Name;
					}
				}
			}
		}
		return null;
	}

	protected void Remove(string section, string name)
	{
		SettingsEventArgs e = InternalRemove(section, name);
		if (e != null)
		{
			OnSettingsChanged(e);
		}
	}

	private SettingsEventArgs InternalRemove(string section, string name)
	{
		SettingsEventArgs result = null;
		lock (_data)
		{
			if (!_data.TryGetValue(section, out var value))
			{
				return null;
			}
			if (!value.TryGetValue(name, out var settingEntry))
			{
				return null;
			}
			value.Remove(name);
			if (value.Count == 0)
			{
				_data.Remove(section);
			}
			SettingChangedEventData settingChangedEventData = new SettingChangedEventData(SettingChangedAction.Deleted, section, settingEntry, settingEntry.Value, null);
			if (_updating)
			{
				_events.Add(settingChangedEventData);
			}
			else
			{
				result = new SettingsEventArgs(settingChangedEventData);
			}
		}
		return result;
	}

	protected void RemoveSection(string section)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value))
			{
				string[] array = new string[value.Keys.Count];
				value.Keys.CopyTo(array, 0);
				string[] array2 = array;
				foreach (string name in array2)
				{
					Remove(section, name);
				}
				_data.Remove(section);
			}
		}
	}

	public bool HasSection(string name)
	{
		lock (_data)
		{
			return _data.ContainsKey(name);
		}
	}

	protected string[] GetSectionNames()
	{
		lock (_data)
		{
			string[] array = new string[_data.Keys.Count];
			_data.Keys.CopyTo(array, 0);
			return array;
		}
	}

	protected string[] GetSectionSettingNames(string section)
	{
		lock (_data)
		{
			if (_data.TryGetValue(section, out var value))
			{
				string[] array = new string[value.Count];
				int num = 0;
				foreach (SettingEntry item in value)
				{
					array[num] = item.Name;
					num++;
				}
				return array;
			}
			return new string[0];
		}
	}

	protected SettingsCollection GetSectionSettings(string section)
	{
		if (_data.TryGetValue(section, out var value))
		{
			return value;
		}
		return new SettingsCollection();
	}

	public void BeginUpdate()
	{
		lock (_data)
		{
			_updating = true;
		}
	}

	public void EndUpdate()
	{
		SettingsEventArgs e = null;
		lock (_data)
		{
			_updating = false;
			if (_events.Count > 0)
			{
				e = new SettingsEventArgs(_events.ToArray());
				_events.Clear();
			}
		}
		if (e != null)
		{
			OnSettingsChanged(e);
		}
	}

	public void UpdateAllSettings()
	{
		List<SettingChangedEventData> list = new List<SettingChangedEventData>();
		lock (_data)
		{
			foreach (KeyValuePair<string, SettingsCollection> datum in _data)
			{
				foreach (SettingEntry item in datum.Value)
				{
					list.Add(new SettingChangedEventData(SettingChangedAction.Modified, datum.Key, item, item.Value, item.Value));
				}
			}
		}
		if (list.Count > 0)
		{
			OnSettingsChanged(new SettingsEventArgs(list.ToArray()));
		}
	}

	public virtual string SaveAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture)))
		{
			xmlTextWriter.WriteStartElement(XmlRootName);
			lock (_data)
			{
				foreach (KeyValuePair<string, SettingsCollection> datum in _data)
				{
					SaveSectionAsXml(xmlTextWriter, datum.Key, datum.Value);
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
		}
		return stringBuilder.ToString();
	}

	public virtual string SaveSectionAsXml(string section)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture)))
		{
			xmlTextWriter.WriteStartElement(XmlRootName);
			lock (_data)
			{
				SettingsCollection sectionTable = _data[section];
				SaveSectionAsXml(xmlTextWriter, section, sectionTable);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
		}
		return stringBuilder.ToString();
	}

	private void SaveSectionAsXml(XmlTextWriter wr, string section, SettingsCollection sectionTable)
	{
		wr.WriteStartElement("settings");
		if (section.Length > 0)
		{
			wr.WriteAttributeString(_sectionAttributeText, section);
		}
		foreach (SettingEntry item in sectionTable)
		{
			wr.WriteStartElement("setting");
			if (!string.IsNullOrEmpty(item.TypeId))
			{
				wr.WriteAttributeString(SettingAttributeText, item.TypeId);
			}
			wr.WriteElementString("name", item.Name);
			wr.WriteElementString("value", item.Value);
			wr.WriteEndElement();
		}
		wr.WriteEndElement();
	}

	public void LoadXml(string xml, SettingsLoadMethod loadMethod)
	{
		if (string.IsNullOrEmpty(xml))
		{
			return;
		}
		List<SettingChangedEventData> list = new List<SettingChangedEventData>();
		using (StringReader input = new StringReader(xml))
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.XmlResolver = null;
			using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
			lock (_data)
			{
				if (xmlReader.IsStartElement())
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element && string.Equals(xmlReader.Name, "settings", StringComparison.Ordinal))
						{
							string section = string.Empty;
							if (xmlReader.HasAttributes)
							{
								xmlReader.MoveToAttribute(0);
								_sectionAttributeText = xmlReader.Name;
								section = xmlReader.Value;
								xmlReader.MoveToElement();
							}
							ReadSettingElements(xmlReader, section, loadMethod, list);
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			SettingsEventArgs args = new SettingsEventArgs(list.ToArray());
			list.Clear();
			OnSettingsChanged(args);
		}
	}

	private void ReadSettingElements(XmlReader reader, string section, SettingsLoadMethod loadMethod, List<SettingChangedEventData> events)
	{
		string text = null;
		string text2 = null;
		string typeId = "";
		if (!_data.TryGetValue(section, out var value))
		{
			value = new SettingsCollection();
			_data.Add(section, value);
		}
		while (reader.Read())
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
				if (reader.HasAttributes)
				{
					reader.MoveToAttribute(0);
					if (string.Equals(reader.Name, SettingAttributeText, StringComparison.Ordinal))
					{
						typeId = reader.Value;
					}
					reader.MoveToElement();
				}
				if (string.Equals(reader.Name, "name", StringComparison.Ordinal))
				{
					text = reader.ReadString();
				}
				else if (string.Equals(reader.Name, "value", StringComparison.Ordinal))
				{
					text2 = reader.ReadString();
				}
				break;
			case XmlNodeType.EndElement:
				if (string.Equals(reader.Name, "setting", StringComparison.Ordinal))
				{
					if (text != null && text2 != null)
					{
						bool flag = false;
						SettingEntry settingEntry = null;
						flag = value.TryGetValue(text, out settingEntry);
						if (flag)
						{
							SettingEntry value2 = new SettingEntry(section, text, text2, typeId);
							int index = value.IndexOf(settingEntry);
							value[index] = value2;
						}
						else
						{
							value.Add(new SettingEntry(section, text, text2, typeId));
						}
						if (loadMethod == SettingsLoadMethod.Reload || !flag || !string.Equals(settingEntry.Value, text2, StringComparison.Ordinal))
						{
							SettingChangedAction action = ((!flag) ? SettingChangedAction.Added : SettingChangedAction.Modified);
							string valueBefore = (flag ? settingEntry.Value : null);
							events.Add(new SettingChangedEventData(action, section, new SettingEntry(section, text, text2, typeId), valueBefore, text2));
						}
					}
					text = null;
					text2 = null;
					typeId = null;
				}
				else if (string.Equals(reader.Name, "settings", StringComparison.Ordinal))
				{
					return;
				}
				break;
			}
		}
	}
}
