using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VideoOS.Rules.Interfaces;

public abstract class RuleActionFactoryBase
{
	protected IRuleActionDataProvider ruleActionDataProvider { get; }

	public RuleActionFactoryBase(IRuleActionDataProvider ruleActionDataProvider)
	{
		this.ruleActionDataProvider = ruleActionDataProvider;
	}

	public abstract IRuleAction CreateRuleAction(ISubRule subRule, XmlNode actionNode);

	public static XmlNode GetActionParameterByName(XmlNode actionNode, string parameterName)
	{
		if (actionNode == null)
		{
			throw new ArgumentNullException("actionNode");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		return actionNode.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "{0}[@{1}='{2}']", "Parameter", "name", parameterName));
	}

	public static Guid[] BuildGuidArray(XmlNode idsNode)
	{
		if (idsNode == null || idsNode.InnerText.Length == 0)
		{
			return Array.Empty<Guid>();
		}
		List<Guid> list = new List<Guid>();
		string[] array = idsNode.InnerText.Split(';', ',');
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new Guid(array[i].Trim()));
		}
		return list.ToArray();
	}

	public static TimeSpan ParseDelay(XmlNode actionNode)
	{
		TimeSpan result = TimeSpan.Zero;
		XmlNode actionParameterByName = GetActionParameterByName(actionNode, "Delay");
		if (actionParameterByName != null && actionParameterByName.InnerText.Length > 0)
		{
			int num = 0;
			try
			{
				num = Convert.ToInt32(actionParameterByName.InnerText, CultureInfo.InvariantCulture);
			}
			catch (Exception inner)
			{
				throw new RuleConfigurationException("The value of the 'Delay' parameter for the action is not a valid number", inner);
			}
			result = TimeSpan.FromSeconds(num);
		}
		return result;
	}

	public static int ParsePriority(XmlNode actionNode)
	{
		int result = 1;
		XmlNode actionParameterByName = GetActionParameterByName(actionNode, "Priority");
		if (actionParameterByName != null && actionParameterByName.InnerText.Length > 0)
		{
			try
			{
				result = Convert.ToInt32(actionParameterByName.InnerText, CultureInfo.InvariantCulture);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new RuleConfigurationException("The value of the 'Priority' parameter for the action is not a valid number", inner);
			}
		}
		return result;
	}

	public static Guid ParseInstanceId(XmlNode actionNode)
	{
		if (actionNode == null)
		{
			throw new ArgumentNullException("actionNode");
		}
		try
		{
			return new Guid(actionNode.Attributes["instanceId"].Value);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "The value of the '{0}' parameter for the action is not a valid guid", "instanceId"), inner);
		}
	}
}
