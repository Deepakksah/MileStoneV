using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform;

[Serializable]
[DataContract]
public class FQID
{
	[DataMember(Name = "ServerId", Order = 1, EmitDefaultValue = false)]
	public ServerId ServerId { get; set; }

	[DataMember(Name = "ParentId", Order = 2, EmitDefaultValue = false)]
	public Guid ParentId { get; set; }

	[DataMember(Name = "ObjectId", Order = 3, EmitDefaultValue = false)]
	public Guid ObjectId { get; set; }

	[DataMember(Name = "ObjectIdString", Order = 4, EmitDefaultValue = false)]
	public string ObjectIdString { get; set; }

	[DataMember(Name = "Kind", Order = 6)]
	public Guid Kind { get; set; }

	[DataMember(Name = "FolderType", Order = 5)]
	public FolderType FolderType { get; set; }

	public FQID()
	{
	}

	public FQID(ServerId serverId, Guid parentId, Guid objectId, FolderType folderType, Guid kind)
	{
		ServerId = serverId;
		ParentId = parentId;
		ObjectId = objectId;
		FolderType = folderType;
		Kind = kind;
	}

	public FQID(ServerId serverId, Guid parentId, string objectIdString, FolderType folderType, Guid kind)
	{
		ServerId = serverId;
		ParentId = parentId;
		ObjectIdString = objectIdString;
		ObjectId = Guid.Empty;
		FolderType = folderType;
		Kind = kind;
	}

	public FQID(ServerId serverId, FolderType folderType, Guid kind)
	{
		ServerId = serverId;
		ObjectId = Guid.Empty;
		ParentId = Guid.Empty;
		FolderType = folderType;
		Kind = kind;
	}

	public FQID(ServerId serverId)
	{
		ServerId = serverId;
		ObjectId = Guid.Empty;
		ParentId = Guid.Empty;
		FolderType = FolderType.No;
		Kind = VideoOS.Platform.Kind.Server;
	}

	public FQID(string xmlString)
	{
		if (xmlString != null)
		{
			BuildFromXmlNode(new XmlDocument
			{
				InnerXml = xmlString
			});
			if (Configuration.Instance != null && Configuration.Instance.ServerFQID != null && Configuration.Instance.ServerFQID.ServerId != null && Configuration.Instance.ServerFQID.ServerId.UserContext != null)
			{
				ServerId.UserContext = Configuration.Instance.ServerFQID.ServerId.UserContext;
			}
		}
	}

	public FQID(string xmlString, UserContext context)
	{
		if (xmlString == null)
		{
			throw new ArgumentNullException("xmlString");
		}
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		XmlDocument xmlNode = new XmlDocument
		{
			InnerXml = xmlString
		};
		BuildFromXmlNode(xmlNode);
		ServerId.UserContext = context;
	}

	public FQID(XmlNode xmlNode)
	{
		if (xmlNode != null)
		{
			BuildFromXmlNode(xmlNode);
		}
	}

	public FQID GetParent()
	{
		try
		{
			if (ParentId == Guid.Empty)
			{
				return null;
			}
			if (Kind == VideoOS.Platform.Kind.Server)
			{
				return FindItem(ServerId, ParentId, VideoOS.Platform.Kind.Server)?.FQID;
			}
			if (Kind == VideoOS.Platform.Kind.Bookmark)
			{
				Item item = FindItem(ServerId, ParentId, VideoOS.Platform.Kind.Camera);
				if (item == null)
				{
					item = FindItem(ServerId, ParentId, VideoOS.Platform.Kind.Microphone);
				}
				if (item == null)
				{
					item = FindItem(ServerId, ParentId, VideoOS.Platform.Kind.Speaker);
				}
				return item?.FQID;
			}
			Item item2 = ((!EnvironmentManager.UsingUserContext) ? Configuration.Instance.GetItem(this) : ServerId.UserContext.Configuration.GetItem(this));
			if (item2 != null)
			{
				Item item = item2.GetParent();
				if (item != null)
				{
					return item.FQID;
				}
			}
			return null;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("FQID:Parent", exception);
			return null;
		}
	}

	private Item FindItem(ServerId serverId, Guid objectId, Guid kind)
	{
		if (EnvironmentManager.UsingUserContext)
		{
			return ServerId.UserContext.Configuration.GetItem(ParentId, kind);
		}
		return Configuration.Instance.GetItem(ParentId, kind);
	}

	public FQID Clone()
	{
		return new FQID(ServerId.Clone(), ParentId, ObjectId, FolderType, Kind)
		{
			ObjectIdString = ObjectIdString
		};
	}

	public override int GetHashCode()
	{
		return ServerId.GetHashCode() * Kind.GetHashCode() * ObjectId.GetHashCode();
	}

	public override bool Equals(object fqid)
	{
		FQID fQID = fqid as FQID;
		if (fQID == null)
		{
			return false;
		}
		if (ServerId == null)
		{
			if (fQID.ServerId != null)
			{
				return false;
			}
		}
		else if (!ServerId.Equals(fQID.ServerId))
		{
			return false;
		}
		if (ParentId != fQID.ParentId)
		{
			return false;
		}
		if (ObjectId != fQID.ObjectId)
		{
			return false;
		}
		if (ObjectIdString != fQID.ObjectIdString)
		{
			return false;
		}
		if (Kind != fQID.Kind)
		{
			return false;
		}
		if (FolderType != fQID.FolderType)
		{
			return false;
		}
		return true;
	}

	public static bool operator ==(FQID obj1, FQID obj2)
	{
		if ((object)obj1 == obj2)
		{
			return true;
		}
		if ((object)obj1 == null)
		{
			return false;
		}
		if ((object)obj2 == null)
		{
			return false;
		}
		return obj1.Equals(obj2);
	}

	public static bool operator !=(FQID obj1, FQID obj2)
	{
		return !(obj1 == obj2);
	}

	public bool EqualsIgnoreFolderType(object fqid)
	{
		FQID fQID = fqid as FQID;
		if (fQID == null)
		{
			return false;
		}
		if (ServerId == null)
		{
			if (fQID.ServerId != null)
			{
				return false;
			}
		}
		else if (!ServerId.Equals(fQID.ServerId))
		{
			return false;
		}
		if (ParentId != fQID.ParentId)
		{
			return false;
		}
		if (ObjectId != fQID.ObjectId)
		{
			return false;
		}
		if (ObjectIdString != fQID.ObjectIdString)
		{
			return false;
		}
		if (Kind != fQID.Kind)
		{
			return false;
		}
		return true;
	}

	public bool EqualGuids(object fqid)
	{
		FQID fQID = fqid as FQID;
		if (fQID == null)
		{
			return false;
		}
		if (ServerId == null || fQID.ServerId == null)
		{
			if (ServerId != null || fQID.ServerId != null)
			{
				return false;
			}
		}
		else if (ServerId.Id != fQID.ServerId.Id)
		{
			return false;
		}
		if (ParentId != fQID.ParentId)
		{
			return false;
		}
		if (ObjectId != fQID.ObjectId)
		{
			return false;
		}
		if (Kind != fQID.Kind)
		{
			return false;
		}
		return true;
	}

	public bool Contains(string match)
	{
		if (match == null)
		{
			return false;
		}
		if (ServerId == null)
		{
			return false;
		}
		if (!match.Contains(Kind.ToString()) && !match.Contains(ObjectId.ToString()))
		{
			return ServerId.Contains(match);
		}
		return true;
	}

	private void BuildFromXmlNode(XmlNode xmlNode)
	{
		string type = "";
		string hostname = "";
		string port = "80";
		string g = Guid.Empty.ToString();
		string text = "";
		string text2 = "";
		string scheme = "http";
		int result = 0;
		if (xmlNode != null)
		{
			try
			{
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("FQID");
				if (xmlNode2 != null)
				{
					xmlNode = xmlNode2;
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("ServerId/Type");
				if (xmlNode3 != null)
				{
					type = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ServerId/Scheme");
				if (xmlNode3 != null)
				{
					scheme = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ServerId/Hostname");
				if (xmlNode3 != null)
				{
					hostname = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ServerId/Port");
				if (xmlNode3 != null)
				{
					port = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ServerId/Id");
				if (xmlNode3 != null)
				{
					g = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ParentId");
				if (xmlNode3 != null)
				{
					text = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ObjectId");
				if (xmlNode3 != null)
				{
					text2 = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("ObjectIdString");
				if (xmlNode3 != null)
				{
					ObjectIdString = xmlNode3.InnerText;
				}
				xmlNode3 = xmlNode.SelectSingleNode("FolderType");
				if (xmlNode3 != null)
				{
					int.TryParse(xmlNode3.InnerText, out result);
				}
				xmlNode3 = xmlNode.SelectSingleNode("Kind");
				if (xmlNode3 != null)
				{
					Kind = new Guid(xmlNode3.InnerText);
				}
				ServerId = new ServerId(type, scheme, hostname, port, new Guid(g));
				if (text != "")
				{
					ParentId = new Guid(text);
				}
				else
				{
					ParentId = Guid.Empty;
				}
				if (text2 != "")
				{
					ObjectId = new Guid(text2);
				}
				else
				{
					ObjectId = Guid.Empty;
				}
				FolderType = (FolderType)result;
				return;
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("FQID.InstantiateFromXml:" + xmlNode.InnerXml, exception);
				ServerId = new ServerId("NA", "nourl", 80, Guid.Empty);
				return;
			}
		}
		ServerId = new ServerId("NA", "nourl", 80, Guid.Empty);
	}

	public void Serialize(XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("FQID");
		xmlWriter.WriteStartElement("ServerId");
		if (ServerId != null)
		{
			xmlWriter.WriteElementString("Type", ServerId.ServerType);
			xmlWriter.WriteElementString("Scheme", ServerId.ServerScheme);
			xmlWriter.WriteElementString("Hostname", ServerId.ServerHostname);
			xmlWriter.WriteElementString("Port", ServerId.Serverport.ToString(CultureInfo.InvariantCulture));
			xmlWriter.WriteElementString("Id", ServerId.Id.ToString());
		}
		xmlWriter.WriteEndElement();
		if (ParentId != Guid.Empty)
		{
			xmlWriter.WriteElementString("ParentId", ParentId.ToString());
		}
		if (ObjectId != Guid.Empty)
		{
			xmlWriter.WriteElementString("ObjectId", ObjectId.ToString());
		}
		if (ObjectIdString != null)
		{
			xmlWriter.WriteElementString("ObjectIdString", ObjectIdString);
		}
		xmlWriter.WriteElementString("FolderType", ((int)FolderType).ToString(CultureInfo.InvariantCulture));
		xmlWriter.WriteElementString("Kind", Kind.ToString());
		xmlWriter.WriteEndElement();
	}

	public XmlNode ToXmlNode()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			Serialize(xmlWriter);
			return new XmlDocument
			{
				InnerXml = stringBuilder.ToString()
			};
		}
		finally
		{
			stringWriter?.Dispose();
		}
	}

	public new string ToString()
	{
		string text = "";
		if (ServerId != null && ServerId.ServerType != null && ServerId.ServerHostname != null)
		{
			text = "Server:" + ServerId.ServerType + ":" + ServerId.ServerHostname + "  Id:" + ServerId.Id.ToString();
		}
		return text + ", ObjectId:" + ObjectId.ToString() + ", Type:" + Kind.ToString();
	}
}
