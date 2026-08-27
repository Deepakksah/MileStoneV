using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Reflection;

namespace VideoOS.Rules;

public class RuleActionFactoryManager
{
	private object lockObject = new object();

	private Dictionary<string, RuleActionFactoryBase> _factories = new Dictionary<string, RuleActionFactoryBase>();

	private Dictionary<string, RuleActionFactoryInfo> _factoryInfos = new Dictionary<string, RuleActionFactoryInfo>();

	private string _ruleactionsPath = "./RuleActions";

	public Dictionary<string, RuleActionFactoryInfo> FactoryInfos => _factoryInfos;

	public string RuleActionsPath
	{
		get
		{
			return _ruleactionsPath;
		}
		set
		{
			_ruleactionsPath = value;
		}
	}

	public RuleActionFactoryBase GetFactory(string actionName, IRuleActionDataProvider ruleActionData)
	{
		lock (lockObject)
		{
			if (_factories.TryGetValue(actionName, out var value))
			{
				return value;
			}
			if (_factoryInfos.TryGetValue(actionName, out var value2))
			{
				if (value2 == null)
				{
					return null;
				}
				RuleActionFactoryBase ruleActionFactoryBase = Activator.CreateInstanceFrom(value2.AssemblyFile, value2.ClassName, ignoreCase: false, BindingFlags.Default, null, new object[1] { ruleActionData }, null, null).Unwrap() as RuleActionFactoryBase;
				if (ruleActionFactoryBase != null)
				{
					_factories.Add(value2.Attribute.RuleActionName, ruleActionFactoryBase);
				}
				return ruleActionFactoryBase;
			}
		}
		return null;
	}

	public RuleActionFactoryInfo GetFactoryInfo(string actionName)
	{
		lock (lockObject)
		{
			return _factoryInfos[actionName];
		}
	}

	public void Refresh()
	{
		lock (lockObject)
		{
			_factoryInfos.Clear();
			DllScanner.Scan(_ruleactionsPath, ProcessRuleActionFactoryClass);
		}
	}

	private void ProcessRuleActionFactoryClass(Type type)
	{
		if (type.IsDefined(typeof(RuleActionFactoryAttribute), inherit: false))
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(RuleActionFactoryAttribute), inherit: false);
			if (customAttributes.Length != 0 && customAttributes[0] is RuleActionFactoryAttribute ruleActionFactoryAttribute)
			{
				_factoryInfos.Add(ruleActionFactoryAttribute.RuleActionName, new RuleActionFactoryInfo(type.Assembly.Location, type.FullName, ruleActionFactoryAttribute));
			}
		}
	}
}
