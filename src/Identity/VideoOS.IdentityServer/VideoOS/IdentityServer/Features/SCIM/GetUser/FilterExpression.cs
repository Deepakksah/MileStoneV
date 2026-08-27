using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public static class FilterExpression
{
	private static readonly string[] _supportedLogicalOperators = new string[1] { "and" };

	private static readonly ParameterExpression _userParameter = Expression.Parameter(typeof(VmsUser), "u");

	public static Expression<Func<VmsUser, bool>> GetFilterExpression(string filterParameter, IProviderInfoService providerInfoService)
	{
		Expression expression = null;
		string[] array = ((filterParameter != null) ? filterParameter.Split(_supportedLogicalOperators, StringSplitOptions.None) : Array.Empty<string>());
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Trim().Split(' ');
			string obj = array3[0];
			string obj2 = array3[1];
			string attributeValue = array3[2].Trim('"');
			string operatorSymbol = obj2;
			string attribute = obj;
			string text2 = ScimToVmsUserMapper.MapScimUserAttributeToVmsUserAttribute(attribute);
			Expression expression2 = ((!(text2 == "Logins")) ? GetGeneralExpression(text2, operatorSymbol, attributeValue) : GetExpressionWhenFilterByExternalId(attributeValue, providerInfoService));
			Expression expression3 = expression2;
			expression = ((expression != null) ? Expression.AndAlso(expression, expression3) : expression3);
		}
		if (expression == null)
		{
			return (VmsUser x) => true;
		}
		return Expression.Lambda<Func<VmsUser, bool>>(expression, new ParameterExpression[1] { _userParameter });
	}

	private static Expression GetGeneralExpression(string vmsAttributeName, string operatorSymbol, string attributeValue)
	{
		MemberExpression left = Expression.Property(_userParameter, vmsAttributeName);
		DateTime result;
		bool result2;
		ConstantExpression right = (DateTime.TryParse(attributeValue, out result) ? Expression.Constant(result.ToUniversalTime()) : ((!bool.TryParse(attributeValue, out result2)) ? Expression.Constant(attributeValue) : Expression.Constant(result2)));
		if (operatorSymbol == "eq")
		{
			return Expression.Equal(left, right);
		}
		return Expression.Empty();
	}

	private static Expression GetExpressionWhenFilterByExternalId(string attributeValue, IProviderInfoService providerInfoService)
	{
		(string, string) providerInfo = providerInfoService.GetProviderInfo();
		MemberExpression memberExpression = Expression.Property(_userParameter, "Logins");
		ParameterExpression parameterExpression = Expression.Parameter(typeof(IdentityUserLogin<string>), "x");
		BinaryExpression left = Expression.Equal(Expression.Property(parameterExpression, "LoginProvider"), Expression.Constant(providerInfo.Item1));
		BinaryExpression right = Expression.Equal(Expression.Property(parameterExpression, "ProviderKey"), Expression.Constant(attributeValue));
		BinaryExpression body = Expression.AndAlso(left, right);
		return Expression.Call(typeof(Enumerable), "Any", new Type[1] { typeof(IdentityUserLogin<string>) }, memberExpression, Expression.Lambda<Func<IdentityUserLogin<string>, bool>>(body, new ParameterExpression[1] { parameterExpression }));
	}
}
