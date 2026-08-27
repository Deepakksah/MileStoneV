using System;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.SharedComponent.Data;

namespace VideoOS.Common.SharedComponent;

public class ViewItemData : ItemDataBase
{
	public Guid SmartClientId
	{
		set
		{
			(_propertyBase as ViewItem2).SmartClientId = value;
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

	public Type GetViewItemType{
		get
		{
			if ((_propertyBase as ViewItem2).Type != null)
			{
				return Type.GetType((_propertyBase as ViewItem2).Type);
			}
			return null;
		}
	}

	public string GetViewItemTypeString => (_propertyBase as ViewItem2).Type;

	public ViewItemType ViewItemType
	{
		get
		{
			ViewItemType viewItemType = ViewItemType.Unknown;
			string text = "VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application";
			string getViewItemTypeString = GetViewItemTypeString;
			if ("VideoOS.RemoteClient.Application.Data.ContentTypes.CameraContentType.CameraViewItem, VideoOS.RemoteClient.Application" == getViewItemTypeString)
			{
				return ViewItemType.Camera;
			}
			if (text == getViewItemTypeString)
			{
				return ViewItemType.Empty;
			}
			return ViewItemType.Unknown;
		}
	}

	public ViewItemInformation ViewItemInformation
	{
		get
		{
			switch (ViewItemType)
			{
			case ViewItemType.Camera:
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(PartialXML);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("//iteminfo/@cameraid");
					if (xmlNode != null)
					{
						return new CameraViewItemInformation(new Guid(xmlNode.Value));
					}
					return new CameraViewItemInformation(Guid.Empty);
				}
				catch (Exception)
				{
					return null;
				}
			case ViewItemType.Empty:
				return null;
			case ViewItemType.Unknown:
				return null;
			default:
				return null;
			}
		}
	}

	private ViewItemData(string partialXML)
	{
		_propertyBase = new ViewItem2();
		PartialXML = partialXML;
	}

	public ViewItemData(ViewItem2 viewItem)
	{
		_propertyBase = viewItem;
	}

	public static ViewItemData CreateFromXMLDefinition(string partialXML)
	{
		return new ViewItemData(partialXML);
	}
}
