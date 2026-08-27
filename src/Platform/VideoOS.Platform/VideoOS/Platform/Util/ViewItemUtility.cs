using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Util;

public static class ViewItemUtility
{
	private static Dictionary<Guid, string> _nameFromType = new Dictionary<Guid, string>
	{
		{
			ViewAndLayoutItem.EmptyBuiltinId,
			"Empty view item"
		},
		{
			ViewAndLayoutItem.CameraBuiltinId,
			"Camera ViewItem"
		},
		{
			ViewAndLayoutItem.HotspotBuiltinId,
			"Hotspot view item"
		},
		{
			ViewAndLayoutItem.CarrouselBuiltinId,
			"Carrousel view item"
		},
		{
			ViewAndLayoutItem.MatrixBuiltinId,
			"Matrix view item"
		},
		{
			ViewAndLayoutItem.HTMLBuiltinId,
			"HTML view item"
		},
		{
			ViewAndLayoutItem.MapBuiltinId,
			"Map view item"
		},
		{
			ViewAndLayoutItem.GisMapBuiltinId,
			"GisMap view item"
		},
		{
			ViewAndLayoutItem.SystemMonitorBuiltinId,
			"SystemMonitor view item"
		}
	};

	private static Dictionary<Guid, string> _classFromType = new Dictionary<Guid, string>
	{
		{
			ViewAndLayoutItem.EmptyBuiltinId,
			"VideoOS.RemoteClient.Application.Data.Configuration.EmptyViewItem, VideoOS.RemoteClient.Application"
		},
		{
			ViewAndLayoutItem.CameraBuiltinId,
			"VideoOS.RemoteClient.Application.Data.ContentTypes.CameraContentType.CameraViewItem, VideoOS.RemoteClient.Application"
		},
		{
			ViewAndLayoutItem.HotspotBuiltinId,
			"VideoOS.RemoteClient.Plugin.Hotspot.HotspotViewItem, VideoOS.RemoteClient.Plugin.Hotspot"
		},
		{
			ViewAndLayoutItem.CarrouselBuiltinId,
			"VideoOS.RemoteClient.Plugin.Carrousel.CarrouselViewItem, VideoOS.RemoteClient.Plugin.Carrousel"
		},
		{
			ViewAndLayoutItem.MatrixBuiltinId,
			"VideoOS.RemoteClient.Plugin.Matrix.MatrixViewItem, VideoOS.RemoteClient.Plugin.Matrix"
		},
		{
			ViewAndLayoutItem.HTMLBuiltinId,
			"VideoOS.RemoteClient.Plugin.HtmlViewItem.HtmlViewItem, VideoOS.RemoteClient.Plugin.HtmlViewItem"
		},
		{
			ViewAndLayoutItem.MapBuiltinId,
			"VideoOS.RemoteClient.Plugin.Map.MapViewItem, VideoOS.RemoteClient.Plugin.Map"
		},
		{
			ViewAndLayoutItem.GisMapBuiltinId,
			"VideoOS.RemoteClient.Plugin.GisMap.GisMapViewItem, VideoOS.RemoteClient.Plugin.GisMap"
		},
		{
			ViewAndLayoutItem.SystemMonitorBuiltinId,
			"VideoOS.RemoteClient.Application.Data.ContentTypes.ReportingType.ReportingViewItem"
		}
	};

	public static string ConvertToXml(ViewItemInformation viewItemInformation, bool includeTopNode)
	{
		if (viewItemInformation == null)
		{
			throw new ArgumentNullException("viewItemInformation");
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		Guid guid = Guid.NewGuid();
		bool flag = viewItemInformation.Attributes.SingleOrDefault((MyKeyValue x) => x.Key == "type" && x.Value == "VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application") != null;
		string value;
		string value2;
		if (flag)
		{
			value = viewItemInformation.Attributes.SingleOrDefault((MyKeyValue x) => x.Key == "displayname")?.Value ?? "Unknown";
			value2 = viewItemInformation.Attributes.SingleOrDefault((MyKeyValue x) => x.Key == "type")?.Value ?? "Unknown";
		}
		else
		{
			value = GetNameFromType(viewItemInformation.ViewItemType);
			value2 = GetClassFromType(viewItemInformation.ViewItemType);
		}
		if (includeTopNode)
		{
			xmlTextWriter.WriteStartElement("viewitems");
		}
		xmlTextWriter.WriteStartElement("viewitem");
		xmlTextWriter.WriteAttributeString("id", guid.ToString());
		xmlTextWriter.WriteAttributeString("displayname", value);
		xmlTextWriter.WriteAttributeString("shortcut", "");
		xmlTextWriter.WriteAttributeString("type", value2);
		xmlTextWriter.WriteStartElement("iteminfo");
		if (viewItemInformation.Attributes != null)
		{
			MyKeyValue[] attributes = viewItemInformation.Attributes;
			foreach (MyKeyValue myKeyValue in attributes)
			{
				xmlTextWriter.WriteAttributeString(myKeyValue.Key, myKeyValue.Value);
			}
		}
		if (flag)
		{
			xmlTextWriter.WriteStartElement("viewitem");
			xmlTextWriter.WriteAttributeString("id", Guid.Empty.ToString());
			xmlTextWriter.WriteStartElement("properties");
			if (viewItemInformation.Properties != null)
			{
				MyKeyValue[] attributes = viewItemInformation.Properties;
				foreach (MyKeyValue myKeyValue2 in attributes)
				{
					xmlTextWriter.WriteStartElement("property");
					xmlTextWriter.WriteAttributeString("name", myKeyValue2.Key);
					xmlTextWriter.WriteAttributeString("value", myKeyValue2.Value);
					xmlTextWriter.WriteEndElement();
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("properties");
		if (!flag && viewItemInformation.Properties != null)
		{
			MyKeyValue[] attributes = viewItemInformation.Properties;
			foreach (MyKeyValue myKeyValue3 in attributes)
			{
				xmlTextWriter.WriteStartElement("property");
				xmlTextWriter.WriteAttributeString("name", myKeyValue3.Key);
				xmlTextWriter.WriteAttributeString("value", myKeyValue3.Value);
				xmlTextWriter.WriteEndElement();
			}
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		if (includeTopNode)
		{
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.Flush();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	private static string GetNameFromType(Guid viewItemType)
	{
		if (_nameFromType.ContainsKey(viewItemType))
		{
			return _nameFromType[viewItemType];
		}
		return "Unknown type";
	}

	private static string GetClassFromType(Guid viewItemType)
	{
		if (_classFromType.ContainsKey(viewItemType))
		{
			return _classFromType[viewItemType];
		}
		return "";
	}
}
