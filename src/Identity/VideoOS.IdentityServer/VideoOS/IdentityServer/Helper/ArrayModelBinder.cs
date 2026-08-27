using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VideoOS.IdentityServer.Helper;

public class ArrayModelBinder : IModelBinder
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		if (!bindingContext.ModelMetadata.IsEnumerableType)
		{
			bindingContext.Result = ModelBindingResult.Failed();
			return Task.CompletedTask;
		}
		string text = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
		if (string.IsNullOrWhiteSpace(text))
		{
			bindingContext.Result = ModelBindingResult.Success(null);
			return Task.CompletedTask;
		}
		Type type = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
		TypeConverter converter = TypeDescriptor.GetConverter(type);
		object[] array;
		try
		{
			array = (from x in text.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries)
				select converter.ConvertFromString(x.Trim())).ToArray();
		}
		catch (Exception ex) when (ex is FormatException || ex.InnerException is FormatException)
		{
			Exception baseException = ex.GetBaseException();
			bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, baseException.Message ?? "");
			return Task.CompletedTask;
		}
		Array array2 = Array.CreateInstance(type, array.Length);
		array.CopyTo(array2, 0);
		bindingContext.Model = array2;
		bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
		return Task.CompletedTask;
	}
}
