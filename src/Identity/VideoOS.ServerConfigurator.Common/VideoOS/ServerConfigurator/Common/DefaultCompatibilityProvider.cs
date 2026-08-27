using System;

namespace VideoOS.ServerConfigurator.Common;

public class DefaultCompatibilityProvider : ICompatibilityProvider
{
	private static Lazy<DefaultCompatibilityProvider> _factory = new Lazy<DefaultCompatibilityProvider>(() => new DefaultCompatibilityProvider());

	public static ICompatibilityProvider Instance => _factory.Value;

	public T GetCompatibleType<T>(object value)
	{
		return (T)value;
	}
}
