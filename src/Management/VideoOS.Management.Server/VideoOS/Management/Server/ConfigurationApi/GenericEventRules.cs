using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GenericEventRules
{
	public interface ILogicalExpression
	{
		string GetExpression(bool match);
	}

	public class CombinedExpression : ILogicalExpression
	{
		public enum LogicalOperator
		{
			AND,
			OR
		}

		public ILogicalExpression Part1 { get; set; }

		public LogicalOperator Operation { get; set; }

		public ILogicalExpression Part2 { get; set; }

		protected void AddParts(List<string> parts, bool match)
		{
			if (parts == null)
			{
				throw new ArgumentNullException("parts");
			}
			if (Operation == LogicalOperator.AND)
			{
				if (Part1 is CombinedExpression)
				{
					(Part1 as CombinedExpression).AddParts(parts, match);
				}
				else
				{
					parts.Add(Part1.GetExpression(match));
				}
				if (Part2 is CombinedExpression)
				{
					(Part2 as CombinedExpression).AddParts(parts, match);
				}
				else
				{
					parts.Add(Part2.GetExpression(match));
				}
			}
			else
			{
				parts.Add("(" + GetExpression(match) + ")");
			}
		}

		public static void Permutation(ref string[] elements, int factoradic)
		{
			if (elements == null)
			{
				throw new ArgumentNullException("elements");
			}
			for (int i = 2; i <= elements.Length; i++)
			{
				int num = factoradic % i;
				string text = elements[i - 1];
				elements[i - 1] = elements[num];
				elements[num] = text;
				factoradic /= i;
			}
		}

		public string GetExpression(bool match)
		{
			_ = string.Empty;
			if (Operation == LogicalOperator.AND)
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<string> list = new List<string>();
				AddParts(list, match);
				int num = 1;
				for (int i = 2; i <= list.Count; i++)
				{
					num *= i;
				}
				stringBuilder.Append("(");
				for (int j = 0; j < num; j++)
				{
					string[] elements = list.ToArray();
					Permutation(ref elements, j);
					for (int k = 0; k < elements.Length; k++)
					{
						stringBuilder.Append(elements[k]);
						if (!match && k != elements.Length - 1)
						{
							stringBuilder.Append(".*");
						}
					}
					if (j != num - 1)
					{
						stringBuilder.Append("|");
					}
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}
			return Part1.GetExpression(match) + "|" + Part2.GetExpression(match);
		}
	}

	public class StringExpression : ILogicalExpression
	{
		private string _Expression;

		public string Expression
		{
			get
			{
				return _Expression;
			}
			set
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (value != null)
				{
					foreach (char c in value)
					{
						switch (c)
						{
						case '\\':
							stringBuilder.Append("\\\\");
							break;
						case '(':
							stringBuilder.Append("\\(");
							break;
						case ')':
							stringBuilder.Append("\\)");
							break;
						case '[':
							stringBuilder.Append("\\[");
							break;
						case ']':
							stringBuilder.Append("\\]");
							break;
						case '{':
							stringBuilder.Append("\\{");
							break;
						case '}':
							stringBuilder.Append("\\}");
							break;
						case '<':
							stringBuilder.Append("\\<");
							break;
						case '>':
							stringBuilder.Append("\\>");
							break;
						case '*':
							stringBuilder.Append("\\*");
							break;
						case '.':
							stringBuilder.Append("\\.");
							break;
						case '+':
							stringBuilder.Append("\\+");
							break;
						case '$':
							stringBuilder.Append("\\$");
							break;
						case '^':
							stringBuilder.Append("\\^");
							break;
						case ':':
							stringBuilder.Append("\\:");
							break;
						case '|':
							stringBuilder.Append("\\|");
							break;
						case '?':
							stringBuilder.Append("\\?");
							break;
						default:
							stringBuilder.Append(c);
							break;
						}
					}
				}
				_Expression = stringBuilder.ToString();
			}
		}

		public string GetExpression(bool match)
		{
			return Expression;
		}
	}

	public static string FinishMatchExpression(string expression)
	{
		if (string.IsNullOrEmpty(expression))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('^');
		stringBuilder.Append('(');
		stringBuilder.Append(expression);
		stringBuilder.Append(')');
		stringBuilder.Append('$');
		return stringBuilder.ToString();
	}

	public static ILogicalExpression BuildGenericEventRuleTree(string rule)
	{
		ValidateGenericEventRule(rule);
		Stack<ILogicalExpression> stack = new Stack<ILogicalExpression>();
		Stack<CombinedExpression.LogicalOperator> stack2 = new Stack<CombinedExpression.LogicalOperator>();
		CombinedExpression.LogicalOperator? logicalOperator = null;
		ILogicalExpression logicalExpression = null;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		bool flag2 = false;
		int num = -1;
		foreach (char c in rule)
		{
			num++;
			if (flag2)
			{
				flag2 = false;
				stringBuilder.Append(c);
				continue;
			}
			switch (c)
			{
			case '\\':
				flag2 = true;
				continue;
			case '"':
				if (flag)
				{
					if (logicalExpression == null)
					{
						logicalExpression = new StringExpression
						{
							Expression = stringBuilder.ToString()
						};
					}
					else
					{
						if (!logicalOperator.HasValue)
						{
							throw new ValueInvalidFormatException("FormatError5", num);
						}
						logicalExpression = new CombinedExpression
						{
							Part1 = logicalExpression,
							Part2 = new StringExpression
							{
								Expression = stringBuilder.ToString()
							},
							Operation = logicalOperator.Value
						};
					}
					stringBuilder.Length = 0;
				}
				flag = !flag;
				continue;
			case '(':
				if (!flag)
				{
					stack.Push(logicalExpression);
					if (logicalExpression != null)
					{
						stack2.Push(logicalOperator.Value);
					}
					logicalExpression = null;
					continue;
				}
				break;
			case ')':
				if (!flag)
				{
					ILogicalExpression logicalExpression2 = stack.Pop();
					if (logicalExpression2 != null)
					{
						logicalExpression = new CombinedExpression
						{
							Part1 = logicalExpression2,
							Part2 = logicalExpression,
							Operation = stack2.Pop()
						};
					}
					continue;
				}
				break;
			case ' ':
				if (!flag)
				{
					string text = stringBuilder.ToString();
					if (text.ToUpper(CultureInfo.InvariantCulture) == "AND")
					{
						logicalOperator = CombinedExpression.LogicalOperator.AND;
					}
					if (text.ToUpper(CultureInfo.InvariantCulture) == "OR")
					{
						logicalOperator = CombinedExpression.LogicalOperator.OR;
					}
					stringBuilder.Length = 0;
					continue;
				}
				break;
			}
			stringBuilder.Append(c);
		}
		return logicalExpression;
	}

	public static void ValidateGenericEventRule(string rule)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		int num2 = -1;
		foreach (char c in rule)
		{
			num2++;
			if (flag5)
			{
				flag5 = false;
				continue;
			}
			switch (c)
			{
			case '\\':
				if (!flag)
				{
					throw new ValueInvalidFormatException("FormatError1", num2);
				}
				flag5 = true;
				continue;
			case '"':
				flag4 = false;
				if (!flag)
				{
					if (flag2)
					{
						throw new ValueInvalidFormatException("FormatError2", num2);
					}
					flag3 = false;
				}
				else
				{
					flag2 = true;
				}
				flag = !flag;
				continue;
			case '(':
				if (!flag)
				{
					flag4 = false;
					if (flag2)
					{
						throw new ValueInvalidFormatException("FormatError3", num2);
					}
					num++;
					flag3 = false;
				}
				continue;
			case ')':
				if (!flag)
				{
					flag4 = false;
					if (!flag2 || num == 0)
					{
						throw new ValueInvalidFormatException("FormatError4", num2);
					}
					num--;
				}
				continue;
			case ' ':
				if (!flag)
				{
					flag4 = false;
				}
				continue;
			}
			if (!flag)
			{
				if (!flag2 && !flag4)
				{
					throw new ValueInvalidFormatException("FormatError5", num2);
				}
				flag2 = false;
				flag3 = true;
				flag4 = true;
			}
		}
		if (num != 0)
		{
			throw new ValueInvalidFormatException("FormatError6", num2);
		}
		if (flag)
		{
			throw new ValueInvalidFormatException("FormatError7", num2);
		}
		if (flag3)
		{
			throw new ValueInvalidFormatException("FormatError8", num2);
		}
		if (!flag2)
		{
			throw new ValueInvalidFormatException("FormatError9", num2);
		}
	}
}
