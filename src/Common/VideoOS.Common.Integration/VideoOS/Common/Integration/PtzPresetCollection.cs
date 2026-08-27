using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration.Drivers;

namespace VideoOS.Common.Integration;

public class PtzPresetCollection : IEnumerable<PtzPreset>, IEnumerable
{
	private LinkedList<PtzPreset> presets;

	public static string EmptyXml
	{
		[DebuggerStepThrough]
		get
		{
			return new PtzPresetCollection().SaveAsXml();
		}
	}

	public int Count
	{
		[DebuggerStepThrough]
		get
		{
			return presets.Count;
		}
	}

	public PtzPreset this[string key] => GetPresetByKey(presets, key);

	public event EventHandler Changed;

	public PtzPresetCollection()
	{
		presets = new LinkedList<PtzPreset>();
	}

	public void OnChange()
	{
		if (this.Changed != null)
		{
			this.Changed(this, new EventArgs());
		}
	}

	public void Clear()
	{
		presets.Clear();
		OnChange();
	}

	public void Remove(string key)
	{
		PtzPreset presetByKey = GetPresetByKey(presets, key);
		if (presetByKey != null)
		{
			presets.Remove(presetByKey);
			OnChange();
		}
	}

	public void Remove(Guid id)
	{
		PtzPreset ptzPreset = presets.FirstOrDefault((PtzPreset p) => p.Id == id);
		if (ptzPreset != null)
		{
			presets.Remove(ptzPreset);
			OnChange();
		}
	}

	public void Add(PtzPreset preset)
	{
		presets.AddLast(preset);
		OnChange();
	}

	public bool ContainsKey(string key)
	{
		return GetPresetByKey(presets, key) != null;
	}

	public static PtzPresetCollection FromXml(string xml)
	{
		PtzPresetCollection ptzPresetCollection = new PtzPresetCollection();
		ptzPresetCollection.LoadXml(xml);
		return ptzPresetCollection;
	}

	public void LoadXml(string xml)
	{
		string text = "";
		Guid guid = Guid.Empty;
		int number = -1;
		bool devicePreset = false;
		bool defaultPreset = false;
		bool flag = false;
		string name = "";
		string key = "";
		string description = "";
		bool locked = false;
		PtzArgs ptzArgs = new PtzArgs();
		Clear();
		using StringReader input = new StringReader(xml);
		using XmlTextReader xmlTextReader = new XmlTextReader(input);
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				text = xmlTextReader.Name;
				if (text == "preset")
				{
					guid = new Guid(xmlTextReader.GetAttribute("id"));
					key = "";
					number = -1;
					devicePreset = false;
					flag = false;
					name = "";
					description = "";
					ptzArgs = new PtzArgs();
					locked = false;
				}
				if (text == "ptzargs")
				{
					ptzArgs = new PtzArgs();
					if (!xmlTextReader.IsEmptyElement)
					{
						ptzArgs.LoadXmlFragment(xmlTextReader);
					}
				}
			}
			if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name == "preset")
			{
				if (guid == Guid.Empty)
				{
					throw new ArgumentException("The preset xml contains a preset without an id.", "xml");
				}
				PtzPreset preset = ((!flag) ? new PtzPreset(guid, key, name, description, ptzArgs, devicePreset, defaultPreset, locked) : new PtzPreset(guid, key, number, name, description, ptzArgs, devicePreset, defaultPreset, locked));
				Add(preset);
			}
			if (xmlTextReader.NodeType == XmlNodeType.Text)
			{
				switch (text)
				{
				case "key":
					key = xmlTextReader.Value;
					break;
				case "number":
					number = Convert.ToInt32(xmlTextReader.Value, CultureInfo.InvariantCulture);
					break;
				case "isnumbered":
					flag = Convert.ToBoolean(xmlTextReader.Value, CultureInfo.InvariantCulture);
					break;
				case "name":
					name = xmlTextReader.Value;
					break;
				case "description":
					description = xmlTextReader.Value;
					break;
				case "isdevicepreset":
					devicePreset = Convert.ToBoolean(xmlTextReader.Value, CultureInfo.InvariantCulture);
					break;
				case "isdefault":
					defaultPreset = Convert.ToBoolean(xmlTextReader.Value, CultureInfo.InvariantCulture);
					break;
				case "locked":
					locked = Convert.ToBoolean(xmlTextReader.Value, CultureInfo.InvariantCulture);
					break;
				}
			}
			OnChange();
		}
	}

	public string SaveAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xmlTextWriter.WriteStartElement("presets");
			WritePresets(xmlTextWriter);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
		}
		return stringBuilder.ToString();
	}

	public PtzPresetCollection Copy()
	{
		return FromXml(SaveAsXml());
	}

	private void WritePresets(XmlTextWriter wr)
	{
		foreach (PtzPreset preset in presets)
		{
			wr.WriteStartElement("preset");
			wr.WriteAttributeString("id", preset.Id.ToString());
			wr.WriteElementString("key", preset.Key);
			wr.WriteElementString("name", preset.Name);
			wr.WriteElementString("number", preset.IsNumbered ? preset.Number.ToString(CultureInfo.InvariantCulture) : "");
			wr.WriteElementString("description", preset.Description);
			wr.WriteElementString("isnumbered", preset.IsNumbered.ToString(CultureInfo.InvariantCulture));
			wr.WriteElementString("isdevicepreset", preset.IsDevicePreset.ToString(CultureInfo.InvariantCulture));
			wr.WriteElementString("isdefault", preset.IsDefault.ToString(CultureInfo.InvariantCulture));
			wr.WriteElementString("locked", preset.Locked.ToString(CultureInfo.InvariantCulture));
			preset.PtzArgs.SaveAsXml(wr);
			wr.WriteEndElement();
		}
	}

	public static PtzPreset GetPresetByKey(LinkedList<PtzPreset> presets, string key)
	{
		PtzPreset ptzPreset = presets.FirstOrDefault((PtzPreset p) => string.Equals(key, p.Key, StringComparison.Ordinal));
		if (ptzPreset != null)
		{
			return ptzPreset;
		}
		ptzPreset = presets.FirstOrDefault((PtzPreset p) => string.Equals(key, p.Id.ToString(), StringComparison.OrdinalIgnoreCase));
		if (ptzPreset != null)
		{
			return ptzPreset;
		}
		ptzPreset = presets.FirstOrDefault((PtzPreset p) => string.Equals(key, p.Name, StringComparison.Ordinal));
		if (ptzPreset != null)
		{
			return ptzPreset;
		}
		return null;
	}

	public IEnumerator<PtzPreset> GetEnumerator()
	{
		return presets.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return presets.GetEnumerator();
	}
}
