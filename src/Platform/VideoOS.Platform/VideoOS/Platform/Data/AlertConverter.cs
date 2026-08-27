using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace VideoOS.Platform.Data;

public class AlertConverter
{
	private XmlNamespaceManager _nsmgr;

	public uint ImageHeight { get; set; }

	public uint ImageWidth { get; set; }

	public string DefaultSpeedUnit { get; set; }

	public string DefaultSizeUnit { get; set; }

	public Dictionary<int, List<Guid>> AlarmIDMap { get; private set; }

	public Dictionary<int, Guid> ObjectIDMap { get; set; }

	public ServerId ServerId { get; set; }

	public AlertConverter()
	{
		ImageHeight = 1u;
		ImageWidth = 1u;
		AlarmIDMap = new Dictionary<int, List<Guid>>();
		ObjectIDMap = new Dictionary<int, Guid>();
		ServerId = new ServerId(string.Empty, string.Empty, 0, Guid.Empty);
	}

	public Alarm CreateAlarm(string alertXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		using (StringReader input = new StringReader(alertXml))
		{
			using XmlReader reader = XmlReader.Create(input);
			xmlDocument.Load(reader);
		}
		return CreateAlarm(xmlDocument);
	}

	public Alarm CreateAlarm(XmlDocument alertDoc)
	{
		_nsmgr = new XmlNamespaceManager(alertDoc.NameTable);
		_nsmgr.AddNamespace("ns", "http://tempuri.org/Alert.xsd");
		return CreateAlarm(alertDoc.DocumentElement);
	}

	private Alarm CreateAlarm(XmlNode alertNode)
	{
		if (alertNode != null)
		{
			return new Alarm
			{
				EventHeader = CreateEventHeader(alertNode),
				Location = ReadString(alertNode.SelectSingleNode("ns:Source/ns:Device/ns:Location/@Name", _nsmgr)),
				Count = (uint)ReadInt(alertNode.SelectSingleNode("ns:Object/@Count", _nsmgr), 0).Value,
				RuleList = CreateRuleList(alertNode.SelectNodes("ns:Rule", _nsmgr)),
				ObjectList = CreateObjectList(alertNode.SelectNodes("ns:Object", _nsmgr)),
				Vendor = CreateVendor(alertNode.SelectSingleNode("ns:Vendor", _nsmgr))
			};
		}
		return null;
	}

	private EventHeader CreateEventHeader(XmlNode alertNode)
	{
		if (alertNode != null)
		{
			Guid guid = Guid.NewGuid();
			int? num = ReadInt(alertNode.SelectSingleNode("@ID"), null);
			if (num.HasValue)
			{
				if (!AlarmIDMap.ContainsKey(num.Value))
				{
					AlarmIDMap[num.Value] = new List<Guid>();
				}
				AlarmIDMap[num.Value].Add(guid);
			}
			return new EventHeader
			{
				ID = guid,
				Timestamp = ReadTimestamp(alertNode.SelectSingleNode("@TimeStamp")),
				Class = "Analytics",
				Type = "",
				Version = "1.0",
				Priority = (byte)ReadInt(alertNode.SelectSingleNode("@Priority"), 0).Value,
				Name = ReadString(alertNode.SelectSingleNode("@Name")),
				Source = new EventSource
				{
					Name = ReadString(alertNode.SelectSingleNode("ns:Source/ns:Device/@Name", _nsmgr)),
					Description = ReadString(alertNode.SelectSingleNode("ns:Source/ns:Device/@Description", _nsmgr)),
					FQID = new FQID(ServerId, Guid.Empty, ReadGuid(alertNode.SelectSingleNode("ns:Source/ns:Device/@GUID", _nsmgr), createGuidOnFail: false), FolderType.No, new Guid("5135BA21-F1DC-4321-806A-6CE2017343C0"))
				}
			};
		}
		return null;
	}

	private RuleList CreateRuleList(XmlNodeList ruleNodes)
	{
		if (ruleNodes != null || ruleNodes.Count == 0)
		{
			RuleList ruleList = new RuleList();
			{
				foreach (XmlNode ruleNode in ruleNodes)
				{
					Rule rule = CreateRule(ruleNode);
					if (rule != null)
					{
						ruleList.Add(rule);
					}
				}
				return ruleList;
			}
		}
		return null;
	}

	private AnalyticsObjectList CreateAnalyticsObjectList(XmlNodeList objectNodes)
	{
		if (objectNodes != null || objectNodes.Count == 0)
		{
			AnalyticsObjectList analyticsObjectList = new AnalyticsObjectList();
			{
				foreach (XmlNode objectNode in objectNodes)
				{
					AnalyticsObject analyticsObject = CreateAnalyticsObject(objectNode);
					if (analyticsObject != null)
					{
						analyticsObjectList.Add(analyticsObject);
					}
				}
				return analyticsObjectList;
			}
		}
		return null;
	}

	private PolygonList CreatePolygonList(XmlNodeList polygonNodes)
	{
		if (polygonNodes != null || polygonNodes.Count == 0)
		{
			PolygonList polygonList = new PolygonList();
			{
				foreach (XmlNode polygonNode in polygonNodes)
				{
					TPolygon tPolygon = CreatePolygon(polygonNode);
					if (tPolygon != null)
					{
						polygonList.Add(tPolygon);
					}
				}
				return polygonList;
			}
		}
		return null;
	}

	private PointList CreatePointList(XmlNodeList pointNodes)
	{
		if (pointNodes != null || pointNodes.Count == 0)
		{
			PointList pointList = new PointList();
			{
				foreach (XmlNode pointNode in pointNodes)
				{
					TPoint tPoint = CreatePoint(pointNode);
					if (tPoint != null)
					{
						pointList.Add(tPoint);
					}
				}
				return pointList;
			}
		}
		return null;
	}

	private AnalyticsObjectList CreateObjectList(XmlNodeList objectNodes)
	{
		if (objectNodes != null || objectNodes.Count == 0)
		{
			AnalyticsObjectList analyticsObjectList = new AnalyticsObjectList();
			{
				foreach (XmlNode objectNode in objectNodes)
				{
					AnalyticsObject analyticsObject = CreateAnalyticsObject(objectNode);
					if (analyticsObject != null)
					{
						analyticsObjectList.Add(analyticsObject);
					}
				}
				return analyticsObjectList;
			}
		}
		return null;
	}

	private Rule CreateRule(XmlNode ruleNode)
	{
		if (ruleNode != null)
		{
			return new Rule
			{
				ID = ReadGuid(ruleNode.SelectSingleNode("@GUID"), createGuidOnFail: true),
				Name = ReadString(ruleNode.SelectSingleNode("@Name")),
				Type = ReadString(ruleNode.SelectSingleNode("@Type")),
				PolygonList = CreatePolygonList(ruleNode.SelectNodes("ns:Polygon", _nsmgr))
			};
		}
		return null;
	}

	private AnalyticsObject CreateAnalyticsObject(XmlNode objectNode)
	{
		if (objectNode != null)
		{
			Guid value = Guid.Empty;
			int? num = ReadInt(objectNode.SelectSingleNode("@ID"), null);
			if (num.HasValue && !ObjectIDMap.TryGetValue(num.Value, out value))
			{
				value = Guid.NewGuid();
				ObjectIDMap[num.Value] = value;
			}
			return new AnalyticsObject
			{
				ID = value,
				Name = ReadString(objectNode.SelectSingleNode("@Name")),
				Type = ReadString(objectNode.SelectSingleNode("@Type")),
				Description = ReadString(objectNode.SelectSingleNode("ns:Description", _nsmgr)),
				Confidence = ReadDouble(objectNode.SelectSingleNode("@Confidence"), 0.0),
				Value = ReadString(objectNode.SelectSingleNode("@Value")),
				AlarmTrigger = true,
				Removed = false,
				Size = ReadDouble(objectNode.SelectSingleNode("@Size"), 0.0),
				SizeUnit = DefaultSizeUnit,
				BoundingBox = CreateBoundingBox(objectNode.SelectSingleNode("ns:BoundingBox", _nsmgr)),
				Polygon = CreatePolygon(objectNode.SelectSingleNode("ns:Polygon", _nsmgr)),
				Motion = CreateObjectMotion(objectNode.SelectSingleNode("ns:Motion", _nsmgr))
			};
		}
		return null;
	}

	private BoundingBox CreateBoundingBox(XmlNode boundingBoxNode)
	{
		if (boundingBoxNode != null)
		{
			return new BoundingBox
			{
				Top = ReadDouble(boundingBoxNode.SelectSingleNode("@Top"), 0.0) / (double)ImageHeight,
				Left = ReadDouble(boundingBoxNode.SelectSingleNode("@Left"), 0.0) / (double)ImageWidth,
				Bottom = ReadDouble(boundingBoxNode.SelectSingleNode("@Bottom"), 0.0) / (double)ImageHeight,
				Right = ReadDouble(boundingBoxNode.SelectSingleNode("@Right"), 0.0) / (double)ImageWidth
			};
		}
		return null;
	}

	private ObjectMotion CreateObjectMotion(XmlNode motionNode)
	{
		if (motionNode != null)
		{
			return new ObjectMotion
			{
				Speed = ReadDouble(motionNode.SelectSingleNode("@Speed"), 0.0),
				SpeedUnit = DefaultSpeedUnit,
				Path = CreatePolygon(motionNode.SelectSingleNode("ns:Path", _nsmgr))
			};
		}
		return null;
	}

	private Vendor CreateVendor(XmlNode vendorNode)
	{
		if (vendorNode != null)
		{
			return new Vendor
			{
				Name = ReadString(vendorNode.SelectSingleNode("@Name")),
				CustomData = ((vendorNode.InnerXml == string.Empty) ? null : vendorNode.InnerXml)
			};
		}
		return null;
	}

	private TPolygon CreatePolygon(XmlNode polygonNode)
	{
		if (polygonNode != null)
		{
			XmlNode colorNode = polygonNode.SelectSingleNode("ns:Color", _nsmgr);
			return new TPolygon
			{
				Color = CreateColor(colorNode),
				PointList = CreatePointList(polygonNode.SelectNodes("ns:Point", _nsmgr))
			};
		}
		return null;
	}

	private TPoint CreatePoint(XmlNode pointNode)
	{
		if (pointNode != null)
		{
			return new TPoint
			{
				X = ReadDouble(pointNode.SelectSingleNode("@X"), 0.0) / (double)ImageWidth,
				Y = ReadDouble(pointNode.SelectSingleNode("@Y"), 0.0) / (double)ImageHeight
			};
		}
		return null;
	}

	private TColor CreateColor(XmlNode colorNode)
	{
		if (colorNode != null)
		{
			return new TColor
			{
				A = (byte)ReadInt(colorNode.SelectSingleNode("@A"), 0).Value,
				R = (byte)ReadInt(colorNode.SelectSingleNode("@R"), 0).Value,
				G = (byte)ReadInt(colorNode.SelectSingleNode("@G"), 0).Value,
				B = (byte)ReadInt(colorNode.SelectSingleNode("@B"), 0).Value
			};
		}
		return null;
	}

	private string ReadString(XmlNode node)
	{
		return node?.InnerText;
	}

	private int? ReadInt(XmlNode node, int? defaultValue)
	{
		if (node != null && int.TryParse(node.InnerText, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	private double ReadDouble(XmlNode node, double defaultValue)
	{
		if (node != null && double.TryParse(node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	private DateTime ReadTimestamp(XmlNode node)
	{
		if (node != null && DateTime.TryParse(node.InnerText, out var result))
		{
			return result;
		}
		return DateTime.Now;
	}

	private Guid ReadGuid(XmlNode node, bool createGuidOnFail)
	{
		if (node != null)
		{
			try
			{
				return new Guid(node.InnerText);
			}
			catch
			{
			}
		}
		if (!createGuidOnFail)
		{
			return Guid.Empty;
		}
		return Guid.NewGuid();
	}
}
