using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Configuration;

public class RegistryConfiguration
{
	private const string FipsAlgorithmPolicyKeyName = "System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy";

	private readonly IRegistryService _registry;

	public RegistryConfiguration()
		: this(new RegistryService())
	{
	}

	public RegistryConfiguration(IRegistryService registry)
	{
		_registry = registry;
	}

	public bool IsFipsEnabled()
	{
		IRegistryKey registryKey = _registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy");
		if (registryKey == null)
		{
			throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("Could not find the key {0}.", "System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy")));
		}
		return Convert.ToBoolean(registryKey.GetValue("Enabled"), CultureInfo.InvariantCulture);
	}
}
