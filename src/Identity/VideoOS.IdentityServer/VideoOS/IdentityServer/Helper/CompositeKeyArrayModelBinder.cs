using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VideoOS.IdentityServer.Helper;

public class CompositeKeyArrayModelBinder<T> : IModelBinder where T : new()
{
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		if (!bindingContext.ModelMetadata.IsEnumerableType)
		{
			bindingContext.Result = ModelBindingResult.Failed();
			return Task.CompletedTask;
		}
		Type typeFromHandle = typeof(T);
		List<PropertyInfo> list = (from p in typeFromHandle.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			where p.CanWrite
			select p).ToList();
		string text = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
		if (string.IsNullOrWhiteSpace(text))
		{
			bindingContext.Result = ModelBindingResult.Success(null);
			return Task.CompletedTask;
		}
		List<T> list2 = new List<T>();
		string[] array = text.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			T val = new T();
			string[] source = text2.Split(new string[1] { "+" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (PropertyInfo item in list)
			{
				Type propertyType = item.PropertyType;
				TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
				string key = item.Name + "=";
				item.SetValue(val, converter.ConvertFromString(source.FirstOrDefault((string s) => s.StartsWith(key, StringComparison.OrdinalIgnoreCase))?.Substring(key.Length)));
			}
			HashSet<ValidationResult> hashSet = new HashSet<ValidationResult>();
			if (!Validator.TryValidateObject((object)val, new ValidationContext((object)val, (IServiceProvider)null, (IDictionary<object, object>)null), (ICollection<ValidationResult>)hashSet, true))
			{
				foreach (ValidationResult item2 in hashSet)
				{
					bindingContext.ModelState.TryAddModelError("", item2.ErrorMessage);
				}
				return Task.CompletedTask;
			}
			list2.Add(val);
		}
		bindingContext.Model = list2;
		bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
		return Task.CompletedTask;
	}
}
