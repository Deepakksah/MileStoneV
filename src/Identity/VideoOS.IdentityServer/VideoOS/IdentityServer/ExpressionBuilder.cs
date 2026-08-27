using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VideoOS.IdentityServer;

public class ExpressionBuilder
{
	private static readonly IReadOnlyDictionary<string, ExpressionType> SupportedComparisonOperators = new Dictionary<string, ExpressionType>
	{
		{
			"eq",
			ExpressionType.Equal
		},
		{
			"ne",
			ExpressionType.NotEqual
		},
		{
			"gt",
			ExpressionType.GreaterThan
		},
		{
			"ge",
			ExpressionType.GreaterThanOrEqual
		},
		{
			"lt",
			ExpressionType.LessThan
		},
		{
			"le",
			ExpressionType.LessThanOrEqual
		}
	}.ToImmutableDictionary();

	public static Expression<Func<TFilterType, bool>> FromString<TFilterType>(string query, IEnumerable<string> allowedSearchTerms = null)
	{
		return (Expression<Func<TFilterType, bool>>)FromString(query, typeof(TFilterType), allowedSearchTerms);
	}

	public static LambdaExpression FromString(string query, Type filterModel, IEnumerable<string> allowedSearchTerms = null)
	{
		if (string.IsNullOrEmpty(query))
		{
			throw new ArgumentException("Value cannot be null or empty.", "query");
		}
		if (filterModel == null)
		{
			throw new ArgumentNullException("filterModel");
		}
		if (!(allowedSearchTerms is HashSet<string>))
		{
			allowedSearchTerms = allowedSearchTerms?.ToHashSet();
		}
		if (allowedSearchTerms != null && !allowedSearchTerms.Any())
		{
			throw new ArgumentException("The list of allowed search names cannot be empty.", "allowedSearchTerms");
		}
		ParameterExpression parameterExpression = Expression.Parameter(filterModel);
		Expression body = ParseQuery(query, parameterExpression, (HashSet<string>)allowedSearchTerms);
		return Expression.Lambda(body, parameterExpression);
	}

	private static Expression ParseQuery(string query, ParameterExpression parameterExpression, HashSet<string> allowedPropNames)
	{
		string[] array = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 3)
		{
			throw new InvalidOperationException("Filter supports the comparison operators " + string.Join(", ", SupportedComparisonOperators.Keys) + " and does not support logical operators.");
		}
		return ParseComparisonExpression(array[0], array[1], array[2], parameterExpression, allowedPropNames);
	}

	private static Expression ParseComparisonExpression(string leftValue, string comparisonOperator, string rightValue, ParameterExpression parameterExpression, HashSet<string> allowedNames)
	{
		if (leftValue == null)
		{
			throw new ArgumentNullException("leftValue");
		}
		if (comparisonOperator == null)
		{
			throw new ArgumentNullException("comparisonOperator");
		}
		if (rightValue == null)
		{
			throw new ArgumentNullException("rightValue");
		}
		if (parameterExpression == null)
		{
			throw new ArgumentNullException("parameterExpression");
		}
		Expression expression = CreateLeftExpression(leftValue, parameterExpression, allowedNames);
		ConstantExpression rightExpression = CreateRightExpression(rightValue, expression.Type);
		return CreateComparisonExpression(comparisonOperator, expression, rightExpression);
	}

	private static Expression CreateComparisonExpression(string comparisonOperator, Expression leftExpression, ConstantExpression rightExpression)
	{
		if (!SupportedComparisonOperators.ContainsKey(comparisonOperator.ToLower()))
		{
			throw new NotSupportedException("The comparison operator '" + comparisonOperator + "' is not supported.");
		}
		ExpressionType binaryType = SupportedComparisonOperators[comparisonOperator.ToLower()];
		return Expression.MakeBinary(binaryType, leftExpression, rightExpression);
	}

	private static ConstantExpression CreateRightExpression(string value, Type propertyType)
	{
		if (!IsValueType(propertyType))
		{
			throw new NotSupportedException("The type of '" + propertyType.Name + "' is not supported for comparisons.");
		}
		object value2 = ConvertValue(value, propertyType);
		return Expression.Constant(value2, propertyType);
	}

	private static bool IsValueType(Type propertyType)
	{
		if (!propertyType.IsValueType && !(propertyType == typeof(string)))
		{
			return propertyType == typeof(DateTime);
		}
		return true;
	}

	private static object ConvertValue(string expressionValue, Type propertyType)
	{
		if (expressionValue.Equals("null", StringComparison.CurrentCultureIgnoreCase))
		{
			return null;
		}
		Type type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
		try
		{
			if (type == typeof(DateTime))
			{
				return Convert.ToDateTime(expressionValue).ToUniversalTime();
			}
			if (type == typeof(DateTimeOffset))
			{
				return new DateTimeOffset(((DateTime)Convert.ChangeType(expressionValue, typeof(DateTime), CultureInfo.InvariantCulture)).ToUniversalTime());
			}
			return Convert.ChangeType(expressionValue, type);
		}
		catch (FormatException innerException)
		{
			throw new InvalidOperationException($"Cannot convert the value '{expressionValue}' to the type {propertyType.Name}.", innerException);
		}
	}

	private static Expression CreateLeftExpression(string leftValue, ParameterExpression parameterExpression, HashSet<string> allowedNames)
	{
		PropertyInfo propertyInfo = FindPropertyInfo(leftValue, parameterExpression.Type, allowedNames);
		if (propertyInfo == null)
		{
			throw new InvalidOperationException($"No property with the name '{leftValue}' exists on the model {parameterExpression.Type.Name}.");
		}
		return Expression.Property(parameterExpression, propertyInfo);
	}

	private static PropertyInfo FindPropertyInfo(string propertyName, Type type, HashSet<string> allowedPropNames)
	{
		IEnumerable<PropertyInfo> runtimeProperties = type.GetRuntimeProperties();
		foreach (PropertyInfo item in runtimeProperties)
		{
			if (item.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
			{
				if (allowedPropNames != null && !allowedPropNames.Contains(item.Name))
				{
					throw new InvalidOperationException(item.Name + " is not allowed in the filter.");
				}
				return item;
			}
		}
		return null;
	}
}
