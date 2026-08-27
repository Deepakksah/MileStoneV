using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public sealed class RuleVariableManager : IRuleVariableManager
{
	private class UserVariable
	{
		public Guid ID { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public UserVariable(Guid id, string name, string value)
		{
			ID = id;
			Name = name;
			Value = value;
		}

		public UserVariable(UserVariable userVarDefined)
			: this(userVarDefined.ID, userVarDefined.Name, userVarDefined.Value)
		{
		}
	}

	private Dictionary<Guid, UserVariable> userVariablesDefined;

	private Dictionary<Guid, Dictionary<Guid, UserVariable>> userVariablesValues;

	private object lockObj;

	public RuleVariableManager()
	{
		lockObj = new object();
		userVariablesDefined = new Dictionary<Guid, UserVariable>();
		userVariablesValues = new Dictionary<Guid, Dictionary<Guid, UserVariable>>();
	}

	public void SetConfiguration(IRuleConfiguration ruleConfiguration)
	{
		if (ruleConfiguration == null)
		{
			throw new ArgumentNullException("ruleConfiguration");
		}
		lock (lockObj)
		{
			foreach (var ruleVariablesChange in ruleConfiguration.RuleVariablesChanges)
			{
				UserVariable userVariable = new UserVariable(ruleVariablesChange.VariableId, ruleVariablesChange.name, ruleVariablesChange.value);
				userVariablesDefined.Add(userVariable.ID, userVariable);
			}
		}
	}

	public bool UpdateConfiguration(IRuleConfiguration rulesInfo)
	{
		if (rulesInfo == null)
		{
			throw new ArgumentNullException("rulesInfo");
		}
		bool result = false;
		lock (lockObj)
		{
			if (rulesInfo.RuleVariablesChanges != null)
			{
				result = true;
				foreach (var ruleVariablesChange in rulesInfo.RuleVariablesChanges)
				{
					switch (ruleVariablesChange.modification)
					{
					case RuleModification.Add:
					{
						UserVariable userVariable2 = new UserVariable(ruleVariablesChange.VariableId, ruleVariablesChange.name, ruleVariablesChange.value);
						userVariablesDefined.Add(userVariable2.ID, userVariable2);
						break;
					}
					case RuleModification.Update:
					{
						Guid item = ruleVariablesChange.VariableId;
						UserVariable userVariable = userVariablesDefined[item];
						userVariable.Name = ruleVariablesChange.name;
						userVariable.Value = ruleVariablesChange.value;
						if (ruleVariablesChange.reset)
						{
							RemoveAnyInstanceVariables(item);
						}
						break;
					}
					case RuleModification.Remove:
						RemoveAnyInstanceVariables(ruleVariablesChange.VariableId);
						userVariablesDefined.Remove(ruleVariablesChange.VariableId);
						break;
					}
				}
			}
		}
		return result;
	}

	private void RemoveAnyInstanceVariables(Guid variableId)
	{
		Guid[] variableValueIds = userVariablesValues.Keys.ToArray();
		ResetVariables(variableValueIds, variableId);
	}

	public void SetVariables(Guid[] variableValueIds, Guid variableId, string variableValue)
	{
		if (variableValueIds == null)
		{
			throw new ArgumentNullException("variableValueIds");
		}
		lock (lockObj)
		{
			if (!userVariablesDefined.TryGetValue(variableId, out var value))
			{
				return;
			}
			foreach (Guid key in variableValueIds)
			{
				if (!userVariablesValues.TryGetValue(key, out var value2))
				{
					value2 = new Dictionary<Guid, UserVariable>();
					userVariablesValues.Add(key, value2);
				}
				if (!value2.ContainsKey(variableId))
				{
					value.Value = variableValue;
					value2.Add(value.ID, value);
				}
			}
		}
	}

	public void ResetVariables(Guid[] variableValueIds, Guid variableId)
	{
		if (variableValueIds == null)
		{
			throw new ArgumentNullException("variableValueIds");
		}
		lock (lockObj)
		{
			if (!userVariablesDefined.ContainsKey(variableId))
			{
				return;
			}
			foreach (Guid key in variableValueIds)
			{
				if (userVariablesValues.TryGetValue(key, out var value))
				{
					value.Remove(variableId);
					if (value.Count == 0)
					{
						userVariablesValues.Remove(key);
					}
				}
			}
		}
	}

	public string GetVariableValue(Guid variableValueId, Guid variableId)
	{
		lock (lockObj)
		{
			string text = string.Empty;
			if (userVariablesDefined.TryGetValue(variableId, out var value))
			{
				if (userVariablesValues.TryGetValue(variableValueId, out var value2) && value2.TryGetValue(variableId, out var value3))
				{
					text = value3.Value;
				}
				return text ?? value.Value;
			}
		}
		return null;
	}
}
