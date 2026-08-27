using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class GeminiResourceComponent : SqlResourceComponent
{
	protected override bool IsDebugEnabled => false;

	protected GeminiResourceComponent()
	{
	}

	protected GeminiResourceComponent(IGeminiRequestContext requestContext)
		: base(requestContext)
	{
	}

	protected GeminiResourceComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	protected void BindObjectId(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 36, allowNull: false, SqlDbType.UniqueIdentifier);
	}

	protected void BindObjectId(string parameterName, string parameterValue, bool allowNullOrEmpty)
	{
		BindString(parameterName, parameterValue, 36, allowNullOrEmpty, SqlDbType.UniqueIdentifier);
	}

	protected void BindArrayAsXml(string parameterName, string[] idArray)
	{
		if (idArray == null || idArray.Length == 0)
		{
			BindNullValue(parameterName, SqlDbType.Xml);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
		{
			xmlWriter.WriteStartElement("Ids");
			for (int i = 0; i < idArray.Length; i++)
			{
				xmlWriter.WriteElementString("Id", idArray[i]);
			}
			xmlWriter.WriteEndElement();
		}
		string parameterValue = stringBuilder.ToString();
		BindXml(parameterName, parameterValue);
	}

	protected void BindShortNameType(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 128, allowNull, SqlDbType.NVarChar);
	}

	protected void BindShortcutType(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 128, allowNull, SqlDbType.NVarChar);
	}

	protected void BindDescription2(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 4000, allowNull: false, SqlDbType.NText);
	}

	protected void BindInternalIdType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 100, allowNull: false, SqlDbType.VarChar);
	}

	protected void BindUriTypeShort(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 1024, allowNull, SqlDbType.NVarChar);
	}

	protected void BindCustomProperties(string parameterName, CustomProperty[] customPropertyArray)
	{
		if (customPropertyArray == null || customPropertyArray.Length == 0)
		{
			BindNullValue(parameterName, SqlDbType.Xml);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
		{
			xmlWriter.WriteStartElement("CustomProperties");
			for (int i = 0; i < customPropertyArray.Length; i++)
			{
				xmlWriter.WriteStartElement("CustomProperty");
				xmlWriter.WriteAttributeString("Name", customPropertyArray[i].Name);
				xmlWriter.WriteAttributeString("Value", customPropertyArray[i].Value);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}
		string parameterValue = stringBuilder.ToString();
		BindXml(parameterName, parameterValue);
	}

	public static string GetGuid()
	{
		return GetGuid(null);
	}

	public static string GetGuid(string userDefinedId)
	{
		string result = Guid.NewGuid().ToString().ToUpper(CultureInfo.InvariantCulture);
		if (userDefinedId.IsGuid())
		{
			result = userDefinedId.ToUpper(CultureInfo.InvariantCulture);
		}
		return result;
	}

	protected override void AddLogEntryDatabaseConnectionLost()
	{
	}

	protected override void LogDebug(string entry)
	{
	}

	protected override void LogWarning(string entryFormat, params object[] args)
	{
	}

	protected override void Trace(string entry)
	{
	}

	protected override void Trace(Exception ex)
	{
	}
}
