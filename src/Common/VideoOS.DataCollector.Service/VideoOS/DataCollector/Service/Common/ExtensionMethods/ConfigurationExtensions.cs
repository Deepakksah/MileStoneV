using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace VideoOS.DataCollector.Service.Common.ExtensionMethods;

public static class ConfigurationExtensions
{
	public static T GetValid<T>(this IConfiguration configuration)
	{
		T val = configuration.Get<T>();
		if (val == null)
		{
			throw new InvalidOperationException($"Configuration with type '{typeof(T)}' is null");
		}
		Validator.ValidateObject(val, new ValidationContext(val), validateAllProperties: true);
		return val;
	}
}
