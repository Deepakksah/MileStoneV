using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.Definition;

public class TypeDefinition
{
	private MainDefinition mainDefinition;

	private CapabilityDefinition capability;

	private typeType type;

	public ILogger Logger { get; private set; }

	public typeType Base
	{
		[DebuggerStepThrough]
		get
		{
			return type;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return type.name;
		}
	}

	public bool Visible
	{
		get
		{
			if (type.visibleSpecified)
			{
				return type.visible;
			}
			return true;
		}
	}

	public access Access
	{
		[DebuggerStepThrough]
		get
		{
			return type.access;
		}
	}

	public bool IsReadOnly
	{
		[DebuggerStepThrough]
		get
		{
			return Access == access.read;
		}
	}

	public datatype ActiveDataType => (GetActiveTypeDefinition ?? throw new ActiveTypeDefinitionMissingException("No active typedef found for the capability. Capability: " + capability.FullName)).datatype;

	public typename ActiveTypeName => (GetActiveTypeDefinition ?? throw new ActiveTypeDefinitionMissingException("No active typedef found for the capability. Capability: " + capability.FullName)).typename;

	public string DisplayNameResolved => type.displayname.Resolve();

	public Guid DisplayNameReferenceId => new Guid(type.displayname.referenceid);

	public Guid ReferenceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(type.referenceid);
		}
	}

	public typedefType GetActiveTypeDefinition
	{
		get
		{
			try
			{
				typedefType[] typedef = type.typedef;
				foreach (typedefType typedefType2 in typedef)
				{
					if (SelectResult(mainDefinition, capability, typedefType2.select))
					{
						return typedefType2;
					}
				}
			}
			catch (NullReferenceException ex)
			{
				string message = ex.Message;
				LoggerExtensions.LogError(Logger, "Type definition of a device is missing {Exception}", new object[1] { message });
				return null;
			}
			return null;
		}
	}

	public TypeDefinition(MainDefinition mainDefinition, CapabilityDefinition capability, typeType type)
	{
		this.mainDefinition = mainDefinition;
		this.capability = capability;
		this.type = type;
		Logger = Telemetry.GetILogger("MainApplication");
	}

	public TypeContent GetActiveTypeContent()
	{
		typedefType getActiveTypeDefinition = GetActiveTypeDefinition;
		if (getActiveTypeDefinition == null)
		{
			return null;
		}
		if (getActiveTypeDefinition.Items != null)
		{
			typedefcontentType[] items = getActiveTypeDefinition.Items;
			foreach (typedefcontentType typedefcontentType2 in items)
			{
				if (SelectResult(mainDefinition, capability, typedefcontentType2.select))
				{
					return new TypeContent(mainDefinition, capability, getActiveTypeDefinition, typedefcontentType2);
				}
			}
		}
		return null;
	}

	public static bool SelectResult(MainDefinition mainDefinition, CapabilityDefinition capability, string select)
	{
		if (select == null || select.Trim().Length == 0)
		{
			return true;
		}
		XPathDocument xPathDocument = null;
		XPathNavigator xPathNavigator = null;
		XPathNavigator xPathNavigator2 = null;
		string text = select;
		while (true)
		{
			Match match = Regex.Match(text, "(\\{(?<innerExpression>[^}]*)})");
			if (!match.Success)
			{
				break;
			}
			string value = match.Groups["innerExpression"].Value;
			Match match2 = Regex.Match(value, "^((xpath:(?<hwidxpath>.+)|(?<hwid>[a-z0-9:\\.]+))\\|)?(?<capability>[a-z0-9]+)=(?<value>.*)$", RegexOptions.IgnoreCase);
			if (match2.Success)
			{
				string value2 = match2.Groups["capability"].Value;
				string value3 = match2.Groups["value"].Value;
				Group obj = match2.Groups["hwidxpath"];
				Group obj2 = match2.Groups["hwid"];
				string hwid;
				if (!obj.Success)
				{
					hwid = ((!obj2.Success) ? capability.Hwid.Text : obj2.Value);
				}
				else
				{
					if (xPathDocument == null)
					{
						xPathDocument = mainDefinition.DefinitionAsXPathDoc;
						xPathNavigator = xPathDocument.CreateNavigator();
					}
					if (xPathNavigator2 == null)
					{
						string xpath = string.Format(CultureInfo.InvariantCulture, "//capabilities[@hwid='{0}']/capability[./name='{1}']", capability.Hwid.Text, capability.Name);
						xPathNavigator2 = xPathNavigator.SelectSingleNode(xpath);
						if (xPathNavigator2 == null)
						{
							throw new InvalidDefinitionException("Unable to find xml node for the capability in the definition xml. Capability: " + capability.FullName);
						}
					}
					hwid = (xPathNavigator2.SelectSingleNode(obj.Value) ?? throw new InvalidDefinitionException(string.Format(CultureInfo.InvariantCulture, "Unable to find xml node containing hwid to use in select expression. Capability: '{0}'; XPath: '{1}'", capability.FullName, obj.Value))).Value;
				}
				string staticSettingValue = DefinitionUtility.GetStaticSettingValue(mainDefinition.Base, hwid, value2);
				string newValue = ((staticSettingValue != null && staticSettingValue == value3) ? "true" : "false");
				text = text.Replace(match.Value, newValue);
				continue;
			}
			if (xPathDocument == null)
			{
				xPathDocument = mainDefinition.DefinitionAsXPathDoc;
				xPathNavigator = xPathDocument.CreateNavigator();
			}
			if (xPathNavigator2 == null)
			{
				string xpath2 = string.Format(CultureInfo.InvariantCulture, "//capabilities[@hwid='{0}']/capability[./name='{1}']", capability.Hwid.Text, capability.Name);
				xPathNavigator2 = xPathNavigator.SelectSingleNode(xpath2);
				if (xPathNavigator2 == null)
				{
					throw new InvalidDefinitionException("Unable to find xml node for the capability in the definition xml. Capability: " + capability.FullName);
				}
			}
			string newValue2 = ((xPathNavigator2.SelectSingleNode(value) != null) ? "true" : "false");
			text = text.Replace(match.Value, newValue2);
		}
		try
		{
			lock (JScriptEvaluator.Instance)
			{
				return (bool)JScriptEvaluator.Instance.Evaluate(text);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return false;
		}
	}
}
