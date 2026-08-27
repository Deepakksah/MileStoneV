using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedComponent.Data;

namespace VideoOS.Common.SharedComponent;

public class ViewGroupData
{
	private ViewGroup _viewGroup;

	private List<GroupData> _groups = new List<GroupData>();

	public ReadOnlyCollection<GroupData> Groups => new ReadOnlyCollection<GroupData>(_groups);

	public string PartialXML
	{
		get
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			xmlWriterSettings.OmitXmlDeclaration = true;
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			_viewGroup.WriteXmlFragment(xmlWriter);
			xmlWriter.Flush();
			xmlWriter.Close();
			string result = Encoding.UTF8.GetString(memoryStream.ToArray());
			memoryStream.Close();
			memoryStream.Dispose();
			return result;
		}
		set
		{
			XmlReader reader = XmlReader.Create(new StringReader(value));
			_viewGroup.ReadXml(reader);
		}
	}

	private ViewGroupData(string partialXML)
	{
		_viewGroup = new ViewGroup();
		_viewGroup.XmlNamespace = "";
		PartialXML = partialXML;
	}

	public ViewGroupData()
	{
		_viewGroup = new ViewGroup();
		_viewGroup.XmlNamespace = "";
	}

	public static ViewGroupData CreateFromXMLDefinition(string partialXML)
	{
		return new ViewGroupData(partialXML);
	}

	public static ViewGroupData CreateFromXML(string fullXML)
	{
		try
		{
			if (!string.IsNullOrEmpty(fullXML))
			{
				XmlReader xmlReader = XmlReader.Create(new StringReader(fullXML));
				if (xmlReader.Read())
				{
					ViewGroupData viewGroupData = new ViewGroupData();
					viewGroupData.ReadXml(xmlReader);
					return viewGroupData;
				}
				throw new SharedComponentException("The fullXML does not contain any usefull information");
			}
			throw new SharedComponentException("The fulleXML cannot be null or empty");
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("ViewGroupData creation error", innerException);
		}
	}

	public GroupData AddGroupData(string partialXML)
	{
		GroupData groupData = GroupData.CreateFromXMLDefinition(partialXML);
		_viewGroup.Groups.Add(groupData.Item as Group);
		_groups.Add(groupData);
		return groupData;
	}

	public void AddGroupData(GroupData groupData)
	{
		_viewGroup.Groups.Add(groupData.Item as Group);
		_groups.Add(groupData);
	}

	public string GetFullXML()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
		WriteXml(xmlWriter);
		xmlWriter.Flush();
		xmlWriter.Close();
		string result = Encoding.UTF8.GetString(memoryStream.ToArray());
		memoryStream.Close();
		memoryStream.Dispose();
		return result;
	}

	private void ReadXml(XmlReader reader)
	{
		_viewGroup.ReadXml(reader);
		foreach (Group group in _viewGroup.Groups)
		{
			GroupData item = new GroupData(group);
			_groups.Add(item);
		}
	}

	private void WriteXml(XmlWriter writer)
	{
		_viewGroup.WriteXml(writer);
	}
}
