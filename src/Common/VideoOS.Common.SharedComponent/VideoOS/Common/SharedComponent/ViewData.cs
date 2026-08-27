using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedComponent.Data;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace VideoOS.Common.SharedComponent;

public class ViewData : ItemDataBase
{
	public ReadOnlyCollection<ViewItemInfo> ViewItemsInfo
	{
		get
		{
			_ = (_propertyBase as View).ViewItems;
			List<ViewItemInfo> list = new List<ViewItemInfo>();
			View view = _propertyBase as View;
			foreach (ViewItem2 viewItem in view.ViewItems)
			{
				ViewItemInfo viewItemInfo = new ViewItemInfo();
				viewItemInfo.SetViewItemData(new ViewItemData(viewItem));
				list.Add(viewItemInfo);
			}
			for (int i = 0; i < view.ViewLayout.ViewItems.Count; i++)
			{
				list[i].SetSizeAndLocation(new SizeAndLocation(view.ViewLayout.ViewItems[i].Size, view.ViewLayout.ViewItems[i].Position, i));
			}
			return new ReadOnlyCollection<ViewItemInfo>(list);
		}
	}

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

	private ViewData(string partialXML)
	{
		if (string.IsNullOrEmpty(partialXML))
		{
			throw new SharedComponentException("ViewData cannot be created without partialXml");
		}
		_propertyBase = new View();
		PartialXML = partialXML;
		AddViewItems();
	}

	public ViewData(bool addItems)
	{
		_propertyBase = new View();
		(_propertyBase as View).ViewLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout1x1, VideoOS.RemoteClient.Application";
		if (addItems)
		{
			AddViewItems();
		}
	}

	public ViewData()
	{
		_propertyBase = new View();
	}

	public ViewData(View view)
	{
		_propertyBase = view;
	}

	public static ViewData CreateFromXMLDefinition(string partialXML)
	{
		return new ViewData(partialXML);
	}

	public static ViewData CreateFromXMLPresets(string fullXML)
	{
		try
		{
			if (!string.IsNullOrEmpty(fullXML))
			{
				XmlReader reader = XmlReader.Create(new StringReader(fullXML));
				ViewData viewData = new ViewData();
				viewData.ReadXml(reader);
				return viewData;
			}
			throw new SharedComponentException("The fulleXML cannot be null or empty");
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("ViewData creation error", innerException);
		}
	}

	public static ViewData CreateFromXML(string fullXML)
	{
		try
		{
			if (!string.IsNullOrEmpty(fullXML))
			{
				XmlReader reader = XmlReader.Create(new StringReader(fullXML));
				ViewData viewData = new ViewData();
				viewData.ReadXml(reader);
				return viewData;
			}
			throw new SharedComponentException("The fulleXML cannot be null or empty");
		}
		catch (Exception innerException)
		{
			throw new SharedComponentException("ViewData creation error", innerException);
		}
	}

	public ViewItemData SetViewItemData(int index, string partialXML)
	{
		if (index < 0)
		{
			throw new ArgumentException("The index cannot be less than the size of the ViewItems");
		}
		if (index >= (_propertyBase as View).ViewItems.Count)
		{
			throw new ArgumentException("The index cannot be greather than the size of ViewItems");
		}
		ViewItemData viewItemData = ViewItemData.CreateFromXMLDefinition(partialXML);
		(_propertyBase as View).ViewItems[index] = viewItemData.Item as ViewItem2;
		return viewItemData;
	}

	public string GetFullXML()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
		xmlWriterSettings.OmitXmlDeclaration = true;
		XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
		if (_propertyBase is View view)
		{
			view.WriteXml(xmlWriter);
		}
		xmlWriter.Flush();
		xmlWriter.Close();
		string result = Encoding.UTF8.GetString(memoryStream.ToArray());
		memoryStream.Close();
		memoryStream.Dispose();
		return result;
	}

	public bool HasSameLayout(ViewData viewData)
	{
		ViewLayout viewLayout = (viewData._propertyBase as View).ViewLayout;
		return (_propertyBase as View).ViewLayout.IsSameLayout(viewLayout);
	}

	public void ResizeView(int newNumberOfItemsSquared)
	{
		if (newNumberOfItemsSquared < 2)
		{
			throw new SharedComponentException("Resize to less than 2x2 is not supported");
		}
		if (newNumberOfItemsSquared > 5)
		{
			throw new SharedComponentException("Resize to greater than 5x5 is not supported");
		}
		int count = (_propertyBase as View).ViewLayout.ViewItems.Count;
		if (newNumberOfItemsSquared * newNumberOfItemsSquared < count)
		{
			throw new SharedComponentException("Resize to smaller views is not supported");
		}
		new KnownViewLayouts();
		(_propertyBase as View).ViewLayoutType = string.Format(CultureInfo.InvariantCulture, "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayout{0}x{0}, VideoOS.RemoteClient.Application", newNumberOfItemsSquared);
		AddViewItems();
	}

	private void ReadXml(XmlReader reader)
	{
		(_propertyBase as View).ReadXml(reader);
	}

	private void AddViewItems()
	{
		if (!(_propertyBase is View view))
		{
			return;
		}
		int count = (_propertyBase as View).ViewItems.Count;
		if (view.ViewLayout.ViewItems.Count < count)
		{
			throw new SharedComponentException("There are more viewitems in the view than in the viewlayout");
		}
		for (int i = 0; i < view.ViewLayout.ViewItems.Count - count; i++)
		{
			using MemoryStream memoryStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Encoding = Encoding.UTF8;
			xmlWriterSettings.OmitXmlDeclaration = true;
			using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			xmlWriter.WriteStartElement("viewitem");
			xmlWriter.WriteAttributeString("id", Guid.NewGuid().ToString());
			xmlWriter.WriteAttributeString("displayname", "Empty ViewItem");
			xmlWriter.WriteAttributeString("shortcut", "");
			xmlWriter.WriteAttributeString("type", "VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application");
			xmlWriter.WriteStartElement("properties");
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			ViewItemData viewItemData = ViewItemData.CreateFromXMLDefinition(new StreamReader(memoryStream, Encoding.UTF8).ReadToEnd());
			(_propertyBase as View).ViewItems.Add(viewItemData.Item as ViewItem2);
		}
	}
}
