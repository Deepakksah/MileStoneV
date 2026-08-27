using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VideoOS.IdentityServer.Helper;

public class FilterModelBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		if (bindingContext == null)
		{
			throw new ArgumentNullException("bindingContext");
		}
		if (!bindingContext.ModelType.IsSubclassOf(typeof(LambdaExpression)) || bindingContext.ModelType.GenericTypeArguments.Length == 0 || bindingContext.ModelType.GenericTypeArguments[0].Name != "Func`2" || bindingContext.ModelType.GenericTypeArguments[0].GenericTypeArguments[1] != typeof(bool))
		{
			throw new InvalidOperationException($"{bindingContext.ModelName} must be of the type {typeof(Expression<Func<object, bool>>)}");
		}
		ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
		if (value == ValueProviderResult.None)
		{
			return Task.CompletedTask;
		}
		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);
		string firstValue = value.FirstValue;
		if (string.IsNullOrEmpty(firstValue))
		{
			return Task.CompletedTask;
		}
		Type type = bindingContext.ModelType.GenericTypeArguments[0].GenericTypeArguments[0];
		IEnumerable<string> allowedSearchTerms = null;
		if (FilterParameters.WhitelistedSearchTermsByType.ContainsKey(type))
		{
			allowedSearchTerms = FilterParameters.WhitelistedSearchTermsByType[type];
		}
		try
		{
			bindingContext.Result = ModelBindingResult.Success(ExpressionBuilder.FromString(firstValue, type, allowedSearchTerms));
			return Task.CompletedTask;
		}
		catch (NotSupportedException ex)
		{
			bindingContext.ModelState.AddModelError(bindingContext.ModelMetadata.Name, ex.Message);
		}
		catch (InvalidOperationException ex2)
		{
			bindingContext.ModelState.AddModelError(bindingContext.ModelMetadata.Name, ex2.Message);
		}
		bindingContext.Result = ModelBindingResult.Failed();
		return Task.CompletedTask;
	}
}
