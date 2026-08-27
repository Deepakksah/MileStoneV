using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Rules.ConditionToken;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class RuleCondition : IRuleCondition
{
	public class Condition
	{
		private ArrayList _postFixArray;

		public bool IsTrue(TriggerEvent triggerEvent)
		{
			try
			{
				if (_postFixArray.Count == 0)
				{
					return true;
				}
				Stack stack = new Stack(_postFixArray.Count);
				foreach (IEvaluate item in _postFixArray)
				{
					item.Evaluate(triggerEvent, stack);
				}
				return (((IToken)stack.Pop()) as BooleanValueToken).Value;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new RuleException("Rule condition could not be evaluated correctly because of an error in the condition expression", inner);
			}
		}

		private static IToken CreateValueTokenObject(XmlNode tokenNode, IRuleEngine ruleEngine)
		{
			if (tokenNode.Attributes["datatype"] == null)
			{
				throw new ArgumentException("No datatype attribute specified for a value token in the rule condition");
			}
			switch (tokenNode.Attributes["datatype"].Value)
			{
			case "Boolean":
				return new BooleanValueToken
				{
					Value = Convert.ToBoolean(tokenNode.InnerText, CultureInfo.InvariantCulture)
				};
			case "String":
				return new StringValueToken
				{
					Value = tokenNode.InnerText
				};
			case "Integer":
				return new IntegerValueToken
				{
					Value = Convert.ToInt32(tokenNode.InnerText, CultureInfo.InvariantCulture)
				};
			case "Double":
				return new DoubleValueToken
				{
					Value = Convert.ToDouble(tokenNode.InnerText, CultureInfo.InvariantCulture)
				};
			case "Null":
				return new NullValueToken();
			case "StringArray":
			{
				StringArrayToken stringArrayToken = new StringArrayToken();
				XmlNodeList xmlNodeList = tokenNode.SelectNodes("Value");
				stringArrayToken.Value = new string[xmlNodeList.Count];
				int num = 0;
				{
					foreach (XmlNode item in xmlNodeList)
					{
						XmlAttribute xmlAttribute2 = item.Attributes["datatype"];
						if (xmlAttribute2 == null || xmlAttribute2.Value != "String")
						{
							throw new ArgumentException("An element in a string array was not defined as a string");
						}
						stringArrayToken.Value[num++] = item.InnerText;
					}
					return stringArrayToken;
				}
			}
			case "IntegerArray":
			{
				IntegerArrayToken integerArrayToken = new IntegerArrayToken();
				XmlNodeList xmlNodeList = tokenNode.SelectNodes("Value");
				integerArrayToken.Value = new int[xmlNodeList.Count];
				int num = 0;
				{
					foreach (XmlNode item2 in xmlNodeList)
					{
						XmlAttribute xmlAttribute = item2.Attributes["datatype"];
						if (xmlAttribute == null || xmlAttribute.Value != "Integer")
						{
							throw new ArgumentException("An element in an integer array was not defined as an integer");
						}
						integerArrayToken.Value[num++] = Convert.ToInt32(item2.InnerText, CultureInfo.InvariantCulture);
					}
					return integerArrayToken;
				}
			}
			case "Object":
			{
				ObjectValueToken objectValueToken = new ObjectValueToken();
				if (tokenNode.InnerText == "RuleEngine")
				{
					objectValueToken.Value = ruleEngine;
					return objectValueToken;
				}
				throw new NotImplementedException("Unexpected object type specified for an object value token in a rule condition");
			}
			default:
				throw new NotImplementedException("Unexpected datatype specified for a value token in a rule condition");
			}
		}

		private static IToken CreateOperatorTokenObject(XmlNode tokenNode)
		{
			return tokenNode.InnerText switch
			{
				"And" => AndOperatorToken.Instance, 
				"Or" => OrOperatorToken.Instance, 
				"Equals" => EqualsOperatorToken.Instance, 
				_ => throw new NotImplementedException("Unexpected value specified for an operator token in a rule condition"), 
			};
		}

		private static IToken CreateParenthesisTokenObject(XmlNode tokenNode)
		{
			string innerText = tokenNode.InnerText;
			if (!(innerText == "("))
			{
				if (innerText == ")")
				{
					return RightParenthesisToken.Instance;
				}
				throw new NotImplementedException("Unexpected value specified for a parenthesis token in a rule condition");
			}
			return LeftParenthesisToken.Instance;
		}

		private static IToken CreateUnaryOperatorTokenObject(XmlNode tokenNode)
		{
			if (tokenNode.InnerText == "Not")
			{
				return NotUnaryOperatorToken.Instance;
			}
			throw new NotImplementedException("Unexpected value specified for a unary operator token in a rule condition");
		}

		private static IToken CreateFunctionTokenObject(XmlNode tokenNode)
		{
			return tokenNode.Attributes["name"].Value switch
			{
				"VideoOS.TimeOfDayBetween" => TimeOfDayBetweenFunctionToken.Instance, 
				"VideoOS.DayOfWeekBetween" => DayOfWeekBetweenFunctionToken.Instance, 
				"VideoOS.WeekdayIsOneOf" => WeekdayIsOneOfFunctionToken.Instance, 
				"VideoOS.WithinTimeProfile" => WithinTimeProfileFunctionToken.Instance, 
				"VideoOS.NotWithinTimeProfile" => NotWithinTimeProfileFunctionToken.Instance, 
				"VideoOS.IsFailover" => IsFailoverFunctionToken.Instance, 
				"VideoOS.IsNotFailover" => IsNotFailoverFunctionToken.Instance, 
				"VideoOS.GetMetaData" => GetMetaDataFunctionToken.Instance, 
				"VideoOS.GetVariable" => GetVariableFunctionToken.Instance, 
				_ => throw new NotImplementedException("Unexpected value specified for a function token in a rule condition"), 
			};
		}

		private static IToken CreateTokenObject(XmlNode tokenNode, IRuleEngine ruleEngine)
		{
			if (tokenNode == null)
			{
				throw new ArgumentNullException("tokenNode");
			}
			if (tokenNode.Attributes["type"] == null)
			{
				throw new ArgumentException("No type attribute specified for a token in the rule condition");
			}
			return tokenNode.Attributes["type"].Value switch
			{
				"Value" => CreateValueTokenObject(tokenNode, ruleEngine), 
				"Operator" => CreateOperatorTokenObject(tokenNode), 
				"UnaryOperator" => CreateUnaryOperatorTokenObject(tokenNode), 
				"Parenthesis" => CreateParenthesisTokenObject(tokenNode), 
				"Function" => CreateFunctionTokenObject(tokenNode), 
				_ => throw new NotImplementedException("Unexpected type specified for a token in a rule condition"), 
			};
		}

		public void Initialize(XmlNode conditionNode, IRuleEngine ruleEngine)
		{
			if (conditionNode == null || conditionNode.ChildNodes.Count == 0)
			{
				_postFixArray = new ArrayList();
				return;
			}
			XmlNodeList xmlNodeList = conditionNode.SelectNodes(".//Token");
			_postFixArray = new ArrayList(xmlNodeList.Count);
			Stack stack = new Stack(xmlNodeList.Count);
			foreach (XmlNode item in xmlNodeList)
			{
				if (item.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				IToken token = CreateTokenObject(item, ruleEngine);
				switch (token.Type)
				{
				case TokenType.Value:
					_postFixArray.Add(token);
					continue;
				case TokenType.Parenthesis:
					if (token is LeftParenthesisToken)
					{
						stack.Push(token);
					}
					else if (stack.Count > 0)
					{
						IToken token2 = (IToken)stack.Pop();
						while (!(token2 is LeftParenthesisToken))
						{
							_postFixArray.Add(token2);
							token2 = (IToken)stack.Pop();
						}
					}
					continue;
				case TokenType.UnaryOperator:
					stack.Push(token);
					continue;
				}
				if (stack.Count > 0)
				{
					IToken token3 = (IToken)stack.Pop();
					while (stack.Count > 0 && (token3.Type == TokenType.UnaryOperator || token3.Type == TokenType.Operator || token3.Type == TokenType.Function) && token3.Precedence >= token.Precedence)
					{
						_postFixArray.Add(token3);
						token3 = (IToken)stack.Pop();
					}
					if ((token3.Type == TokenType.UnaryOperator || token3.Type == TokenType.Operator || token3.Type == TokenType.Function) && token3.Precedence >= token.Precedence)
					{
						_postFixArray.Add(token3);
					}
					else
					{
						stack.Push(token3);
					}
				}
				stack.Push(token);
			}
			while (stack.Count > 0)
			{
				_postFixArray.Add(stack.Pop());
			}
		}
	}

	public class TypeCondition
	{
		private List<IToken> _typeConditionsList;

		private readonly ITimeProvider _timeProvider;

		public TypeCondition(ITimeProvider timeProvider)
		{
			_timeProvider = timeProvider;
		}

		public bool IsTrue(TriggerEvent triggerEvent)
		{
			try
			{
				if (_typeConditionsList == null || _typeConditionsList.Count == 0)
				{
					return true;
				}
				Stack stack = new Stack(_typeConditionsList.Count);
				foreach (IEvaluate typeConditions in _typeConditionsList)
				{
					typeConditions.Evaluate(triggerEvent, stack);
				}
				return (((IToken)stack.Pop()) as BooleanValueToken).Value;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new RuleException("Rule condition could not be evaluated correctly because of an error in the condition expression", inner);
			}
		}

		public void InitializeRuleTypeCondition(IXPathNavigable ruleTypeNode)
		{
			if (!(ruleTypeNode is XmlNode xmlNode) || xmlNode.ChildNodes.Count == 0)
			{
				_typeConditionsList = new List<IToken>();
				return;
			}
			_typeConditionsList = new List<IToken>();
			foreach (XmlNode item2 in xmlNode)
			{
				if (item2.Name == "Schedule")
				{
					IToken item = new ScheduledTimeReachedFunction(item2.OuterXml, DateTime.MinValue, _timeProvider);
					_typeConditionsList.Add(item);
				}
			}
		}
	}

	private readonly Condition timeCondition = new Condition();

	private readonly Condition condition = new Condition();

	private readonly TypeCondition recurrenceTimeCondition;

	private readonly ITimeProvider _timeProvider;

	public RuleCondition(ITimeProvider timeProvider)
	{
		_timeProvider = timeProvider;
		recurrenceTimeCondition = new TypeCondition(_timeProvider);
	}

	public bool IsTrue(TriggerEvent triggerEvent)
	{
		if (timeCondition.IsTrue(triggerEvent) && condition.IsTrue(triggerEvent))
		{
			return recurrenceTimeCondition.IsTrue(triggerEvent);
		}
		return false;
	}

	public void Initialize(IXPathNavigable conditionsNode, IXPathNavigable ruleTypeNode, IRuleEngine ruleEngine)
	{
		if (conditionsNode == null)
		{
			timeCondition.Initialize(null, null);
			condition.Initialize(null, null);
			if (ruleTypeNode == null)
			{
				recurrenceTimeCondition.InitializeRuleTypeCondition(null);
			}
		}
		else
		{
			XmlNode xmlNode = (XmlNode)conditionsNode;
			timeCondition.Initialize(xmlNode.SelectSingleNode("TimeCondition"), ruleEngine);
			condition.Initialize(xmlNode.SelectSingleNode("Condition"), ruleEngine);
			recurrenceTimeCondition.InitializeRuleTypeCondition(ruleTypeNode);
		}
	}
}
