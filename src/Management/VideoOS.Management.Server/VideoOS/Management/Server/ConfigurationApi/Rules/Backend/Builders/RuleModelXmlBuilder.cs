using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class RuleModelXmlBuilder : IRuleModelXmlBuilder
{
	private readonly IRuleTypeXmlBuilder _typeBuilder;

	private readonly IConditionXmlBuilder _conditionBuilder;

	private readonly IActionXmlBuilder _actionBuilder;

	public RuleModelXmlBuilder(IRuleTypeXmlBuilder ruleTypeXmlBuilder, IConditionXmlBuilder conditionXmlBuilder, IActionXmlBuilder actionXmlBuilder)
	{
		_actionBuilder = actionXmlBuilder;
		_typeBuilder = ruleTypeXmlBuilder;
		_conditionBuilder = conditionXmlBuilder;
	}

	public string GetXml(RuleModel rule)
	{
		rule.NullCheck("", "GetXml", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Builders\\RuleModelXmlBuilder.cs", 27);
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("Rule");
			xmlTextWriter.WriteStartElement("StartRule");
			xmlTextWriter.WriteRaw(_typeBuilder.GetXml(rule.RuleStart));
			xmlTextWriter.WriteRaw(_conditionBuilder.GetXml(rule.RuleStart.Conditions));
			WriteActions(xmlTextWriter, rule.RuleStart.Actions);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("StopRule");
			if (rule.RuleStop.TriggerType != TriggerType.None)
			{
				xmlTextWriter.WriteRaw(_typeBuilder.GetXml(rule.RuleStop));
				WriteActions(xmlTextWriter, rule.RuleStop.Actions);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
			stringWriter = null;
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	private void WriteActions(XmlTextWriter wr, IEnumerable<ActionModel> actions)
	{
		wr.WriteStartElement("Actions");
		foreach (ActionModel action in actions)
		{
			wr.WriteRaw(_actionBuilder.GetXml(action));
		}
		wr.WriteEndElement();
	}
}
