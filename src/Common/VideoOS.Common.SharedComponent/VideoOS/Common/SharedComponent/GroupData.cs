using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedComponent.Data;

namespace VideoOS.Common.SharedComponent;

public class GroupData : ItemDataBase
{
	private List<ViewData> _views = new List<ViewData>();

	private List<GroupData> _groups = new List<GroupData>();

	private object _groupListLock = new object();

	public Guid SmartClientId
	{
		set
		{
			(_propertyBase as Group).SmartClientId = value;
		}
	}

	public ReadOnlyCollection<GroupData> Groups => new ReadOnlyCollection<GroupData>(_groups);

	public ReadOnlyCollection<ViewData> Views => new ReadOnlyCollection<ViewData>(_views);

	public override string PartialXML
	{
		get
		{
			MemoryStream memoryStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			xmlWriterSettings.OmitXmlDeclaration = true;
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			_propertyBase.WriteXmlFragment(xmlWriter);
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
			_propertyBase.ReadXml(reader);
		}
	}

	private GroupData(string partialXML)
	{
		_propertyBase = new Group();
		_groups = new List<GroupData>();
		_views = new List<ViewData>();
		PartialXML = partialXML;
	}

	public GroupData()
	{
		_propertyBase = new Group();
		_groups = new List<GroupData>();
		_views = new List<ViewData>();
	}

	public GroupData(Group group)
	{
		_propertyBase = group;
		_groups = new List<GroupData>();
		_views = new List<ViewData>();
		foreach (Group group2 in group.Groups)
		{
			_groups.Add(new GroupData(group2));
		}
		foreach (View view in group.Views)
		{
			_views.Add(new ViewData(view));
		}
	}

	public static GroupData CreateFromXMLDefinition(string partialXML)
	{
		return new GroupData(partialXML);
	}

	public GroupData AddGroupData(string partialXML)
	{
		GroupData groupData = new GroupData(partialXML);
		lock (_groupListLock)
		{
			(_propertyBase as Group).Groups.Add(groupData.Item as Group);
			_groups.Add(groupData);
			return groupData;
		}
	}

	public ViewData AddViewData(string partialXML)
	{
		ViewData viewData = ViewData.CreateFromXMLDefinition(partialXML);
		(_propertyBase as Group).Views.Add(viewData.Item as View);
		_views.Add(viewData);
		return viewData;
	}

	public void AddViewData(ViewData viewData)
	{
		(_propertyBase as Group).Views.Add(viewData.Item as View);
		_views.Add(viewData);
	}
}
