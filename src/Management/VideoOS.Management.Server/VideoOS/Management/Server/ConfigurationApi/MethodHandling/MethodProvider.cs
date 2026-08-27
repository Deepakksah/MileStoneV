using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.MethodHandling;

public class MethodProvider : IMethodProvider
{
	private readonly IDictionary<string, IMethod> _methods;

	public MethodProvider(IEnumerable<IMethod> methods)
	{
		methods.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\MethodHandling\\MethodProvider.cs", 17);
		_methods = methods.ToDictionary((IMethod m) => m.Id);
	}

	public string[] GetIds()
	{
		return _methods.Keys.ToArray();
	}

	public IMethod Provide(ConfigurationItem item, string methodId)
	{
		item.NullCheck("", "Provide", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\MethodHandling\\MethodProvider.cs", 28);
		methodId.NullCheck("", "Provide", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\MethodHandling\\MethodProvider.cs", 29);
		if (!_methods.TryGetValue(methodId, out var value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", value, item.ItemType), "methodId");
		}
		return value;
	}
}
