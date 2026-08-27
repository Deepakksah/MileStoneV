using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Types;
using VideoOS.Platform;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MIPItemComponent : GeminiResourceComponent, IMIPItemComponent, IDisposable
{
	private static class MIPItemColumns
	{
		private static DatabaseColumnBinder ItemId = new DatabaseColumnBinder("ItemId");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Kind = new DatabaseColumnBinder("Kind");

		private static DatabaseColumnBinder ParentId = new DatabaseColumnBinder("ParentId");

		private static DatabaseColumnBinder ServerId = new DatabaseColumnBinder("ServerId");

		private static DatabaseColumnBinder ServerScheme = new DatabaseColumnBinder("ServerScheme");

		private static DatabaseColumnBinder ServerHost = new DatabaseColumnBinder("ServerHost");

		private static DatabaseColumnBinder ServerPort = new DatabaseColumnBinder("ServerPort");

		private static DatabaseColumnBinder ServerType = new DatabaseColumnBinder("ServerType");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder Properties = new DatabaseColumnBinder("Properties");

		private static DatabaseColumnBinder GisPoint = new DatabaseColumnBinder("GisPointString");

		private static DatabaseColumnBinder Encrypted = new DatabaseColumnBinder("Encrypted");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static Item Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			Guid guid = ItemId.GetGuid2(reader);
			string name = Name.GetString(reader, allowNulls: true);
			Guid guid2 = Kind.GetGuid2(reader);
			Guid guid3 = ParentId.GetGuid2(reader, allowNulls: true, Guid.Empty);
			Guid guid4 = ServerId.GetGuid2(reader);
			string scheme = ServerScheme.GetString(reader);
			string hostname = ServerHost.GetString(reader);
			int @int = ServerPort.GetInt32(reader);
			string type = ServerType.GetString(reader, allowNulls: true);
			bool boolean = Enabled.GetBoolean(reader);
			string text = Properties.GetString(reader);
			string geographyString = GisPoint.GetGeographyString(reader);
			bool boolean2 = Encrypted.GetBoolean(reader);
			DateTime dateTime = LastModified.GetDateTime(reader);
			Item item = new Item(new FQID(new ServerId(type, scheme, hostname, @int, guid4), guid3, guid, FolderType.No, guid2), name);
			item.PositioningInformation = new PositioningInformation(geographyString, 0.0, 0.0, 0.0);
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (boolean2)
				{
					text = requestContext.GetEncryptionService().Decrypt(text);
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("properties");
				if (xmlNode != null)
				{
					XmlNodeList xmlNodeList = xmlNode.SelectNodes("property");
					if (xmlNodeList != null)
					{
						foreach (XmlNode item2 in xmlNodeList)
						{
							string value = item2.Attributes["name"].Value;
							string value2 = item2.Attributes["value"].Value;
							item.Properties[value] = value2;
						}
					}
				}
			}
			if (item.Properties.ContainsKey("Enabled") && bool.TryParse(item.Properties["Enabled"], out var _))
			{
				item.Properties["Enabled"] = boolean.ToString(CultureInfo.InvariantCulture);
				item.Properties["EnabledPropertySpecialFormat"] = "Boolean";
			}
			else
			{
				item.Enabled = boolean;
			}
			if (item.Properties.ContainsKey("__Encrypt"))
			{
				item.Properties["__Encrypt"] = boolean2.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				item.Properties.Add("__Encrypt", boolean2.ToString(CultureInfo.InvariantCulture));
			}
			item.Properties["LastModified"] = dateTime.ToString("O", CultureInfo.InvariantCulture);
			return item;
		}
	}

	public const string EnabledPropertySpecialFormat = "EnabledPropertySpecialFormat";

	private const string BooleanProperty = "Boolean";

	public void DeleteItem(Guid id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[DeleteItem]");
			BindUniqueIdentifier("@ItemId", id);
			ExecuteNonQuery();
		});
	}

	public Item QueryItem(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryItem]");
			BindUniqueIdentifier("@ItemId", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Item)null;
			}
			Item result = MIPItemColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Item[] QueryItemsByKind(Guid kind)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryItemsByKind]");
			BindUniqueIdentifier("@Kind", kind);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Item> list = new List<Item>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPItemColumns.Bind(databaseDataReader, RequestContext));
			}
			return list.ToArray();
		});
	}

	public Item[] QueryItemsByKind(Guid kind, Guid parentId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryItemsByKindAndParent]");
			BindUniqueIdentifier("@Kind", kind);
			BindUniqueIdentifier("@ParentId", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Item> list = new List<Item>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPItemColumns.Bind(databaseDataReader, RequestContext));
			}
			return list.ToArray();
		});
	}

	public Item[] QueryItemsByParent(Guid parentId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryItemsByParent]");
			BindUniqueIdentifier("@ParentId", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Item> list = new List<Item>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPItemColumns.Bind(databaseDataReader, RequestContext));
			}
			return list.ToArray();
		});
	}

	public void SaveItem(Item item, string modifiedUser)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (modifiedUser == null)
		{
			throw new ArgumentNullException("modifiedUser");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[SaveItem]");
			DateTime? parameterValue = null;
			if (item.Properties.ContainsKey("LastModified") && DateTime.TryParse(item.Properties["LastModified"], out var result))
			{
				parameterValue = result.ToUniversalTime();
			}
			if (item.Properties.ContainsKey("Enabled") && !item.Properties.ContainsKey("EnabledPropertySpecialFormat") && bool.TryParse(item.Properties["Enabled"], out var _))
			{
				item.Properties["EnabledPropertySpecialFormat"] = "Boolean";
			}
			bool flag = false;
			if (item.Properties.ContainsKey("__Encrypt"))
			{
				if (bool.TryParse(item.Properties["__Encrypt"], out var result3))
				{
					flag = result3;
				}
				item.Properties.Remove("__Encrypt");
			}
			BindUniqueIdentifier("@ItemId", item.FQID.ObjectId);
			BindText("@Name", item.Name, allowNull: true);
			BindUniqueIdentifier("@Kind", item.FQID.Kind);
			BindUniqueIdentifier("@ParentId", item.FQID.ParentId);
			BindUniqueIdentifier("@ServerId", item.FQID.ServerId.Id);
			BindText("@ServerScheme", item.FQID.ServerId.ServerScheme, allowNull: false);
			BindText("@ServerHost", item.FQID.ServerId.ServerHostname, allowNull: false);
			BindInt("@ServerPort", item.FQID.ServerId.Serverport);
			BindText("@ServerType", item.FQID.ServerId.ServerType, allowNull: true);
			BindBoolean("@Enabled", item.Enabled);
			if (item.Properties.ContainsKey("Enabled"))
			{
				item.Properties.Remove("Enabled");
			}
			if (item.Properties.ContainsKey("Disabled"))
			{
				item.Properties.Remove("Disabled");
			}
			string parameterValue2 = PositionToPointString(item.PositioningInformation);
			if (item.Properties.ContainsKey("GisPoint"))
			{
				item.Properties.Remove("GisPoint");
			}
			if (item.Properties.ContainsKey("LastModified"))
			{
				item.Properties.Remove("LastModified");
			}
			string text = SerializeProperties(item);
			if (flag)
			{
				text = RequestContext.GetEncryptionService().Encrypt(text);
			}
			BindText("@Properties", text, allowNull: true);
			BindText("@GisPoint", parameterValue2, allowNull: true);
			BindBoolean("@Encrypted", flag);
			BindDateTime("@LastModifiedBefore", parameterValue);
			BindText("@ModifiedUser", modifiedUser, allowNull: false);
			ExecuteNonQuery();
		});
	}

	private static string PositionToPointString(PositioningInformation positioningInformation)
	{
		if (positioningInformation != null && positioningInformation.IsPointDefined)
		{
			return new string(SqlGeometry.Point(positioningInformation.Longitude, positioningInformation.Latitude, 4326).STAsText().Value);
		}
		return null;
	}

	private static string SerializeProperties(Item item)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.WriteStartElement("properties");
			foreach (KeyValuePair<string, string> property in item.Properties)
			{
				xmlTextWriter.WriteStartElement("property");
				xmlTextWriter.WriteAttributeString("name", property.Key);
				xmlTextWriter.WriteAttributeString("value", property.Value);
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}
}
