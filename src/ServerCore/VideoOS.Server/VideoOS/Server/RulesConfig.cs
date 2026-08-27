using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Server;

public sealed class RulesConfig : IRulesConfig
{
	public double MaxExecuteRuleActionsCommandDelay { get; private set; }

	public RulesConfig()
	{
		MaxExecuteRuleActionsCommandDelay = 2000.0;
	}

	public void Initialize(XmlElement rulesNode)
	{
		if (rulesNode == null)
		{
			throw new ServerConfigInvalidException("Rule configuration missing.");
		}
		XmlElement xmlElement = rulesNode["MaxRuleActionExecutionCommandDelay"];
		if (xmlElement == null)
		{
			throw new ServerConfigInvalidException("Max rule action execution command delay setting missing.");
		}
		MaxExecuteRuleActionsCommandDelay = Convert.ToDouble(xmlElement.InnerText, CultureInfo.InvariantCulture);
	}
}
